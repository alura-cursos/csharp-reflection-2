using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura.IoC
{
    public class ContainerSimples : IContainer
    {
        private readonly Dictionary<Type, Type> _mapaDeTipos = new Dictionary<Type, Type>();

        // Registrar( typeof(ICambioService), typeof(CambioServiceTeste) );
        
            
            // Registrar( typeof(ICambioService), typeof(CartaServiceTeste) );o
        
            
            // Recuperar( typeof(ICambioService) )
        //   deve retornar para nós uma instância do tipo CambioServiceTeste
        public void Registrar(Type tipoOrigem, Type tipoDestino)
        {
            if (_mapaDeTipos.ContainsKey(tipoOrigem))
                throw new InvalidOperationException("Tipo já mapeado!");

            VerificarHierarquiaOuLancarExcecao(tipoOrigem, tipoDestino);

            _mapaDeTipos.Add(tipoOrigem, tipoDestino);
        }

        public void Registrar<TOrigem, TDestino>() where TDestino : TOrigem
        {
            if (_mapaDeTipos.ContainsKey(typeof(TOrigem)))
                throw new InvalidOperationException("Tipo já mapeado!");

            _mapaDeTipos.Add(typeof(TOrigem), typeof(TDestino));
        }

        private void VerificarHierarquiaOuLancarExcecao(Type tipoOrigem, Type tipoDestino)
        {
            // Verificar se tipoDestino herda ou implementa tipoOrigem
            if(tipoOrigem.IsInterface)
            {
                var tipoDestinoImplementaInterface = 
                    tipoDestino
                        .GetInterfaces()
                        .Any(tipoInterface => tipoInterface == tipoOrigem);

                if (!tipoDestinoImplementaInterface)
                    throw new InvalidOperationException("O tipo destino não implementa a interface");
            }
            else
            {
                var tipoDestinoHerdaTipoOrigem = tipoDestino.IsSubclassOf(tipoOrigem);
                if (!tipoDestinoHerdaTipoOrigem)
                    throw new InvalidOperationException("O tipo destino não herda o tipo de origem");
            }
        }

        public object Recuperar(Type tipoOrigem)
        {
            var tipoOrigemFoiMapeado = _mapaDeTipos.ContainsKey(tipoOrigem);

            if (tipoOrigemFoiMapeado)
            {
                var tipoDestino = _mapaDeTipos[tipoOrigem];
                return Recuperar(tipoDestino);
            }

            var construtores = tipoOrigem.GetConstructors();
            var construtorSemParametros =
                construtores.FirstOrDefault(construtor => construtor.GetParameters().Any() == false);

            if(construtorSemParametros != null)
            {
                var instanciaDeConstrutorSemParametro = construtorSemParametros.Invoke(new object[0]);
                return instanciaDeConstrutorSemParametro;
            }

            var construtorQueVamosUsar = construtores[0];
            var parametrosDoConstrutor = construtorQueVamosUsar.GetParameters();
            var valoresDeParametros = new object[parametrosDoConstrutor.Count()];

            for (int i = 0; i < parametrosDoConstrutor.Count(); i++)
            {
                var paramtro = parametrosDoConstrutor[i];
                var tipoParametro = paramtro.ParameterType;

                valoresDeParametros[i] = Recuperar(tipoParametro);
            }

            var instancia = construtorQueVamosUsar.Invoke(valoresDeParametros);
            return instancia;
        }
    }
}
