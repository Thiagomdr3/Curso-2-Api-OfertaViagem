using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Integrator.Test.API.Fakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integrator.Test.API.ViagemTests
{
    public class OfertaViagem_PUT : IClassFixture<JornadaMilhasWebApplicationFactory>
    {
        private readonly JornadaMilhasWebApplicationFactory _factory;
        public OfertaViagem_PUT(JornadaMilhasWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AtualizaOfertaViagem()
        {
            //arrange
            var ofertaExistente = _factory.context.OfertasViagem.FirstOrDefault();
            if (ofertaExistente is null)
            {
                ofertaExistente = new OfertaViagemDataBuilder().Build();
                _factory.context.OfertasViagem.Add(ofertaExistente);
                _factory.context.SaveChanges();
            }
            using var client = await _factory.GetClientWithAccessTokenAsync();

            var novaOferta = new OfertaViagem
            {
                Id = ofertaExistente.Id,
                Preco = 10,
                Rota = new RotaDataBuilder().Build(),
                Periodo = new PeriodoDataDuilder().Build()
            };

            //act
            var response = await client.PutAsJsonAsync($"/ofertas-viagem/", novaOferta);

            //assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
