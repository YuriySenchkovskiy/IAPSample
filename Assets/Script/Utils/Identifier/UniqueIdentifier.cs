using System;
using UnityEngine;

namespace Script.Utils.Identifier
{
    [Serializable]
    public class UniqueIdentifier : MonoBehaviour
    {
        private const string Exist = "exist";
        private static string _number;

        public static string Number => _number;

        private void Awake()
        {
            _number = GetIdentifier();
            
            if (string.IsNullOrEmpty(_number))
            {
                SetIdentifier();
                _number = GetIdentifier();
            }
        }

        private string GetIdentifier()
        {
            return PlayerPrefs.HasKey(Exist)
                ? PlayerPrefs.GetString(Exist) != SystemInfo.unsupportedIdentifier 
                    ? PlayerPrefs.GetString(Exist) 
                    : null
                : null;
        }
        
        private void SetIdentifier()
        {
            var identifier = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString(Exist, identifier);
        }
    }
}