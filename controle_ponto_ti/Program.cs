using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controle_ponto_ti
{
    internal class Program
    {
        static void Main(string[] args)
        {// Nome do arquivo de texto para armazenar as informações
            string nomeArquivo = "registro_ponto.txt";

            // Solicitar informações do funcionário
            Console.Write("Digite o nome do funcionário: ");
            string nomeFuncionario = Console.ReadLine();

            // Obter a data atual
            DateTime dataAtual = DateTime.Now.Date;

            // Verificar se há um registro para o dia atual
            string registroExistente = LerRegistroPonto(nomeArquivo, nomeFuncionario, dataAtual);

            if (registroExistente != null)
            {
                Console.WriteLine($"Registro de ponto para {dataAtual:dd/MM/yyyy} já existe:\n{registroExistente}");
            }
            else
            {
                // Solicitar a quantidade de horas extras apenas para o dia atual
                Console.Write("Digite a quantidade de horas extras para hoje: ");
                double horasExtrasHoje = Convert.ToDouble(Console.ReadLine());

                // Calcular o total de horas extras acumuladas
                double horasExtrasAcumuladas = horasExtrasHoje;

                // Obter as horas extras acumuladas de dias anteriores
                string registroAnterior = LerRegistroPonto(nomeArquivo, nomeFuncionario, dataAtual.AddDays(-1));
                if (registroAnterior != null)
                {
                    horasExtrasAcumuladas += ExtrairHorasExtras(registroAnterior);
                }

                // Formatar a string com as informações
                string registroPonto = $"{dataAtual:dd/MM/yyyy} - {nomeFuncionario} - Horas Extras Acumuladas: {horasExtrasAcumuladas}";

                // Salvar as informações no arquivo de texto
                SalvarRegistroPonto(nomeArquivo, registroPonto);

                Console.WriteLine("Registro de ponto salvo com sucesso!");
            }

            Console.ReadLine(); // Aguarda a entrada do usuário para fechar a aplicação
        }

        // Método para salvar o registro de ponto no arquivo
        static void SalvarRegistroPonto(string nomeArquivo, string registroPonto)
        {
            try
            {
                // Utiliza StreamWriter para escrever no arquivo de texto
                using (StreamWriter sw = new StreamWriter(nomeArquivo, true))
                {
                    sw.WriteLine(registroPonto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar o registro de ponto: {ex.Message}");
            }
        }

        // Método para ler o registro de ponto do arquivo
        static string LerRegistroPonto(string nomeArquivo, string nomeFuncionario, DateTime data)
        {
            try
            {
                // Utiliza StreamReader para ler o arquivo de texto
                using (StreamReader sr = new StreamReader(nomeArquivo))
                {
                    string linha;
                    while ((linha = sr.ReadLine()) != null)
                    {
                        // Procura por uma linha correspondente ao funcionário e à data
                        if (linha.Contains($"{data:dd/MM/yyyy} - {nomeFuncionario}"))
                        {
                            return linha;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler o registro de ponto: {ex.Message}");
            }

            return null;
        }

        // Método para extrair as horas extras de um registro de ponto
        static double ExtrairHorasExtras(string registro)
        {
            // Procura pela substring que contém as horas extras
            int indiceInicio = registro.IndexOf("Horas Extras Acumuladas: ") + 26;
            int indiceFim = registro.IndexOf("'", indiceInicio);
            string horasExtrasString = registro.Substring(indiceInicio, indiceFim - indiceInicio);

            // Converte a string para um número de ponto flutuante
            if (double.TryParse(horasExtrasString, out double horasExtras))
            {
                return horasExtras;
            }

            return 0.0;
        
        }
    }
}
