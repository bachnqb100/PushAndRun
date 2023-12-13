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
            _enemyControllers = new List<EnemyController>();
            _enemyControllers.Clear();
            
            var mapCount = mapDataMap.Count;
            
            var idx = index % mapCount;

            _currentMap = Instantiate(mapDataMap[idx], mapRoot);
            
            SpawnEnemy();
            
            SpawnPlayerPos();
        }

        void SpawnPlayerPos()
        {
            player.InitPlayer(_currentMap.SpawnPlayerTransform);
        }

        void SpawnEnemy()
        {
            foreach (var item in _currentMap.SpawnEnemyTransformMap)
            {
                var e = Instantiate(enemyPatternMap[item.Key], item.Value.position, item.Value.rotation);
                
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