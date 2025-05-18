using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Xunit;
using System.Threading.Tasks;
using CBT.API.Repositories.Interface;
using Moq;
using CBT.API.Models;

namespace CBT.API.Tests
{
    public class UserTests
    {
        [Fact]
        public async Task CreateUser_ValiData_UserCreated()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            var user = new User
            {
                Id = 1,
                Username = "john",
                Email = "john@example.com"
            };

            //Act
            var result = await userRepository.Object.CreateUser(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetuserById_ValidId_UserReturned()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            var user = new User
            {
                Id = 1,
                Username = "john",
                Email = "john@example.com"
            };

            //Act
            var result = await userRepository.Object.GetUserById(user.Id);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateUser_ValiData_UserUpdated()
        {
            //Arrange
            var userRepository = new Mock<IUserRepository>();
            var user = new User
            {
                Id = 1,
                Username = "john",
                Email = "john@example.com"
            };

            //Act
            var result = await userRepository.Object.CreateUser(user);

            //Assert
            Assert.True(result);
        }
    }
}
