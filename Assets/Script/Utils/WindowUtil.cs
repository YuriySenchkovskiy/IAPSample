using UnityEngine;

namespace Script.Utils
{
    public static class WindowUtil
    {
        private static string _tag = "MainCanvas";
        public static void CreateWindow(string resourcePath)
        {
            var window = Resources.Load<GameObject>(resourcePath);
            var canvas = GameObject.FindWithTag(_tag).GetComponent<Canvas>();
            Object.Instantiate(window, canvas.transform);
        }
    }
}