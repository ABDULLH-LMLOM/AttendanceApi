namespace AttendanceApi.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public int SectorId { get; set; }
        public string Name { get; set; }
        public DateTime ExamDate { get; set; }
        public DateTime ExamStart { get; set; }
        public DateTime ExamEnd { get; set; }


        public ICollection<StudentsExams> StudentsExams { get; set; }
        public Sector Sector { get; set; }
    }
}
