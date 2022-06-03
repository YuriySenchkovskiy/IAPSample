using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class FailedWindow : AnimatedWindowController
    {
        [SerializeField] private Text _text;
        [SerializeField] private string _message;

        protected override void Start()
        {
            base.Start();
            _text.text = _message;
        }

        public void BackToMainMenu()
        {
            // логика загрузки главного меню по нажатию на кнопку
            Debug.Log("Now it's empty button. Add some logic");
        }
    }
}