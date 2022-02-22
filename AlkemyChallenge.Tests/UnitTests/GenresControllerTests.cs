using AlkemyChallenge.Controllers;
using AlkemyChallenge.DTOs;
using AlkemyChallenge.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlkemyChallenge.Tests.UnitTests
{
    [TestClass]
    public class GenresControllerTests: BaseTests
    {
        [TestMethod]
        public async Task GetGenres()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = ContextBuild(dbName);
            var mapper = AutoMapperConfig();

            context.Genres.Add(new Genre() { Name = "Genre 1" });
            context.Genres.Add(new Genre() { Name = "Genre 2" });
            context.Genres.Add(new Genre() { Name = "Genre 3" });
            await context.SaveChangesAsync();

            var context2 = ContextBuild(dbName);

            var controller = new GenresController(context2, mapper);
            var response = await controller.Get();

            var genres = response.Value;
            Assert.AreEqual(3, genres.Count);
        }

        [TestMethod]
        public async Task GetNonExistentGenre()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = ContextBuild(dbName);
            var mapper = AutoMapperConfig();


            var controller = new GenresController(context, mapper);
            var response = await controller.GetById(1);

            var result = response.Result as StatusCodeResult;
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task GetExistentGenre()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = ContextBuild(dbName);
            var mapper = AutoMapperConfig();

            context.Genres.Add(new Genre() { Name = "Genre 1" });
            context.Genres.Add(new Genre() { Name = "Genre 2" });
            context.Genres.Add(new Genre() { Name = "Genre 3" });
            await context.SaveChangesAsync();

            var context2 = ContextBuild(dbName);

            var controller = new GenresController(context2, mapper);
            var response = await controller.GetById(2);

            var result = response.Value;
            Assert.AreEqual(2, result.Id);
        }

        [TestMethod]
        public async Task CreateNewGenre()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = ContextBuild(dbName);
            var mapper = AutoMapperConfig();

            var newGenre = new GenreCreationDTO() { Name = "New Genre" };

            var controller = new GenresController(context, mapper);

            var response = await controller.Post(newGenre);

            var result = response as CreatedAtRouteResult;
            Assert.IsNotNull(result);

            var context2 = ContextBuild(dbName);
            var cant = await context2.Genres.CountAsync();
            Assert.AreEqual(1, cant);
        }

        [TestMethod]
        public async Task EditGenre()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = ContextBuild(dbName);
            var mapper = AutoMapperConfig();

            context.Genres.Add(new Genre() { Name = "Genre"});
            await context.SaveChangesAsync();

            var context2 = ContextBuild(dbName);
            var controller = new GenresController(context2, mapper);

            var genreCreactionDTO = new GenreCreationDTO() { Name = "Genre Edited" };

            var response = await controller.Put(1, genreCreactionDTO);
            var result = response as StatusCodeResult;
            Assert.AreEqual(204, result.StatusCode);

            var context3 = ContextBuild(dbName);
            var exist = await context3.Genres.AnyAsync(x => x.Name == "Genre Edited");
            Assert.IsTrue(exist);
        }

        [TestMethod]
        public async Task DeleteNonExistentGenre()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = ContextBuild(dbName);
            var mapper = AutoMapperConfig();

            var controller = new GenresController(context, mapper);
            var response = await controller.Delete(1);

            var result = response as StatusCodeResult;
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task DeleteGenre()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = ContextBuild(dbName);
            var mapper = AutoMapperConfig();

            context.Genres.Add(new Genre() { Name = "Genre 1" });
            context.Genres.Add(new Genre() { Name = "Genre 2" });
            await context.SaveChangesAsync();

            var context2 = ContextBuild(dbName);
            var controller = new GenresController(context2, mapper);

            var response = await controller.Delete(1);
            var result = response as StatusCodeResult;
            Assert.AreEqual(204, result?.StatusCode);

            var context3 = ContextBuild(dbName);
            var cant = await context3.Genres.CountAsync();
            Assert.AreEqual(1, cant);
        }
    }
}
