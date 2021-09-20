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
        public string ID { get; set; }
        public string Nome { get; set; }

        public Usuario(string lg = "", string pwd = "", string id = "", string nome = "") {
            this.Login = lg;
            this.Senha = pwd;
            this.Nome = nome;
            this.ID = id;
        }
    

    }
}
