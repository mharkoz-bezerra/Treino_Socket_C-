using ServidorSocket.Geral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ServidorSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            bool _rodando = true;
            //Coleta a informação para o usuário;
            Console.WriteLine("Sistema de Troca de Mensagens entre cliente-servidor \n");
            

            while (_rodando) {
                int numServidor = 9999;

                Console.WriteLine("Digite as informações presentes para iniciar \n");
                Console.WriteLine("0 - Servidor | 1 - Cliente  |5 - Sair");
                string dados = Console.ReadLine();
                if (!Int32.TryParse(dados, out numServidor)) {
                    Console.WriteLine("Informação passada é incorreta \n");
                    return;
                }

                if (numServidor != 0 ^ numServidor != 1 ^ numServidor != 5) {
                    Console.WriteLine("Informação passada é incorreta \n");
                    return;
                }
                switch (numServidor) {

                    case 0:
                        Console.WriteLine("Modo Servidor Ativo");
                        while (true)
                        {
                            Servidor.Ouvir();
                        }
                        break;
                    case 1:
                        Console.WriteLine("Modo Cliente Ativo");
                        bool sair = false;

                        while (sair == false)
                        {
                            Cliente.EnviarMensagem();

                            Console.WriteLine("Digite: 3 - para continuar | 4 - Para sair");
                            string sSair = Console.ReadLine();
                            sair = sSair == "4";
                        }
                        break;
                    case 5:
                        Console.WriteLine("\n Processo finalizado!!!");
                        _rodando = false;
                        break;
                }
            }
            Console.ReadKey();

        }
    }
}
