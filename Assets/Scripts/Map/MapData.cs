using System;
using System.Collections.Generic;
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

        public Transform SpawnPlayerTransform => spawnPlayerTransform;
        public SerializedDictionary<EnemyType, Transform> SpawnEnemyTransformMap => spawnEnemyTransformMap;

        private void Awake()
        {
            //destination.OnTriggerDestinationByPlayer.AddListener()
        }

        public Transform GetRandomPatrolTransform => patrolTransforms[Random.Range(0, patrolTransforms.Count)];
    }
    
    
}