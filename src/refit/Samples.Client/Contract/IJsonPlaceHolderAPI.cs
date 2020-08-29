namespace Samples.HttpClientCon.Contract
{
    using System.Threading.Tasks;
    using Model;
    using Refit;

    public interface IJsonPlaceHolderAPI
    {
        [Get("/todos/{itemNumber}")]
        [Headers("Authorization: Bearer")]
        Task<TodoItem> GetTodo(int itemNumber);
    }
}
