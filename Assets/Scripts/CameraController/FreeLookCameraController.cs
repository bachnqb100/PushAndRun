using System;
using Cinemachine;
using CnControls;
using UnityEngine;

namespace CameraController
{
    public class FreeLookCameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineFreeLook freeLookCamera;
        

        private void Update()
        {
            float cameraX = CnInputManager.GetAxis("CameraX");

            if (Mathf.Abs(cameraX) >= 1f || Mathf.Abs(cameraX) >= 1f)
            {
                freeLookCamera.m_XAxis.Value += cameraX;
            }
        }
    }
}