using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treino_Socket_III.Catracas.Model;

namespace Treino_Socket_III.Catracas.Uteis
{
    class ServicoSegundoPlano
    {
        public static bool Execultando { get; set; }
        public static void EscutarCatracaHenry() {

            Console.WriteLine("Iniciando modo Henry .....");

            List<Catraca> catracas = new List<Catraca>();

            Catraca catraca1 = new Catraca {
                IPComunicacao = "127.0.0.1",
                Porta = 3000,
            };
            /*
            Catraca catraca2 = new Catraca
            {
                IPComunicacao = "127.",
                Porta = 3000,
            };
            Catraca catraca3 = new Catraca
            {
                IPComunicacao = "10.0.0.235",
                Porta = 3000,
            };
            */
            //Adiciona 3 catracas para teste
            catracas.Add(catraca1);
            //catracas.Add(catraca2);
           // catracas.Add(catraca3);

            
            while (Execultando) {

                foreach (var c in catracas)
                    Henry.Execultar(c);
                

                Console.WriteLine("Sistema de Catraca está rodando...");
                Console.WriteLine("Precione C e depois Enter para Fechar o Sistema");
                string sair = Console.ReadLine();
                sair = sair.ToUpper();
                Execultando = !(sair == "C"); //Realiza uma contradição se(true) = false / se(false) = true;
            }
        }
    }
}
