using MPlayWebAPI.Models;
using MPlayWebAPI.Models.Data;
using System.Linq;

namespace MPlayWebAPI.Models {
    public class MusicRepository : IMusicRepository {
        private MplayContext context;

        public MusicRepository(MplayContext ctx) {
            context = ctx;
        }

        public IQueryable<Music> Musics => context.Musics;
    }
}