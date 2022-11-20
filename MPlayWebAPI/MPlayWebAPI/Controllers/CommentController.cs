using Microsoft.AspNetCore.Mvc;
using MPlayWebAPI.Models;
using MPlayWebAPI.Models.Data;

namespace MPlayWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        
        private ICommentRepository repository;

        public CommentController(ICommentRepository repo)
        {
            repository = repo;
        }

        [HttpGet("{name}/{page}")]
        public IEnumerable<Comment> Get(string name, int page)
        {
            return repository.Comments
                    .Where(c => c.Music.Name.Contains(name))
                    .OrderBy(c => c.CommentId)
                    .ToList();
        }

        [HttpPost("{id}/{value}")]
        public void Post(int id, string value)
        {
            Comment comment = new Comment();
            comment.Text = value;
            comment.MusicId = id;

            repository.AddComment(comment);
        }
    }
}