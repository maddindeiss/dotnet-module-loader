using System;
using System.Threading.Tasks;
using ModuleLoader.Core;

namespace Module1.Services
{
    public class InitializeService2: IModuleInitialization
    {
        private readonly ISayService _sayService;

        public InitializeService2(ISayService sayService)
        {
            Console.WriteLine("Module1: InitializeService2");
            _sayService = sayService;
        }

        public async Task Initialize()
        {
            Console.WriteLine("Module1: InitializeService2 Initialize");
            _sayService.Say();
        }
    }
}
