using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Integrator.Test.API.Fakers;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace JornadaMilhas.Integrator.Test.API.ViagemTests
{
    public class OfertaViagem_GET : IClassFixture<JornadaMilhasWebApplicationFactory>
    {
        private readonly JornadaMilhasWebApplicationFactory _factory;

        public OfertaViagem_GET(JornadaMilhasWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ObtemOfertasViagemPorId()
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
            var response = await client.GetAsync("/ofertas-viagem/" + ofertaExistente.Id);


            //assert
            Assert.NotNull(response);
            var ofertaRetornada = await response.Content.ReadFromJsonAsync<OfertaViagem>();

            Assert.Equal(ofertaExistente.Preco, ofertaRetornada!.Preco, 0.001);
            Assert.Equal(ofertaExistente.Rota.Origem, ofertaRetornada.Rota.Origem);
            Assert.Equal(ofertaExistente.Rota.Destino, ofertaRetornada.Rota.Destino);
        }

        [Fact]
        public async Task RecuperarOfertasViagemNaConsultaPaginada()
        {
            //arrange
            var teste = _factory.context.OfertasViagem.ToList();
            var lstOfertas = new OfertaViagemDataBuilder().Generate(100);
            _factory.context.OfertasViagem.AddRange(lstOfertas);
            _factory.context.SaveChanges();

            using var client = await _factory.GetClientWithAccessTokenAsync();
            int pagina = 1, tamanhoPagina = 80;

            //act
            var response = await client.
                GetAsync($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPagina}");

            //assert
            Assert.NotNull(response);
            var ofertasRetornadas = await response.Content.ReadFromJsonAsync<List<OfertaViagem>>();

            Assert.Equal(tamanhoPagina, ofertasRetornadas!.Count);
        }

        [Fact]
        public async Task RecuperarOfertaViagemNaconsultaPaginadaUltimaPagina()
        {
            //arrange
            _factory.context.Database.ExecuteSqlRaw("DELETE OfertasViagem");

            var lstOfertas = new OfertaViagemDataBuilder().Generate(80);
            _factory.context.OfertasViagem.AddRange(lstOfertas);
            _factory.context.SaveChanges();

            using var client = await _factory.GetClientWithAccessTokenAsync();
            int pagina = 4, tamanhoPagina = 25;

            //act
            var response = await client.
                GetAsync($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPagina}");

            //assert
            Assert.NotNull(response);
            var ofertasRetornadas = await response.Content.ReadFromJsonAsync<List<OfertaViagem>>();

            Assert.Equal(5, ofertasRetornadas!.Count);
        }

        [Fact]
        public async Task RecuperarOfertaViagemNaconsultaComPaginadaInexistente()
        {
            //arrange
            _factory.context.Database.ExecuteSqlRaw("DELETE OfertasViagem");

            var lstOfertas = new OfertaViagemDataBuilder().Generate(80);
            _factory.context.OfertasViagem.AddRange(lstOfertas);
            _factory.context.SaveChanges();

            using var client = await _factory.GetClientWithAccessTokenAsync();
            int pagina = 5, tamanhoPagina = 25;

            //act
            var response = await client.
                GetAsync($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPagina}");

            //assert
            Assert.NotNull(response);
            var ofertasRetornadas = await response.Content.ReadFromJsonAsync<List<OfertaViagem>>();

            Assert.Equal(0, ofertasRetornadas!.Count);
        }

        //esse aqui eh paleativo. Tem que tratar isso na API
        [Fact]
        public async Task RecuperarOfertaViagemNaconsultaComPaginaComValorNegativo()
        {
            //arrange
            _factory.context.Database.ExecuteSqlRaw("DELETE OfertasViagem");

            var lstOfertas = new OfertaViagemDataBuilder().Generate(80);
            _factory.context.OfertasViagem.AddRange(lstOfertas);
            _factory.context.SaveChanges();

            using var client = await _factory.GetClientWithAccessTokenAsync();
            int pagina = -5, tamanhoPagina = 25;

            //act * assert
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                var response  = await client
                .GetFromJsonAsync<ICollection<OfertaViagem>>
                ($"/ofertas-viagem?pagina={pagina}&tamanhoPorPagina={tamanhoPagina}");

            });
        }
    }
}
