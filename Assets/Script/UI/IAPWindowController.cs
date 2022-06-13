using System;
using Script.IAP;
using Script.Utils;
using UnityEngine;

namespace Script.UI
{ 
    public class IAPWindowController : MonoBehaviour
    {
        [SerializeField] private string _shopPath;
        [SerializeField] private string _failedPath;

        public static Action<string> PopUpCreate;
        public static Action<string> FailedCreate;
        
        private void OnEnable()
         {
             IAPManager.InitializedFailed += OnInitializedFailed;
             IAPManager.PurchaseInitializeFailed += OnPurchaseInitializeFailed;
             IAPManager.PurchaseFailed += OnPurchaseFailed;
             IAPManager.RestoreFailed += OnRestoreFailed;
         }

        private void OnDisable()
         {
             IAPManager.InitializedFailed -= OnInitializedFailed;
             IAPManager.PurchaseInitializeFailed -= OnPurchaseInitializeFailed;
             IAPManager.PurchaseFailed -= OnPurchaseFailed;
             IAPManager.RestoreFailed -= OnRestoreFailed;
         }
        
        public static void ShowPopUp(string productName)
        {
            var popUpPath = "UI/PopUpWindow";
            WindowUtil.CreateWindow(popUpPath);
            PopUpCreate?.Invoke(productName);
        }

        public void ShowShop()
        {
            WindowUtil.CreateWindow(_shopPath);
        }
        
        private void OnInitializedFailed(string error)
        {
            WindowUtil.CreateWindow(_failedPath);
            FailedCreate?.Invoke(error);
        }
        
        private void OnPurchaseInitializeFailed(string error)
        {
            WindowUtil.CreateWindow(_failedPath);
            FailedCreate?.Invoke(error);
        }

        private void OnPurchaseFailed(string error)
        {
            WindowUtil.CreateWindow(_failedPath);
            FailedCreate?.Invoke(error);
        }
        
        private void OnRestoreFailed(string error)
        {
            WindowUtil.CreateWindow(_failedPath);
            FailedCreate?.Invoke(error);
        }
    }
}