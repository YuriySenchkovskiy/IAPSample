using System;

namespace Script.IAP
{
    [Serializable]
    public struct IAPDefinition
    {
        private readonly string _productId;
        private readonly string _identifier;

        public string ProductID => _productId;
        public string Identifier => _identifier;
        
        public IAPDefinition(string productId, string identifier)
        {
            _productId = productId;
            _identifier = identifier;
        }
    }
}