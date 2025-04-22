using AutoMapper;
using it_explained.WebApi.Controllers.DataTransferObjects;
using it_explained.WebApi.Domain.Models;

namespace it_explained.WebApi.Repository.Automapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Repository to DTO 
        CreateMap<Comment, CommentDto>();

    }    
}