using System;
using System.Collections.Generic;
using Script.Generated;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using UnityEngine.Purchasing.Security;

public class MyIAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private IAppleExtensions m_AppleExtensions;
    private IGooglePlayStoreExtensions m_GoogleExtensions;

    // ProductIDs
    public static string GOLD_50 = "gold50";
    public static string NONCONSUMABLE1 = "nonconsume1";
    public static string WEEKLYSUB = "weeklysub";
   
    public Text myText;

    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing, can use button click instead
            //InitializePurchasing(); 

            InitializePurchasing();
            var a = m_StoreController.products.all;
            foreach (var product in a)
            {
                var b = product.metadata.localizedPrice;
                Debug.Log(b);
            }
        }
    }

    public void MyInitialize()
    {
        InitializePurchasing();
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
        ProductMetadata tom = new ProductMetadata("12$", "tom", "i love", "dollar", 12);

        builder.AddProduct(NONCONSUMABLE1, ProductType.NonConsumable);
        builder.AddProduct(GOLD_50, ProductType.Consumable);
        builder.AddProduct(WEEKLYSUB, ProductType.Subscription);

        MyDebug("Starting Initialized...");
        UnityPurchasing.Initialize(this, builder);

        Debug.Log(m_StoreController == null);
    }
    
    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuySubscription()
    {
        BuyProductID(WEEKLYSUB);
    }
    
    public void BuyGold50()
    {
        BuyProductID(GOLD_50);
    }

    public void BuyNonConsumable()
    {
        BuyProductID(NONCONSUMABLE1);
    }
    
    public void RestorePurchases()
    {
        m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result => {
            if (result)
            {
                MyDebug("Restore purchases succeeded.");
            }
            else
            {
                MyDebug("Restore purchases failed.");
            }
        });
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            UnityEngine.Purchasing.Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                MyDebug(string.Format("Purchasing product:" + product.definition.id.ToString()));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                MyDebug("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            MyDebug("BuyProductID FAIL. Not initialized.");
        }
    }

    public void ListProducts()
    {
        foreach (UnityEngine.Purchasing.Product item in m_StoreController.products.all)
        {
            if (item.receipt != null)
            {
                MyDebug("Receipt found for Product = " + item.definition.id.ToString());
            }
        }
    }
    
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        MyDebug("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        m_GoogleExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

        Dictionary<string, string> dict = m_AppleExtensions.GetIntroductoryPriceDictionary();

        foreach (UnityEngine.Purchasing.Product item in controller.products.all)
        {
            if (item.receipt != null)
            {
                string intro_json = (dict == null || !dict.ContainsKey(item.definition.storeSpecificId)) ? null : dict[item.definition.storeSpecificId];

                if (item.definition.type == ProductType.Subscription)
                {
                    SubscriptionManager p = new SubscriptionManager(item, intro_json);
                    SubscriptionInfo info = p.getSubscriptionInfo();

                    MyDebug("SubInfo: " + info.getProductId().ToString());
                    MyDebug("isSubscribed: " + info.isSubscribed().ToString());
                    MyDebug("isFreeTrial: " + info.isFreeTrial().ToString());
                }
            }
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        MyDebug("OnInitializeFailed InitializationFailureReason:" + error);
    }
    
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        try
        {
            var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
            var result = validator.Validate(args.purchasedProduct.receipt);
            MyDebug("Validate = " + result.ToString());

            foreach (IPurchaseReceipt productReceipt in result)
            {
                MyDebug("Valid receipt for " + productReceipt.productID.ToString());
            }
        }
        catch (Exception e)
        {
            MyDebug("Error is " + e.Message.ToString());
        }

        MyDebug(string.Format("ProcessPurchase: " + args.purchasedProduct.definition.id));

        return PurchaseProcessingResult.Complete;
    }
    
    public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
    {
        MyDebug(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
    
    private void MyDebug(string debug)
    {
        Debug.Log(debug);
        //myText.text += "\r\n" + debug;
    }
}
