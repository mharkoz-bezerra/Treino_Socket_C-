using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Usados para Serializar=============================
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;

namespace Treino_Socket_III.Geral
{
    
    class BinarioSerializacao
    {
        public static byte[] Serializar(object paraSerializar)
        {

            MemoryStream memoria = new MemoryStream();              //Declara dados para memoria.
            BinaryFormatter formatador = new BinaryFormatter();     //Declara um formatador de dados.

            formatador.Serialize(memoria, paraSerializar);          //Realiza a formatação do objeto e joga para a memória.

            return memoria.ToArray();                               //Retorna o dados já serializados.
        }

        public static object Deserializar(byte[] bytes)
        {
            MemoryStream memoria = new MemoryStream(bytes);             // Declara dados para memoria.
            try
            {
                
                BinaryFormatter formatador = new BinaryFormatter();         // Declara um formatador de dados.
                formatador.Binder = new AtualAssemblyDesarizcaoBinder();    // Declara um novo Binder para ser utilizado.             
                return formatador.Deserialize(memoria);                     // Retorna o dados já deserializados.


            }
            catch (FormatException fe)
            {
                Console.WriteLine("Erro ao realizar a formatação do objeto:{0}", fe.Message);
                return null;
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Erro ao realizar a formatação do objeto:{0}", se.Message);
                return null;
            }
            catch (NullReferenceException ne)
            {
                Console.WriteLine("Erro ao realizar a formatação do objeto:{0}", ne.Message);
                return null;
            }
            finally {
                memoria.Close();
                memoria.Dispose();
            }
        }

    }

    public class AtualAssemblyDesarizcaoBinder : SerializationBinder
    {
        public override Type BindToType(string nomeAssenbly, string nomeTipo)
        { 
            return Type.GetType(String.Format("{0},{1}", nomeTipo, Assembly.GetExecutingAssembly().FullName));
        }
    }
    

}
