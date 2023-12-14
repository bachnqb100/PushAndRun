using System;
using System.Collections.Generic;
using DefaultNamespace.Enemy;
using DefaultNamespace.Map;
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

        private MapData _currentMap;
        private List<EnemyController> _enemyControllers = new List<EnemyController>();


        public MapData CurrentMap => _currentMap;

        [Button]
        public void SpawnMap(int index)
        {
            ClearCurrentMap();
            
            var mapCount = mapDataMap.Count;
            
            var idx = index % mapCount;

            _currentMap = Instantiate(mapDataMap[idx], mapRoot);
            
            SpawnEnemy();
            
            SpawnPlayerPos();

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
                if (_currentMap) Destroy(_currentMap.gameObject);
            }
        }

        void SpawnPlayerPos()
        {
            player.InitPlayer(_currentMap.SpawnPlayerTransform);
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


        public void StartGame()
        {
            //TODO: logic choose map game
            SpawnMap(0);
        }
    }
}