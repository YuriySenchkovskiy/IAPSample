using System;
using Script.Repositories;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace Script
{
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        private static IStoreController m_StoreController;          
        private static IExtensionProvider m_StoreExtensionProvider; 
        private IAppleExtensions m_AppleExtensions;
        private IGooglePlayStoreExtensions m_GoogleExtensions;

        public UnityAction ProductSold;
        public UnityAction SoldFailed;
        public UnityAction RestoreFailed;
            
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
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
            m_GoogleExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
        }
        
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log($"Not initialized. Reason: {error}");
        }
        
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var product = purchaseEvent.purchasedProduct;
            var id = product.definition.id;
            var transactionID = product.transactionID;
            
            // логика успешной покупки - пишем в файл - какой?

            // продажа всех видов товаров в одном bundle идет последней в InAppRepository
            if (id == InAppRepository.I.Collection[InAppRepository.I.Collection.Length - 1].ProductID)
            {
                BuyBundle();
            }
            else
            {
                PlayerPrefs.SetInt(id, 0);
            }
            
            ProductSold?.Invoke();
            Debug.Log($"Purchase Complete: {product.definition.id}");

            return PurchaseProcessingResult.Complete;
        }
        
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // в случае неудачи делаем возврат в главное меню
            Debug.Log($"{product.definition.id} failed because {failureReason}");
            SoldFailed?.Invoke();
        }

        public void BuyProduct(string name)
        {
            var id = InAppRepository.I.GetID(name);
            
            if (IsInitialized())
            {
                Product product = m_StoreController.products.WithID(id);

                if (product != null && product.availableToPurchase)
                {
                    Debug.Log($"Purchasing product: {product.definition.id}");
                    m_StoreController.InitiatePurchase(product); // запускает ProcessPurchase()
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }
        
        // метод только для эпл и их политики восстановления покупок
        public void RestorePurchases()
        {
            m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result => 
            {
                if (result)
                {
                    // успешный возврат покупки - всплывающее окно - успех
                }
                else
                {
                    // неуспешный возврат покупки - всплывающее окно - неудача
                    RestoreFailed?.Invoke();
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