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
    class Henry
    {
        private int TipoEquipamento { get; set; }
        private Socket SctCatraca { get; set; }
        private void Ouvir(object objSocket)
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
                    byte[] bytes = Encoding.ASCII.GetBytes(msgRecebida);
                    MensagemCatraca msg = FuncoesHenry.RecuperarMensagem(bytes);
                    
                    

                    //Verifica a informação
                    
                    if (msg.NumeroCartao <= 0)
                        continue;


                    //Formata a mensagem
                    string responder = string.Empty;
                    responder = PrepararComando(msg.CodigoComando);
                    responder = FuncoesHenry.FormatarTexto(responder);
                    FuncoesHenry.Enviar(responder, sct);
                    
                }
            }
            catch (SocketException s_erro)
            {

                Console.WriteLine("Erro no método ouvir causado pelo socket {0}",s_erro.Message);
            }
        }
        public void Execultar(Catraca c) {

            SctCatraca = FuncoesHenry.Configurar(c.IPComunicacao, c.Porta);
            SctCatraca.Connect(FuncoesHenry.IPPontoFinal);
            TipoEquipamento = c.TipoEquipamento;
            if (!SctCatraca.Connected)
                return;
            try
            {
                Thread th = new Thread(Ouvir);
                th.Start(SctCatraca);
            }
            catch (ThreadAbortException tErroAborta){
                Console.WriteLine("Erro, thread abortada {0}", tErroAborta.Message);
            }
            catch (ThreadInterruptedException tErroInterrupcao) {
                Console.WriteLine("Erro, thread interrompida {0}", tErroInterrupcao.Message);
            }
            catch (SocketException sctErro) {
                Console.WriteLine("Erro, na comunicação socket {0}", sctErro.Message);
            }

        }
        private bool VerificarNumeroComanda(int numeroComanda)
        {
            Comanda comanda = new Comanda {
                Numero = numeroComanda,
                Pago = true
            };

            return comanda.Pago;

        }

        private string PrepararComando(int numeroComanda)
        {
            if (numeroComanda <= 0)
                return "";

            string comando = "01+REON+00+30]7] B L O Q U E A D O ]";
            if (VerificarNumeroComanda(numeroComanda))
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
    }
}
