using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Treino_Socket_II.Geral;
namespace Treino_Socket_II
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            bool _rodando = true;
            string informeUsuario = string.Empty;
            int numero = 0;
            Console.WriteLine("# ================================================================ # ");
            Console.WriteLine("# =                                                              = # ");
            Console.WriteLine("# =                                                              = # ");
            Console.WriteLine("# =    Projeto de Estudo Cliente-Servidor Com Socket Sinncrona   = # ");
            Console.WriteLine("# =                                                              = # ");
            Console.WriteLine("# =  by: Mharkoz Bezerra - 2021-09-18 23:27 V.001   C#           = # ");
            Console.WriteLine("# ================================================================ # ");
            Console.WriteLine("# =                                                              = # ");
            Console.WriteLine("# =                                                              = # ");
            Console.WriteLine("# ================================================================ # ");
            Console.WriteLine("# =                                                              = # ");
            Console.WriteLine("# =____________________  Menu Explicativo  ______________________= # ");
            Console.WriteLine("# =                                                              = # ");
            Console.WriteLine("# =   Digite: 1 - Para rodar como  servidor.                     = # ");
            Console.WriteLine("# =   Digite: 2 - Para rodar como  Cliente .                     = # ");
            Console.WriteLine("# =   Digite: 9 - Para sair.                                     = # ");
            Console.Write("# Informe sua opção:");
            informeUsuario = Console.ReadLine();
            Console.WriteLine("# ================================================================ # ");
            
            while(_rodando){

                if(!int.TryParse(informeUsuario, out  numero)){
                    informeUsuario = string.Empty;
                    Console.WriteLine("# =   Opção inválida, tente novamente!                           = # ");
                    Console.WriteLine("# =                                                              = # ");
                    Console.WriteLine("# =                                                              = # ");
                    Console.WriteLine("# =   Digite: 1 - Para rodar como  servidor.                     = # ");
                    Console.WriteLine("# =   Digite: 2 - Para rodar como  Cliente .                     = # ");
                    Console.WriteLine("# =   Digite: 9 - Para sair.                                     = # ");
                    Console.Write("# Informe sua opção:");
                    informeUsuario = Console.ReadLine();
                    return;
                }
                switch (numero)
                {
                    case 1:
                        Servidor servidor = new Servidor("127.0.0.1",8080);
                        servidor.Iniciar();
                        servidor.Ouvir();
                        break;
                    case 2:
                        Cliente cliente = new Cliente("127.0.0.1", 8080);
                        cliente.Iniciar();
                        cliente.Conversar();
                        break;
                    case 9: 
                        _rodando = false;
                        Console.WriteLine("# =   Até mais!!!                                                = #");
                    break;
                    default:
                        informeUsuario = string.Empty;
                        Console.WriteLine("# =   Opção inválida, tente novamente!                           = #");
                        Console.WriteLine("# =                                                              = #");
                        Console.WriteLine("# =                                                              = #");
                        Console.WriteLine("# =   Digite: 1 - Para rodar como  servidor.                     = #");
                        Console.WriteLine("# =   Digite: 2 - Para rodar como  Cliente .                     = #");
                        Console.WriteLine("# =   Digite: 9 - Para sair.                                     = #");
                        Console.Write("# Informe sua opção:");
                        informeUsuario = Console.ReadLine();
                        break;
                }
                
            }
            Console.ReadKey();
        }
    }
}
