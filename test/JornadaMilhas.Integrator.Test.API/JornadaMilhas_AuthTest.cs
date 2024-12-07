using JornadaMilhas.API.DTO.Auth;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace JornadaMilhas.Integrator.Test.API
{
    public class JornadaMilhas_AuthTest
    {
        private readonly string _user;
        private readonly string _password;
        public JornadaMilhas_AuthTest()
        {
            var config = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = config.Build();

            _user = configuration.GetSection("DadosAcesso")["User"];
            _password = configuration.GetSection("DadosAcesso")["Password"];
        }
        [Fact]
        public async Task POST_EfetuaLoginInvalido()
        {
            //arrange
            var client = new JornadaMilhasWebApplicationFactory().CreateClient();
            var user = new UserDTO
            {
                Email = "tester@email.com",
                Password = "123"
            };

            //act
            var response = await client.PostAsJsonAsync("/auth-login", user);
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<string>(responseContent);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Login inválido.", errorResponse);
        }

        [Fact]
        public async Task POST_EfetuaLoginComSucesso()
        {
            //arrange
            var client = new JornadaMilhasWebApplicationFactory().CreateClient();
            var user = new UserDTO
            {
                Email = _user,
                Password = _password
            };

            //act
            var response = await client.PostAsJsonAsync("/auth-login", user);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
    }
}