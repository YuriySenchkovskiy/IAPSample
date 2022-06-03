using UnityEngine;

namespace Script.Generated
{
    public class PrefsCleaner : MonoBehaviour
    {
        public void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}