using AlkemyChallenge.DTOs;
using AlkemyChallenge.Entities;
using AlkemyChallenge.Helpers;
using AlkemyChallenge.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlkemyChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsUser")]
    public class CharactersController: CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;
        private readonly string container = "characters";

        public CharactersController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage): base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<CharacterGetDTO>>> Filter([FromQuery] CharacterFilterDTO  characterFilterDTO)
        {
            var characterQueryable = context.Characters.AsQueryable();

            if (!string.IsNullOrEmpty(characterFilterDTO.Name))
            {
                characterQueryable = characterQueryable.Where(x => x.Name.Contains(characterFilterDTO.Name));
            }

            if (characterFilterDTO.Age != null)
            {
                characterQueryable = characterQueryable.Where(x => x.Age == characterFilterDTO.Age);
            }

            if (characterFilterDTO.MovieId != 0)
            {
                characterQueryable = characterQueryable
                    .Where(x => x.MoviesCharacters.Select(y => y.MovieId)
                    .Contains(characterFilterDTO.MovieId));
            }

            await HttpContext.InsertPaginationParameters(characterQueryable, characterFilterDTO.NumberOfEntrysPerPage);

            var character = await characterQueryable.Pagination(characterFilterDTO.Pagination).ToListAsync();

            return mapper.Map<List<CharacterGetDTO>>(character);

        }

        [HttpGet("{id:int}", Name = "getCharacterById")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CharacterDTO>> GetCharacterById(int id)
        {
            return await Get<Character, CharacterDTO>(id);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> CreateCharacter([FromForm] CharacterCreationDTO characterCreationDTO)
        {
            var entity = mapper.Map<Character>(characterCreationDTO);

            if (characterCreationDTO.Image != null)
                using(var memoryStream = new MemoryStream())
                {
                    await characterCreationDTO.Image.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(characterCreationDTO.Image.FileName);
                    entity.Image = await fileStorage.SaveFile(content, extension, container, characterCreationDTO.Image.ContentType);
                }

            context.Add(entity);
            await context.SaveChangesAsync();

            var dto = mapper.Map<CharacterDTO>(entity);
            return new CreatedAtRouteResult("getCharacterById", new { id = entity.Id }, dto);
        }

        

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> EditCharacter(int id, [FromForm] CharacterCreationDTO characterCreationDTO)
        {
            var characterDB = await context.Characters.FirstOrDefaultAsync(x => x.Id == id);

            if (characterDB == null)
                return NotFound();

            characterDB = mapper.Map(characterCreationDTO, characterDB);

            if (characterCreationDTO.Image != null)
                using (var memoryStream = new MemoryStream())
                {
                    await characterCreationDTO.Image.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(characterCreationDTO.Image.FileName);
                    characterDB.Image = await fileStorage.EditFile(content, extension, container, characterDB.Image, characterCreationDTO.Image.ContentType);
                }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<CharacterPatchDTO> patchDocument)
        {
            return await Patch<Character, CharacterPatchDTO>(id, patchDocument);

        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteCharacter(int id)
        {
            return await Delete<Character>(id);
        }

    }
}
