using Microsoft.EntityFrameworkCore;
using RobberLanguageAPI.Models;

namespace RobberLanguageAPI.Data
{
    public class RubberTranslationDBContext : DbContext
    {

        public RubberTranslationDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Translation> Translations { get; set; }
    }
}
