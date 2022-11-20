using MPlayWebAPI.Models.Data;
using System.Linq;

namespace MPlayWebAPI.Models
{
    public interface IMusicRepository
    {
        IQueryable<Music> Musics { get; }
    }
}