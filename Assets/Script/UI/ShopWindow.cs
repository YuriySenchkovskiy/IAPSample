using System;
using Script.Repositories;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    [RequireComponent(typeof(IAPManager))]
    public class ShopWindow : AnimatedWindowController
    {
        [SerializeField] private Transform _inAppContainer;
        [SerializeField] private InAppWidget _prefabInAppWidget;
        [SerializeField] private Button _restoreButton;
        // тут нужен звук

        private DataGroup<InAppDefinition, InAppWidget> _dataGroup;
        private InAppWidget[] _widgets;

        private void Awake()
        {
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                _restoreButton.gameObject.SetActive(false);
            }
        }

        protected override void Start()
        {
            base.Start();
            _dataGroup = new DataGroup<InAppDefinition, InAppWidget>(_prefabInAppWidget, _inAppContainer);
            SetData();
        }

        private void SetData()
        {
            _dataGroup.SetData(InAppRepository.I.Collection);
        }
    }
}