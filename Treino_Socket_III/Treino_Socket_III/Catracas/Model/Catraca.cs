using System;

namespace Treino_Socket_III.Catracas.Model
{
   public class Catraca
    {
            public long ID { get; set; }
            public bool Ativo { get; set; }
            /// <summary>
            ///  NumeroInner é referente ao número de catracas que um mesmo local possa ter: 1:N
            /// </summary>
            public int NumeroInner { get; set; }
            public int Porta { get; set; }
            public int QuantidadeDigitos { get; set; }
            public int TipoConexao { get; set; }
            public int TipoLeitor { get; set; }
            public int PadraoCartao { get; set; }
            public int TipoEquipamento { get; set; }
            public int Acionamento { get; set; }
            public int TipoMarca { get; set; }
            public string IPComunicacao { get; set; }
        
    }
}
