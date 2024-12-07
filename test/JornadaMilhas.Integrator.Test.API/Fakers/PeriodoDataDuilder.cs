using Bogus;
using JornadaMilhas.Dominio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Integrator.Test.API.Fakers
{
    public class PeriodoDataDuilder:Faker<Periodo>
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public PeriodoDataDuilder()
        {
            CustomInstantiator(f =>
            {
                var dataInicio = DataInicio ?? f.Date.Soon();
                var dataFim = DataFim ?? dataInicio.AddDays(7);
                return new Periodo(dataInicio, dataFim);
            });
        }

        public Periodo Build() => Generate();
    }
}
