using AlkemyChallenge.Controllers;
using AlkemyChallenge.DTOs;
using AlkemyChallenge.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlkemyChallenge.Tests.UnitTests
{
    [TestClass]
    public class MoviesControllerTests: BaseTests
    {
        [TestMethod]
        public async Task MoviesFilterOrderAsc()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = ContextBuild(dbName);
            var mapper = AutoMapperConfig();

            var moviesList = new List<Movie>()
            {
                new Movie() { Title = "Movie 1"},
                new Movie() { Title = "Movie 2"},
                new Movie() { Title = "Movie 3"}
            };

            await context.SaveChangesAsync();

            var controller = new MoviesController(context,mapper, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filerDTO = new MovieFilterDTO()
            {
                Order = "asc"
            };

            var response = await controller.Filter(filerDTO);
            var movies = response.Value;

            var context2 = ContextBuild(dbName);
            var moviesDb = context2.Movies.OrderBy(x => x.Title).ToList();

            Assert.AreEqual(moviesDb.Count, movies.Count);

            for (int i = 0; i < moviesDb.Count; i++)
            {
                var moviesFromController = movies[i];
                var moviesDataBase = moviesDb[i];

                Assert.AreEqual(moviesFromController.Title, moviesDataBase.Title);
            }

        }

        [TestMethod]
        public async Task MoviesFilterOrderDesc()
        {
            var dbName = Guid.NewGuid().ToString();
            var context = ContextBuild(dbName);
            var mapper = AutoMapperConfig();

            var moviesList = new List<Movie>()
            {
                new Movie() { Title = "Movie 1"},
                new Movie() { Title = "Movie 2"},
                new Movie() { Title = "Movie 3"}
            };

            await context.SaveChangesAsync();

            var controller = new MoviesController(context, mapper, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filerDTO = new MovieFilterDTO()
            {
                Order = "desc"
            };

            var response = await controller.Filter(filerDTO);
            var movies = response.Value;

            var context2 = ContextBuild(dbName);
            var moviesDb = context2.Movies.OrderByDescending(x => x.Title).ToList();

            Assert.AreEqual(moviesDb.Count, movies.Count);

            for (int i = 0; i < moviesDb.Count; i++)
            {
                var moviesFromController = movies[i];
                var moviesDataBase = moviesDb[i];

                Assert.AreEqual(moviesFromController.Title, moviesDataBase.Title);
            }

        }
    }
}
