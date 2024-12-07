using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Integrator.Test.API.Fakers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integrator.Test.API.RatasTeste
{
    public class Rotas_GET:IClassFixture<JornadaMilhasWebApplicationFactory>
    {
        private readonly JornadaMilhasWebApplicationFactory _factory;
        public Rotas_GET(JornadaMilhasWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ObtemRota()
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
            var response = await client.GetAsync("/rota-viagem/" + rotaExistente.Id);

            //assert
            var rota = JsonConvert.DeserializeObject<Rota>(await response.Content.ReadAsStringAsync());
            Assert.Equal(rotaExistente.Id, rota!.Id);
            Assert.Equal(rotaExistente.Origem, rota.Origem);
            Assert.Equal(rotaExistente.Destino, rota.Destino);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
