using System.Collections.Specialized;

namespace Common
{
    public interface ICurrentUser
    {
        long? Id { get; }

        string Name { get; }

        string HeadIcon { get; }

        NameValueCollection Cookie { get; }
    }
}
