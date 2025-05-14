using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AlbumSong.Models
{
    public class SongMetadata
    {
        [Required]
        public string Name { get; set; } = null!;
    }

    [MetadataType(typeof(AlbumMetadata))]
    public partial class Song

    {
        public bool Create(Ex2DatabaseContext dbcontext,int albumId)
        {
            DateTime datenow = DateTime.Now;
            this.AlbumId = albumId;
            this.IsDelete = false;
            this.CreateBy = "John";
            this.CreateDate = datenow;
            this.UpdateBy = "John";
            this.UpdateDate = datenow;
            dbcontext.Songs.Add(this);
            dbcontext.SaveChanges();
            return true;

        }
    }
    
    }
