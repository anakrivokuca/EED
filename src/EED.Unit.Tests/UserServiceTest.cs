﻿using System.Collections.Generic;
using System.Linq;
using EED.DAL;
using EED.Domain;
using EED.Service;
using Moq;
using NUnit.Framework;

namespace EED.Unit.Tests
{
    [TestFixture]
    public class UserServiceTest
    {
        [Test]
        public void Can_List_All_Users()
        {
            // Arrange
            var mock = new Mock<IRepository<User>>();
            mock.Setup(r => r.FindAll()).Returns(new List<User> {
                new User { Name = "Ana", Surname = "Krivokuca", 
                    Email = "anakrivokuca@gmail.com"},
                new User { Name = "Pera", Surname = "Peric", Email = "pera@gmail.com"},
                new User { Name = "John", Surname = "Doe", Email = "johndoe@gmail.com"},
            });
            var service = new UserService(mock.Object);
            
            // Act
            var result = service.FindAllUsers();

            // Assert
            Assert.IsNotNull(result, "Null list is returned.");
            Assert.AreEqual(3, result.Count(), "Number of all users should be three.");
        }
    }
}
