using UnityEngine.Purchasing.Security;

namespace Script.Generated
{
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("iRRODmQyqmObd+2UDfsiZvVTvuiKNkMmw1nUqp3X5veOQjTIrLXady0Mhv+AJtQle/bJ1MhtZSI9EZYBcsBDYHJPREtox");
        private static int[] order = new int[] { 12,4,3,12,6,12,13,10,13,13,10,13,12,13,14 };
        private static int key = 66;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
            if (IsPopulated == false)
                return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}