using System.Text.RegularExpressions;

namespace EmployeeAPI.Application.Services
{
    public interface ITranslationService
    {
        Task<string> TranslateAsync(string text, string languageCode);
    }

    public class TranslationService : ITranslationService
    {
        private static readonly Dictionary<string, Dictionary<string, string>> LanguageMap =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["hi"] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["employee"] = "कर्मचारी",
                    ["employees"] = "कर्मचारियों",
                    ["not found"] = "नहीं मिला",
                    ["not found."] = "नहीं मिला।",
                    ["created"] = "बनाया गया",
                    ["updated"] = "अपडेट किया गया",
                    ["deleted"] = "हटाया गया",
                    ["success"] = "सफलता",
                    ["error"] = "त्रुटि",
                    ["an unexpected error occurred."] = "एक अनपेक्षित त्रुटि हुई।",
                    ["bad request"] = "खराब अनुरोध",
                    ["unauthorized"] = "अनधिकृत",
                    ["forbidden"] = "वर्जित",
                    ["conflict"] = "संघर्ष",
                    ["internal server error"] = "आंतरिक सर्वर त्रुटि",
                    ["welcome"] = "स्वागत है",
                    ["hello"] = "नमस्ते"
                },
                ["fr"] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["employee"] = "employé",
                    ["employees"] = "employés",
                    ["not found"] = "introuvable",
                    ["created"] = "créé",
                    ["updated"] = "mis à jour",
                    ["deleted"] = "supprimé",
                    ["success"] = "succès",
                    ["error"] = "erreur",
                    ["welcome"] = "bienvenue",
                    ["hello"] = "bonjour"
                },
                ["ar"] = new(StringComparer.OrdinalIgnoreCase)
                {
                    ["employee"] = "موظف",
                    ["employees"] = "موظفين",
                    ["not found"] = "غير موجود",
                    ["created"] = "تم الإنشاء",
                    ["updated"] = "تم التحديث",
                    ["deleted"] = "تم الحذف",
                    ["success"] = "نجاح",
                    ["error"] = "خطأ",
                    ["welcome"] = "أهلاً وسهلاً",
                    ["hello"] = "مرحبا"
                }
            };

        public Task<string> TranslateAsync(string text, string languageCode)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Task.FromResult(text);
            }

            var targetLanguage = NormalizeLanguage(languageCode);
            if (string.IsNullOrWhiteSpace(targetLanguage) || targetLanguage.Equals("en", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(text);
            }

            if (!LanguageMap.TryGetValue(targetLanguage, out var replacements))
            {
                return Task.FromResult(text);
            }

            var translated = text;
            foreach (var entry in replacements)
            {
                translated = Regex.Replace(
                    translated,
                    $"\\b{Regex.Escape(entry.Key)}\\b",
                    entry.Value,
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }

            return Task.FromResult(translated);
        }

        private static string NormalizeLanguage(string? languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                return "en";
            }

            var language = languageCode.Split('-', StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            return language.ToLowerInvariant();
        }
    }
}
