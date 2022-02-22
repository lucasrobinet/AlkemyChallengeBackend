using AlkemyChallenge.Helpers;
using AlkemyChallenge.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AlkemyChallenge.DTOs
{
    public class MovieCreationDTO: MoviePatchDTO
    {
        [FileSizeValidation(4)]
        [TypeFileValidation(GroupFileType.Image)]
        public IFormFile Image { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder))]
        public List<int> GenreIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder))]
        public List<int> CharacterIds { get; set; }

    }
}
