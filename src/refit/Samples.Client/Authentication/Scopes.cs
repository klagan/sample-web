namespace Samples.HttpClientCon.Authentication
{
    using System.Collections.ObjectModel;
    using Contract;

    public class Scopes : Collection<string>, IScopes
    { }
}