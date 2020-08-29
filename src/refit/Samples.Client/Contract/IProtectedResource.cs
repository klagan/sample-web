namespace Samples.HttpClientCon.Contract
{
    public interface IProtectedResource
    {
        string Name { get; set; }

        IScopes Scopes { get; set; }

        string BaseAddress { get; set; }
    }
}