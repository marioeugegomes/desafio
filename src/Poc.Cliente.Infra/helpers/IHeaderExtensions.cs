using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Poc.Cliente.Infra.Helpers;

public static class IHeaderExtensions
{
    public static string GetValueOrDefault(this IHeaderDictionary dictionary, string key)
    {
        return dictionary.TryGetValue(key, out var stringValues)
            ? stringValues.FirstOrDefault()
            : null;
    }
}
