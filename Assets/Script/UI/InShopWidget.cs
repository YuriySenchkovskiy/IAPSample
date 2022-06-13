using System.Globalization;
using Script.IAP;
using Script.Repositories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class InShopWidget : MonoBehaviour, IItemRenderer<InAppDefinition>
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private Image _image;

        [SerializeField] private Image _purchased;
        [SerializeField] private Button _button;
        [SerializeField] private IAPManager _iapManager;

        private InAppDefinition _data;

        private void OnEnable()
        {
            IAPManager.PurchaseSuccess += OnPurchaseSuccess;
        }

        private void Start()
        {
            UpdateWidget();
            OnPurchaseSuccess();
        }
        
        private void OnDisable()
        {
            IAPManager.PurchaseSuccess -= OnPurchaseSuccess;
        }
        
        public void SetDataInWidget(InAppDefinition localInfo)
        {
            _data = localInfo;
            UpdateWidget();
        }

        public void OnSelect()
        {
            _iapManager.BuyProduct(_data.ProductID);
        }

        private void UpdateWidget()
        {
            _image.sprite = _data.Image;
            _name.text = _data.Name;

            float priceFromStore = _iapManager.GetPrice(_data.ProductID);
            _price.text = priceFromStore == 0f ? _data.Price.ToString(CultureInfo.InvariantCulture) : priceFromStore.ToString(CultureInfo.InvariantCulture);
        }

        private void OnPurchaseSuccess()
        {
            if (_iapManager.IsProductPurchased(_data.Name))
            {
                _purchased.gameObject.SetActive(true);
                _button.interactable = false;
            }
        }
    }
}