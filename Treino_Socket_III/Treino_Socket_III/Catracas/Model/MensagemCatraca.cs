using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treino_Socket_III.Catracas.Model
{
    class MensagemCatraca
    {
        public DateTime Data { get; set; }
        public string Parametro { get; set; }
        public int NumeroCartao { get; set; }
        public int CodigoComando { get; set; }
        public string DescricaoCodigo { get; set; }
    }
}
