using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WebClient
{
    static class Program
    {
        static string server = "http://localhost:5000/customers/";
        static HttpClient _httpClient = new HttpClient();
        static async Task Main(string[] args)
        {
            string cmd = "";
            Customer customer = null;
            Console.WriteLine("Команды:");
            Console.WriteLine("/get id - получение пользователя");
            Console.WriteLine("/crt id имя фамилия - создание пользователя");

            cmd = Console.ReadLine();
            while (cmd.ToLower()!= "exit")
            {
                string[] cmdsplt = cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                customer = null;
                if (cmdsplt[0].ToLower().StartsWith("/get") && cmdsplt.Length == 2)
                {
                    if (long.TryParse(cmdsplt[1], out long Id))
                    {
                        try
                        {
                            customer = await GetCustomerAsync(Id);
                            Console.WriteLine(customer.ToString());
                        }
                        catch (HttpRequestException e)
                        {
                            switch (e.StatusCode)
                            {
                                case System.Net.HttpStatusCode.NotFound:
                                    Console.WriteLine($"Пользователя с id - {Id} не существует.");
                                    break;
                                default:
                                    Console.WriteLine("Неизвестная ошибка");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода идентефикатора");
                    }
                }
                else if (cmdsplt[0].ToLower().StartsWith("/crt") && cmdsplt.Length == 4)
                {
                    
                    if (long.TryParse(cmdsplt[1], out long Id))
                    {
                        try
                        {
                            var rslt = await CreateCustomerAsync(new Customer {Id = Id, Firstname = cmdsplt[2], Lastname = cmdsplt[3] });
                            switch (rslt.StatusCode)
                            {
                                case System.Net.HttpStatusCode.OK:
                                    Console.WriteLine($"Пользователь с id - {Id} добавлен.");
                                    break;
                                case System.Net.HttpStatusCode.Conflict:
                                    Console.WriteLine($"Пользователь с id - {Id} уже существует.");
                                    break;
                                case System.Net.HttpStatusCode.InternalServerError:
                                    Console.WriteLine($"Ошибка внутри API сервиса.");
                                    break;
                                default:
                                    Console.WriteLine("Неизвестная ошибка");
                                    break;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Неизвестная ошибка");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода идентефикатора");
                    }
                }
                else Console.WriteLine("Команда не найдена.");
                cmd = Console.ReadLine();
            }
            return;
        }

        static async Task<Customer> GetCustomerAsync(long id)
        {
            return await _httpClient.GetFromJsonAsync<Customer>(server + id);
        }
        static async Task<HttpResponseMessage> CreateCustomerAsync(Customer customer)
        {
            return await _httpClient.PostAsJsonAsync(server, customer);
        }
    }
}