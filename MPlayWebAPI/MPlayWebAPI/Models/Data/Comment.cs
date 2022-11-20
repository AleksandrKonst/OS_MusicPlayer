using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MPlayWebAPI.Models.Data
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Text { get; set; }

        [ForeignKey("Music")]
        public int MusicId { get; set; }

        [JsonIgnore]
        public virtual Music Music { get; set; }
    }
}