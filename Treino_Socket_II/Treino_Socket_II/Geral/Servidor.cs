using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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

            Thread tr;
            while (true) {
                Console.WriteLine("Aguardando Conexão....");
                SctCliente = SctServidor.Accept();
                tr = new Thread(ClienteConexao);
                tr.Start(SctCliente);
                Console.WriteLine("Conexão realizada!");
            }
        }
        public void Ouvir() {
            byte[] bytes;
            while (true)
            {
                bytes = new byte[1024];
                SctCliente.Receive(bytes);
                string mensagem = deBytesParaString(bytes);
                Console.WriteLine($"Cliente: {deBytesParaString(bytes)}");    //Informa a mensagem
                bytes = null;
            }
        }
        public void Responder() {
            
               
           
        }
        public void ClienteConexao(object sct) {

            Socket sCliente = (Socket)sct;
            try {
                while (true)
                {
                    byte[] bytes = new byte[1024];                            //Número máximo que poderá receber 1 MegaByte
                    string mensagem = string.Empty;
                    sCliente.Receive(bytes);                                //Passa valores recebidos para o array de bytes
                    mensagem = Encoding.ASCII.GetString(bytes);             //Converte o byte para string.
                    int finalIndex = mensagem.IndexOf('\0');                //Remove tudo que contert \0
                    if (finalIndex > 0)
                        mensagem = mensagem.Substring(0, finalIndex);

                    Console.WriteLine($"Cliente: {mensagem}");    //Informa a mensagem
                    Console.Out.Flush();                                    //Obriga a atualização do console
                    Console.Write("Responder : ");
                    string resposta = Console.ReadLine();
                    bytes = Encoding.ASCII.GetBytes(resposta);
                    SctCliente.Send(bytes);
                }
            }
            catch (SocketException se) {

                Console.WriteLine("Cliente desconectado {0}", se.Message);
            }
            

        }

        /// <summary>
        /// Converte uma array de bytes em string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string deBytesParaString(byte[] bytes) {
            string mensagem = Encoding.ASCII.GetString(bytes);
            int finalDeMensagem = mensagem.IndexOf('\0');
            if (finalDeMensagem > 0)
                mensagem = mensagem.Substring(0, finalDeMensagem);
            return mensagem;
        }
        #region Métodos
        #endregion

    }
}
