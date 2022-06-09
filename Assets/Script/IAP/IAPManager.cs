using System;
using Script.Repositories;
using Script.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace Script.IAP
{
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        private const string Initialize = "OnInitialize";
        private const string NullError = "NullInProduct";
        private const string InitializeError = "IStoreControllerUnavailable";
        private const string RestoreError = "RestoreFailed";
        
        private static IStoreController _storeController;          
        private static IExtensionProvider _storeExtensionProvider;

        public static UnityAction<string> InitializedFailed;
        public static UnityAction<string> PurchaseInitializeFailed;
        public static UnityAction PurchaseSuccess;
        public static UnityAction<string> PurchaseFailed;
        public static UnityAction<string> RestoreFailed;
        
        private bool IsInitialized => _storeController != null && _storeExtensionProvider != null;
            
        private void Start()
        {
            if (_storeController == null)
            {
                InitializePurchasing();
            }
        }
        
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _storeExtensionProvider = extensions;
        }
        
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            var number = ErrorDatabase.GetErrorNumber(error + Initialize);
            InitializedFailed?.Invoke(number);
        }
        
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var product = purchaseEvent.purchasedProduct;
            var id = product.definition.id;
            
            if (InAppRepository.I.GetBundleStatus(id))
            {
                BuyBundle();
            }
            else
            {
                IAPDataManager.SaveID(id);
            }
            
            PurchaseSuccess?.Invoke();
            return PurchaseProcessingResult.Complete;
        }
        
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            var number = ErrorDatabase.GetErrorNumber(failureReason.ToString());
            PurchaseFailed?.Invoke(number);
        }

        public void BuyProduct(string id)
        {
            if (IsInitialized)
            {
                Product product = _storeController.products.WithID(id);

                if (product != null)
                {
                    _storeController.InitiatePurchase(product);
                }
                else
                {
                    var number = ErrorDatabase.GetErrorNumber(NullError);
                    PurchaseInitializeFailed?.Invoke(number);
                }
            }
            else
            {
                var number = ErrorDatabase.GetErrorNumber(InitializeError);
                PurchaseInitializeFailed?.Invoke(number);
            }
        }
        
        public void RestorePurchases()
        {
            _storeExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result => 
            {
                if (result == false)
                {
                    var number = ErrorDatabase.GetErrorNumber(RestoreError);
                    RestoreFailed?.Invoke(number);
                }
            });
        }

        public float GetPrice(string id)
        {
            if (_storeController == null)
            {
                InitializePurchasing();
            }

            var productsAll = _storeController?.products.all;
            
            if (productsAll == null)
            {
                return default;
            }

            foreach (var product in productsAll)
            {
                if (product.definition.id == id)
                {
                    return (float)Math.Round(product.metadata.localizedPrice, 2);
                }
            }

            return default;
        }

        public bool IsProductPurchased(string name)
        {
            if (IAPDataManager.HasID(name))
            {
                return true;
            }

            return default;
        }

        private void InitializePurchasing()
        {
            if (IsInitialized)
            {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (var item in InAppRepository.I.Collection)
            {
                builder.AddProduct(item.ProductID, ProductType.NonConsumable);
            }
            
            UnityPurchasing.Initialize(this, builder);
        }
        
        private void BuyBundle()
        {
            foreach (var item in InAppRepository.I.Collection)
            {
                if (IAPDataManager.HasID(item.Name))
                {
                    continue;
                }
                
                IAPDataManager.SaveID(item.ProductID);
            }
        }
    }
}