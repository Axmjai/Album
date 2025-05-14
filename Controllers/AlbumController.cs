using AlbumSong.Models; /* Aom 20250514 ต้องใส่เป็นเอกพจ คือ ไม่เติม s */
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Index()
        {
            List<Album> albums = new Album().GetAll(_context);
             
            return View(albums);
        }

        //GET
        public IActionResult Create()
        {
            
            return View();
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Album? album)
        {
            if (ModelState.IsValid)
            {
                album.Create(_context , album.Ifile);
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
            Album? album = new Album().GetById(_context, id.Value);

            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
             //   try
            //    {
                    album.Update(_context ,album.Ifile);
           //     }
            //    catch (DbUpdateConcurrencyException)
                //{
                //    if (!ItemExists(album.Id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
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
        private bool ItemExists(object id)
        {
            throw new NotImplementedException();
        }
    }

}



