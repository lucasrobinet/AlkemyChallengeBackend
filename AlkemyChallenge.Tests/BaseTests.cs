using AlkemyChallenge.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlkemyChallenge.Tests
{
    public class BaseTests
    {
        protected ApplicationDbContext ContextBuild(string DbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(DbName).Options;

            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        }

        protected IMapper AutoMapperConfig()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfiles());
            });

            return config.CreateMapper();
        }
    }
}
