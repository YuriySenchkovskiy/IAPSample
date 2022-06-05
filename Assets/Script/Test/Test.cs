using System;
using Script.IAP;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Script.Test
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            IAPDataManager.DeleteAll();
            IAPDataManager.SaveID("asdfasdf");
            IAPDataManager.HasID("asdfasdf");
            //IAPDataManager.DeleteAll();
            // IAPDataManager.HasID("asdfasdf");
        }
    }
}