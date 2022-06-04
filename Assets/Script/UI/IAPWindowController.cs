using Script.IAP;
using Script.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Script.UI
{ 
    public class IAPWindowController : MonoBehaviour
    {
        [SerializeField] private string _shopPath;
        [SerializeField] private string _popUpPath;
        [SerializeField] private string _failedPath;

        public static UnityAction<string> PopUpCreate;
        
        private void OnEnable()
         {
             IAPManager.PurchaseFailed += OnPurchaseFailed;
         }
        
         private void OnDisable()
         {
             IAPManager.PurchaseFailed -= OnPurchaseFailed;
         }

        public void ShowShop()
        {
            WindowUtil.CreateWindow(_shopPath);
        }

        public void ShowPopUp(string productName)
        {
            WindowUtil.CreateWindow(_popUpPath);
            PopUpCreate?.Invoke(productName);
        }
        
        private void OnPurchaseFailed()
        {
            WindowUtil.CreateWindow(_failedPath);
        }
    }
}
