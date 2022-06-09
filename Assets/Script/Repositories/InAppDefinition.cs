using System;
using UnityEngine;

namespace Script.Repositories
{
    [Serializable]
    public struct InAppDefinition
    {
        [SerializeField] private string _productID;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _image;
        [SerializeField] private float _price;
        [SerializeField] private bool _isBundle;
        
        public string ProductID => _productID;
        public string Name => _name;
        public Sprite Image => _image;
        public float Price => _price;
        public bool IsBundle => _isBundle;
    }
}