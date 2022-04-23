namespace ArchaeologistAchievementHelper
{
    internal class Texts
    {
        internal enum Text
        {
            MissingFact,
            MissingSubentrie
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
                        case Text.MissingSubentrie:
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
                        case Text.MissingSubentrie:
                            translatedText = "ПРОПУЩЕНА СВЯЩЬ";
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
                        case Text.MissingSubentrie:
                            translatedText = "MISSING SUBENTRIE";
                            break;
                    }
                    break;
            }
            return translatedText;
        }

    }
}
