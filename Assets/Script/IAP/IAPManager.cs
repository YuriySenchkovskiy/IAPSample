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
        private static IStoreController m_StoreController;          
        private static IExtensionProvider m_StoreExtensionProvider;

        public static UnityAction<string> InitializedFailed;
        public static UnityAction<string> PurchaseInitializeFailed;
        public static UnityAction PurchaseSuccess;
        public static UnityAction<string> PurchaseFailed;
        public static UnityAction<string> RestoreFailed;
            
        private void Start()
        {
            if (m_StoreController == null)
            {
                InitializePurchasing();
            }
        }
        
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
        }
        
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            var initialize = "OnInitialize";
            var number = ErrorDatabase.GetErrorNumber(error + initialize);
            InitializedFailed?.Invoke(number);
        }
        
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var product = purchaseEvent.purchasedProduct;
            var id = product.definition.id;
            var transactionID = product.transactionID;
            
            // продажа всех видов товаров в одном bundle идет последней в InAppRepository
            if (id == InAppRepository.I.Collection[InAppRepository.I.Collection.Length - 1].ProductID)
            {
                BuyBundle();
            }
            else
            {
                PlayerPrefs.SetInt(id, 0);
            }
            
            PurchaseSuccess?.Invoke();
            return PurchaseProcessingResult.Complete;
        }
        
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            var number = ErrorDatabase.GetErrorNumber(failureReason.ToString());
            PurchaseFailed?.Invoke(number);
        }

        public void BuyProduct(string name)
        {
            var id = InAppRepository.I.GetID(name);
            var nullError = "NullInProduct";
            var initializeError = "IStoreControllerUnavailable";
            
            if (IsInitialized())
            {
                Product product = m_StoreController.products.WithID(id);

                if (product != null)
                {
                    m_StoreController.InitiatePurchase(product);
                }
                else
                {
                    var number = ErrorDatabase.GetErrorNumber(nullError);
                    PurchaseInitializeFailed?.Invoke(number);
                }
            }
            else
            {
                var number = ErrorDatabase.GetErrorNumber(initializeError);
                PurchaseInitializeFailed?.Invoke(number);
            }
        }
        
        public void RestorePurchases()
        {
            var appleRestore = "AppleRestoreComplete";
            var restoreError = "RestoreFailed";

            m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result => 
            {
                if (result)
                {
                    PlayerPrefs.SetInt(appleRestore, 0);
                }
                else
                {
                    var number = ErrorDatabase.GetErrorNumber(restoreError);
                    RestoreFailed?.Invoke(number);
                }
            });
        }

        public float GetPrice(string name)
        {
            if (m_StoreController == null)
            {
                InitializePurchasing();
            }

            var productsAll = m_StoreController?.products.all;
            
            if (productsAll == null)
            {
                return default;
            }
            
            var id = InAppRepository.I.GetID(name);

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
            var id = InAppRepository.I.GetID(name);
            
            if (PlayerPrefs.HasKey(id))
            {
                return true;
            }

            return default;
        }

        private bool IsInitialized()
        {
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        private void InitializePurchasing()
        {
            if (IsInitialized())
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
                if (PlayerPrefs.HasKey(item.ProductID))
                {
                    continue;
                }
                
                PlayerPrefs.SetInt(item.ProductID, 0);
            }
        }
    }
}