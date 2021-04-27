using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullArgument_ThrowsArgumentException()
        {
            new UsersController(null!);
        }

        [TestMethod]
        public void Get_WithUsers_ReturnsListOfUsers()
        {
            User user1 = new()
            {
                Id = 42,
                FirstName = "John",
                LastName = "Doe"
            };
            User user2 = new()
            {
                Id = 41,
                FirstName = "Jane",
                LastName = "Smith"
            };
            TestingRepository userRepository = new(user1, user2);
            UsersController sut = new(userRepository);

            List<User> users = sut.Get().ToList();

            Assert.AreEqual(2, users.Count);
            Assert.AreEqual(42, users[0].Id);
            Assert.AreEqual("John", users[0].FirstName);
            Assert.AreEqual("Doe", users[0].LastName);

            Assert.AreEqual(41, users[1].Id);
            Assert.AreEqual("Jane", users[1].FirstName);
            Assert.AreEqual("Smith", users[1].LastName);
        }

        [TestMethod]
        public void Get_WithValidId_ReturnsUser()
        {
            User user = new() 
            { 
                Id = 42,
                FirstName = "John",
                LastName = "Doe"
            };
            TestingRepository userRepository = new(user);
            UsersController sut = new(userRepository);

            ActionResult<User?> result = sut.Get(42);
            Assert.AreEqual(42, result.Value!.Id);
            Assert.AreEqual("John", result.Value.FirstName);
            Assert.AreEqual("Doe", result.Value.LastName);
        }

        [TestMethod]
        public void Get_WithInvalidId_ReturnsNotFound()
        {
            User user = new()
            {
                Id = 42,
                FirstName = "John",
                LastName = "Doe"
            };
            TestingRepository userRepository = new(user);
            UsersController sut = new(userRepository);

            ActionResult<User?> result = sut.Get(1);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Delete_WithValidId_ReturnsOk()
        {
            User user = new()
            {
                Id = 42,
                FirstName = "John",
                LastName = "Doe"
            };
            TestingRepository userRepository = new(user);
            UsersController sut = new(userRepository);

            ActionResult result = sut.Delete(42);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Delete_WithInvalidId_ReturnsNotFound()
        {
            User user = new()
            {
                Id = 42,
                FirstName = "John",
                LastName = "Doe"
            };
            TestingRepository userRepository = new(user);
            UsersController sut = new(userRepository);

            ActionResult result = sut.Delete(2);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Post_WithNullUser_ReturnsBadRequest()
        {
            UsersController sut = new(new TestingRepository());

            ActionResult<User?> result = sut.Post(null);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_WithValidUser_CreatesUser()
        {
            TestingRepository userRepository = new();
            UsersController sut = new(userRepository);

            User user = new()
            {
                Id = 42,
                FirstName = "John",
                LastName = "Doe"
            };

            ActionResult<User?> result = sut.Post(user);

            Assert.AreEqual(1, userRepository.Users.Count);
            Assert.AreEqual("John", userRepository.Users[42].FirstName);
            Assert.AreEqual("Doe", userRepository.Users[42].LastName);

            User? returnedUser = result.Value;
            Assert.AreEqual(42, returnedUser!.Id);
            Assert.AreEqual("John", returnedUser.FirstName);
            Assert.AreEqual("Doe", returnedUser.LastName);
        }

        [TestMethod]
        public void Put_WithNullUser_ReturnsBadRequest()
        {
            UsersController sut = new(new TestingRepository());

            ActionResult<User?> result = sut.Put(42, null);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Put_WhenUserIsNotFound_ReturnsNotFound()
        {
            TestingRepository userRepository = new();
            UsersController sut = new(userRepository);
            User user = new()
            {
                Id = 42,
                FirstName = "John",
                LastName = "Doe"
            };

            ActionResult<User?> result = sut.Put(42, user);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Put_WithValidUser_UpdatesUser()
        {
            User user = new()
            {
                Id = 42,
                FirstName = "John",
                LastName = "Doe"
            };
            TestingRepository userRepository = new(user);
            UsersController sut = new(userRepository);

            ActionResult result = sut.Put(42, new User
            {
                Id = 43,
                FirstName = "Jane",
                LastName = "Smith"
            });


            Assert.AreEqual(42, userRepository.Users[42].Id);
            Assert.AreEqual("Jane", userRepository.Users[42].FirstName);
            Assert.AreEqual("Smith", userRepository.Users[42].LastName);

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        private class TestingRepository : IUserRepository
        {
            public Dictionary<int, User> Users { get; } = new();

            public TestingRepository(params User[] users)
            {
                foreach(User user in users)
                {
                    Users[user.Id] = user;
                }
            }

            public User Create(User item)
            {
                if (item is null)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                Users[item.Id] = item;
                return item;
            }

            public User? GetItem(int id)
            {
                if (Users.TryGetValue(id, out User? user))
                {
                    return user;
                }
                return null;
            }

            public ICollection<User> List() => Users.Values;

            public bool Remove(int id) => Users.Remove(id);

            public void Save(User item) => Create(item);
        }
    }
}
