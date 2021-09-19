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

        public void Conversar() {

            try {
                while (true)
                {
                    Console.Write(" -> Mensagem: ");
                    string mensagem = Console.ReadLine();
                    byte[] bytes = new byte[1024];
                    bytes = Encoding.ASCII.GetBytes(mensagem);
                    SctCliente.Send(bytes);
                    Console.Out.Flush();
                    OuvirServidor();
                }
            }
            catch (SocketException se) {
                Console.WriteLine("Conexao com servidor caiu {0}", se.Message);
            }
           
        }

        public void OuvirServidor() {

            byte[] bytes = new byte[1024];
            SctCliente.Receive(bytes);
            String mensagem = deBytesParaString(bytes);
            Console.WriteLine($"Resposta: {mensagem}");
            
        }
        public string deBytesParaString(byte[] bytes)
        {
            string mensagem = Encoding.ASCII.GetString(bytes);
            int finalDeMensagem = mensagem.IndexOf('\0');
            if (finalDeMensagem > 0)
                mensagem = mensagem.Substring(0, finalDeMensagem);
            return mensagem;
        }
    }
}
