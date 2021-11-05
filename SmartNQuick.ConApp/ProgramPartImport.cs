using System.Threading.Tasks;

namespace SmartNQuick.ConApp
{
    internal partial class Program
    {
        private static Task ImportDataAsync()
        {
            return Task.Delay(2000);
        }
    }
}
