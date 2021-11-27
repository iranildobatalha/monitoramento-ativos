using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace ProjetoFinanceiro
{
    class Program
    {
        static void Main(string[] args)
        {
            string ativo = args[0];
            double max = Double.Parse(args[1].Replace('.',','));
            double min = Double.Parse(args[2].Replace('.',','));

            Console.WriteLine("Executando...");

            while(true){
                RunAsync(ativo, min, max).Wait();
                Thread.Sleep(15000);
            }
        }

        static async Task RunAsync(string ativo, double min, double max)
        {
            string chave = "59WT4TMME4052POX";
            // replace the "demo" apikey below with your own key from https://www.alphavantage.co/support/#api-key
            
            string BASE_ADDRESS = "https://www.alphavantage.co/";
            string GET_URL = $"query?function=GLOBAL_QUOTE&symbol={ativo}.SA&apikey={chave}";
            
            string assunto = "Conselho sobre o ativo " + ativo;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(BASE_ADDRESS);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(GET_URL);
                if (response.IsSuccessStatusCode)
                {  //GET
                    string respostaServer = await response.Content.ReadAsStringAsync();
                    var json_data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(respostaServer);
                    string resposta = "";

                    double preco = Double.Parse(json_data["Global Quote"]["05. price"].Replace('.', ','));
                    //Console.WriteLine("Preço: " + preco);

                    if (preco < min)
                    {
                        resposta = "Aconselhamos comprar ativo!";
                    }
                    if (preco > max)
                    {
                        resposta = "Aconselhamos vender este ativo!";
                    }

                    if(resposta != ""){
                        //Console.WriteLine(resposta);
                        sendEmail(assunto, resposta);
                    }
                }
            }
        }

        static string sendEmail(string subject, string body)
        {
            try
            {
                // Read the file as one string.
                string text = System.IO.File.ReadAllText(@".\config.json");
                var json_config = JsonSerializer.Deserialize<Dictionary<string, string>>(text);

                string remetente = json_config["remetente"];
                string destinatario = json_config["destinatario"];
                string host = json_config["host_smtp"];
                int port = Convert.ToInt32(json_config["port_smtp"]);
                string user = json_config["user_smtp"];
                string senha = json_config["pass_smtp"];

                // valida o email
                bool bValidaEmail = ValidaEnderecoEmail(destinatario);

                // Se o email não é validao retorna uma mensagem
                if (bValidaEmail == false)
                    return "Email do destinatário inválido: " + destinatario;

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(remetente);
                message.To.Add(new MailAddress(destinatario));
                message.Subject = subject;
                message.IsBodyHtml = false; //to make message body as html  
                message.Body = body;
                smtp.Port = port;
                //smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.Host = host; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(user, senha);
                //smtp.Credentials = new NetworkCredential("iranildobatalha@gmail.com", "Progr4m4dor;");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                
                return "Email enviado com sucesso!";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        static bool ValidaEnderecoEmail(string enderecoEmail)
        {
            try
            {
                //define a expressão regulara para validar o email
                string texto_Validar = enderecoEmail;
                Regex expressaoRegex = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");

                // testa o email com a expressão
                if (expressaoRegex.IsMatch(texto_Validar))
                {
                    // o email é valido
                    return true;
                }
                else
                {
                    // o email é inválido
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
