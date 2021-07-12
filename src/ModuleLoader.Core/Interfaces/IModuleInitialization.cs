using System.Threading.Tasks;

namespace ModuleLoader.Core
{
    public interface IModuleInitialization
    {
        Task Initialize();
    }
}
