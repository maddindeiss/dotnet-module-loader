using System;
using ModuleLoader.Core;

namespace Module1.Services
{
    public class InitializeService: IServiceInitialization
    {
        private readonly ISayService _sayService;

        public InitializeService(ISayService sayService)
        {
            Console.WriteLine("Module1: InitializeService");
            _sayService = sayService;
        }

        public void Initialize()
        {
            Console.WriteLine("Module1: InitializeService Initialize");
            _sayService.Say();
        }
    }
}
