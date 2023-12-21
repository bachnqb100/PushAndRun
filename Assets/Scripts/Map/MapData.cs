using System;
using System.Collections.Generic;
using Sigtrap.Relays;
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

        public Relay PlayerVictory = new Relay();

        public Transform SpawnPlayerTransform => spawnPlayerTransform;
        public SerializedDictionary<EnemyType, Transform> SpawnEnemyTransformMap => spawnEnemyTransformMap;

        public float TimePlay => timePlay;
        
        public Transform GetRandomPatrolTransform => patrolTransforms[Random.Range(0, patrolTransforms.Count)];

        private void OnEnable()
        {
            destination.OnTriggerDestinationByPlayer.AddListener(PlayerVictory.Dispatch);
            HideDestination();
        }

        private void OnDisable()
        {
            destination.OnTriggerDestinationByPlayer.RemoveListener(PlayerVictory.Dispatch);
        }

        public void HideDestination() => destination.gameObject.SetActive(false);
        public void ShowDestination() => destination.gameObject.SetActive(true);

    }
    
    
}