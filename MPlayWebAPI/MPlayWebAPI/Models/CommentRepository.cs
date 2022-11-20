using MPlayWebAPI.Models;
using MPlayWebAPI.Models.Data;
using System.Linq;

namespace MPlayWebAPI.Models {
    public class CommentRepository : ICommentRepository {
        private MplayContext context;

        public CommentRepository(MplayContext ctx) {
            context = ctx;
        }

        public IQueryable<Comment> Comments => context.Comments;

        public void AddComment(Comment comment)
        {
            context.Add(comment);
            context.SaveChanges();
        }
    }
}