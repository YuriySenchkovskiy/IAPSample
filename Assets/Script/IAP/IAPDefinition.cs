using System;

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
}