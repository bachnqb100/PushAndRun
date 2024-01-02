using System;
using CameraManager;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Tutorial
{
    public class TutorialPunchEnemy : MonoBehaviour
    {
        [SerializeField] private Button btn;

        private void OnEnable()
        {
            btn.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            btn.onClick.RemoveListener(Hide);
        }

        private void Start()
        {
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            
            CameraController.Instance.SetCameraTutorial(true, GameController.Instance.EnemyTutorial.transform);
        }

        void Hide()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            
            CameraController.Instance.SetCameraTutorial(false);

        }
    }
}