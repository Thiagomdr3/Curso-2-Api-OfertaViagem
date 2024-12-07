using JornadaMilhas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integrator.Test.API.RatasTeste
{
    public class Rotas_POST:IClassFixture<JornadaMilhasWebApplicationFactory>  
    {
        private readonly JornadaMilhasWebApplicationFactory _factory;
        public Rotas_POST(JornadaMilhasWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CadastraRota()
        {
            //arrrange
            using var client = await _factory.GetClientWithAccessTokenAsync();

            var rota = new Rota
            {
                Origem = "São Paulo",
                Destino = "Rio de Janeiro"
            };

            //act
            var response = await client.PostAsJsonAsync("/rota-viagem", rota);

            //assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CadastraRotaSemTokem()
        {
            //arrrange
            using var client = _factory.CreateClient();

            var rota = new Rota
            {
                Origem = "São Paulo",
                Destino = "Rio de Janeiro"
            };

            //act
            var response = await client.PostAsJsonAsync("/rota-viagem", rota);

            //assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
