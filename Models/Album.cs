using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AlbumSong.Models;

[Table("Album")]
public partial class Album
{ /* Aom 20250514 การดัก ควรใส่ ไว้ใน model */
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; } = null!; /* Aom 20250514 หา ข้อมูล null! */

    public int? FileId { get; set; }
    
    public string? Description { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Createby { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Updateby { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    [ForeignKey("FileId")]
    [InverseProperty("Albums")]
    public File? File { get; set; } = new File { };

    [InverseProperty("Album")]
    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
