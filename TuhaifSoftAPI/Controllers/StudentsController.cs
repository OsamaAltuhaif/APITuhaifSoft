using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TuhaifSoftAPI.Data;
using TuhaifSoftAPI.Models;
using TuhaifSoftAPI.Models.DTO;

namespace TuhaifSoftAPI.Controllers
{
    /*Important for app.MapControllers(); that's in Program.cs and we can name it what ever we need but when
    we name it [controller] this will name it as the class named until we chaange the class name */
  //  [Authorize]
    //To Tell The Project that this is API Controller
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]


    //this class shuld to inhirt from ControllerBase to be Controller that's important in MVC Concept
    public class StudentsController:ControllerBase
    {
        private readonly TuhaifSoftAPIDBContext _db;
       private readonly ILogger<StudentsController> logger;
        private readonly IConfiguration configuration;
       
        public StudentsController(IConfiguration configuration, ILogger<StudentsController> logger,TuhaifSoftAPIDBContext _db)
        {
            this._db = _db;
            this.logger = logger;
            this.configuration = configuration;

        }
            
        //to tell that this function is End Point to Get Data
        [HttpGet("GetStudentsMajor")] 
        //[ProducesResponseType(StatusCodes.Status200OK)]
        
        public ActionResult<StudentsDTO> GetStudents(int id)
        {
            var data = _db.Students.Where(st=>st.Major_id==id);
               // logger.LogInformation($"SomeUser  is select All Students Data in {DateTime.Now} ");
                return Ok(data);

        }
        [HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult<StudentsDTO> GetStudentsInMajor()
        {
            var data = _db.Students.ToList();
            // logger.LogInformation($"SomeUser  is select All Students Data in {DateTime.Now} ");
            return Ok(data);

        }

        /*[HttpPost]
        public void addData(int id, string name)
        {
            StudentsDTO students = new StudentsDTO()
            {
                id = id,
                Name = name,
            };
            StudentsData.data.Add(students); 
        }*/


        [HttpGet("SearchEndPoint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable< StudentsDTO>> SearchStudents(string firstStar)
        {
            var sd = _db.Students.Where(s => s.f_Name.ToLower().Contains(firstStar.ToLower())).ToList();
            if (sd != null)
            {
                return Ok(sd);
            }
            else
            {
                return NotFound("We Don't Found This Student");
            }
        }


        [HttpGet("{id:int}", Name ="GetStudent")]
        //[AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        /*[ProducesResponseType(404)]
        [ProducesResponseType(200,Type=typeof(StudentsDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]*/
        public ActionResult getStudentById(int id)
        {
            var student = _db.Students.FirstOrDefault(s => s.id == id);
            if (id <= 0)
            {
                return BadRequest("يجب ادخال قيمة حقيقية");
            }
            else if (id > 0&& student != null)
            {
                return Ok(student);
            }else
            {
                return NotFound("no student with this number");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<StudentsDTO> addStudent([FromBody]StudentsDTO studentsDTO)
        {
            if (_db.Students.FirstOrDefault(st => st.f_Name.ToLower() == studentsDTO.f_Name.ToLower()) != null)
            {
                 ModelState.AddModelError("CustomError", "This Name is Aready Excit");
                return BadRequest(ModelState);
            }
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/
            if (studentsDTO == null)
            {
                return BadRequest(studentsDTO);
            }else if (studentsDTO.id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            studentsDTO.id = _db.Students.FirstOrDefault() == null ? 1 : _db.Students.OrderByDescending(st => st.id).FirstOrDefault().id + 1;
            _db.Students.Add(new()
            {
                f_Name=studentsDTO.f_Name,
                l_Name=studentsDTO.l_Name,
                Major_id=studentsDTO.Major_id,
                Age=studentsDTO.Age,
                birthDate=studentsDTO.birthDate,
                imageURl=studentsDTO.imageURl,
            });
            _db.SaveChanges();
            return CreatedAtRoute("GetStudent", new { id = studentsDTO.id }, studentsDTO);
        }

        [HttpDelete("{id:int}",Name ="DeleteStudent")]
        public ActionResult deleteStudent(int id)
        {
            var res= _db.Students.FirstOrDefault(st => st.id == id);
            if (id <= 0)
            {
                ModelState.AddModelError("DeleteError", "UnValid ID");
                return BadRequest(ModelState);

            }else if (res != null)
            {
                _db.Students.Remove(res);
                _db.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("{id:int}", Name ="UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateStudent(int id, [FromBody] StudentsDTO studentsDTO)
        {
            if (studentsDTO == null|| id != studentsDTO.id)
            {
                ModelState.AddModelError("updateError", "No Student With This Data");

                return BadRequest(ModelState);
            }
            var student = _db.Students.FirstOrDefault(st => st.id == id);
            student.f_Name = studentsDTO.f_Name;
            student.l_Name = studentsDTO.l_Name;
            student.Major_id = studentsDTO.Major_id;
            student.Age = studentsDTO.Age;
            student.birthDate = studentsDTO.birthDate;
            student.imageURl = studentsDTO.imageURl;
            /*Students studentsDTO1 = new()
            {
                f_Name = studentsDTO.f_Name,
                l_Name = studentsDTO.l_Name,
                Major = studentsDTO.Major,
                Age = studentsDTO.Age,
                birthDate = studentsDTO.birthDate,
                imageURl = studentsDTO.imageURl
            };*/
            _db.Update(student);
            _db.SaveChanges();


            return NoContent();


        }

        [HttpPatch("{id:int}",Name ="UpdatePartecalStudent")]
        public IActionResult updatePartecalStudent(int id,JsonPatchDocument<StudentsDTO> patchDTO)
        {
            if (patchDTO == null|| id <= 0)
            {
                ModelState.AddModelError("updatePatchError", "No Student With That Data");
                return BadRequest(ModelState);
            }
            var student = _db.Students.FirstOrDefault(st => st.id == id);
            if (student == null)
            {
                return BadRequest(ModelState);
            }
            StudentsDTO studentDTO = new()
            {
                f_Name = student.f_Name,
                l_Name = student.l_Name,
                Major_id = student.Major_id,
                Age = student.Age,
                birthDate = student.birthDate,
                imageURl = student.imageURl,

            };
            // patchDTO.ApplyTo(student,ModelState);
            patchDTO.ApplyTo(studentDTO, ModelState);
            Students newstudent = new Students()
            {
                f_Name = studentDTO.f_Name,
                l_Name = studentDTO.l_Name,
                Major_id = studentDTO.Major_id,
                Age = studentDTO.Age,
                birthDate = studentDTO.birthDate,
                imageURl = studentDTO.imageURl,

            };
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _db.Students.Update(newstudent);
            _db.SaveChanges();
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost("upload")]
        public async Task<IActionResult> ImagestudentImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("لم يتم تحميل أي ملف.");

            try
            {
                // تحديد مسار الحفظ
                var ImagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

                // التأكد من وجود المجلد
                if (!Directory.Exists(ImagesFolder))
                    Directory.CreateDirectory(ImagesFolder);

                // إنشاء اسم فريد للملف
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(ImagesFolder, fileName);

                // حفظ الملف على السيرفر
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // إنشاء رابط الصورة (URL)
                var imageUrl = $"/Images/{fileName}";

                return Ok(new { ImagePath = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"خطأ في رفع الملف: {ex.Message}");
            }
        }




    }
}
