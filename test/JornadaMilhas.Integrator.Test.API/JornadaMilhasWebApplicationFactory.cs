using JornadaMilhas.API.DTO.Auth;
using JornadaMilhas.Dados;
using JornadaMilhas.Dominio.Entidades;
using JornadaMilhas.Integrator.Test.API.Fakers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace JornadaMilhas.Integrator.Test.API
{
    public class JornadaMilhasWebApplicationFactory:WebApplicationFactory<Program>
    {
        private readonly IConfigurationRoot _configuration;
        public JornadaMilhasContext context { get; }
        private IServiceScope _scope;
        public JornadaMilhasWebApplicationFactory()
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = config.Build();

            _scope = Services.CreateScope();
            context = _scope.ServiceProvider.GetRequiredService<JornadaMilhasContext>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            string connectionString = _configuration.GetSection("AppSettings")["Principal"];

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<JornadaMilhasContext>));

                services.AddDbContext<JornadaMilhasContext>(options =>
                {
                    options.UseLazyLoadingProxies().UseSqlServer(connectionString);
                });
            });
            base.ConfigureWebHost(builder);
        }

        public async Task<HttpClient> GetClientWithAccessTokenAsync()
        {
            var client = this.CreateClient();

            string user = _configuration.GetSection("DadosAcesso")["User"];
            string password = _configuration.GetSection("DadosAcesso")["Password"];

            var userDTO = new UserDTO
            {
                Email = user,
                Password = password
            };

            var response = await client.PostAsJsonAsync("/auth-login", userDTO);
            
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<UserTokenDTO>();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token!.Token);

            return client;
        }

        public List<OfertaViagem> CriaDadosFake(bool salvar, int quantidade)
        {
            var ofertas = new OfertaViagemDataBuilder().Build(quantidade);

            if (salvar)
            {
                context.OfertasViagem.AddRange(ofertas);
                context.SaveChanges();
            }

            return ofertas;
        }
    }
}
