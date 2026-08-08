using AttendanceApi.Data;
using Microsoft.AspNetCore.Mvc;
using AttendanceApi.Service;
using Microsoft.EntityFrameworkCore;
namespace AttendanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AttendanceService _attendanceService;

        public AttendanceController(AppDbContext context, AttendanceService attendanceService)
        {
            _context = context;
            _attendanceService = attendanceService;
        }


        [HttpPut("Taking_Attendance")]
        public async Task<IActionResult> Taking_Attendance(List<int> studentIds)
        {
            var time = DateTime.Now;
            var exam = await _context.Exams.FirstOrDefaultAsync(e => time >= e.ExamStart && time <= e.ExamEnd);

            if (exam == null)
            {
                return NotFound("No exam is currently active");
            }
            foreach (var studentId in studentIds)
            {
                var StudentExam = await _context.StudentsExams.FirstOrDefaultAsync(se => se.StudentId == studentId && se.ExamId == exam.Id);
                if (StudentExam != null)
                {
                    StudentExam.EntryTime = time;
                    _context.StudentsExams.Update(StudentExam);
                    _context.SaveChanges();

                    // Send the student IDs to the ESP32 device
                    await _attendanceService.SendStudentIdAsync(studentId);
                }
            }
            return Ok("done");
        }
    }
}
