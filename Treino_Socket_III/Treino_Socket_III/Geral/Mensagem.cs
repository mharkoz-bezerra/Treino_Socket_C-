using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treino_Socket_III.Geral
{
    [Serializable]
    class Mensagem
    {
        public Usuario DeUsuario { get; set; }
        public Usuario ParaUsuario { get; set; }
        public string Msg { get; set; }

        public Mensagem(Usuario de, Usuario para, string mensagem) {
            this.DeUsuario =de;
            this.ParaUsuario = para;
            this.Msg = mensagem; 
        }

    }
}
