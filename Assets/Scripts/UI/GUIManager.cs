using System;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class GUIManager : MonoBehaviour
    {
        #region Singleton

        private static GUIManager _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }
        
        public static GUIManager Instance => _instance;

        #endregion

        [SerializeField] private SerializedDictionary<PanelType, UIPanel> panelMap;
        
        [SerializeField] private NotificationManager notify;
        
        private PanelType _currentPanelType;


        public void ShowPanel(PanelType panelType, Action action = null)
        {
            HidePanel(_currentPanelType);
            
            panelMap[panelType].Show(action);
            _currentPanelType = panelType;
        }

        public void HidePanel(PanelType panelType, Action action = null)
        {
            panelMap[panelType].Hide(action);
        }
        
        
    }
}

public enum PanelType
{
    MainScreen,
    PlayScreen,
    Loading,
    VictoryScreen,
    DefeatScreen,
}