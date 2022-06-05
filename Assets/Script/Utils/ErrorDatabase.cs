using System.Collections.Generic;
using System.Linq;

namespace Script.Utils
{
    public class ErrorDatabase
    {
        private static readonly Dictionary<string, string> _errors = new Dictionary<string, string>()
        {
            // неучтенная ошибка
            {"001", "UnknownError"},
            // ошибки во время инициализации магазина
            {"010", "PurchasingUnavailableOnInitialize"},
            {"011", "NoProductsAvailableOnInitialize"},
            {"012", "AppNotKnownOnInitialize"},
            // ошибки во время инициализации продажи
            {"020", "NullInProduct"},
            // человек зашел в магазин, потом вышел в настройки и отключил возможность покупок, вернулся в магазин
            {"021", "IStoreControllerUnavailable"}, 
            // ошибки во время продажи
            {"030", "PurchasingUnavailable"},
            {"031", "ExistingPurchasePending"},
            {"032", "ProductUnavailable"},
            {"033", "SignatureInvalid"},
            {"034", "UserCancelled"},
            {"035", "PaymentDeclined"},
            {"036", "Unknown"},
            // ошибка восстановления покупок apple
            {"040", "RestoreFailed"}
        };

        public static string GetErrorNumber(string error)
        {
            foreach (var err in _errors)
            {
                if (err.Value == error)
                {
                    return err.Key;
                }
            }

            return _errors.First().Key;
        }
    }
}