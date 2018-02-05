using System.Collections.Specialized;

namespace Common
{
    public interface ICurrentUser
    {
        int? Id { get; }

        string Name { get; }

        NameValueCollection Cookie { get; }
    }
}
