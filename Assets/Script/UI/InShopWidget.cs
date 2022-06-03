using System.Globalization;
using Script.IAP;
using Script.Repositories;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class InShopWidget : MonoBehaviour, IItemRenderer<InAppDefinition>
    {
        [SerializeField] private Text _name;
        [SerializeField] private Image _image;
        [SerializeField] private Text _price;
        
        [SerializeField] private Image _purchased;
        [SerializeField] private Button _button;
        [SerializeField] private IAPManager _iapManager;
        
        private InAppDefinition _data;

        private void OnEnable()
        {
            IAPManager.ProductSold += OnProductSold;
        }

        private void Start()
        {
            UpdateWidget();
            OnProductSold();
        }
        
        private void OnDisable()
        {
            IAPManager.ProductSold -= OnProductSold;
        }

        public void SetDataInWidget(InAppDefinition localInfo)
        {
            _data = localInfo;
            UpdateWidget();
        }

        public void OnSelect()
        {
            _iapManager.BuyProduct(_data.Name);
        }

        private void UpdateWidget()
        {
            _image.sprite = _data.Image;
            _name.text = _data.Name;

            float priceFromStore = _iapManager.GetPrice(_data.Name);
            _price.text = priceFromStore == 0f ? _data.Price.ToString(CultureInfo.InvariantCulture) : priceFromStore.ToString(CultureInfo.InvariantCulture);
        }

        private void OnProductSold()
        {
            if (_iapManager.IsProductPurchased(_data.Name))
            {
                _purchased.gameObject.SetActive(true);
                _button.interactable = false;
            }
        }
    }
}