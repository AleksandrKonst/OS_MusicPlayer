using Microsoft.AspNetCore.Mvc;
using MPlayWebAPI.Models;
using MPlayWebAPI.Models.Data;

namespace MPlayWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private IMusicRepository repository;

        public MusicController(IMusicRepository repo)
        {
            repository = repo;
        }

        [HttpGet]
        public IEnumerable<Music> Get()
        {
            return repository.Musics;
        }

        [HttpGet("{name}")]
        public string Get(string name)
        {
            var music = repository.Musics
               .Where(m => m.Name.Contains(name))
               .Select(m => m.Text)
               .FirstOrDefault();

            return music;
        }
    }
}
