using AlkemyChallenge.DTOs;
using AlkemyChallenge.Entities;
using AlkemyChallenge.Helpers;
using AlkemyChallenge.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlkemyChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController: CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorage fileStorage;
        private readonly string container = "movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileStorage fileStorage): base(context,mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorage = fileStorage;
        }


        [HttpGet]
        public async Task<ActionResult<List<MovieGetDTO>>> Filter([FromQuery] MovieFilterDTO movieFilterDTO)
        {
            var moviesQueryable = context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(movieFilterDTO.Title))
            {
                moviesQueryable = moviesQueryable.Where(x => x.Title.Contains(movieFilterDTO.Title));
            }

            if(movieFilterDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.MoviesGenres.Select(y => y.GenreId)
                    .Contains(movieFilterDTO.GenreId));
            }

            if (!string.IsNullOrEmpty(movieFilterDTO.Order))
            {
                if(movieFilterDTO.Order == "ASC" || movieFilterDTO.Order == "asc")
                {
                    moviesQueryable = moviesQueryable.OrderBy(x => x.Title);
                }
                else if (movieFilterDTO.Order == "DESC" || movieFilterDTO.Order == "desc")
                {
                    moviesQueryable = moviesQueryable.OrderByDescending(x => x.Title);
                }
            }

            await HttpContext.InsertPaginationParameters(moviesQueryable, movieFilterDTO.NumberOfEntrysPerPage);

            var movies = await moviesQueryable.Pagination(movieFilterDTO.Pagination).ToListAsync();

            return mapper.Map<List<MovieGetDTO>>(movies);

        }

        [HttpGet("{id:int}", Name = "getMovieById")]
        public async Task<ActionResult<MovieDetailsDTO>> GetById(int id)
        {
            var movies = await context.Movies
                .Include(x => x.MoviesCharacters)
                    .ThenInclude(x => x.Character)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(movies == null)
                return NotFound();

            movies.MoviesCharacters = movies.MoviesCharacters.OrderBy(x => x.Order).ToList();

            return mapper.Map<MovieDetailsDTO>(movies);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movie = mapper.Map<Movie>(movieCreationDTO);

            if (movieCreationDTO.Image != null)
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreationDTO.Image.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDTO.Image.FileName);
                    movie.Image = await fileStorage.SaveFile(content, extension, container, movieCreationDTO.Image.ContentType);
                }

            AssignCharacterOrder(movie);
            context.Add(movie);
            await context.SaveChangesAsync();

            var movieDTO = mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("getMovieById", new { id = movie.Id }, movieDTO);
        }

        private void AssignCharacterOrder(Movie movie)
        {
            if (movie.MoviesCharacters != null)
            {
                for (int i = 0; i < movie.MoviesCharacters.Count; i++)
                {
                    movie.MoviesCharacters[i].Order = i;
                }
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movieDB = await context.Movies
                .Include(x => x.MoviesCharacters)
                .Include(x => x.MoviesGenres)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movieDB == null)
                return NotFound();

            movieDB = mapper.Map(movieCreationDTO, movieDB);

            if (movieCreationDTO.Image != null)
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreationDTO.Image.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDTO.Image.FileName);
                    movieDB.Image = await fileStorage.EditFile(content, extension, container, movieDB.Image, movieCreationDTO.Image.ContentType);
                }

            AssignCharacterOrder(movieDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {
            return await Patch<Movie, MoviePatchDTO>(id, patchDocument);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCharacter(int id)
        {
            return await Delete<Movie>(id);
        }
    }
}
