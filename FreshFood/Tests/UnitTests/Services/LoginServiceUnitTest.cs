using Cache.Interfaces;
using Database.Repositories.Interfaces;
using Moq;
using Security.Interfaces;
using Security.Services;

namespace UnitTests.Services
{
    public class LoginServiceUnitTest
    {
        [Fact]
        public async void Login_WithRightCredentials_ReturnsSuccess()
        {
            //#region Arrange
            //var email = "joao@gmail.com";
            //var password = "string";
            //var moq = new Mock<ISecurityRepository>();
            //moq.Setup(x => x.CheckIfEmailExists(email)).Returns(true);
            //moq.Setup(x => x.GetUsersPassword(email)).ReturnsAsync(Encoding.UTF8.GetBytes("0x244D59484153482456312431303024443644444A34494857327A4634316E5437654E786542613778314E645452376A457159627641786B3261302F50486959"));
            //#endregion

            //#region Act                  
            //ISecurityService securityService = new SecurityService(moq.Object);
            //var result = await securityService.LoginAsync(email, password);
            //#endregion

            //#region Assert
            //Assert.True(result);
            //#endregion
        }

        [Fact]
        public async void Login_WithInvalidEmail_ReturnsError()
        {
            #region Arrange
            var email = "joao129@gmail.com";
            var password = "string";
            var securityRepo = new Mock<ISecurityRepository>();
            var cacheService = new Mock<ICacheService>();
            securityRepo.Setup(x => x.CheckIfEmailExists(email)).Returns(false);

            #endregion

            #region Act                  
            ISecurityService securityService = new SecurityService(securityRepo.Object, cacheService.Object);
            var result = await securityService.LoginAsync(email, password);
            #endregion

            #region Assert
            Assert.False(result);
            #endregion
        }

        [Fact]
        public void Login_WithInvalidCredentials_ReturnsError()
        {
            #region Arrange

            #endregion

            #region Act

            #endregion

            #region Assert

            #endregion

        }
    }
}