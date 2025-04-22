using AutoMapper;
using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Repository.Initializers;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace it_explained.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly DbContextService _dbContextService;
        private readonly IMapper _mapper;

        public CommentsController(DbContextService dbContextService, IMapper mapper)
        {
            _dbContextService = dbContextService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            try
            {
                var collection = _dbContextService.MongoClient
                    .GetDatabase("sample_mflix")
                        .GetCollection<Comment>("comments");
                
                var data = await collection.Find(Builders<Comment>.Filter.Empty).ToListAsync();


                var result = _mapper.Map<IEnumerable<Comment>>(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}