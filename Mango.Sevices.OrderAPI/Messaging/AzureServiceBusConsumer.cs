using Azure.Messaging.ServiceBus;
using Mango.Sevices.OrderAPI.Messages;
using Mango.Sevices.OrderAPI.Models;
using Mango.Sevices.OrderAPI.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Sevices.OrderAPI.Messaging
{


    public class AzureServiceBusConsumer:IAzureServiceBusConsumer
    {

        private readonly string serviceBusConnectionString;
        private readonly string subscriptionCheckOut;
        private readonly string checkoutMessageTopic;
        private readonly string orderPaymentProcessTopic;
        private readonly string orderUpdatePaymentResultTopic;



        private ServiceBusProcessor _checkOutProcessor;


        private readonly OrderRepository _orderRepository;
        private readonly IConfiguration _configuration;

        public AzureServiceBusConsumer(OrderRepository orderRepository,IConfiguration configuration)
        {
            this._orderRepository = orderRepository;
            this._configuration = configuration;



            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionCheckOut = _configuration.GetValue<string>("SubscriptionCheckOut");
            checkoutMessageTopic = _configuration.GetValue<string>("CheckoutMessageTopic");
            orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopics");
            orderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);

            _checkOutProcessor = client.CreateProcessor(checkoutMessageTopic, subscriptionCheckOut);
            
        }


        public async Task Start()
        {
            _checkOutProcessor.ProcessMessageAsync += OnCheckOutMessageReceived;
            _checkOutProcessor.ProcessErrorAsync += ErrorHandler;

            await _checkOutProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {

            await _checkOutProcessor.StopProcessingAsync();
            await _checkOutProcessor.DisposeAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // you can log it also
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnCheckOutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            // We can do automapper configuration aswell
            OrderHeader orderHeader = new()
            {
                UserId = checkoutHeaderDto.UserId,
                FirstName = checkoutHeaderDto.FirstName,
                LastName = checkoutHeaderDto.LastName,
                OrderDetails = new List<OrderDetails>(),
                CardNumber = checkoutHeaderDto.CardNumber,
                CouponCode = checkoutHeaderDto.CouponCode,
                CVV = checkoutHeaderDto.CVV,
                DiscountTotal = checkoutHeaderDto.DiscountTotal,
                Email = checkoutHeaderDto.Email,
                ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
                OrderTime = DateTime.Now,
                OrderTotal = checkoutHeaderDto.OrderTotal,
                PaymentStatus = false,
                Phone = checkoutHeaderDto.Phone,
                PickUpDateTime = checkoutHeaderDto.PickUpDateTime
            };
            foreach (var detailList in checkoutHeaderDto.CartDetails)
            {
                OrderDetails orderDetails = new()
                {
                    ProductId = detailList.ProductId,
                    ProductName = detailList.Product.Name,
                    Price = detailList.Product.Price,
                    Count = detailList.Count
                };
                orderHeader.CartTotalItems += detailList.Count;
                orderHeader.OrderDetails.Add(orderDetails);
            }

            await _orderRepository.AddOrder(orderHeader);

        }
    }
}
