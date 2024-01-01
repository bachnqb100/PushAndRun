using System;
using System.Collections.Generic;
using CameraManager;
using DefaultNamespace.Enemy;
using DefaultNamespace.Map;
using DefaultNamespace.UI;
using DG.Tweening;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;

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

        public float ShotConfettiRate => shotConfettiRate;
        
        private MapData _currentMap;
        private List<EnemyController> _enemyControllers = new List<EnemyController>();

        private bool _isPlaying;
        
        private float _timeLeft;

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
            ClearCurrentMap();
            
            var mapCount = mapDataMap.Count;
            
            var idx = index % mapCount;

            _currentMap = Instantiate(mapDataMap[idx], mapRoot);

            _currentMap.PlayerVictory.AddListener(Victory);

            _timeLeft = _currentMap.TimePlay;
            
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
            
            SpawnMap(0);
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

            GameStatus = GameStatus.Defeat;
            DefeatReason = DefeatReason.Timeout;
            player.TimeOut();
            
            DOVirtual.DelayedCall(2f, () =>
            {
                GUIManager.Instance.ShowPanel(PanelType.DefeatScreen);
            });
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
            Debug.Log("Victory");
            
            if (!_isPlaying) return;
            _isPlaying = false;

            GameStatus = GameStatus.Victory;
            player.Victory();
            
            CameraController.Instance.EnableVictoryCamera();
            
            DOVirtual.DelayedCall(2f, () =>
            {
                GUIManager.Instance.ShowPanel(PanelType.VictoryScreen);
            });
            
        }

        #endregion



    }
}