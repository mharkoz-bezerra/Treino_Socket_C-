using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace ServidorSocket.Geral
{
    class Cliente
    {
       private static Socket Ouvinte{get;set;}
       private static IPEndPoint Conectar { get; set; }

        public static void EnviarMensagem() {

            //Verifica se servidor encontra-se online
            if (!ConectarServidor()) {
                Console.WriteLine("Servidor não está Online!!!");
                return;
            }
            
                byte[] bytes = new byte[1024];              //Define o tamanho máximo do byte
                Console.WriteLine("Digite sua mensagem: "); //Solicita ao usuário que digite sua mensagem
                string dados = Console.ReadLine();          //Coleta a informação diggitada pelo usuário
                bytes = Encoding.Default.GetBytes(dados);   //Converte a informação para um array de byte
                Ouvinte.Send(bytes);                        //Envia mensagem para o servidor
                Console.ReadKey();                          //Utilizado para não finalizar o programa
                
            
            
        }
        public static bool ConectarServidor() {
            try
            {
                Ouvinte = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Conectar = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                Ouvinte.Connect(Conectar);
                return Ouvinte.Connected;
            }
            catch (Exception e) {
                Console.WriteLine($"Erro encontrado: {e.Message}");
                return false;
            }
            
        }
    }
}
