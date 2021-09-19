using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Treino_Socket_III.Geral
{
    [Serializable]
    class Usuario
    {
        public string Login { get; set; }
        public string Senha { get; set; }

        public Usuario(string lg, string pwd) {
            this.Login = lg;
            this.Senha = pwd;
        }

    }
}
