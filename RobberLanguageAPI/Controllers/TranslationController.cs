using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RobberLanguageAPI.Data;
using RobberLanguageAPI.Models;

namespace RobberLanguageAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly RubberTranslationDBContext _context;


        public TranslationController(RubberTranslationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("RobberLanguageTranslation")]
        public async Task<ActionResult<Translation>> PostTranslation(Translation sentence)
        {

            if (string.IsNullOrWhiteSpace(sentence.OriginalSentance))
            {
                return BadRequest();
            }

            var translation = new Translation
            {
                OriginalSentance = sentence.OriginalSentance,
                TranslatedSentance = $"{TranslateSentence(sentence.OriginalSentance)}",
                CreationDate = sentence.CreationDate,
                ModificationDate = sentence.ModificationDate
            };

            await _context.AddAsync(translation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTranslation), new { id = translation.Id }, translation);

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("GetTranslation")]
        public async Task<ActionResult<Translation>> GetTranslation(int id)
        {
            Translation? translation =
                await _context.Translations.FindAsync(id);

            if (translation is null)
            {
                return NotFound();
            }


            return translation;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("GetTranslations")]
        public async Task<ActionResult<IEnumerable<Translation>>> GetTranslations()
        {
            return await _context.Translations.ToListAsync();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("GetTranslationsFromQuery")]
        public async Task<ActionResult<IEnumerable<Translation>>> GetTranslationsFromQuery([FromQuery] string? keyword)
        {

            if (keyword != null && !(string.IsNullOrWhiteSpace(keyword)))
            {
                return await _context.Translations
                    .Where(t => t.OriginalSentance.Contains(keyword))
                    .ToListAsync();
            }

            return BadRequest();

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Route("UpdateTranslation")]
        public async Task<ActionResult<Translation>> UpdateTranslationValues(int id, Translation translation)
        {

            if (id != translation.Id)
            {
                return BadRequest();
            }

            var thisTranslation = await
                _context.Translations.FirstOrDefaultAsync(t => t.Id == id);

            thisTranslation.OriginalSentance = translation.OriginalSentance;
            thisTranslation.TranslatedSentance =
                TranslateSentence(translation.OriginalSentance);
            thisTranslation.ModificationDate =
                translation.ModificationDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TranslationExists(id))
                {
                    return NotFound();
                }

                throw;
            }
            return NoContent();
        }

        private bool TranslationExists(int id) =>
            _context.Translations.Any(e => e.Id == id);


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTranslation(int id)
        {
            var thisTranslation =
                await _context.Translations.FindAsync(id);

            if (thisTranslation is null)
            {
                return NotFound();
            }

            _context.Translations.Remove(thisTranslation);
            await _context.SaveChangesAsync();

            return NoContent();
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
