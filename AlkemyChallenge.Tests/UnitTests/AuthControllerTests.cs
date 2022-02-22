using AlkemyChallenge.Controllers;
using AlkemyChallenge.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlkemyChallenge.Tests.UnitTests
{
    [TestClass]
    public class AuthControllerTests: BaseTests
    {
        [TestMethod]
        public async Task CreateUser()
        {
            var dbName = Guid.NewGuid().ToString();
            await CreateUserHelper(dbName);
            var context2 = ContextBuild(dbName);
            var count = await context2.Users.CountAsync();
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public async Task CreateUserWithEmailTaken()
        {
            var dbName = Guid.NewGuid().ToString();
            await CreateUserHelper(dbName);
            var context2 = ContextBuild(dbName);
            var count = await context2.Users.CountAsync();
            Assert.AreEqual(1, count);

            var controller = AuthControllerBuild(dbName);
            var userInfo = new UserInfo() { Name = "name test", Email = "testmail@hotmail.com", Password = "asdasddsa123?!" };
            var response = await controller.CreateUser(userInfo);

            var context3 = ContextBuild(dbName);
            var count2 = await context3.Users.CountAsync();
            Assert.AreEqual(1, count2);
            Assert.IsNull(response.Value);
            var result = response.Result as BadRequestObjectResult;
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public async Task UserCantLogIn()
        {
            var dbName = Guid.NewGuid().ToString();
            await CreateUserHelper(dbName);

            var controller = AuthControllerBuild(dbName);
            var userInfo = new UserInfo() { Name = "User", Email = "testmail@hotmail.com", Password = "wrongPasword" };
            var response = await controller.Login(userInfo);

            Assert.IsNull(response.Value);
            var result = response.Result as BadRequestObjectResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UserCanLogin()
        {
            var dbName = Guid.NewGuid().ToString();
            await CreateUserHelper(dbName);

            var controller = AuthControllerBuild(dbName);
            var userInfo = new UserInfo() { Name = "UserTest1", Email = "testemail@gmail.com", Password = "Contraseña123!" };
            var response = await controller.Login(userInfo);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.Token);
        }

        private async Task CreateUserHelper(string nombreBD)
        {
            var cuentasController = AuthControllerBuild(nombreBD);
            cuentasController.ControllerContext.HttpContext = new DefaultHttpContext();
            var userInfo = new UserInfo() { Name = "UserTest1", Email = "testemail@gmail.com", Password = "Contraseña123!" };
            await cuentasController.CreateUser(userInfo);
        }

        private AuthController AuthControllerBuild(string dbName)
        {
            var context = ContextBuild(dbName);
            var miUserStore = new UserStore<IdentityUser>(context);
            var userManager = BuildUserManager(miUserStore);
            var mapper = AutoMapperConfig();

            var httpContext = new DefaultHttpContext();
            MockAuth(httpContext);
            var signInManager = SetupSignInManager(userManager, httpContext);


            var myConfiguration = new Dictionary<string, string>
            {
                {"jwt:key", "KAFGAKDSBHJFVKJDSHFBVKSDAUHFGSDKJFHGAKUYASEGFOASLUIFGASOIGFUAOIUXCVJHNCVNADLFHIKXCBVSAKDHGF" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            var emailSenderMock = new Mock<IEmailSender>();
            emailSenderMock.Setup(x => x.SendEmailAsync(null, null, null))
                .Returns(Task.FromResult(true));

            return new AuthController(userManager, signInManager, configuration, context, mapper, emailSenderMock.Object);
        }

        private UserManager<TUser> BuildUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;

            options.Setup(o => o.Value).Returns(idOptions);

            var userValidators = new List<IUserValidator<TUser>>();

            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());

            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);

            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            return userManager;
        }

        private static SignInManager<TUser> SetupSignInManager<TUser>(UserManager<TUser> manager,
            HttpContext context, ILogger logger = null, IdentityOptions identityOptions = null,
            IAuthenticationSchemeProvider schemeProvider = null) where TUser : class
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(a => a.HttpContext).Returns(context);

            identityOptions = identityOptions ?? new IdentityOptions();

            var options = new Mock<IOptions<IdentityOptions>>();
            options.Setup(a => a.Value).Returns(identityOptions);

            var claimsFactory = new UserClaimsPrincipalFactory<TUser>(manager, options.Object);

            schemeProvider = schemeProvider ?? new Mock<IAuthenticationSchemeProvider>().Object;

            var sm = new SignInManager<TUser>(manager, contextAccessor.Object, claimsFactory, options.Object, null, schemeProvider, new DefaultUserConfirmation<TUser>());
            sm.Logger = logger ?? (new Mock<ILogger<SignInManager<TUser>>>()).Object;
            return sm;
        }

        private Mock<IAuthenticationService> MockAuth(HttpContext context)
        {
            var auth = new Mock<IAuthenticationService>();
            context.RequestServices = new ServiceCollection().AddSingleton(auth.Object).BuildServiceProvider();
            return auth;
        }
    }
}
