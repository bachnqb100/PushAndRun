using System;
using Cinemachine;
using CnControls;
using DefaultNamespace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CameraManager
{
    public class CameraController : MonoBehaviour
    {
        #region Singleton

        private static CameraController _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        public static CameraController Instance => _instance;
        #endregion

        [SerializeField] private CinemachineFreeLook freeLookCamera;
        [SerializeField] private float speedX = 1f;
        [SerializeField] private float speedY = 1f;
        
        
        [Title("Fall Camera")]
        [SerializeField] private CinemachineVirtualCamera fallCamera;
        [SerializeField] private Transform fallCameraFollow;
        
        [Title("Victory Camera")]
        [SerializeField] private CinemachineVirtualCamera victoryCamera;
        
        [Title("MainScreen Camera")]
        [SerializeField] private CinemachineVirtualCamera mainScreenCamera;

        [Header("Right Camera")] 
        [SerializeField] private CinemachineVirtualCamera rightCamera;
        
        [Header("Left Camera")]
        [SerializeField] private CinemachineVirtualCamera leftCamera;

        private void Start()
        {
            SetStatusCameraLeft(false);
            SetStatusCameraRight(false);
        }

        private void Update()
        {
            if (!GameController.Instance.IsPlaying) return;
            
            float cameraX = CnInputManager.GetAxis("CameraX");
            float cameraY = CnInputManager.GetAxis("CameraY");
            
            //Debug.Log("cameraX: " + cameraX + ", cameraY: " + cameraY);
            

            if (Mathf.Abs(cameraX) >= 1f || Mathf.Abs(cameraX) >= 1f)
            {
                freeLookCamera.m_XAxis.Value += cameraX * speedX;
            }
            
            if (Mathf.Abs(cameraY) >= 1f || Mathf.Abs(cameraY) >= 1f)
            {
                freeLookCamera.m_YAxis.Value += cameraY * speedY;
            }
        }
        
        


        [Button]
        public void EnableFallCamera()
        {
            fallCamera.enabled = true;
            fallCamera.Follow = null;
        }

        [Button]
        public void DisableFallCamera()
        {
            fallCamera.Follow = fallCameraFollow;
            fallCamera.enabled = false;
        }

        public void EnableVictoryCamera()
        {
            victoryCamera.enabled = true;
        }

        public void DisableVictoryCamera()
        {
            victoryCamera.enabled = false;
        }

        public void SetStatusCameraMain(bool enable)
        {
            mainScreenCamera.enabled = enable;
        }

        public void SetStatusCameraRight(bool enable)
        {
            rightCamera.enabled = enable;
        }

        public void SetStatusCameraLeft(bool enable)
        {
            leftCamera.enabled = enable;
        }
    }
}