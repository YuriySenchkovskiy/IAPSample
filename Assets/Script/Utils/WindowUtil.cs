using UnityEngine;

namespace Script.Utils
{
    public static class WindowUtil
    {
        private const string Tag = "MainCanvas";
        public static void CreateWindow(string resourcePath)
        {
            var window = Resources.Load<GameObject>(resourcePath);
            var canvas = GameObject.FindWithTag(Tag).GetComponent<Canvas>();
            Object.Instantiate(window, canvas.transform);
        }
    }
}