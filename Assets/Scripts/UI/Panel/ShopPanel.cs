using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Haptic;
using DefaultNamespace.UI.Shop;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class ShopPanel : UIPanel
    {
        [SerializeField] private ButtonExtension closeButton;
        
        [Header("Main Item")] 
        [SerializeField] private ShopMainAnimItem shopMainAnimItemPrefab;
        [SerializeField] private Transform mainAnimItemRoot;
        
        [Header("Victory Item")] 
        [SerializeField] private ShopVictoryAnimItem shopVictoryAnimItemPrefab;
        [SerializeField] private Transform victoryAnimItemRoot;
        
        
        [SerializeField] private List<TabShopButton> btns;


        private List<ShopMainAnimItem> _shopMainAnimItems = new List<ShopMainAnimItem>();
        private List<ShopVictoryAnimItem> _shopVictoryAnimItems = new List<ShopVictoryAnimItem>();
        protected override void Init()
        {
            base.Init();
            _shopMainAnimItems.Clear();
            global::Utils.DestroyChildren(mainAnimItemRoot);
            

            for (int i = 0; i < Enum.GetValues(typeof(MainAnimType)).Length; i++)
            {
                var type = (MainAnimType)i;
                var item = Instantiate(shopMainAnimItemPrefab, mainAnimItemRoot);
                _shopMainAnimItems.Add(item);

                item.Init(type, () => _shopMainAnimItems.ForEach(x => x.DeSelect()),
                    () => _shopMainAnimItems.ForEach(x => x.DeSelectUse()));
            }
            _shopMainAnimItems[0].Use();
            
            
            for (int i = 0; i < Enum.GetValues(typeof(VictoryAnimType)).Length; i++)
            {
                var type = (VictoryAnimType)i;
                var item = Instantiate(shopVictoryAnimItemPrefab, victoryAnimItemRoot);
                _shopVictoryAnimItems.Add(item);

                item.Init(type, () => _shopVictoryAnimItems.ForEach(x => x.DeSelect()),
                    () => _shopVictoryAnimItems.ForEach(x => x.DeSelectUse()));
            }
            _shopVictoryAnimItems[0].Use();
            
            
            btns.ForEach(x => x.Init(() => btns.ForEach(x => x.HideContent())));
            btns[0].ShowContent();
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            
            closeButton.onClick.AddListener(ClosePopup);
        }

        protected override void UnregisterEvent()
        {
            base.UnregisterEvent();
            
            closeButton.onClick.RemoveListener(ClosePopup);
        }

        void ClosePopup()
        {
            BHHaptic.Haptic(HapticTypes.Selection);
            
            GUIManager.Instance.ShowPanel(PanelType.MainScreen);
        }
    }
}