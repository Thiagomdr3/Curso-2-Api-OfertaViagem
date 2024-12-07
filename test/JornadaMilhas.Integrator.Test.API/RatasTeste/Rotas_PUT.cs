using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Integrator.Test.API.Fakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integrator.Test.API.RatasTeste
{
    public class Rotas_PUT:IClassFixture<JornadaMilhasWebApplicationFactory>
    {
        private readonly JornadaMilhasWebApplicationFactory _factory;
        public Rotas_PUT(JornadaMilhasWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AtualizaRota()
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

            var rota = new Rota
            {
                Id = rotaExistente.Id,
                Origem = "São Paulo 123",
                Destino = "Rio de Janeiro"
            };

            //act
            var response = await client.PutAsJsonAsync("/rota-viagem", rota);

            //assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var rotaAtualizada = _factory.context.Rota.FirstOrDefault(f => f.Id == rota.Id);
            Assert.Equal(rota.Id, rotaAtualizada!.Id);
            Assert.Equal(rota.Origem, rotaAtualizada.Origem);
            Assert.Equal(rota.Destino, rotaAtualizada.Destino);
        }
    }
}
