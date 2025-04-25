using it_explained.WebApi.Domain.Models;
using System.Text.RegularExpressions;

namespace it_explained.WebApi.Helpers
{
    public class MarkdownParser
    {
        public static Topic ParseTopic(string markdownData)
        {
            // Regex to extract the metadata block (between <!-- and -->)
            var metadataPattern = @"<!--\s*Title:\s*(?<Title>.*?)\s*Tags:\s*(?<Tags>.*?)\s*Slug:\s*(?<Slug>.*?)\s*-->";
            var match = Regex.Match(markdownData, metadataPattern, RegexOptions.Singleline);

            if (!match.Success)
            {
                throw new Exception("Invalid markdown format: Metadata block not found.");
            }

            // Extract metadata values
            var title = match.Groups["Title"].Value.Trim();
            var tags = match.Groups["Tags"].Value.Trim();
            var slug = match.Groups["Slug"].Value.Trim();

            // Content is everything after the metadata block
            var content = markdownData.Substring(match.Index + match.Length).Trim();

            // Create and return the Topic object
            var topic = new Topic
            {
                Name = title,  
                Content = content,  
                Metadata = $"<--!Title: {title}\nTags: {tags}\nSlug: {slug}-->" 
            };

            return topic;
        }
    }
}
