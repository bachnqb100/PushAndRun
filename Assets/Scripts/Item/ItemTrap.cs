using System;
using DefaultNamespace;
using UnityEngine;

namespace Item
{
    public class ItemTrap : MonoBehaviour
    {
        [SerializeField] private int trapCount = 5;
        [SerializeField] private TrapItem trapItemPrefab;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                SpawnTrap();
                
                Destroy(gameObject);
            }
        }

        private void SpawnTrap()
        {
            for (int i = 0; i < trapCount; i++)
            {
                var trap = Instantiate(trapItemPrefab,transform.position, transform.rotation, GameController.Instance.CurrentMap.SpawnItemRoot);
                trap.Init();
            }
        }
    }
}