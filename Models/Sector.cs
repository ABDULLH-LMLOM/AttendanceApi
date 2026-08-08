namespace AttendanceApi.Models
{
    public class Sector
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Exam> exams { get; set; }
    }
}
