using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CameraManager;
using DefaultNamespace.Configs;
using DefaultNamespace.Enemy;
using DefaultNamespace.Haptic;
using DefaultNamespace.Map;
using DefaultNamespace.UI;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class GameController : MonoBehaviour
    {
        #region Singleton

        private static GameController _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static GameController Instance => _instance;

        #endregion

        [Title("Enemy Pattern")] 
        [SerializeField] private Transform enemyRoot;
        [SerializeField] private SerializedDictionary<EnemyType, GameObject> enemyPatternMap;

        [Title("Map")]
        [SerializeField] private Transform mapRoot;
        [SerializeField] private SerializedDictionary<int, MapData> mapDataMap;
        

        [Title("Player")] [SerializeField] private PlayerController player;

        [SerializeField] private Transform playerPosMain;
        [SerializeField] private GameObject playerContainer;


        [Title("Audio")] 
        [SerializeField] private float shotConfettiRate = 0.2f;

        [Header("Money")] 
        [SerializeField] private int moneyPunch = 50;

        [SerializeField] private int moneyTimePerMinute = 10; 

        public float ShotConfettiRate => shotConfettiRate;
        
        private MapData _currentMap;
        private List<EnemyController> _enemyControllers = new List<EnemyController>();

        private bool _isPlaying;
        
        private float _timeLeft;

        private int _moneyMap;

        public EnemyController EnemyTutorial => _enemyControllers[0];

        public bool IsPlaying
        {
            get => _isPlaying;
            set => _isPlaying = value;
        }
        public float TimeLeft => _timeLeft;
        public PlayerController Player => player;
        public MapData CurrentMap => _currentMap;
        public DefeatReason DefeatReason { get; private set; }
        
        public GameStatus GameStatus { get; private set; }
        
        public int ImpactCount { get; set; }

        public float TimeCompleted => _currentMap.TimePlay - _timeLeft;

        #region Map

        [Button]
        public void SpawnMap(int index)
        {
            ImpactCount = 0;
            
            ClearCurrentMap();
            
            var mapCount = mapDataMap.Count;
            
            var idx = index % mapCount;

            _currentMap = Instantiate(mapDataMap[idx], mapRoot);

            _currentMap.PlayerVictory.AddListener(Victory);

            _timeLeft = _currentMap.TimePlay;

            _moneyMap = _currentMap.MoneyMap;
            
            SpawnEnemy();
            
            SpawnPlayerPos();
        }
        
        void ClearCurrentMap()
        {
            //enemy
            _enemyControllers ??= new List<EnemyController>();

            if (_enemyControllers.Count > 0)
            {
                var n = _enemyControllers.Count;

                for (int i = 0; i < n; i++)
                {
                    Destroy(_enemyControllers[i].transform.parent.gameObject);
                }
            }
                
            _enemyControllers.Clear();
                
            //map

            if (_currentMap)
            {
                _currentMap.PlayerVictory.RemoveListener(Victory);
                Destroy(_currentMap.gameObject);
            }
        }

        void SpawnPlayerPos()
        {
            player.InitPlayer(_currentMap.SpawnPlayerTransform);
            Debug.Log("Spawning player");
        }

        void SpawnEnemy()
        {
            foreach (var item in _currentMap.SpawnEnemyTransformMap)
            {
                var e = Instantiate(enemyPatternMap[item.Key], item.Value.position, item.Value.rotation, enemyRoot);
                
                var eCtrl = e.GetComponentInChildren<EnemyController>(); 
                _enemyControllers.Add(eCtrl);
            }
        }
        
        #endregion

        private void Start()
        {
            playerContainer.gameObject.SetActive(true);
        }

        public void StartGame()
        {
            //TODO: logic choose map game

            GameStatus = GameStatus.Playing;
            CameraController.Instance.DisableFallCamera();
            CameraController.Instance.DisableVictoryCamera();
            
            SpawnMap(GameManager.Instance.GameData.userData.map);
            
        }

        private void Update()
        {
            if (_isPlaying)
            {
                _timeLeft -= Time.deltaTime;

                if (_timeLeft <= 0)
                {
                    DefeatByTimeUp();
                }
            }
        }

        [Button]
        public void PlacePlayerMain()
        {
            player.ResetPlayerMainScreen(playerPosMain);
            ClearCurrentMap();
        }

        public void SetGameStatus(GameStatus status)
        {
            this.GameStatus = status;
            
            player.PlayerUpdateAnim();

            switch (status)
            {
                case GameStatus.MainScreen:
                    player.PlayerOnlyUseAnimation();
                    break;
                case GameStatus.ShopAnimVictory:
                    player.PlayerOnlyUseAnimation();
                    break;
            }
        }

        #region Defeat

        public void DefeatByPlayerFall()
        {
            if (!_isPlaying) return;
            _isPlaying = false;
            
            BHHaptic.Haptic(HapticTypes.Failure);

            GameStatus = GameStatus.Defeat;
            DefeatReason = DefeatReason.Fall;
            DOVirtual.DelayedCall(2f, () =>
            {
                GUIManager.Instance.ShowPanel(PanelType.DefeatScreen);
            });
        }


        public void EnemyDetectedPlayer()
        {
            if (!_isPlaying) return;
            _isPlaying = false;
            
            BHHaptic.Haptic(HapticTypes.Failure);

            GameStatus = GameStatus.Defeat;
            DefeatReason = DefeatReason.Detect;
            player.Detected();
            
            DOVirtual.DelayedCall(2f, () =>
            {
                GUIManager.Instance.ShowPanel(PanelType.DefeatScreen);
            });
        }
        
        public void DefeatByTimeUp()
        {
            if (!_isPlaying) return;
            _isPlaying = false;
            
            BHHaptic.Haptic(HapticTypes.Failure);

            GameStatus = GameStatus.Defeat;
            DefeatReason = DefeatReason.Timeout;
            player.TimeOut();
            
            DOVirtual.DelayedCall(2f, () =>
            {
                GUIManager.Instance.ShowPanel(PanelType.DefeatScreen);
            });
        }


        public int CalculateMoneyDefeat()
        {
            float moneyValue = moneyTimePerMinute * (_timeLeft / 60f) + moneyPunch * ImpactCount + _moneyMap / 10f;
            return (int) moneyValue;
        }
        
        #endregion


        #region Victory

        public void UpdateEnemyFallStatus()
        {
            var check = true;
            foreach (var enemy in _enemyControllers)
            {
                if (!enemy.IsFell)
                {
                    check = false;
                    break;
                }
            }
            
            if (!check) return;

            ShowDestination();
        }

        void ShowDestination()
        {
            _currentMap.ShowDestination();
        }

        void Victory()
        {
            //Debug.Log("Victory");
            
            if (!_isPlaying) return;
            _isPlaying = false;

            BHHaptic.Haptic(HapticTypes.Success);
            
            EnemyHideFov();
            
            GameStatus = GameStatus.Victory;
            player.Victory();

            GameManager.Instance.GameData.userData.map++;
            
            CameraController.Instance.EnableVictoryCamera();
            
            DOVirtual.DelayedCall(2f, () =>
            {
                GUIManager.Instance.ShowPanel(PanelType.VictoryScreen);
            });
            
            
        }
        
        public int CalculateMoneyVictory(out int moneyTime, out int moneyPunch)
        {
            moneyTime = moneyTimePerMinute * (int)(_timeLeft / 60f);
            moneyPunch = this.moneyPunch * ImpactCount;
            return moneyTime + moneyPunch + _moneyMap;
        }

        #endregion


        #region Gift

        public bool SetDailyReward()
        {
            if (GameManager.Instance.GameData.userData.currentDailyReward != MainAnimType.SlowRun &&
                !GameManager.Instance.GameData.userData.mainAnimItemStatusMap[
                    GameManager.Instance.GameData.userData.currentDailyReward])
                return true;
            
            
            List<AnimMainConfig> itemList = new List<AnimMainConfig>();
            foreach (var config in ConfigManager.Instance.animMainGroupConfig.configs)
            {
                if (config.isExclusive && !GameManager.Instance.GameData.userData.mainAnimItemStatusMap[config.type])
                    itemList.Add(config);
            }
            if (itemList.Count <= 0) return false;
            else
            {
                var index = Random.Range(0, itemList.Count);

                GameManager.Instance.GameData.userData.currentDailyReward = itemList[index].type;
            }

            return true;
        }
        
        
        public bool SetWheelReward()
        {
            if (GameManager.Instance.GameData.userData.currentWheelReward != VictoryAnimType.BellyDance &&
                !GameManager.Instance.GameData.userData.victoryAnimItemStatusMap[
                    GameManager.Instance.GameData.userData.currentWheelReward])
                return true;
            
            
            List<AnimVictoryConfig> itemList = new List<AnimVictoryConfig>();
            foreach (var config in ConfigManager.Instance.animVictoryGroupConfig.configs)
            {
                if (config.isExclusive && !GameManager.Instance.GameData.userData.victoryAnimItemStatusMap[config.type])
                    itemList.Add(config);
            }
            if (itemList.Count <= 0) return false;
            else
            {
                var index = Random.Range(0, itemList.Count);

                GameManager.Instance.GameData.userData.currentWheelReward = itemList[index].type;
            }

            return true;
        }
        

        #endregion

        public void ShowEnemyIndicator()
        {
            foreach (var enemy in _enemyControllers)
            {
                enemy.ShowIndicator();
            }
        }

        public void EnemyHideFov()
        {
            foreach (var enemy in _enemyControllers)
            {
                enemy.StopFOV();
            }
        }
    }
}