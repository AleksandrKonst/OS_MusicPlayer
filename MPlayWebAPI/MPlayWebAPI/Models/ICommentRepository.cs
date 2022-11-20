using MPlayWebAPI.Models.Data;
using System.Linq;

namespace MPlayWebAPI.Models
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }

        public void AddComment(Comment comment);
    }
}