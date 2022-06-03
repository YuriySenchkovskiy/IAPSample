using Script.Utils;
using UnityEngine;

namespace Script.UI
{
    public class MenuWindowController : MonoBehaviour
    {
        [SerializeField] private string _shopPath;
        [SerializeField] private string _popUpPath;
    
        public void ShowShop()
        {
            WindowUtil.CreateWindow(_shopPath);
        }

        public void ShowPopUp()
        {
            WindowUtil.CreateWindow(_popUpPath);
        }
    }
}
