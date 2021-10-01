using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Treino_Socket_III.Catracas.Model;

namespace Treino_Socket_III.Catracas
{
    class ServidorHenry
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
        public ServidorHenry(string ip, int porta)
        {

            Host = Dns.GetHostByName(ip);
            IPServidor = Host.AddressList[0];
            IPPontoFinal = new IPEndPoint(IPServidor, porta);

            SctServidor = new Socket(IPServidor.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            SctServidor.Bind(IPPontoFinal);       //Inicia a ouvir o cliente;
            SctServidor.Listen(5);                //Número máximo que servidor poderá escutar

        }
        /// <summary>
        ///  Usado para inicar uma comunicação um cliente.6207713
        ///  
        /// </summary>
        public void Iniciar()
        {

            Thread tr;
            while (true)
            {
                Console.WriteLine("Aguardando Conexão....");
                SctCliente = SctServidor.Accept();
                tr = new Thread(ClienteConexao);
                tr.Start(SctCliente);
                Console.WriteLine("Conexão realizada!");
            }
        }
        public void Ouvir()
        {
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
        public void Responder()
        {



        }
        public void ClienteConexao(object sct)
        {

            Socket sCliente = (Socket)sct;
            
            //string mensagem = string.Empty;
            try
            {
                MensagemCatraca mensagem;
                while (true)
                {
                    mensagem = null;
                    string msg = string.Empty;
                    byte[] bytes = new byte[1024];                              //Número máximo que poderá receber 1 MegaByte
                                                                                //sCliente.Receive(bytes);                                    //Passa valores recebidos para o array de bytes
                    //Envia a solicitação                                                         //mensagem = Encoding.ASCII.GetString(bytes);                 //Converte o byte para string.
                    mensagem = SolicitarAcesso();
                    string msgEnviar = $"{mensagem.Parametro}]{mensagem.NumeroCartao}]{mensagem.Data.ToString("dd/MM/yyyy HH:mm:ss")}]1]0]0";
                    byte[] paraEnviar = Encoding.ASCII.GetBytes(msgEnviar);
                    sCliente.Send(paraEnviar);

                    //Recebe a comunicação
                    SctCliente.Receive(bytes);
                    msg = deBytesParaString(bytes);
                    Console.WriteLine($"Cliente: {msg}");    //Informa a mensagem

                    Thread.Sleep(3000); 
                }
            }
            catch (SocketException se)
            {

                Console.WriteLine("Cliente desconectado {0}", se.Message);
            }


        }

        /// <summary>
        /// Converte uma array de bytes em string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string deBytesParaString(byte[] bytes)
        {
            string mensagem = Encoding.ASCII.GetString(bytes);
            int finalDeMensagem = mensagem.IndexOf("\0");
            if (finalDeMensagem > 0)
                mensagem = mensagem.Substring(0, finalDeMensagem);
            return mensagem;
        }

        public MensagemCatraca SolicitarAcesso() {
            int iResposta = 0;
            bool perguntar = true;
            MensagemCatraca mensagem = null;
            while (perguntar)
            {
                Console.WriteLine("# =   Informe a mensagem que será enviada                        = # ");
                Console.WriteLine("# =                                                              = # ");
                Console.WriteLine("# =                                                              = # ");
                Console.WriteLine("# =   Digite: 1 - Para Catraca aguardar                          = # ");
                Console.WriteLine("# =   Digite: 2 - Para solicitar acesso.                         = # ");
                string resposta = Console.ReadLine();

                if (!int.TryParse(resposta, out iResposta))
                {
                    Console.WriteLine("#----------------------------------------------------------------- # ");
                    Console.WriteLine("# =  OPÇÃO INCORRETA                                             = # ");
                }
                switch (iResposta)
                    {
                        case 1:
                            perguntar = false;
                            mensagem = new MensagemCatraca
                            {
                                Data = DateTime.Now,
                                Parametro = "56+REON+000+81",
                                NumeroCartao = 0,
                            };
                            break;
                        case 2:
                            Console.WriteLine("#----------------------------------------------------------------- # ");
                            Console.Write("# =   Informe o número do cartão (apenas número):");
                            resposta = Console.ReadLine();
                            int numero = 0;
                            if (!int.TryParse(resposta, out numero))
                            {
                                Console.WriteLine("#----------------------------------------------------------------- # ");
                                Console.WriteLine("# =  NÃO UTILIZE LETRAS OU ESPAÇOS                               = # ");
                                break;
                            }
                            perguntar = false;
                            mensagem = new MensagemCatraca
                                {
                                    Data = DateTime.Now,
                                    Parametro = "041+REON+000+0",
                                    NumeroCartao = numero,
                                };
                            
                            
                            break;
                        default:
                            Console.WriteLine("#----------------------------------------------------------------- # ");
                            Console.WriteLine("# =  OPÇÃO INCORRETA                                             = # ");
                            break;
                    }
                
                
                }
                return mensagem;
            }
        }
        
        #region Métodos
        #endregion

    
}
