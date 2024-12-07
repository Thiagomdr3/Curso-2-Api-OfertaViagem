using JornadaMilhas.Integrator.Test.API.Fakers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integrator.Test.API.ViagemTests
{
    public class OfertaViagem_DELETE : IClassFixture<JornadaMilhasWebApplicationFactory>
    {
        private readonly JornadaMilhasWebApplicationFactory _factory;
        public OfertaViagem_DELETE(JornadaMilhasWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task DeletaOfertaViagem()
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

            //act
            var response = await client.DeleteAsync("/ofertas-viagem/" + ofertaExistente.Id);

            //assert
            Assert.Null(_factory.context.OfertasViagem.FirstOrDefault(f => f.Id == ofertaExistente.Id));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
