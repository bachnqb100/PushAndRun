using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject controller;
        [SerializeField] private int layerPlayer;
        [SerializeField] private int layerCharacterController;

        [Button]
        void ChangeLayer()
        {
            if (controller.layer == layerPlayer) controller.layer = layerCharacterController;
            else
            {
                controller.layer = layerPlayer;
            }
        }
    }
}