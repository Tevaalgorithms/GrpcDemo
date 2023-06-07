using Grpc.Core;

namespace GrpcServer.Services
{
    public class CustomersService : Customers.CustomersBase
    {
        private readonly ILogger<GreeterService> _logger;

        public CustomersService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new CustomerModel();
            // Simulation of Database lookup
            if(request.UserId == 1)
            {
                output.FirstName = "Teva";
                output.LastName = "Velu";
            }
            else if (request.UserId == 1)
            {
                output.FirstName = "Jane";
                output.LastName = "Doe";
            } 
            else
            {
                output.FirstName = "Greg";
                output.LastName = "Thomas";
            }
            return Task.FromResult(output);
        }

        public override async Task GetNewCustomer(
            NewCustomerRequest request, 
            IServerStreamWriter<CustomerModel> responseStream, 
            ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>()
            { 
                new CustomerModel
                {
                    FirstName = "Teva",
                    LastName = "Velu",
                    EmailAddress = "test123@gmail.com",
                    Age = 101,
                    IsAlive= true,
                },
                 new CustomerModel
                {
                    FirstName = "Sue",
                    LastName = "Strom",
                    EmailAddress = "sure123@gmail.com",
                    Age = 31,
                    IsAlive= true,
                },
                  new CustomerModel
                {
                    FirstName = "Bibilo",
                    LastName = "Beggins",
                    EmailAddress = "bib234@gmail.com",
                    Age = 51,
                    IsAlive= false,
                }
            };

            foreach(var customer in customers)
            {
                // This is just for simulating the delay in getting records
                await Task.Delay(1000);

                await responseStream.WriteAsync(customer);
            }
        }
    }
}
