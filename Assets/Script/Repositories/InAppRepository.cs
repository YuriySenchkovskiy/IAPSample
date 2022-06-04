using UnityEngine;

namespace Script.Repositories
{
    [CreateAssetMenu(fileName = "Definition/InApp", menuName = "InApp", order = 51)]
    public class InAppRepository : ScriptableObject
    {
        [SerializeField] private InAppDefinition[] _collection;
        
        public static InAppRepository I => _instance == null ? LoadDefinitions() : _instance;
        private static InAppRepository _instance;
        
        public InAppDefinition[] Collection => _collection;

        public string GetID(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return default;
            }
            
            foreach (var definition in _collection)
            {
                if (definition.Name == name)
                {
                    return definition.ProductID;
                }
            }

            return default;
        }

        public InAppDefinition GetData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return default;
            }
            
            foreach (var definition in _collection)
            {
                if (definition.Name == name)
                {
                    return definition;
                }
            }
            
            return default;
        }
        
#if UNITY_EDITOR
        public InAppDefinition[] ItemsForEditor => _collection;
#endif
        
        private static InAppRepository LoadDefinitions()
        {
            return _instance = Resources.Load<InAppRepository>("InAppDef");
        }
    }
}