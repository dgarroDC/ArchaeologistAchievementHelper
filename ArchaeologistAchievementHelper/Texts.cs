namespace ArchaeologistAchievementHelper
{
    internal class Texts
    {
        internal enum Text
        {
            MissingFact,
            MissingSubentry
        }

        internal static string GetTranslated(Text text)
        {
            TextTranslation.Language language = TextTranslation.Get().GetLanguage();
            string translatedText = "UNKNOWN TEXT";
            switch (language)
            {
                case TextTranslation.Language.SPANISH_LA:
                    switch (text)
                    {
                        case Text.MissingFact:
                            translatedText = "DATO FALTANTE";
                            break;
                        case Text.MissingSubentry:
                            translatedText = "SUBENTRADA FALTANTE";
                            break;
                    }
                    break;
                case TextTranslation.Language.RUSSIAN:
                    switch (text)
                    {
                        case Text.MissingFact:
                            translatedText = "ПРОПУЩЕНА ЗАПИСЬ";
                            break;
                        case Text.MissingSubentry:
                            translatedText = "ПРОПУЩЕНА СВЯЗЬ";
                            break;
                    }
                    break;
                case TextTranslation.Language.ENGLISH:
                default:
                    switch (text)
                    {
                        case Text.MissingFact:
                            translatedText = "MISSING FACT";
                            break;
                        case Text.MissingSubentry:
                            translatedText = "MISSING SUBENTRY";
                            break;
                    }
                    break;
            }
            return translatedText;
        }

    }
}
