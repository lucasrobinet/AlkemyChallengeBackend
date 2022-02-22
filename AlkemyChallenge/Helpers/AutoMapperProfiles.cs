using AlkemyChallenge.DTOs;
using AlkemyChallenge.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AlkemyChallenge.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();

            CreateMap<IdentityUser, UserDTO>().ReverseMap();

            CreateMap<Character, CharacterDTO>().ReverseMap();
            CreateMap<CharacterCreationDTO, Character>()
                .ForMember(x => x.Image, options => options.Ignore());
            CreateMap<CharacterPatchDTO, Character>().ReverseMap();
            CreateMap<Character, CharacterGetDTO>().ReverseMap();

            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<Movie, MovieGetDTO>().ReverseMap();
            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.Image, options => options.Ignore())
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(MoviesGenresMap))
                .ForMember(x => x.MoviesCharacters, options => options.MapFrom(MoviesCharactersMap));

            CreateMap<Movie, MovieDetailsDTO>()
                .ForMember(x => x.Characters, options => options.MapFrom(MoviesCharacterMap));

            CreateMap<MoviePatchDTO, Movie>().ReverseMap();
        }

        private List<CharacterMovieDetailsDTO> MoviesCharacterMap(Movie movie, MovieDetailsDTO movieDetailsDTO)
        {
            var result = new List<CharacterMovieDetailsDTO>();
            if (movie.MoviesCharacters == null)
                return result;

            foreach(var characterMovie in movie.MoviesCharacters)
            {
                result.Add(new CharacterMovieDetailsDTO() { CharacterId = characterMovie.CharacterId, CharacterName = characterMovie.Character.Name });
            }

            return result;
        }

        private List<MoviesGenres> MoviesGenresMap(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();
            if (movieCreationDTO.GenreIds == null)
                return result;
            foreach (var id in movieCreationDTO.GenreIds)
            {
                result.Add(new MoviesGenres() { GenreId = id }); 
            }

            return result;
        }

        private List<MoviesCharacters> MoviesCharactersMap(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesCharacters>();
            if (movieCreationDTO.CharacterIds == null)
                return result;
            foreach (var id in movieCreationDTO.CharacterIds)
            {
                result.Add(new MoviesCharacters() { CharacterId = id });
            }

            return result;
        }
    }
}
