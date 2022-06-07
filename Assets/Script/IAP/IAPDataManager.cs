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

        static IAPDataManager()
        {
            _filePath = Application.persistentDataPath + "/player.iap";
        }
        
        public static void SaveID(string id)
        {
            IAPDefinition iapDefinition = new IAPDefinition(id, UniqueIdentifier.Number);
            var binary = BinaryUtil.SerializeObject(iapDefinition) + "\n";
            //var binary = BinaryUtil.SerializeObject(iapDefinition);
            //var aes = AesUtil.Encrypt(binary) + "\n";
            
            //Debug.Log("SAVE BINARY: " + binary);
            //File.AppendAllText(_filePath, aes);
            //Debug.Log("SAVE AES: " + aes);
            
            File.AppendAllText(_filePath, binary);
        }

        public static bool HasID(string name)
        {
            var status = false;
            List<IAPDefinition> definitions = new List<IAPDefinition>();

            if (File.Exists(_filePath))
            {
                var allProducts = File.ReadAllLines(_filePath);

                for (int i = 0; i < allProducts.Length; i++)
                {
                    var product = BinaryUtil.DeserializeObject(allProducts[i]) as IAPDefinition;
                    definitions.Add(product);

                    // Debug.Log($"{i} PRODUCT AES: " + allProducts[i]);
                    // var binary = AesUtil.Decrypt(allProducts[i]);
                    // Debug.Log($"{i} PRODUCT BINARY: " + binary);
                    // var product = BinaryUtil.DeserializeObject(binary) as IAPDefinition;
                    // definitions.Add(product);
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