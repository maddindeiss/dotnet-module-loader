using System;

namespace Module1.Services
{
    public class SayService : ISayService
    {
        public SayService(ISayHelloService sayHelloService)
        {
            Console.WriteLine("HELLO FROM HELLO SERVICE");
            sayHelloService.SayHello();
        }

        public void SayHello() {
            Console.WriteLine("HELLO FROM HELLO SERVICE... AGAIN");
        }
    }
}
