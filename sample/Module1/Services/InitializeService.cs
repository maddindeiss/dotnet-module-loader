using System;
using System.Threading.Tasks;
using ModuleLoader.Core;

namespace Module1.Services
{
    public class InitializeService: IModuleInitialization
    {
        private readonly ISayService _sayService;

        public InitializeService(ISayService sayService)
        {
            Console.WriteLine("Module1: InitializeService");
            _sayService = sayService;
        }

        public async Task Initialize()
        {
            Console.WriteLine("Module1: InitializeService Initialize");
            _sayService.Say();
        }
    }
}
