using System;

namespace Script.IAP
{
    [Serializable]
    public struct IAPDefinition
    {
        public string ProductID;
        public string Identifier;

        public IAPDefinition(string productId, string identifier)
        {
            ProductID = productId;
            Identifier = identifier;
        }
    }
}