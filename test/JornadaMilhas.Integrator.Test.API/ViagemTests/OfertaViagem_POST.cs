using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Dominio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integrator.Test.API.ViagemTests
{
    public class OfertaViagem_POST : IClassFixture<JornadaMilhasWebApplicationFactory>
    {
        private readonly JornadaMilhasWebApplicationFactory _factory;
        public OfertaViagem_POST(JornadaMilhasWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CadastraOfertaViagem()
        {
            //arrrange
            using var client = await _factory.GetClientWithAccessTokenAsync();

            var ofertaViagem = new OfertaViagem
            {
                Preco = 100,
                Rota = new Rota("São Paulo", "Rio de Janeiro"),
                Periodo = new Periodo(DateTime.Now, DateTime.Now.AddDays(7))
            };

            //act

            var response = await client.PostAsJsonAsync("/ofertas-viagem", ofertaViagem);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CadastraOfertaViagemSemAutorizacao()
        {
            //arrrange
            using var client = _factory.CreateClient();

            var ofertaViagem = new OfertaViagem
            {
                Preco = 100,
                Rota = new Rota("São Paulo", "Rio de Janeiro"),
                Periodo = new Periodo(DateTime.Now, DateTime.Now.AddDays(7))
            };

            //act

            var response = await client.PostAsJsonAsync("/ofertas-viagem", ofertaViagem);

            //assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
