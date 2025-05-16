using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace AlbumSong.Models
{
    public class FileMetadata
    {
        [Required(ErrorMessage = "Please enter the song title.")]
        public IFormFile? Ifile { get; set; } = null;
        //[Required]
        //public string? FileName { get; set; }
        //[Required]
        //public string? FilePath { get; set; }
    }

    [MetadataType(typeof(FileMetadata))]
    public partial class File
    {
        // [NotMapped]
        // [Required(ErrorMessage = "Please enter the Album image.")]
        //public IFormFile? Ifile { get; set; }
        public File Create(Ex2DatabaseContext dbcontext, IFormFile? Ifile)
        {
            string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            // string fileName = Guid.NewGuid().ToString() + Path.GetFileName(Ifile.FileName);
            string fileName = Path.GetFileNameWithoutExtension(Ifile.FileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + Path.GetExtension(Ifile.FileName);
            string filePath = Path.Combine(uploads, fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                Ifile.CopyTo(stream);
            }

            DateTime datenow = DateTime.Now;

            FileName = fileName;
            FilePath = "/uploads/" + fileName;
            IsDelete = false;
            CreateBy = "John";
            CreateDate = datenow;
            UpdateBy = "John";
            UpdateDate = datenow;

            dbcontext.Files.Add(this);
            dbcontext.SaveChanges(); /* Aom 20250514  SaveChanges() ควร save เเค่ทีเดียว*/
            //this.File = null;
            //FileId = files.Id;
            //this.File = files;

            return this;

        }

        public File Update(Ex2DatabaseContext dbContext, IFormFile? Ifile, Album album)
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
                this.FileName = fileName;
                this.FilePath = "/upload/" + fileName;
                this.UpdateBy = "John";
                this.UpdateDate = datenow;
                dbContext.Files.Update(this);
            }
            
          //File oldfile = dbContext.Files.FirstOrDefault(i => i.Id == album.FileId.Value);
          //oldfile.FileName = this.FileName;
          //oldfile.FilePath = "/upload/" + this.FileName;
          //  oldfile.UpdateBy = this.UpdateBy;
          //  oldfile.UpdateDate = datenow;
          //  oldfile.IsDelete = this.IsDelete;
          //  dbContext.Files.Update(oldfile);


            // else   /* Aom 20250514 เเก้ไข ไท่ต้องใช้ else เพราะสมมารถดึงมาจาก id เก่าได้เลย ^_^ */
            //{
            //    File oldFile = new File();
            //    oldFile.FileName = this.FileName;
            //    oldFile.FilePath = "/upload/" + this.FileName;
            //    oldFile.UpdateBy = "John";
            //    oldFile.UpdateDate = datenow;
            //    dbContext.Files.Update(oldFile);
            //}
           // dbContext.SaveChanges();

            return this;
        }

        public File Delete(Ex2DatabaseContext dbContext) 
        {
            DateTime datenow = DateTime.Now;
            this.IsDelete = true;
            this.UpdateBy = "John";
            this.UpdateDate = datenow;
            dbContext.Files.Update(this);
            return this;
        
        }

    }
}
