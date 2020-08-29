namespace Samples.HttpClientCon.Authentication
{
    using Contract;

    public abstract class ProtectedResource : IProtectedResource
    {
        protected ProtectedResource()
        {
            Scopes = new Scopes();
        }

        public string Name { get; set; }
        public IScopes Scopes { get; set; }
        public string BaseAddress { get; set; }
    }
}