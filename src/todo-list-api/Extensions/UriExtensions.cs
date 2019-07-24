using System;

namespace TodoListAPI.Extensions
{
    internal static class UriExtensions
    {
        public static string ToStringRfc(this Uri uri)
        {
            return uri.IsAbsoluteUri 
                       ? uri.AbsoluteUri 
                       : uri.OriginalString;
        }
    }
}
