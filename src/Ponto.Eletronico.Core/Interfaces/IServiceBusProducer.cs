using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponto.Eletronico.Core.Interfaces
{
    public interface IServiceBusProducer
    {
        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <param name="topicName">Name of the topic.</param>
        /// <returns></returns>
        Task SendAsync<T>(T message, string topicName);
    }
}
