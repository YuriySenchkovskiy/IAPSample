using System;
using Script.Utils;
using UnityEngine;

namespace Script.UI
{
    [RequireComponent(typeof(IAPManager))]
    public class IAPWindowController : MonoBehaviour
    {
        [SerializeField] private string _shopPath;
        [SerializeField] private string _popUpPath;
        [SerializeField] private string _failedPath;
        [SerializeField] private IAPManager _iapManager;

        private void OnEnable()
        {
            _iapManager.PurchaseFailed += OnPurchaseFailed;
        }
        
        private void OnDisable()
        {
            _iapManager.PurchaseFailed -= OnPurchaseFailed;
        }

        public void ShowShop()
        {
            WindowUtil.CreateWindow(_shopPath);
        }

        public void ShowPopUp()
        {
            WindowUtil.CreateWindow(_popUpPath);
        }
        
        private void OnPurchaseFailed()
        {
            WindowUtil.CreateWindow(_failedPath);
        }
    }
}
