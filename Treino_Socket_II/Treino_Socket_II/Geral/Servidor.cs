using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Treino_Socket_II.Geral
{
    class Servidor
    {
        #region  Variáveis
        private IPHostEntry Host { get; set; }
        private IPAddress IPServidor { get; set; }
        private IPEndPoint IPPontoFinal { get; set; }
        private Socket SctServidor { get; set; }
        private Socket SctCliente { get; set; }
        #endregion

        ///Construtor da clase;
        [Obsolete]
        public Servidor(string ip, int porta) {

            Host = Dns.GetHostByName(ip);
            IPServidor = Host.AddressList[0];
            IPPontoFinal = new IPEndPoint(IPServidor, porta);

            SctServidor = new Socket(IPServidor.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
            SctServidor.Bind(IPPontoFinal);       //Inicia a ouvir o cliente;
            SctServidor.Listen(5);                //Número máximo que servidor poderá escutar

        }
        /// <summary>
        ///  Usado para inicar uma comunicação um cliente.
        /// </summary>
        public void Iniciar() {
            byte[] bytes = new byte[1024];                          //Número máximo que poderá receber 1 MegaByte
            string mensagem = string.Empty;

            SctCliente = SctServidor.Accept();
           
            SctCliente.Receive(bytes);                              //Passa valores recebidos para o array de bytes
            mensagem = Encoding.ASCII.GetString(bytes);             //Converte o byte para string.
            Console.WriteLine($"Mensagem Recebida: {mensagem}");    //Informa a mensagem

        }


        #region Métodos
        #endregion

    }
}
