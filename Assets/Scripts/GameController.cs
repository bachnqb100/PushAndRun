using System;
using DefaultNamespace.Map;
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


        private MapData _currentMap;


        public MapData CurrentMap => _currentMap;

        [Button]
        public void SetCurrentMap(MapData mapData)
        {
            Debug.Log("SetCurrentMap");
            _currentMap = mapData;
        }
    }
}