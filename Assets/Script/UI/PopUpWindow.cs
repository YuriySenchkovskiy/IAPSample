using System.Globalization;
using Script.IAP;
using Script.Repositories;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class PopUpWindow : IAPAnimateWindowController
    {
        [SerializeField] private Text _name;
        [SerializeField] private Image _image;
        [SerializeField] private Text _price;
        [SerializeField] private Button _button;
        [SerializeField] private IAPManager _iapManager;
        
        private InAppDefinition _data;
        
        private void OnEnable()
        {
            IAPWindowController.PopUpCreate += OnPopUpCreate;
            IAPManager.PurchaseSuccess += OnPurchaseSuccess;
        }

        protected override void Start()
        {
            base.Start();
            OnPurchaseSuccess();
        }

        private void OnDisable()
        {
            IAPWindowController.PopUpCreate -= OnPopUpCreate;
            IAPManager.PurchaseSuccess -= OnPurchaseSuccess;
        }
        
        public void OnSelect()
        {
            _iapManager.BuyProduct(_data.ProductID);
        }

        private void OnPopUpCreate(string productName)
        {
            _data = InAppRepository.I.GetData(productName);
            
            _image.sprite = _data.Image;
            _name.text = _data.Name;

            float priceFromStore = _iapManager.GetPrice(_data.ProductID);
            _price.text = priceFromStore == 0f ? _data.Price.ToString(CultureInfo.InvariantCulture) : priceFromStore.ToString(CultureInfo.InvariantCulture);
        }
        
        private void OnPurchaseSuccess()
        {
            if (_iapManager.IsProductPurchased(_data.Name))
            {
                _button.interactable = false;
                Close();
            }
        }
    }
}