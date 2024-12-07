using JornadaMilhas.Integrator.Test.API.Fakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integrator.Test.API.RatasTeste
{
    public class Rotas_DELETE:IClassFixture<JornadaMilhasWebApplicationFactory>
    {
        private readonly JornadaMilhasWebApplicationFactory _factory;
        public Rotas_DELETE(JornadaMilhasWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task DeletaRota()
        {
            //arrange
            var rotaExistente = _factory.context.Rota.FirstOrDefault();
            if (rotaExistente is null)
            {
                rotaExistente = new RotaDataBuilder().Build();
                _factory.context.Rota.Add(rotaExistente);
                _factory.context.SaveChanges();
            }
            using var client = await _factory.GetClientWithAccessTokenAsync();

            //act
            var response = await client.DeleteAsync("/rota-viagem/" + rotaExistente.Id);

            //assert
            Assert.Null(_factory.context.Rota.FirstOrDefault(f => f.Id == rotaExistente.Id));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
