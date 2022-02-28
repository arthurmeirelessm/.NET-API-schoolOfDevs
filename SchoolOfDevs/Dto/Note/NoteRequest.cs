using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolOfDevs.Dto.Note
{
    public class NoteRequest
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal Value { get; set; }
    }
}
