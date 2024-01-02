using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Tutorial
{
    public class TutorialMovement : MonoBehaviour
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
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        void Hide()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            
            TutorialManager.Instance.ShowTutorialPunchEnemy();

        }
    }
}