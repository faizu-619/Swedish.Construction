using Microsoft.AspNetCore.Mvc;

namespace Payments.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentServiceController : ControllerBase
    {
        private readonly ILogger<PaymentServiceController> _logger;
        private readonly IPaymentAPI _paymentGatewayApi;
        
        public PaymentServiceController(IPaymentAPI paymentGatewayApi, ILogger<PaymentServiceController> logger)
        {
            _logger = logger;
            _paymentGatewayApi = paymentGatewayApi;
        }

        [HttpPost]
        public async Task Post(string cardNumber, decimal amount)
        {

            // Process the payment using the payment gateway API
            var paymentRequest = new PaymentRequest
            {
                CreditCardNumber = cardNumber,
                ExpirationDate = "12/24",
                Amount = amount,
            };

            var paymentResponse = await _paymentGatewayApi.ProcessPaymentAsync(paymentRequest);

            // Store the payment information in the database
            var payment = new Payment
            {
                Amount = amount,
                TransactionId = paymentResponse.TransactionId,
                Status = paymentResponse.Status,
                Date = DateTime.UtcNow,
            };
        }
    }
}