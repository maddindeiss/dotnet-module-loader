using System;

namespace Module1.Services
{
    public class SayService : ISayService
    {
        private readonly ISayHelloService _sayHelloService;
        public SayService(ISayHelloService sayHelloService)
        {
            _sayHelloService = sayHelloService;
            // Console.WriteLine("HELLO FROM HELLO SERVICE");
        }

        public void Say() {
            Console.WriteLine("HELLO FROM SAY SERVICE");
            _sayHelloService.SayHello();
        }
    }
}
