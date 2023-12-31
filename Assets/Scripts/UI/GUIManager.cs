using System;
using DG.Tweening;
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


        private void Start()
        {
            DOVirtual.DelayedCall(0.01f, () =>
            {
                _currentPanelType = PanelType.Loading;
                foreach (var item in panelMap)
                {
                    item.Value.Close();

                    panelMap[PanelType.Loading].Show(() => ShowPanel(PanelType.MainScreen));
                }
            });
        }


        public void ShowPanel(PanelType panelType, Action action = null)
        {
            HidePanel(_currentPanelType);
            
            panelMap[panelType].Show(action);
            _currentPanelType = panelType;
        }

        public void HidePanel(PanelType panelType, Action action = null)
        {
            panelMap[panelType].Hide(action);
            Debug.Log("Hide panel " + panelType);
        }

        public UIPanel GetPanel(PanelType panelType) => panelMap[panelType];


    }
}

public enum PanelType
{
    MainScreen,
    PlayScreen,
    Loading,
    VictoryScreen,
    DefeatScreen,
    Setting,
    Clothes,
    Upgrade,
    OnlineReward,
    DailyReward,
    LuckyWheel,
    Shop,
}