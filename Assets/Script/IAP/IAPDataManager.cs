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
            var serializeData = JsonUtility.ToJson(iapDefinition);
            Debug.Log(serializeData);

            using (BinaryWriter writer = new BinaryWriter(File.Open(_filePath, FileMode.Append)))
            {
                var aesData = AesUtil.Encrypt(serializeData);
                writer.Write(aesData);
                writer.Close();
            }
        }

        public static bool HasID(string name)
        {
            var status = false;
            
            if (File.Exists(_filePath))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(_filePath, FileMode.Open)))
                {
                    var data = reader.ReadString();
                    var aesData = AesUtil.Decrypt(data);
                    var definitions = JsonUtility.FromJson<List<IAPDefinition>>(aesData);
                    Debug.Log("in has name " + aesData);
                    
                    //status = GetIdStatus(definitions, name);
                    Debug.Log(definitions.Count);
                    reader.Close();
                }
            }
            
            return status;
        }

        public static void DeleteAll()
        {
            File.Delete(_filePath);
        }

        private static bool GetIdStatus(IAPDefinition[] definitions, string name)
        {
            var id = InAppRepository.I.GetID(name);
            Debug.Log(name + " in name");
            Debug.Log(id + " in id");
            Debug.Log(UniqueIdentifier.Number + " in number");
            Debug.Log("///////////////////////");
            Debug.Log(definitions.Length);

            foreach (var definition in definitions)
            {
                Debug.Log(definition.ProductID + " id");
                Debug.Log(definition.Identifier + " number");
                if (definition.ProductID == id && definition.Identifier == UniqueIdentifier.Number)
                {
                    Debug.Log(definition.ProductID);
                    return true;
                }

                Debug.Log("*****");
            }

            return false;
        }
    }
}