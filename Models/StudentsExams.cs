namespace AttendanceApi.Models
{
    public class StudentsExams
    {
        public int StudentId { get; set; }
        public int ExamId { get; set; }

        public DateTime? EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }


        public Student Student { get; set; }
        public Exam Exam { get; set; }

    }
}
