using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Treino_Socket_III.Geral;

namespace Treino_Socket_III.Chat
{
    class ChatCliente
    {
        //Events
        public delegate void UpdateDelegate(object o);
        public UpdateDelegate onUpdate;

        //Chat manager
        Usuario usuario;
        ChatManager chat;

        //Chat connection
        readonly IPHostEntry host;
        readonly IPAddress addr;
        readonly IPEndPoint endPoint;
        readonly Socket socket;
        Thread listenThread;

        public ChatCliente(string username, UpdateDelegate onUpdate)
        {
            this.onUpdate = onUpdate;

            this.usuario = new Usuario(Guid.NewGuid().ToString("N"), username);
            //this.chat = new ChatManager(u);

            try
            {
                host = Dns.GetHostEntry("localhost");
                addr = host.AddressList[0];
                endPoint = new IPEndPoint(addr, 4404);
                socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception e)
            {

            }

        }

        public void Iniciar() {

            this.socket.Connect(endPoint);
            byte[] bytes = new byte[1024];
            socket.Receive(bytes);
            string mensagem = "Olá mundo";
            this.socket.Send(Encoding.Default.GetBytes(mensagem));
            listenThread = new Thread(this.Ouvir);
            //listenThread.SetApartmentState(ApartmentState.STA);
            listenThread.Start();
        }

        /// <summary>
        /// Listen for messages and users from the server
        /// </summary>
        private void Ouvir()
        {
            object recebido;
            while (true) {

                recebido = this.Receber(socket);
                if (recebido is Mensagem)
                {
                    AdicionarMensagem((Mensagem)recebido);
                }
                else if (recebido is Usuario) {
                    this.AdicionarUsuario((Usuario)recebido);
                }
            }
        }
        private void AdicionarUsuario(Usuario user)
        {
            if(user.ID != usuario.ID && chat.AddUser(user))
            onUpdate(user);
        }

        private object Receber(Socket s)
        {
            byte[] buffer = new byte[1024];
            s.Receive(buffer);
            return BinarioSerializacao.Deserializar(buffer);
        }
        private void AdicionarMensagem(Mensagem m)
        {

            onUpdate(m);
        }
    }
}
