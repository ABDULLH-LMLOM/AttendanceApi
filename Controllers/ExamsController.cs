
using AttendanceApi.Data;
using AttendanceApi.Models;
using AttendanceApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExamsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetExam")]
        public IActionResult GetExam(DateTime date)
        {
            var exams = _context.Exams.Where(e => e.ExamDate == date);
            var examDtos = new List<ExamDto>();
            foreach (var exam in exams)
            {
                var StudentTotal = _context.StudentsExams.Count(se => se.ExamId == exam.Id);
                var StudentCount = _context.StudentsExams.Count(se => se.ExamId == exam.Id && se.EntryTime != null);

                examDtos.Add(new ExamDto(exam, StudentCount, StudentTotal));
            }
            return Ok(examDtos);
        }

        [HttpGet("ViewRecord")]
        public async Task<IActionResult> ViewRecord(int ExamId)
        {
            var result = await (
                from se in _context.StudentsExams
                join s in _context.Students
                on se.StudentId equals s.Id
                where se.ExamId == ExamId
                select new
                {
                    s.Name,
                    s.Department,
                    se.EntryTime,
                    se.ExitTime
                }
                ).ToListAsync();

            return Ok(result);


        }

        [HttpPut("EditExamTime_now")]
        public async Task<IActionResult> EditExamTime_now(int id)
        {
            var now = DateTime.Now;
            var exam = _context.Exams.FirstOrDefault(e => e.Id == id);
            exam.ExamDate = now.Date;
            exam.ExamStart = now;
            exam.ExamEnd = now.AddHours(3);
            _context.Exams.Update(exam);
            _context.SaveChanges();

            return Ok();
        }


    }
}
