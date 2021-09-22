using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Treino_Socket_III.Catracas.Model;

namespace Treino_Socket_III.Catracas.Controller
{
    class FuncoesHenry
    {
        #region  Variáveis
        private static IPHostEntry Host { get; set; }
        private static IPAddress IPServidor { get; set; }
        public static IPEndPoint IPPontoFinal { get; set; }

        #endregion
        public static Socket Configurar(string host, int porta) {
            try
            {
                Host = Dns.GetHostByName(host);
                IPServidor = Host.AddressList[0];
                IPPontoFinal = new IPEndPoint(IPServidor, porta);
                return new Socket(IPServidor.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                
            }
            catch (SocketException serror) {
                Console.WriteLine(serror.Message);
                return null;            }
        }
        public static void Enviar(string mensagem, Socket sct) {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(mensagem);
                sct.Send(bytes);
            }
            catch (SocketException serror)
            {
                Console.WriteLine($"Erro ao enviar mensagem do tipo: {serror.Message}");
            }
        }
        public static string Receber(Socket stc) {
            try
            {
                byte[] bytes = new byte[1024];
                stc.Receive(bytes);
                string recebida = deBytesParaString(bytes);
                return recebida;
            }
            catch (SocketException serror) {
                Console.WriteLine($"Erro ao receber mensagem : {serror.Message}");
                return string.Empty;
            }
        }
        
        public static string FormatarTexto(string comando) {
            //O comando precisa ser informado um tamanho máximo e tamanho minimo
            string preComando = $"{(char)(comando.Length)}{(char)(0)}{comando}";
            return $"{(char)(2)}{preComando}{(char)(Convert.ToByte(CalcularStringCheckSum(preComando)))}{(char)(3)}";
        }
        private static byte CalcularStringCheckSum(string sTexto)
        {
            byte bit = 0;
            int iValor = 0;
            while (iValor < sTexto.Length)
            {
                bit = (byte)(bit ^ (byte)(sTexto.ElementAt(iValor)));
                iValor++;
            }
            return bit;

        }
        private static string deBytesParaString(byte[] bytes)
        {
            string mensagem = Encoding.ASCII.GetString(bytes);
            int finalDeMensagem = mensagem.IndexOf('\0');
            if (finalDeMensagem > 0)
                mensagem = mensagem.Substring(0, finalDeMensagem);
            return mensagem;
        }

        private static string RespostaHenry(int numero)
        {
            switch (numero)
            {
                case 00: return "Não há erro";
                case 01: return "Não há Dados";
                case 10: return "Comando Desconhecido";
                case 11: return "Tamanho do pacote é inválido";
                case 12: return "Parâmetros informados são inválidos";
                case 13: return "Erro de checksum";
                case 14: return "Tamanho dos parâmetros são inválidos";
                case 15: return "Número da mensagem é inválido";
                case 16: return "Start Byte é inválido";
                case 17: return "Erro para receber pacote";
                case 20: return "Não há empregador cadastrado";
                case 21: return "Não há usuários cadastrados";
                case 22: return "Usuário não cadastrado";
                case 23: return "Usuário não cadastrado";
                case 24: return "Limite de cadastro de usuários atingido";
                case 25: return "Equipamento não possui biometria";
                case 26: return "Index biométrico não encontrado";
                case 27: return "Limite de cadastro de digitais atingido";
                case 28: return "Equipamento não possui eventos";
                case 29: return "Erro na manipulação de biometrias";
                case 30: return "Documento do empregador é inválido";
                case 31: return "Tipo do documento do empregador é inválido";
                case 32: return "Ip é inválido";
                case 33: return "Tipo de operação do usuário é inválida";
                case 34: return "Identificador do empregado é inválido";
                case 35: return "Documento do empregador é inválido";
                case 36: return "Referencia do empregado é inválida";
                case 37: return "Referencia de cartão de usuario é inválida";
                case 43: return "Erro ao gravar dados";
                case 44: return "Erro ao ler dados";
                case 50: return "Erro desconhecido";
                case 61: return "Matrícula já existe";
                case 62: return "Identificador já existe";
                case 63: return "Opção inválida";
                case 64: return "Matrícula não existe";
                case 65: return "Identificador não existe";
                case 66: return "Cartão necessário mas não informado";
                case 180: return "Horário contido no usuário não existe";
                case 181: return "Período contido no horário não existe";
                case 182: return "Escala contida no usuário não existe";
                case 183: return "Faixa de dias da semana não informada ou inválida (acionamento e períodos)";
                case 184: return "Hora não informada ou inválida (acionamento e períodos)";
                case 185: return "Período não informado ou inválido (horários)";
                case 186: return "Horário não informado ou inválido (cartões)";
                case 187: return "Indice não informado ou inválido (horários, periodos e acionamentos)";
                case 188: return "Data não informada ou inválida (feriados)";
                case 189: return "Mensagem não informada (funções)";
                case 190: return "Erro na memoria (acionamento)";
                case 191: return "Mensagem não informada (funções)";
                case 192: return "Informação de tipo de acesso invalida";
                case 193: return "Informação de tipo decartão invalida";
                case 240: return "Registro não foi encontrado (Grupos de acesso, período, horários, acionamentos)";
                case 241: return "Registro já existe (Grupos de acesso, período, horários, acionamentos)";
                case 242: return "Registro não existe (Grupos de acesso, período, horários, acionamentos)";
                case 243: return "Limite atingido (Grupos de acesso, período, horários, acionamentos)";
                case 244: return "Erro no tipo de operação (Grupos de acesso, período, horários, acionamentos)";
                default: return "Reservado";

            }
        }

        public static MensagemCatraca RecuperarMensagem(string mensagem) {

            byte[] bytes = Encoding.ASCII.GetBytes(mensagem);
            MensagemCatraca objMensagem = new MensagemCatraca();
            string[] msgSeparada = mensagem.Split(']');

            string[] comando = msgSeparada[0].Split('+'); 
            int numero = 0;
            if (int.TryParse(msgSeparada[1], out numero))
                objMensagem.NumeroCartao = numero;
            else
                objMensagem.NumeroCartao = 0;

            DateTime dt;
            if (DateTime.TryParse(msgSeparada[2], out dt))
                objMensagem.Data = dt;

            int codcomando = 0;
            if (int.TryParse(comando[3], out codcomando))
                objMensagem.CodigoComando = codcomando; 

            objMensagem.DescricaoCodigo = RespostaHenry(codcomando);
            return objMensagem;
        }
    }
}
