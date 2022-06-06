using Script.IAP;
using UnityEngine;

namespace Script.Test
{
    public class PrefsCleaner : MonoBehaviour
    {
        public void ClearPrefs()
        {
            IAPDataManager.DeleteAll();
        }
    }
}