using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AlbumSong.Models
{
    public class AlbumMetadata
    {
        [Required]
        public string Name { get; set; } = null!;
    }

    [MetadataType(typeof(AlbumMetadata))]
    public partial class Album
    {
        [NotMapped]
        public IFormFile? Ifile { get; set; }

        //[NotMapped]
        //public bool isUploaded { get; set; }

        public bool Create(Ex2DatabaseContext dbcontext, IFormFile Ifile)
        {
           
            string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            //    string fileName = Guid.NewGuid().ToString() + Path.GetFileName(Ifile.FileName);
            string fileName = Path.GetFileNameWithoutExtension(Ifile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + Path.GetExtension(Ifile.FileName);
            string filePath = Path.Combine(uploads, fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                Ifile.CopyTo(stream);
            }

            DateTime datenow = DateTime.Now;
            File files = new File
            {
                FileName = fileName,
                FilePath = "/uploads/" + fileName,
                IsDelete = false,
                CreateBy = "John",
                CreateDate = datenow,
                UpdateBy = "John",
                UpdateDate = datenow

            };
            dbcontext.Files.Add(files);
            //dbcontext.SaveChanges(); /* Aom 20250514  SaveChanges() ควร save เเค่ทีเดียว*/
            this.File = null;
            FileId = files.Id;
            this.File = files;

            List<Song> sonn = this.Songs.ToList();
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
            if (Ifile != null)
            {

                string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                string fileName = Path.GetFileNameWithoutExtension(Ifile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + Path.GetExtension(Ifile.FileName);
                string filePath = Path.Combine(uploads, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Ifile?.CopyTo(stream);
                }
                
                this.File.FileName = fileName;
                this.File.FilePath = "/upload/" + fileName;
                this.File.UpdateBy = "John";
                this.File.UpdateDate = datenow;
                dbContext.Files.Update(this.File);
            }
            else   /* Aom 20250514 เเก้ไข */
            {
                File oldFile = new File();
                oldFile.FileName = this.File.FileName;
                oldFile.FilePath = "/upload/" + this.File.FileName;
                oldFile.UpdateBy = "John";
                oldFile.UpdateDate = datenow;
                dbContext.Files.Update(oldFile);
            }

            //List<Song> existingSongs = dbContext.Songs.Where(s => s.AlbumId == this.Id).ToList();

            // --- [2] จัดการกับ Songs ---
            foreach (var song in this.Songs) /* Aom 20250514 เรื่องตรวจความถูกต้อง add songmatedata ดัก [Required] */
            {
                if (!string.IsNullOrWhiteSpace(song.Name))/* Aom 20250514  */
                {
                    if (song.Id != 0)
                    {
                        song.UpdateBy = "John";
                        song.UpdateDate = datenow;

                        //// ค้นหาเพลงเดิมจาก DB
                        //   Song existingSong = existingSongs.FirstOrDefault(s => s.Id == song.Id);
                        //// ชื่อเก่า != ใหม่
                        // if (existingSong.Name != song.Name)
                        //{
                        //song.Name = song.Name;
                        //  song.UpdateBy = "John";
                        //song.UpdateDate = DateTime.Now;
                        //dbContext.Songs.Update(existingSong);
                        //dbContext.SaveChanges();
                        //}
                        // ชื่อเพลงตัวเก่า เหมือนกับ ชื่อใหม่  ไม่ทำอะไร
                    }

                    else
                    {
                        // เพิ่มเพลงใหม่
                        song.AlbumId = this.Id;
                        song.CreateBy = "John";
                        song.CreateDate = datenow;
                        song.UpdateBy = "John";
                        song.UpdateDate = datenow;
                        //dbContext.Songs.Add(song);
                    }
                }
            }

            // --- [3] Update ข้อมูล Album ---
            this.Updateby = "John";
            this.UpdateDate = datenow;
            dbContext.Albums.Update(this);
            dbContext.SaveChanges();
            return this;
        }


        public bool Delete(Ex2DatabaseContext dbContext)
        {
            DateTime datenow = DateTime.Now;
            foreach (var songs in this.Songs)
            {
                songs.IsDelete = true;
                songs.UpdateBy = "John";
                songs.UpdateDate = datenow;
                //dbContext.Songs.Update(songs);
            }

            IsDelete = true;
            Updateby = "John";
            UpdateDate = datenow;
            dbContext.Albums.Update(this);

            this.File.IsDelete = true;
            this.File.UpdateBy = "John";
            this.File.UpdateDate = datenow;
            dbContext.Files.Update(this.File);

            dbContext.SaveChanges();
            return true;
        }

        public List<Album> GetAll(Ex2DatabaseContext dbContext)
        {
            return dbContext.Albums.Where(q => q.IsDelete != true) // db เเก้ isdelete ต้องไม่สามารถ null ได้
                                   .Include(f => f.File)
                                   .Include(s => s.Songs.Where(q => q.IsDelete != true))
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
