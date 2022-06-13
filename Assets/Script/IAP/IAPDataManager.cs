using System.Collections.Generic;
using System.IO;
using Script.Repositories;
using Script.Utils;
using Script.Utils.Identifier;
using UnityEngine;

namespace Script.IAP
{
    public static class IAPDataManager
    {
        private static readonly string _filePath = Application.persistentDataPath + "/player.iap";
        private static readonly byte[] _keyBytes =
        {
            26, 98, 14, 207, 216, 177, 72, 129, 149, 249, 62, 164, 175, 79, 177, 123, 
            235, 61, 199, 81, 235, 155, 174, 43, 93, 93, 105, 109, 26, 146, 118, 123 
        };

        public static void SaveID(string id)
        {
            AesUtil aes = new AesUtil();
            IAPDefinition iapDefinition = new IAPDefinition(id, UniqueIdentifier.Number);

            var iapBase = GetAllData();
            iapBase.AddDefinition(iapDefinition);
            var binary = BinaryUtil.SerializeObject(iapBase);
            var aesData = aes.Encrypt(binary, _keyBytes);
            
            File.WriteAllBytes(_filePath, aesData);
        }

        public static bool HasID(string name)
        {
            var iapBase = GetAllData();
            return GetIdStatus(iapBase?.Definitions, name);;
        }

        public static void DeleteAll()
        {
            File.Delete(_filePath);
        }

        private static IAPBase GetAllData()
        {
            if (File.Exists(_filePath))
            {
                AesUtil aes = new AesUtil();
                var aesData = File.ReadAllBytes(_filePath);
                var binary = aes.Decrypt(aesData, _keyBytes);
                
                return BinaryUtil.DeserializeObject(binary) as IAPBase;
            }

            return new IAPBase();
        }

        private static bool GetIdStatus(List<IAPDefinition> definitions, string name)
        {
            var id = InAppRepository.I.GetID(name);

            foreach (var definition in definitions)
            {
                if (definition.ProductID == id && definition.Identifier == UniqueIdentifier.Number)
                {
                    return true;
                }
            }

            return false;
        }
    }
}