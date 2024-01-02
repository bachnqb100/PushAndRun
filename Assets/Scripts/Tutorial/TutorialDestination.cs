using CameraManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

namespace DefaultNamespace.Tutorial
{
    public class TutorialDestination : MonoBehaviour
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
            
            CameraController.Instance.SetCameraTutorial(true, GameController.Instance.CurrentMap.Destination.transform);
        }

        void Hide()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
            
            CameraController.Instance.SetCameraTutorial(false);

        }
    }
}