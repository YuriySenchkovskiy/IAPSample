using System.Collections.Generic;
using System.Linq;

namespace Script.Utils
{
    public class ErrorDatabase
    {
        private static readonly Dictionary<string, string> _errors = new Dictionary<string, string>()
        {
            {"UnknownError", "001"},
            // ошибки во время инициализации магазина
            {"PurchasingUnavailableOnInitialize", "010"},
            {"NoProductsAvailableOnInitialize", "011"},
            {"AppNotKnownOnInitialize", "012"},
            // ошибки во время инициализации продажи
            {"NullInProduct", "020"},
            // человек зашел в магазин, потом вышел в настройки и отключил возможность покупок, вернулся в магазин
            {"IStoreControllerUnavailable", "021"}, 
            // ошибки во время продажи
            {"PurchasingUnavailable", "030"},
            {"ExistingPurchasePending", "031"},
            {"ProductUnavailable", "032"},
            {"SignatureInvalid", "033"},
            {"UserCancelled", "034"},
            {"PaymentDeclined", "035"},
            {"Unknown", "036"},
            // ошибка восстановления покупок apple
            {"RestoreFailed", "040"}
        };

        public static string GetErrorNumber(string error)
        {
            return _errors.TryGetValue(error, out string value) ? value : _errors.First().Value;
        }
    }
}