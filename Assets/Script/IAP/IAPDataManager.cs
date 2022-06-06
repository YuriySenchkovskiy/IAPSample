using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
            IAPDefinition[] iapDefinition = {new IAPDefinition(id, UniqueIdentifier.Number)};
            var jsonData = JsonUtil.ToJson(iapDefinition,true);
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(_filePath, FileMode.Append))
            {
                var aesData = AesUtil.Encrypt(jsonData);
                formatter.Serialize(stream, aesData);
                stream.Close();
            }
        }

        public static bool HasID(string name)
        {
            var status = false;
            
            if (File.Exists(_filePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(_filePath, FileMode.Open))
                {
                    var aesData = (string)formatter.Deserialize(stream);
                    var jsonData = AesUtil.Decrypt(aesData);
                    var definitions = JsonUtil.FromJson<IAPDefinition>(jsonData);
                    status = GetIdStatus(definitions, name);
                    stream.Close();
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