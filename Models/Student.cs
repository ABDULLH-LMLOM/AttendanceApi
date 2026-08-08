namespace AttendanceApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }

        public ICollection<StudentsExams> StudentsExams { get; set; }
       

    }
}
