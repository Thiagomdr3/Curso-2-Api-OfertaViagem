using Bogus;
using JornadaMilhas.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integrator.Test.API.Fakers
{
    public class OfertaViagemDataBuilder:Faker<OfertaViagem>
    {
        public OfertaViagemDataBuilder()
        {
            CustomInstantiator(f =>
            {
                return new OfertaViagem
                {
                    Preco = f.Random.Double(100, 1000),
                    Rota = new RotaDataBuilder().Build(),
                    Periodo = new PeriodoDataDuilder().Build()
                };
            });
        }

        public OfertaViagem Build() => Generate();
        public List<OfertaViagem> Build(int qntdd) => Generate(qntdd);
    }
}
