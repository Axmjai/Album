// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function previewImage() {
    // 1. ดึง input และ preview image
    var input = document.getElementById('Ifile');
    var preview = document.getElementById('preview');

    // 2. ถ้าเลือกไฟล์
    if (input.files && input.files[0]) {
        var reader = new FileReader();// ตัวช่วยอ่านไฟล์
        reader.onloadend = function (e) {     // 3. เมื่ออ่านไฟล์เสร็จ → เปลี่ยน src รูป
            preview.src = e.target.result;    // รูปใหม่ที่เลือก
            preview.style.display = "block";  // แสดงรูป
        };

        reader.readAsDataURL(input.files[0]); // เริ่มอ่านไฟล์
    }
}

let songIndex = 0;

function addSong(name = " ") {
    const tbody = document.getElementById("songBody");
    const tr = document.createElement("tr");
    tr.innerHTML = `
            <td>
                <input type="text" name="Songs[${songIndex}].Name" class="form-control song-input" value="${name}" />
            </td>
            <td class="text-center">
                <button type="button" class="btn btn-danger " onclick="removeRow(this) ">Delete</button>
            </td>`;
    tbody.appendChild(tr);
   // $.validator.unobtrusive.parse("#albumForm");
    Updateindex();
}//    <span class="text-danger field-validation-valid" data-valmsg-for="Songs[${songIndex}].Name" data-valmsg-replace="true"></span>

function removeRow(button) {
    const row = button.closest("tr");
    row.remove();
    Updateindex();
}

function RemoveRow(btn) {
    btn.closest('tr').remove();
    const rows = document.querySelectorAll('#songTable tbody tr');
    rows.forEach((row, index) => {
        row.querySelector('input[name$=".Id"]').name = `Songs[${index}].Id`;
        row.querySelector('input[name$=".Name"]').name = `Songs[${index}].Name`;
    });
}

function Updateindex() {       //<tbody id="songBody">  ,  class
    const inputs = document.querySelectorAll("#songBody .song-input") // ค้นหา element หลายๆ ตัว 
    for (let i = 0; i < inputs.length; i++)                         //เเละคืนค่า เป็น list ที่อยู่ใน class .songinput
    {
        inputs[i].setAttribute("name", `Songs[${ i }].Name`); //ใช้สำหรับ "กำหนดค่า" ("name" , `value`) 
                                                           //ให้กลับ inputs ในช่องที่ i
    }
}
// เพิ่มแถวเปล่าเริ่มต้นไว้ 1 แถว
window.onload = () => addSong();

//window.onload = () => {
//    const existing = document.querySelectorAll("#songBody .song-input").length;
//    songIndex = existing; // เริ่มต่อจากของเดิม
//};

