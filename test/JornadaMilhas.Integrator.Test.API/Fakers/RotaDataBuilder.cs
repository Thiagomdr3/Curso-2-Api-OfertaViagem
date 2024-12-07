using Bogus;
using JornadaMilhas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integrator.Test.API.Fakers
{
    public class RotaDataBuilder:Faker<Rota>
    {
        public string? Origem { get; set; }
        public string? Destino { get; set; }

        public List<string> Estados = new List<string>
            {
                "Acre", "Alagoas", "Amapá", "Amazonas", "Bahia", "Ceará", "Distrito Federal",
                "Espírito Santo", "Goiás", "Maranhão", "Mato Grosso", "Mato Grosso do Sul",
                "Minas Gerais", "Pará", "Paraíba", "Paraná", "Pernambuco", "Piauí", "Rio de Janeiro",
                "Rio Grande do Norte", "Rio Grande do Sul", "Rondônia", "Roraima", "Santa Catarina",
                "São Paulo", "Sergipe", "Tocantins"
            };

        public RotaDataBuilder()
        {
            CustomInstantiator(f =>
            {
                var origem = Origem ?? f.PickRandom(Estados);
                var destino = Destino ?? f.PickRandom(Estados.Where(s => s != origem));
                return new Rota(origem, destino);
            });
        }

        public Rota Build() => Generate();
    }
}
