using System;
using UnityEngine;

namespace Script.Utils.Identifier
{
    public class UniqueIdentifier : MonoBehaviour
    {
        private static string _number;
        private string _exist = "exist";

        public static string Number => _number;

        private void Awake()
        {
            _number = GetIdentifier();
            
            if (string.IsNullOrEmpty(_number))
            {
                AddIdentifier();
                _number = GetIdentifier();
            }
        }

        private string GetIdentifier()
        {
            return PlayerPrefs.HasKey(_exist) ? 
                PlayerPrefs.GetString(_exist) != SystemInfo.unsupportedIdentifier ?
                    PlayerPrefs.GetString(_exist) : null : null;
        }
        
        private void AddIdentifier()
        {
            var identifier = SystemInfo.deviceUniqueIdentifier;
            PlayerPrefs.SetString(_exist, identifier);
        }
    }
}