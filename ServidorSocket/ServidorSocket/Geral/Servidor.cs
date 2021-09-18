using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServidorSocket.Geral
{
    class Servidor
    {
       private static Socket ouvinte = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
       private static Socket conexao;
       private static IPEndPoint conectar = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

        public static void Ouvir()
        {
            ouvinte.Bind(conectar);                                     //Conecta ao ip que foi informado;
            ouvinte.Listen(3);                                          //Ouve uma quantidade máxima de conexão

            conexao = ouvinte.Accept();                                 //Recebe a conexão que  foi realizada.
            Console.WriteLine($"Conexão aceita :)");

            byte[] bytes = new byte[1024];                              //Tamanho máximo que será suportado;
            string dados = string.Empty;                                //Armazenará as informações que forem recebidas;
            int tamanho_array = 0;                                      //Armazenará o tamanho das informações recebida;


            //conexao.Receive recebe 4 parametros nesse caso [array que será armazenado], [onde começa a armazenar dentro do array], [tamanho máximo desse array],[especifica o buff armazenado, deixe sempre zero]
            tamanho_array = conexao.Receive(bytes, 0, bytes.Length, 0); // Informa o tamanho do arquivo recebido;

            //Recebe as informações
            Array.Resize(ref bytes, tamanho_array);

            dados = Encoding.Default.GetString(bytes);                  //Converte  para uma string os bytes recebidos

            Console.Title = "Mensagem cliente-servidor - [Servidor]";    //Muda o titulo do console
            Console.WriteLine($"Informação recebida é {dados}");        //Informa a mensagem
            Console.ReadKey();                                          //Pausa o sistema para não ser fechado
        }
    }
}
