using System;
using System.Collections.Generic;
using DefaultNamespace.Tutorial;
using Item;
using Sigtrap.Relays;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Map
{
    public class MapData : MonoBehaviour
    {
        [Header("Gameplay")]
        [SerializeField] private Transform spawnPlayerTransform;
        [SerializeField] private Destination destination;
        
        [Header("Enemy")]
        [SerializeField] private List<Transform> patrolTransforms;
        [SerializeField] private SerializedDictionary<EnemyType, Transform> spawnEnemyTransformMap;

        [Header("Time")] 
        [SerializeField] private float timePlay = 30f;

        [Header("Spawn Item Root")] 
        [SerializeField] private Transform spawnItemRoot;

        [Header("Spawn Item Transform")] 
        [SerializeField] private float spawnItemInterval = 15f;
        [SerializeField] private List<SpawnItemPos> spawnItemTransforms;

        [Header("Money")] 
        [SerializeField] private int moneyMap = 500;

        private float _cdSpawnItem;
        private List<SpawnItemPos> _spawnItemMaps;

        public Transform SpawnItemRoot => spawnItemRoot;

        public Relay PlayerVictory = new Relay();

        public Transform SpawnPlayerTransform => spawnPlayerTransform;
        public SerializedDictionary<EnemyType, Transform> SpawnEnemyTransformMap => spawnEnemyTransformMap;

        public float TimePlay => timePlay;
        
        public Transform GetRandomPatrolTransform => patrolTransforms[Random.Range(0, patrolTransforms.Count)];
        
        public int MoneyMap => moneyMap;

        public Destination Destination => destination;

        private void OnEnable()
        {
            destination.OnTriggerDestinationByPlayer.AddListener(PlayerVictory.Dispatch);
            HideDestination();
        }

        private void OnDisable()
        {
            destination.OnTriggerDestinationByPlayer.RemoveListener(PlayerVictory.Dispatch);
        }

        private void Start()
        {
            _cdSpawnItem = spawnItemInterval;

            _spawnItemMaps = new List<SpawnItemPos>();
            foreach (var spawnItemTransform in spawnItemTransforms)
            {
                _spawnItemMaps.Add(spawnItemTransform);
            }
        }

        private void Update()
        {
            if (!GameController.Instance.IsPlaying) return;

            _cdSpawnItem -= Time.deltaTime;

            if (_cdSpawnItem <= 0)
            {
                SpawnRandomItem();
                _cdSpawnItem = spawnItemInterval;
            }
        }

        public void HideDestination() => destination.gameObject.SetActive(false);
        public void ShowDestination()
        {
            TutorialManager.Instance.ShowTutorialDestination();
            
            destination.gameObject.SetActive(true);
        }

        public void SpawnRandomItem()
        {
            var pos = GetRandomPosCanSpawnItem();

            if (pos)
            {
               var item  = Instantiate(ItemManager.Instance.GetRandomItem(), pos.transform.position, pos.transform.rotation, spawnItemRoot);

               pos.Item = item;
            }


            SpawnItemPos GetRandomPosCanSpawnItem()
            {
                var listCanSpawn = new List<SpawnItemPos>();
                foreach (var spawnItemPos in _spawnItemMaps)
                {
                    if (!spawnItemPos.Item) listCanSpawn.Add(spawnItemPos);
                }

                if (listCanSpawn.Count > 0)
                {
                    return listCanSpawn[Random.Range(0, listCanSpawn.Count)];
                }
                
                return null;
            }
        }
        

    }
    
    
}