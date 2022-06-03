using System;
using System.Collections.Generic;
using Script.Generated;
using Script.Repositories;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using UnityEngine.Purchasing.Security;

namespace Script
{
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        private static IStoreController m_StoreController;          
        private static IExtensionProvider m_StoreExtensionProvider; 
        private IAppleExtensions m_AppleExtensions;
        private IGooglePlayStoreExtensions m_GoogleExtensions;

        private void Start()
        {
            if (m_StoreController == null)
            {
                InitializePurchasing();
            }
        }
        
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            throw new NotImplementedException();
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            
        }

        public void BuyProduct()
        {
            
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
        
        public void RestorePurchases()
        {
            m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result => 
            {
                if (result)
                {
                    // успешный возврат покупки
                }
                else
                {
                    // неуспешный возврат покупки
                }
            });
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
        
        private bool IsInitialized()
        {
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }
    }
}