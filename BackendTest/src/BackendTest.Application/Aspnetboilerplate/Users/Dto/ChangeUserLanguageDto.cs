using System.ComponentModel.DataAnnotations;

namespace BackendTest.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}