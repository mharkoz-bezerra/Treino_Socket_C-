using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Treino_Socket_III.Geral;
using System.Collections;

namespace Treino_Socket_III.Chat
{
    class ChatServidor
    {
        #region  Variáveis
        private IPHostEntry Host { get; set; }
        private IPAddress IPServidor { get; set; }
        private IPEndPoint EndPoint { get; set; }
        private Socket SctServidor { get; set; }
        private Hashtable TabelaDeUsuario { get; set; }
        private Thread OuvirThread { get; set; }
        #endregion

        public ChatServidor() {

            try
            {
                //Seta dados do servidor
                Host = Dns.GetHostEntry("127.0.0.1");
                IPServidor = Host.AddressList[0];
                EndPoint = new IPEndPoint(IPServidor, 4404);

                //Seta dados da conexão socket
                SctServidor = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                SctServidor.Bind(EndPoint);
                SctServidor.Listen(10);
            }
            catch (SocketException s_erro) { Console.WriteLine($"Erro no socket:  {s_erro.Message}"); }
            catch (ThreadAbortException thr_erro) { Console.WriteLine($"Erro thread abortar:  {thr_erro.Message}"); }
            catch (ThreadInterruptedException thr_erro) { Console.WriteLine($"Erro thread interrupção:  {thr_erro.Message}"); }
            catch (Exception e_erro) { Console.WriteLine($"Erro geral:  {e_erro.Message}"); }
        }
        public void LigarServidor() {

            while (true) {

                //Seta dados da thread que será usada.
                OuvirThread = new Thread(this.Ouvir);
                OuvirThread.Start();

                //Seta dados para tabela de usuários;
                TabelaDeUsuario = new Hashtable();
            }

        }
        /// <summary>
        /// Começa a aceitar uma conexão com o cliente
        /// </summary>
        private void Ouvir() {

            Socket cliente;
            while (true) {
                cliente = this.SctServidor.Accept();
                OuvirThread = new Thread(this.OuvirCliente);
                OuvirThread.Start(cliente);

            }
        }
        /// <summary>
        /// Ouve um determinado cliente
        /// </summary>
        private void OuvirCliente(object obj) {

            Socket cliente = (Socket)obj;
            object recebendo;
            do { recebendo = this.Receber(cliente); }
            while ( !(recebendo is Usuario) );

            //Registra novos usuários
            this.TabelaDeUsuario.Add(recebendo, cliente);

            //Comunica todos os usuários que está conectado
            this.EnviarParaTodos(recebendo);

            //Envia a mensagem para todos os usuários;
            this.EnviarParaTodosUsuarios(cliente);

            //Realiza a   opção até que aplicação feche
            while (true) {
                recebendo = this.Receber(cliente);
                if (recebendo is Usuario) {
                    this.EnviarMensagem((Mensagem)recebendo);
                }
            }
        }

        /// <summary>
        /// Envia um objeto para todos clientes conectados - BroadCast
        /// </summary>
        /// <param name="obj"></param>
        private void EnviarParaTodos(object obj) {

            foreach ( DictionaryEntry dicionario in this.TabelaDeUsuario)
            {
                this.Enviar((Socket)dicionario.Value,obj);
            }
        }

        /// <summary>
        /// Envia para todas conexão conectadas a um cliente
        /// </summary>
        /// <param name="sct"></param>
        private void EnviarParaTodosUsuarios(Socket sct) {

            foreach (DictionaryEntry dicionario in this.TabelaDeUsuario)
            {
                this.Enviar(sct, dicionario.Key);
            }
        }
        
        /// <summary>
        /// Envia uma mensagem para um cliente
        /// </summary>
        /// <param name="msg"></param>
        private void EnviarMensagem(Mensagem msg) {

            Usuario tmpUsuario;
            foreach (DictionaryEntry dicionario in this.TabelaDeUsuario)
            {
                tmpUsuario = (Usuario)dicionario.Key;
                if (tmpUsuario.ID == msg.DeUsuario.ID) {
                    this.Enviar((Socket)dicionario.Value,msg);
                    break;
                }
                
            }
        }

        /// <summary>
        /// Envia um objeto ao cliente
        /// </summary>
        /// <param name="sct"></param>
        /// <param name="obj"></param>
        private void Enviar(Socket sct, object obj) {
            byte[] bytes = new byte[1024];
            byte[] bytesObj = BinarioSerializacao.Serializar(obj);
            Array.Copy(bytesObj, bytes, bytesObj.Length);
            sct.Send(bytes);
        }
        /// <summary>
        /// Recebe toda a serialização do objeto
        /// </summary>
        /// <param name="sct"></param>
        private object Receber(Socket sct) {

            byte[] bytes = new byte[1024];
            sct.Receive(bytes);
            return BinarioSerializacao.Deserializar(bytes);
        }
    }
}
