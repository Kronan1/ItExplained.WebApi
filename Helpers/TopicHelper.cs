using it_explained.WebApi.Domain.Models;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace it_explained.WebApi.Helpers
{
    public static partial class TopicHelper
    {
        public static Topic CreateTopic(string topic, List<string> tags, string content)
        {
            var slug = ToUrlSlug(topic);

            return new() { Name = topic, Slug = slug, Content = content, Tags = tags};
        }

        private static string ToUrlSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string result = input.ToLowerInvariant();

            // Remove parentheses
            result = RemoveParentheses().Replace(result, "");

            // Remove non-alphanumerics except dash/space
            result = RemoveInvalidChars().Replace(result, "");

            // Replace multiple dashes/spaces with a single dash
            result = CollapseWhitespaceAndDashes().Replace(result, "-");

            return result.Trim('-');
        }


        [GeneratedRegex(@"[\(\)]")]
        private static partial Regex RemoveParentheses();

        [GeneratedRegex(@"[^a-z0-9\s-]")]
        private static partial Regex RemoveInvalidChars();

        [GeneratedRegex(@"[\s-]+")]
        private static partial Regex CollapseWhitespaceAndDashes();
    }
}
