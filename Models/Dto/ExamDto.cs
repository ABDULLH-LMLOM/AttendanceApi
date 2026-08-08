namespace AttendanceApi.Models.Dto
{
    public class ExamDto
    {
        public int Id { get; set; }
        public int SectorId { get; set; }
        public string Name { get; set; }
        public DateTime ExamStart { get; set; }
        public DateTime ExamEnd { get; set; }
        public int StudentsCount { get; set; }
        public int StudentTotal { get; set; }

        public ExamDto(Exam exam, int studentsCount, int StudentTotal)
        {
            Id = exam.Id;
            SectorId = exam.SectorId;
            Name = exam.Name;
            ExamStart = exam.ExamStart;
            ExamEnd = exam.ExamEnd;
            StudentsCount = studentsCount;
            this.StudentTotal = StudentTotal;
        }
    }
}

