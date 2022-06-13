using Script.IAP;
using Script.Repositories;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class ShopWindow : IAPAnimateWindowController
    {
        [SerializeField] private Transform _inAppContainer;
        [SerializeField] private InShopWidget _inShopWidgetPrefab;
        [SerializeField] private Button _restoreButton;
        [SerializeField] private IAPManager _iapManager;

        private DataGroup<InAppDefinition, InShopWidget> _dataGroup;
        private InShopWidget[] _widgets;

#if !UNITY_IOS && !UNITY_STANDALONE_OSX
        protected override void Awake()
        {
            base.Awake();
            _restoreButton.gameObject.SetActive(false);
        }
#endif

        protected override void Start()
        {
            base.Start();
            _dataGroup = new DataGroup<InAppDefinition, InShopWidget>(_inShopWidgetPrefab, _inAppContainer);
            SetData();
        }

        public void MakeRestore()
        {
            _iapManager.RestorePurchases();
        }

        private void SetData()
        {
            _dataGroup.SetData(InAppRepository.I.Collection);
        }
    }
}