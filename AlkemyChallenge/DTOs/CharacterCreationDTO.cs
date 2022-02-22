using AlkemyChallenge.Validations;
using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.DTOs
{
    public class CharacterCreationDTO: CharacterPatchDTO
    {
        [FileSizeValidation(4)]
        [TypeFileValidation(GroupFileType.Image)]
        public IFormFile Image { get; set; }
    }
}
