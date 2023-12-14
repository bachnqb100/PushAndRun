using System;
using UnityEngine;

namespace DefaultNamespace.Map
{
    public class MapBoundary : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.TryGetComponent<IFallable>(out var item);

            if (item != null)
            {
                item.Fall();
            }
        }
    }
}