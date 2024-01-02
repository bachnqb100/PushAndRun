using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        #region Singleton

        private static TutorialManager _instance;

        void InitSingleton()
        {
            if (_instance == null)
                _instance = this;
        }
        
        public static TutorialManager Instance => _instance;

        #endregion

        [SerializeField] private TutorialMovement tutorialMovement;
        [SerializeField] private TutorialPunchEnemy tutorialPunchEnemy;
        [SerializeField] private TutorialDestination tutorialDestination;


        private void Awake()
        {
            InitSingleton();
        }

        [Button]
        public void ShowTutorialMovement()
        {
            if (!GameManager.Instance.GameData.userData.isFirstMovement) return;
            tutorialMovement.Show();
            GameManager.Instance.GameData.userData.isFirstMovement = false;

            Time.timeScale = 0f;
        }

        public void ShowTutorialPunchEnemy()
        {
            tutorialPunchEnemy.Show();

            Time.timeScale = 0f;
        }

        public void ShowTutorialDestination()
        {
            if (!GameManager.Instance.GameData.userData.isFirstDestination) return;
            tutorialDestination.Show();
            GameManager.Instance.GameData.userData.isFirstDestination = false;

            Time.timeScale = 0f;
        }
    }
}