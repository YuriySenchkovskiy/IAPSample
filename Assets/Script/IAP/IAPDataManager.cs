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
        private static readonly string _filePath;
        private static readonly string _key;

        static IAPDataManager()
        {
            _filePath = Application.persistentDataPath + "/player.iap";
            _key = "Ijustsayingyoucanthaveitbothways";
        }
        
        public static void SaveID(string id)
        {
            IAPDefinition iapDefinition = new IAPDefinition(id, UniqueIdentifier.Number);
            var binary = BinaryUtil.SerializeObject(iapDefinition);
            AesUtil aes = new AesUtil();
            var rawData = aes.Encrypt(binary, _key) + "\n";
            
            File.AppendAllText(_filePath, rawData);
        }

        public static bool HasID(string name)
        {
            var status = false;
            List<IAPDefinition> definitions = new List<IAPDefinition>();

            if (File.Exists(_filePath))
            {
                var allProducts = File.ReadAllLines(_filePath);

                foreach (var product in allProducts)
                {
                    AesUtil aes = new AesUtil();
                    var binary = aes.Decrypt(product, _key);
                    var definition = BinaryUtil.DeserializeObject(binary) as IAPDefinition;
                    definitions.Add(definition);
                }

                status = GetIdStatus(definitions, name);
            }
            
            return status;
        }

        public static void DeleteAll()
        {
            File.Delete(_filePath);
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