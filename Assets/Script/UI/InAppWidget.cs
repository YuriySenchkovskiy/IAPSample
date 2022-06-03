using System.Globalization;
using Script.Repositories;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class InAppWidget : MonoBehaviour, IItemRenderer<InAppDefinition>
    {
        [SerializeField] private Text _name;
        [SerializeField] private Image _image;
        [SerializeField] private Text _price;
        [SerializeField] private IAPManager _iapManager;

        private InAppDefinition _data;
        

        private void Start()
        {
            UpdateWidget();
        }

        public void SetDataInWidget(InAppDefinition localInfo)
        {
            _data = localInfo;
            UpdateWidget();
        }

        private void UpdateWidget()
        {
            _image.sprite = _data.Image;
            _name.text = _data.Name;

            float priceFromStore = _iapManager.GetPrice(_data.Name);
            _price.text = priceFromStore == 0f ? _data.Price.ToString(CultureInfo.InvariantCulture) : priceFromStore.ToString(CultureInfo.InvariantCulture);
        }
    }
}