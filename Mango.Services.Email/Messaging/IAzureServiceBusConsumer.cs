﻿using System.Threading.Tasks;

namespace Mango.Sevices.EmailAPI.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
