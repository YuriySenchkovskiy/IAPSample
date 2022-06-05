using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            IAPDefinition iapDefinition = new IAPDefinition(id, UniqueIdentifier.Number);
            
            using (FileStream stream = new FileStream(_filePath, FileMode.Append))
            {
                binaryFormatter.Serialize(stream, iapDefinition);
                stream.Close();
            }
        }

        public static bool HasID(string id)
        {
            if (File.Exists(_filePath))
            {
                
            }
            else
            {
                
            }
            // разобрать файл до строк
            // найти нужную строку
            // убедиться, что идентификатор сходится и вернуть true
            return false;
        }

        public static void DeleteAll()
        {
            File.Delete(_filePath);
        }
    }
}