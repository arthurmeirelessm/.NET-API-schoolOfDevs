namespace SchoolOfDevs.Dto.Note
{
    public class NoteResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal Value { get; set; }
        public DateTime CreatedId { get; set; }
        public DateTime UpdatedId { get; set; }
        public virtual Entities.User Student { get; set; }
        public virtual Entities.Course Course { get; set; }

    }
}
