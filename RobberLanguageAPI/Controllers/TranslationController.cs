using Microsoft.AspNetCore.Mvc;
using RobberLanguageAPI.Models;

namespace RobberLanguageAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class TranslationController : ControllerBase
    {

        [HttpPost]
        [Route("RobberLanguage")]
        public Translation PostTranslation(Translation sentence)
        {
            var translation = new Translation
            {
                OriginalSentance = sentence.OriginalSentance,
                TranslatedSentance = $"{TranslateSentence(sentence.OriginalSentance)}"
            };

            return translation;

        }

        private static string TranslateSentence(string sentence)
        {

            string robberSentence = "";

            for (int i = 0; i < sentence.Length; i++)
            {
                if (sentence[i] == 'a' || sentence[i] == 'e' ||
                    sentence[i] == 'i' || sentence[i] == 'o' ||
                    sentence[i] == 'u' || sentence[i] == 'å' ||
                    sentence[i] == 'ä' || sentence[i] == 'ö')
                {
                    robberSentence += sentence[i];
                }
                else if (sentence[i] == ' ')
                {
                    robberSentence += sentence[i];
                }
                else
                {
                    robberSentence = robberSentence + sentence[i] + 'o' + sentence[i];
                }
            }

            return robberSentence;
        }




    }
}
