using System;

namespace it_explained.WebApi.Controllers.DataTransferObjects
{
    public class CommentsResponseDto
    {
        public ICollection<CommentDto> CommentCollection { get; set; } = [];
    }
}