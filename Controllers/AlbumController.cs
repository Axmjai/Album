using System;
using AlbumSong.Models; /* Aom 20250514 ต้องใส่เป็นเอกพจ คือ ไม่เติม s */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using File = AlbumSong.Models.File;

namespace AlbumSong.Controllers /* Aom 20250514 ต้องใส่เป็นเอกพจ คือ ไม่เติม s */
{
    public class AlbumController : Controller
    {
        //private readonly Ex2DatabaseContext _context; /* Aom 20250514 เพื่อ ให้ได้ inheritance สามารถใช้ protected */
        // private จำกัดให้เข้าถึงตัวแปร _context ได้ เฉพาะภายในคลาส AlbumController เท่านั้น
        // readonly ค่าจะถูกกำหนดได้แค่ครั้งเดียว (เช่น ผ่าน constructor) และเปลี่ยนไม่ได้ภายหลัง

        protected readonly Ex2DatabaseContext _context;
        // protected เข้าถึงได้จาก คลาสปัจจุบัน และ คลาสลูก
        // readonly ยังคงรักษาคุณสมบัติที่ "assign ได้ครั้งเดียวใน constructor"
        public AlbumController(Ex2DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchName)
        {
            List<Album> albums = new Album().GetAll(_context , searchName);

            return View(albums);
        }

        //GET
        public IActionResult Create()
        {
            var album = new Album
            {
                Songs = new List<Song> { new Song() } // แถวเปล่าเริ่มต้น
            };
            return View(album);
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Album? album, string action, int? removeIndex , IFormFile? Ifile)
        {
            //List<Song> songs = album.Songs.ToList();
            //if (songs.Count > 0)
            //{
            //    if (string.IsNullOrWhiteSpace(songs[0].Name))
            //    {
            //        ModelState.AddModelError("Songs[0].Name", "กรุณากรอกชื่อเพลงแรก");
            //    }
            //}
            if (ModelState.IsValid)
            {
                album.Songs ??= new List<Song>();

                if (action == "addSong")
                {
                   if (Ifile != null)
                   {
                    //var file = new File();
                    //  album.File = file.Createsoft(Ifile);
                      album.File.Create(_context, Ifile);
                    }
                    album.Songs.Add(new Song()); // เพิ่มช่องใหม่

                    return View(album); // กลับไปหน้าเดิมพร้อม Model (มี ImagePath)   
                }

                if (action == "remove" && removeIndex.HasValue)
                {
                    if (album.Songs is List<Song> songList && removeIndex.Value < songList.Count)
                    {
                        songList.RemoveAt(removeIndex.Value);
                    }
                    return View(album);
                }

                album.Create(_context, album.Ifile);
                //_context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        //GET/Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Album album = new Album().GetById(_context, id.Value);

            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Album album , string action, int? removeIndex, IFormFile? Ifile)
        {

            if (ModelState.IsValid)
            {
                album.Songs ??= new List<Song>();

                if (action == "addSong")
                {
                    if (Ifile != null)
                    {
                        //    //var file = new File();
                        //    //  album.File = file.Createsoft(Ifile);
                        album.File.Update(_context, Ifile );
                    }
                    album.Songs.Add(new Song()); // เพิ่มช่องใหม่

                    return View(album); // กลับไปหน้าเดิมพร้อม Model (มี ImagePath)   
                }

                else if (action == "remove")
                {
                    if (album.Songs is List<Song> songList && removeIndex.Value < songList.Count)
                    {
                        songList.RemoveAt(removeIndex.Value);
                        
                    }
                    return View(album);
                }
                album.Update(_context, album.Ifile);
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Album items = new Album().GetById(_context, id.Value);
           
            if (items != null)
            {
                items.Delete(_context);
            }
            return RedirectToAction(nameof(Index));
        }

    }

}



