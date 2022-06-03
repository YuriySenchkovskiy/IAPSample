using Script.IAP;
using Script.Utils;
using UnityEngine;

namespace Script.UI
{ 
    public class IAPWindowController : MonoBehaviour
    {
        [SerializeField] private string _shopPath;
        [SerializeField] private string _popUpPath;
        [SerializeField] private string _failedPath;

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
