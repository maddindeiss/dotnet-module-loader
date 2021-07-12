using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ModuleLoader.Core;
using ModuleWeb.Options;

namespace ModuleWeb.Services
{
    public class InitializeService: IModuleInitialization
    {
        private readonly IOptions<MyOptions> _options;

        public InitializeService(IOptions<MyOptions> options)
        {
            Console.WriteLine("Module2: InitializeService");
            _options = options;
        }

        public async Task Initialize()
        {
            Console.WriteLine("Module2: InitializeService Initialize");
            Console.WriteLine(_options.Value.Key1);
        }
    }
}
