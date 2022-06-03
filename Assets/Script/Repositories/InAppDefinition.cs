using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Repositories
{
    [Serializable]
    public struct InAppDefinition
    {
        [SerializeField] private string _productID;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _image;
        [SerializeField] private float _price;
        
        public string ProductID => _productID;
        public string Name => _name;
        public Sprite Image => _image;
        public float Price => _price;
    }
}