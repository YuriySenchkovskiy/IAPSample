using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class FailedWindow : IAPAnimateWindowController
    {
        [SerializeField] private Text _text;
        [SerializeField] private string _message;
        [SerializeField] private Text _error;

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
            // логика загрузки главного меню по нажатию на кнопку
            Debug.Log("Now it's empty button. Add some logic");
        }

        private void OnFailedCreate(string error)
        {
            _error.text += error;
        }
    }
}