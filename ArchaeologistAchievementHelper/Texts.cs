namespace ArchaeologistAchievementHelper
{
    internal class Texts
    {
        internal enum Text
        {
            MissingFact,
            MissingSubentry,
            MissingEntry,
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
                        case Text.MissingEntry:
                            translatedText = "ENTRADA FALTANTE";
                            break;
                    }
                    break;
                // TODO: fix Russian translation
                // case TextTranslation.Language.RUSSIAN:
                //     switch (text)
                //     {
                //         case Text.MissingFact:
                //             translatedText = "ПРОПУЩЕНА ЗАПИСЬ";
                //             break;
                //         case Text.MissingSubentry:
                //             translatedText = "ПРОПУЩЕНА СВЯЗЬ";
                //             break;
                //     }
                //     break;
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
                        case Text.MissingEntry:
                            translatedText = "MISSING ENTRY";
                            break;
                    }
                    break;
            }
            return translatedText;
        }

    }
}
