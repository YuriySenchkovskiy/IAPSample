using System;
using System.Collections.Generic;

namespace Script.IAP
{
    [Serializable]
    public class IAPDefinition
    {
        private string _productId;
        private string _identifier;

        public string ProductID => _productId;
        public string Identifier => _identifier;

        public IAPDefinition(string productId, string identifier)
        {
            _productId = productId;
            _identifier = identifier;
        }
    }

    [Serializable]
    public class IAPBase
    {
        private readonly List<IAPDefinition> _definitions = new List<IAPDefinition>();

        public List<IAPDefinition> Definitions => _definitions;

        public void AddDefinition(IAPDefinition definition)
        {
            _definitions.Add(definition);
        }
    }
}