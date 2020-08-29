namespace Samples.HttpClientCon.Authentication
{
    public class MyResource : ProtectedResource
    {
        public static MyResource Empty()
        {
            return new MyResource() {Name = "Empty", BaseAddress = "https:127.0.0.1", Scopes = new Scopes()};
        }
    }
}