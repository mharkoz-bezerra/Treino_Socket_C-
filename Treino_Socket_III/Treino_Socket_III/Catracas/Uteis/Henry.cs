using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Treino_Socket_III.Catracas.Model;
using Treino_Socket_III.Catracas.Controller;
using System.Threading;

namespace Treino_Socket_III.Catracas
{
    public class Henry
    {
        #region Variáveis
        private static int TipoEquipamento { get; set; }
        private static Socket SctCatraca { get; set; }
        public static Object Banco { get; set; }

        #endregion Variáveis
        public Henry()
        {
        }

        #region Métodos
        private static async void Ouvir(object objSocket)
        {
            try
            {
                Socket sct = (Socket)objSocket;
                if (!sct.Connected)
                {
                    Console.WriteLine($"Não há conexao para ser ouvida ");
                    return;
                }
                while (sct.Connected)
                {
                    //Recebe a mensagem enviada
                    string msgRecebida = FuncoesHenry.Receber(sct);
                    MensagemCatraca msg = FuncoesHenry.RecuperarMensagem(msgRecebida);
                    //Verifica a informação
                    if (msg.NumeroCartao <= 0)
                    {
                        msg = null;
                        continue;
                    }
                    //Formata a mensagem
                    string responder = PrepararComando(msg.NumeroCartao);
                    responder = FuncoesHenry.FormatarTexto(responder);
                    //Envia a mensagem para a catraca.
                    FuncoesHenry.Enviar(responder, sct);

                    //Limpa as informações
                    msg = null;
                    msgRecebida = string.Empty;
                    responder = string.Empty;
                }
            }
            catch (SocketException s_erro)
            {
                Console.WriteLine($"Erro no método ouvir causado pelo socket {s_erro.Message}");
            }

        }
        /// <summary>
        /// Utilizado quando é execultado dentro de um laço infinito, e se quer ficar rodando.
        /// </summary>
        /// <param name="c"></param>
        public static void Execultar(Catraca c)
        {
#pragma warning disable CS0612 // Type or member is obsolete
            SctCatraca = FuncoesHenry.Configurar(c.IPComunicacao, c.Porta);
#pragma warning restore CS0612 // Type or member is obsolete
            SctCatraca.Connect(FuncoesHenry.IPPontoFinal);
            TipoEquipamento = c.TipoEquipamento;

            if (!SctCatraca.Connected)
                return;
            try
            {
                //Trabalhar com Thread
                //Thread th = new Thread(Ouvir);
                //th.Start(SctCatraca);
                
                //Trabalhar com Task
                Task task = Task.Factory.StartNew(() => Ouvir(SctCatraca));
                task.Wait();
            }
            catch (ThreadAbortException tErroAborta)
            {
                Console.WriteLine($"Erro, thread abortada {tErroAborta.Message}");
            }
            catch (ThreadInterruptedException tErroInterrupcao)
            {
                Console.WriteLine($"Erro, thread interrompida {tErroInterrupcao.Message}");
            }
            catch (SocketException sctErro)
            {
                Console.WriteLine($"Erro, na comunicação socket {sctErro.Message}");
            }
        }
        /// <summary>
        /// Utilizando quando se quer puxar a informação uma única vez.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string Escutar(Catraca c)
        {
            SctCatraca = FuncoesHenry.Configurar(c.IPComunicacao, c.Porta);
            SctCatraca.Connect(FuncoesHenry.IPPontoFinal);
            TipoEquipamento = c.TipoEquipamento;

            if (!SctCatraca.Connected)
                return "Equipamento solicita não está conectado no momento!";

            //Recebe a mensagem enviada
            string msgRecebida = FuncoesHenry.Receber(SctCatraca);
            MensagemCatraca msg = FuncoesHenry.RecuperarMensagem(msgRecebida);

            //Verifica a informação

            if (msg.NumeroCartao <= 0)
                return "Cartão não foi informado/Registrado...";

            //Formata a mensagem
            string responder = string.Empty;
            responder = PrepararComando(msg.CodigoComando);
            responder = FuncoesHenry.FormatarTexto(responder);
            FuncoesHenry.Enviar(responder, SctCatraca);
            return msg.DescricaoCodigo;

        }
        private static bool VerificarNumeroComanda(int numeroComanda, object banco)
        {
            //Aplicar a lógica para consultar o cartão no banco de dados
            return numeroComanda > 0;
        }
        private static string PrepararComando(int numeroComanda)
        {
            if (numeroComanda <= 0)
                return "";

            string comando = "01+REON+00+30]7] B L O Q U E A D O ]";
            if (VerificarNumeroComanda(numeroComanda, Banco))
            {
                //Verifica o tipo da definição atribuida da catraca
                switch (TipoEquipamento)
                {
                    case 1://1 - Catraca_Entrada_E_Saida
                    case 6://6 - Catraca_Liberada_2_Sentidos
                        comando = "01+REON+00+1]7] L I B E R A D O]";
                        break;
                    case 2: //2 - Catraca_Entrada
                    case 5: //5 - Catraca_Entrada_Liberada
                        comando = "01+REON+00+5]7] L I B E R A D O]";
                        break;
                    case 3:  //3 - Catraca_Saida
                    case 4:  //4 - Catraca_Saida_Liberada
                        comando = "01+REON+00+6]7] L I B E R A D O]";
                        break;
                    default:
                        //0 - Coletor
                        //7 - Catraca_Sentido_Giro
                        //8 - Catraca_Urna
                        //9 - Catraca_Expedidora
                        //Não há dados na documentação para esse tipo até o momento 17/09/21 15:07
                        comando = "01+REON+00+1]7] L I B E R A D O]";
                        break;
                }

            }
            return comando;
        }
        #endregion Métodos

    }
}
