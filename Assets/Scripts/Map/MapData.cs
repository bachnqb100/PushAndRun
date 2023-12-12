using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.Map
{
    public class MapData : MonoBehaviour
    {
        [SerializeField] private List<Transform> patrolTransforms;


        public Transform GetRandomPatrolTransform => patrolTransforms[Random.Range(0, patrolTransforms.Count)];
    }
}