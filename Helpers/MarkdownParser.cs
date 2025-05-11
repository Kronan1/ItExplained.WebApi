using it_explained.WebApi.Domain.Models;
using System.Text.RegularExpressions;

namespace it_explained.WebApi.Helpers
{
    public class MarkdownParser
    {
        public static Topic ParseTopic(string markdownData)
        {
            //var metadataPattern = @"<!--(.*?)-->";
            //var tags = Regex.Match(markdownData, metadataPattern, RegexOptions.Singleline);

            //var titlePattern = @"<!--\s*Title:\s*(?<Title>.*?)\s*(?:Tags:|Slug:|-->)";
            //var titleMatch = Regex.Match(markdownData, titlePattern, RegexOptions.Singleline);

            //if (!tags.Success)
            //    throw new Exception("Invalid markdown format: Metadata block not found.");

            //if (!titleMatch.Success)
            //    throw new Exception("Invalid markdown format: Title not found");

            //var title = titleMatch.Groups["Title"].Value.Trim();
            //var content = markdownData.Substring(tags.Index + tags.Length).Trim();

            //var topic = new Topic
            //{
            //    Name = title,
            //    Slug = "test",
            //    Content = content,  
            //    Tags = tags.Value
            //};

            return new();
        }
    }
}
