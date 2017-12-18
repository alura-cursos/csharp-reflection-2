using ByteBank.Portal.Infraestrutura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Controller
{
    public class ErroController : ControllerBase
    {
        public string Inesperado()
            => View();
    }
}
