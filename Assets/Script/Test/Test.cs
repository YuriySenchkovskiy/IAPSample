using System;
using Script.IAP;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Script.Test
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            IAPDataManager.DeleteAll();
            IAPDataManager.SaveID("com.yuriy.testtemplate.firstProduct");
            
            Debug.Log(IAPDataManager.HasID("first"));
            //IAPDataManager.DeleteAll();
            //IAPDataManager.HasID("asdfasdf");
        }
    }
}