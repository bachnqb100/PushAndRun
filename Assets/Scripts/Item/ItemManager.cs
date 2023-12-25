using System;
using UnityEngine;

namespace Item
{
    public class ItemManager : MonoBehaviour
    {
        #region Singleton

        private static ItemManager _instance;

        void InitSingleton()
        {
            if (_instance == null)
                _instance = this;
        }
        
        public static ItemManager Instance => _instance;

        #endregion

        [SerializeField]
        private SerializedDictionary<ItemType, GameObject> itemMap = new SerializedDictionary<ItemType, GameObject>();

        private void Awake()
        {
            InitSingleton();
        }

        public GameObject GetItem(ItemType itemType) => itemMap[itemType];

        public GameObject GetRandomItem() =>
            itemMap[(ItemType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ItemType)).Length)];


    }
}