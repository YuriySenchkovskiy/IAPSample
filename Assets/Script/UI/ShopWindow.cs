using System;
using Script.Repositories;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class ShopWindow : AnimatedWindowController
    {
        [SerializeField] private Transform _inAppContainer;
        [SerializeField] private InShopWidget prefabInShopWidget;
        [SerializeField] private Button _restoreButton;
        // тут нужен звук

        private DataGroup<InAppDefinition, InShopWidget> _dataGroup;
        private InShopWidget[] _widgets;

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _restoreButton.gameObject.SetActive(false);
            }
        }

        protected override void Start()
        {
            base.Start();
            _dataGroup = new DataGroup<InAppDefinition, InShopWidget>(prefabInShopWidget, _inAppContainer);
            SetData();
        }

        public void MakeRestore()
        {
            
        }

        private void SetData()
        {
            _dataGroup.SetData(InAppRepository.I.Collection);
        }
    }
}