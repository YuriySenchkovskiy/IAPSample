using System.Collections.Generic;
using UnityEngine;

namespace Script.Repositories
{
    [CreateAssetMenu(fileName = FileName, menuName = "Definition/InApp", order = 51)]
    public class InAppRepository : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private InAppDefinition[] _collection;
        
        private const string FileName = "InApp";
        private static InAppRepository _instance;
        private readonly Dictionary<string, InAppDefinition> _nameDefinitions = new Dictionary<string, InAppDefinition>();
        private readonly Dictionary<string, InAppDefinition> _bundleDefinitions = new Dictionary<string, InAppDefinition>();

        public static InAppRepository I => _instance == null ? LoadDefinitions() : _instance;
        public InAppDefinition[] Collection => _collection;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _nameDefinitions.Clear();
            _bundleDefinitions.Clear();
            
            foreach (var definition in _collection)
            {
                _nameDefinitions.Add(definition.Name, definition);
                _bundleDefinitions.Add(definition.ProductID, definition);
            }
        }
        
        public string GetID(string name)
        {
            return _nameDefinitions.TryGetValue(name, out InAppDefinition def) ? def.ProductID : default;
        }

        public InAppDefinition GetData(string name)
        {
           return _nameDefinitions.TryGetValue(name, out InAppDefinition def) ? def : default;
        }

        public bool GetBundleStatus(string id)
        {
            return _bundleDefinitions.TryGetValue(id, out InAppDefinition def) ? def.IsBundle : default;
        }

#if UNITY_EDITOR
        public InAppDefinition[] ItemsForEditor => _collection;
#endif
        
        private static InAppRepository LoadDefinitions()
        {
            return _instance = Resources.Load<InAppRepository>(FileName);
        }
    }
}