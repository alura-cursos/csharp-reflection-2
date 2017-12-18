using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Model
{
    public class CalculoCambioModel
    {
        public string MoedaOrigem { get; set; }
        public string MoedaDestino { get; set; }

        public decimal ValorOrigem { get; set; }
        public decimal ValorDestino { get; set; }
    }
}
