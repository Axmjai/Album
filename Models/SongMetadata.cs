using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AlbumSong.Models
{
    public class SongMetadata
    { 
    }

    [MetadataType(typeof(SongMetadata))]
    public partial class Song
    {
        public Song Create(Ex2DatabaseContext dbcontext,int albumId)
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
            return this;
        }
        public Song Update(Ex2DatabaseContext dbContext)
        {
            DateTime datenow = DateTime.Now;

            Song existingSong = dbContext.Songs.AsNoTracking().FirstOrDefault(s => s.Id == this.Id);
            
            if (this.Id != 0 && existingSong.Name != Name)
            {
                this.UpdateBy = "John";
                this.UpdateDate = datenow;
            }
            else
            {
                this.AlbumId = this.Id;
                this.CreateBy = "John";
                this.CreateDate = datenow;
                this.UpdateBy = "John";
                this.UpdateDate = datenow;
                this.IsDelete = false;
                //dbContext.Songs.Add(this);
            }
       
            return this;

            //List<Song> existingSongs = dbContext.Songs.Where(s => s.AlbumId == this.Id).ToList();

            // --- [2] จัดการกับ Songs ---

            //if (song.Id != 0)
            //{
            //    song.UpdateBy = "John";
            //    song.UpdateDate = datenow;

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
            //}

            //else
            //{
            //    // เพิ่มเพลงใหม่
            //    song.AlbumId = this.Id;
            //    song.CreateBy = "John";
            //    song.CreateDate = datenow;
            //    song.UpdateBy = "John";
            //    song.UpdateDate = datenow;
            //    //dbContext.Songs.Add(song);
            //}
        }
        public Song Delete(Ex2DatabaseContext dbContext) 
        {
            DateTime datenow = DateTime.Now;

            this.IsDelete = true;
            this.UpdateBy = "John";
            this.UpdateDate = datenow;
            dbContext.Songs.Update(this);
            return this;

        }
    }
    
    }
