using UnityEngine;

namespace Script.Test
{
    public class PrefsCleaner : MonoBehaviour
    {
        public void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}