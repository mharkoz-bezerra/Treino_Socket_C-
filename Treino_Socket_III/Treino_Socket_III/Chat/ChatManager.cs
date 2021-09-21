using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treino_Socket_III.Geral;

namespace Treino_Socket_III.Chat
{
    class ChatManager
    {
        LinkedList<Usuario> users;
        LinkedList<Mensagem> messages;
        Usuario current;

        public ChatManager(Usuario current)
        {
            this.users = new LinkedList<Usuario>();
            this.messages = new LinkedList<Mensagem>();
            this.current = current;
        }

        public bool AddUser(Usuario u)
        {
            LinkedListNode<Usuario> currentUser = users.Find(u);

            if (currentUser == null && this.current.ID != u.ID)
            {
                this.users.AddLast(u);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddMessage(Mensagem m)
        {
            this.messages.AddLast(m);
        }

        public LinkedList<Mensagem> GetMessages(Usuario from)
        {
            LinkedList<Mensagem> tmpMSG = new LinkedList<Mensagem>();
            LinkedListNode<Mensagem> currentMSG = this.messages.First;
            while (currentMSG != null)
            {
                if (currentMSG.Value.DeUsuario.ID == from.ID || currentMSG.Value.ParaUsuario.ID == from.ID)
                {
                    tmpMSG.AddLast(currentMSG.Value);
                }
                currentMSG = currentMSG.Next;
            }

            return tmpMSG;
        }

        public LinkedList<Usuario> GetUsers()
        {
            return this.users;
        }
    }
}
