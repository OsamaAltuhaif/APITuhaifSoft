using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TuhaifSoftAPI.Data;
using TuhaifSoftAPI.Models.DTO;

namespace TuhaifSoftAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MajorController:ControllerBase
    {
        private TuhaifSoftAPIDBContext _db;

        public MajorController(TuhaifSoftAPIDBContext _db)
        {
            this._db = _db;
        }
        [HttpGet]
        public IActionResult getMajors()
        {
            var majors = _db.Majors.ToList();

            return Ok(majors);
        }
        [HttpGet("getMajorId")]
        public IActionResult getMajorId( MajorDTO majorDTO)
        {
            var majorId = _db.Majors.FirstOrDefault(mj => mj.name == majorDTO.name).Major_id;
            if (majorDTO == null)
            {
                return NotFound();
            }
            return Ok(majorId);

        }
        [HttpPost]
        public IActionResult addMajor(MajorDTO majorDTO)
        {

            _db.Majors.Add(new()
            {
                Major_id = 0,
                name = majorDTO.name
            });
            _db.SaveChanges();
            return CreatedAtRoute("", new {name=majorDTO.name},majorDTO);
        }


    }
}
