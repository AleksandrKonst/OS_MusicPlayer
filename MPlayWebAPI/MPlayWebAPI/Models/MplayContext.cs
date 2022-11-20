using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MPlayWebAPI.Models.Data;

namespace MPlayWebAPI.Models
{
    public partial class MplayContext : DbContext
    {
        public MplayContext()
        {
        }

        public MplayContext(DbContextOptions<MplayContext> options)
            : base(options) { }

        public virtual DbSet<Music> Musics { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
    }
}
