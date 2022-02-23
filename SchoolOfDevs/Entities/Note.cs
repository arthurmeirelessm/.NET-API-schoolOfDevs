using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolOfDevs.Entities
{
    public class Note : BaseEntity
    {
     [ForeignKey("User")]
     public int StudentId { get; set; }

     [ForeignKey("Course")]
     public int CourseId { get; set; }

     [Column(TypeName = "decimal(18,2)")]
     public decimal Value { get; set; }
     public virtual User Student { get; set; }
     public virtual Course Course { get; set; }


   
    }
}
