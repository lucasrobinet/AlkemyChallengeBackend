using AlkemyChallenge.Controllers;
using AlkemyChallenge.DTOs;
using AlkemyChallenge.Entities;
using AlkemyChallenge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlkemyChallenge.Tests.UnitTests
{
    [TestClass]
    public class CharactersControllerTests: BaseTests
    {
        [TestMethod]
        public string DataTest()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = ContextBuild(dbName);

            var movie = new Movie() { Title = "Movie Test" };


            var characters = new List<Character>()
            {
                new Character() { Name = "Character Name Test", Age = 20},
                new Character() { Name = "Lucas", Age = 30}
            };

            var characterWithMovie = new Character()
            {
                Name = "Character With Movie",
                Age = 25
            };

            characters.Add(characterWithMovie);

            context.Add(movie);
            context.AddRange(characters);
            context.SaveChanges();

            var movieCharacter = new MoviesCharacters() { MovieId = movie.Id, CharacterId = characterWithMovie.Id};
            context.Add(movieCharacter);
            context.SaveChanges();

            return dbName;
        }

        [TestMethod]
        public async Task FilterByName()
        {
            var dbName = DataTest();
            var mapper = AutoMapperConfig();
            var context = ContextBuild(dbName);

            var controller = new CharactersController(context, mapper, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var characterName = "Character Name Test";

            var filterDTO = new CharacterFilterDTO()
            {
                Name = characterName,
                NumberOfEntrysPerPage = 10,
            };

            var response = await controller.Filter(filterDTO);
            var character = response.Value;
            Assert.AreEqual(1, character.Count);
            Assert.AreEqual(characterName, character[0].Name);
        }

        [TestMethod]
        public async Task FilterByAge()
        {
            var dbName = DataTest();
            var mapper = AutoMapperConfig();
            var context = ContextBuild(dbName);

            var controller = new CharactersController(context, mapper, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var characterAge = 30;

            var filterDTO = new CharacterFilterDTO()
            {
                Age = characterAge,
                NumberOfEntrysPerPage = 10,
            };

            var response = await controller.Filter(filterDTO);
            var character = response.Value;
            Assert.AreEqual(1, character.Count);
            Assert.AreEqual("Lucas", character[0].Name);  
        }

        [TestMethod]
        public async Task FilterByMovieId()
        {
            var dbName = DataTest();
            var mapper = AutoMapperConfig();
            var context = ContextBuild(dbName);

            var controller = new CharactersController(context, mapper, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var movieId = context.Movies.Select(x => x.Id).First();

            var filterDTO = new CharacterFilterDTO()
            {
                Movies = movieId,
                NumberOfEntrysPerPage = 10,
            };

            var response = await controller.Filter(filterDTO);
            var character = response.Value;
            Assert.AreEqual(1, character.Count);
            Assert.AreEqual("Character With Movie", character[0].Name);
        }

        [TestMethod]
        public async Task CreateCharacterWithoutImage()
        {
            var dbName = Guid.NewGuid().ToString();
            var mapper = AutoMapperConfig();
            var context = ContextBuild(dbName);

            var character = new CharacterCreationDTO() { Name = "Lucas" };

            var mock = new Mock<IFileStorage>();
            mock.Setup(x => x.SaveFile(null, null, null, null))
                .Returns(Task.FromResult("url"));

            var controller = new CharactersController(context, mapper, mock.Object);
            var response = await controller.CreateCharacter(character);
            var result = response as CreatedAtRouteResult;
            Assert.AreEqual(201, result.StatusCode);

            var context2 = ContextBuild(dbName);
            var list = await context2.Characters.ToListAsync();
            Assert.AreEqual(1, list.Count);
            Assert.IsNull(list[0].Image);
            Assert.AreEqual(0, mock.Invocations.Count);
        }

        [TestMethod]
        public async Task CreateCharacterWithImage()
        {
            var dbName = Guid.NewGuid().ToString();
            var mapper = AutoMapperConfig();
            var context = ContextBuild(dbName);

            var content = Encoding.UTF8.GetBytes("Image Test");
            var file = new FormFile(new MemoryStream(content), 0, content.Length, "Data", "Image.jpg");
            file.Headers = new HeaderDictionary();
            file.ContentType = "image/jpg";

            var character = new CharacterCreationDTO() { Name = "Lucas", Image = file };

            var mock = new Mock<IFileStorage>();
            mock.Setup(x => x.SaveFile(content, ".jpg", "characters", file.ContentType))
                .Returns(Task.FromResult("url"));

            var controller = new CharactersController(context, mapper, mock.Object);
            var response = await controller.CreateCharacter(character);
            var result = response as CreatedAtRouteResult;
            Assert.AreEqual(201, result.StatusCode);

            var context2 = ContextBuild(dbName);
            var list = await context2.Characters.ToListAsync();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("url", list[0].Image);
            Assert.AreEqual(1, mock.Invocations.Count);
        }

        [TestMethod]
        public async Task PatchReturns404IfCharacterNotExist()
        {
            var dbName = Guid.NewGuid().ToString();
            var mapper = AutoMapperConfig();
            var context = ContextBuild(dbName);

            var controller = new CharactersController(context, mapper, null);
            var patchDocument = new JsonPatchDocument<CharacterPatchDTO>();
            var response = await controller.Patch(1, patchDocument);
            var result = response as StatusCodeResult;
            Assert.AreEqual(404, result?.StatusCode);
        }

        [TestMethod]
        public async Task PatchUpdateAField()
        {
            var dbName = Guid.NewGuid().ToString();
            var mapper = AutoMapperConfig();
            var context = ContextBuild(dbName);

            var age = 20;
            var character = new Character()
            {
                Name = "Lucas",
                Age = age
            };
            context.Add(character);
            await context.SaveChangesAsync();

            var context2 = ContextBuild(dbName);
            var controller = new CharactersController(context2, mapper, null);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(x => x.Validate(It.IsAny<ActionContext>(), 
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>()));

            controller.ObjectValidator = objectValidator.Object;

            var patchDocument = new JsonPatchDocument<CharacterPatchDTO>();
            patchDocument.Operations.Add(new Operation<CharacterPatchDTO>("replace", "/name", null, "Carlos"));

            var response = await controller.Patch(1, patchDocument);
            var result = response as StatusCodeResult;
            Assert.AreEqual(204, result.StatusCode);

            var context3 = ContextBuild(dbName);
            var characterName = await context3.Characters.FirstAsync();
            Assert.AreEqual("Carlos", characterName.Name);
            Assert.AreEqual(age, characterName.Age);
        }
    }
}
