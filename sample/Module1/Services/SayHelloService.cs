using System;

namespace Module1.Services
{
    public class SayHelloService : ISayHelloService
    {
        public SayHelloService()
        {
            // Console.WriteLine("HELLO FROM SAYHELLO SERVICE");
        }

        public void SayHello() {
            Console.WriteLine("HELLO FROM SAYHELLO SERVICE... AGAIN");
        }
    }
}
