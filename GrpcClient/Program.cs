using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;
using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("First Service - greet Proto");

            var input = new HelloRequest { Name = "Teva"};
            var channel = GrpcChannel.ForAddress("https://localhost:7027");
            var client = new Greeter.GreeterClient(channel);

            var reply = await client.SayHelloAsync(input);

            Console.WriteLine(reply.Message);



            Console.WriteLine("Second Service - Customer Proto");

            var customerclient = new Customers.CustomersClient(channel);

            var clientRequest = new CustomerLookupModel { UserId = 1 };

            var customer = await customerclient.GetCustomerInfoAsync(clientRequest);

            Console.WriteLine($"{ customer.FirstName} {customer.LastName}");

            Console.WriteLine("New Customer List");

            using (var call = customerclient.GetNewCustomer(new NewCustomerRequest()))
            {
                while(await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;

                    Console.WriteLine($"{currentCustomer.FirstName} : {currentCustomer.LastName}: {currentCustomer.EmailAddress}");
                }
            }

            Console.ReadLine();

        }
    }
}
