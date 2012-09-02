﻿using System;
using System.Net;
using System.Threading;
using DispatcherUtility;
using Hyperletter.Core;
using Hyperletter.Dispatcher;

namespace DispatcherConnectTest {
    public class ConnectProgram {
        public static void Main()
        {
            var hyperSocket = new UnicastSocket();
            var handleDispatcher = new HandlerDispatcher(hyperSocket, new DefaultHandlerFactory(), new JsonTransportSerializer());
            handleDispatcher.Register<TestMessage, MessageHandler>();
            hyperSocket.Connect(IPAddress.Parse("127.0.0.1"), 8900);

            for (int i = 0; i < 100; i++)
            {
                handleDispatcher.Send(new TestMessage { Message = "Message from ConnectProgram " });
                Console.WriteLine(DateTime.Now + " SENT MESSAGE");
                Thread.Sleep(1000);
            }

            Console.WriteLine("Waiting for messages (Press any key to continue)...");
            Console.ReadKey();
        }
    }

    public class MessageHandler : IHandler {
        private readonly TestMessage _message;

        public MessageHandler(TestMessage message) {
            _message = message;
        }

        public void Execute() {
            Console.WriteLine(DateTime.Now + " RECEIVED MESSAGE: " + _message.Message);
        }
    }
}