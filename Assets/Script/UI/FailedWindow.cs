using TMPro;
using UnityEngine;

namespace Script.UI
{
    public class FailedWindow : IAPAnimateWindowController
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _error;
        [SerializeField] private string _message;

        private void OnEnable()
        {
            IAPWindowController.FailedCreate += OnFailedCreate;
        }

        protected override void Start()
        {
            base.Start();
            _text.text = _message;
        }
        
        private void OnDisable()
        {
            IAPWindowController.FailedCreate -= OnFailedCreate;
        }

        public void BackToMainMenu()
        {
            Debug.Log("add some logic here to load game menu scene");
        }

        private void OnFailedCreate(string error)
        {
            _error.text += error;
        }
    }
}