using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using TodoListAPI.Dtos;

namespace TodoListAPI.Extensions
{
    internal static class HttpResponseMessageExtensions
    {
        public static void AddLinks(this HttpResponseMessage responseMessage, PaginationLinkInfo[] linksList)
        {
            if (!linksList.Any()) 
            {
                return;

            }
            string linkHeaderValue = linksList.Select(ConvertToString)
                                              .Aggregate((current, next) => $"{current}, {next}");

            responseMessage.Headers.Add("Link", linkHeaderValue);
        }

        private static string ConvertToString(PaginationLinkInfo link)
        {
            Uri paginatedUrl = CreatePaginatedUrl(link);
            return $"<{paginatedUrl.ToStringRfc()}>; rel=\"{link.Relation.ToString().ToLower()}\"";
        }

        private static Uri CreatePaginatedUrl(PaginationLinkInfo link)
        {
            NameValueCollection queryItems = link.Url.ParseQueryString();

            AddKeyValue(queryItems, key: "page"    , value: link.PageNumber.ToString());
            AddKeyValue(queryItems, key: "per_page", value: link.PageSize.ToString());

            var uriBuilder = new UriBuilder(link.Url)
                             {
                                 Query = queryItems.ToString()
                             };

            return uriBuilder.Uri;
        }

        private static void AddKeyValue(NameValueCollection queryItems, string key, string value)
        {
            string[] allKeys = queryItems.AllKeys;

            if (allKeys.Contains(key))
            {
                queryItems[key] = value;
            }
            else
            {
                queryItems.Add(key, value);
            }
        }
    }
}