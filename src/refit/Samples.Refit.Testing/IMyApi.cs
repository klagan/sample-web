using System.Threading.Tasks;
using Refit;

namespace Samples.Refit.Http.Client
{
    public interface IMyApi
    {
        [Get("/todos/{itemNumber}")]
        Task<TodoItem> GetTodo(int itemNumber);
    }
}
