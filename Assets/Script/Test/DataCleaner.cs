using Script.IAP;
using UnityEngine;

namespace Script.Test
{
    public class DataCleaner : MonoBehaviour
    {
        public void ClearData()
        {
            IAPDataManager.DeleteAll();
        }
    }
}