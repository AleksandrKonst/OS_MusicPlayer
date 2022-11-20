using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MPlayWebAPI.Models.Data
{
    public class Music
    {
        public Music()
        {
            this.Comments = new HashSet<Comment>();
        }

        [Key]
        public int MusicId { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}