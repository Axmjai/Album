using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AlbumSong.Models
{
    public class AlbumMetadata
    {
        //[Required(ErrorMessage = "Please Enter Your Name")]
        //public string Name { get; set; } = null; /* Aom 20250514  ปิดคำเตือน null ที่คอมไพเลอร์ตรวจพบ */
       
    }

    [MetadataType(typeof(AlbumMetadata))]
    public partial class Album
    {
        [NotMapped]
        public IFormFile? Ifile { get; set; }

        //[NotMapped]
        //public bool isUploaded { get; set; }

        public bool Create(Ex2DatabaseContext dbcontext, IFormFile? Ifile)
        {
            DateTime datenow = DateTime.Now;
            //dbcontext.SaveChanges(); /* Aom 20250514  SaveChanges() ควร save เเค่ทีเดียว*/
            //this.File = null;
            //FileId = files.Id;
            //this.File = files;
           
            File?.Create(dbcontext,Ifile);

            List <Song> sonn = this.Songs.ToList();

            this.Songs = null;
            IsDelete = false;
            Createby = "John";
            CreateDate = datenow;
            Updateby = "John";
            UpdateDate = datenow;
            dbcontext.Albums.Add(this);
            dbcontext.SaveChanges();
             
            foreach (Song songs in sonn)
            {
                if (!string.IsNullOrEmpty(songs.Name))
                {
                    songs.Create(dbcontext, this.Id);
                }
            }

            return true;
        }

        public Album Update(Ex2DatabaseContext dbContext, IFormFile? Ifile)
        {
            DateTime datenow = DateTime.Now;
            List<Song> allSongIds = dbContext.Songs.Where(s => s.AlbumId == this.Id && s.IsDelete != true).AsNoTracking().ToList();
            List<int> thisSongIds = this.Songs.Where(s => s.Id != 0).Select(s => s.Id).ToList();

            foreach (Song oldSong in allSongIds)
            {
                if (!thisSongIds.Contains(oldSong.Id))
                {
                    oldSong.IsDelete = true;
                    oldSong.UpdateBy = "D";
                    oldSong.UpdateDate = DateTime.Now;
                    dbContext.Songs.Update(oldSong);
                }
               
            }
            foreach (Song song in Songs) /* Aom 20250514 เรื่องตรวจความถูกต้อง add songmatedata ดัก [Required] */
            {
                if (!string.IsNullOrWhiteSpace(song.Name))/* Aom 20250514  */
                {
                    song.Update(dbContext);
                }
            }
            //var formSongIds = dbContext.Songs.Where(s => s.AlbumId == this.Id).Select(s => s.Id).ToList();
            //if (Ifile != null)
            //{
                File?.Update(dbContext, Ifile);
            //}
           // this.Songs = null;
            this.Updateby = "John";
            this.UpdateDate = datenow;
            dbContext.Albums.Update(this);
            dbContext.SaveChanges();
            // --- [3] Update ข้อมูล Album ---

            return this;
        }

        public bool Delete(Ex2DatabaseContext dbContext)
        {
            DateTime datenow = DateTime.Now;
            foreach (Song songs in this.Songs)
            {
                songs.Delete(dbContext);
            }

            File.Delete(dbContext);

            IsDelete = true;
            Updateby = "John";
            UpdateDate = datenow;
            dbContext.Albums.Update(this);
            dbContext.SaveChanges();
            return true;
        }

        public List<Album> GetAll(Ex2DatabaseContext dbContext, string searchName)
        {
            return dbContext.Albums.Where(q => q.IsDelete != true) // db เเก้ isdelete ต้องไม่สามารถ null ได้
                                   .Include(f => f.File)
                                   .Include(s => s.Songs.Where(q => q.IsDelete != true))
                                   .Where(a => string.IsNullOrEmpty(searchName) || a.Name.Contains(searchName))
                                   .ToList();
        }

        public Album? GetById(Ex2DatabaseContext dbContext, int id)
        {
            Album? album = dbContext.Albums.Include(s => s.Songs.Where(q => q.IsDelete != true))
                                           .Include(f => f.File)
                                           .FirstOrDefault(q => q.IsDelete != true && q.Id == id);
         
            return album;
        }
    }
}
