using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Treino_Socket_II.Geral
{
    class Cliente
    {
        #region  Variáveis
        private IPHostEntry Host { get; set; }
        private IPAddress IPServidor { get; set; }
        private IPEndPoint IPPontoFinal { get; set; }
        private Socket SctCliente { get; set; }
        #endregion

        public Cliente(string ip, int porta) {

            Host = Dns.GetHostByName(ip);
            IPServidor = Host.AddressList[0];
            IPPontoFinal = new IPEndPoint(IPServidor, porta);

            SctCliente = new Socket(IPServidor.AddressFamily, SocketType.Stream,ProtocolType.Tcp);
            
        }

        public void Iniciar() {
            SctCliente.Connect(IPPontoFinal);
        }

        public void Enviar(string mensagem) {

            byte[] bytes = new byte[1024];
            bytes = Encoding.ASCII.GetBytes(mensagem);
            SctCliente.Send(bytes);
            Console.WriteLine("Mensagem enviada para o servidor!");
        }
    }
}
