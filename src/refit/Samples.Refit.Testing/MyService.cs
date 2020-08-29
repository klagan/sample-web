using System.Threading.Tasks;

namespace Samples.Refit.Http.Client
{
    public class MyService : IMyApi
    {
        private readonly IMyApi _api;

        public MyService(IMyApi api)
        {
            _api = api;
        }

        public Task<TodoItem> GetTodo(int itemNumber)
        {
            return _api.GetTodo(itemNumber);
        }
    }
}
