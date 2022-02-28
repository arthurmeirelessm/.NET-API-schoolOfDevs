using SchoolOfDevs.Enuns;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SchoolOfDevs.Entities
{
    public class User : BaseEntity
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }
        [NotMapped]
        public string CurrentPassword { get; set; }
        public TypeUser TypeUser { get; set; }
        [JsonIgnore]
        public ICollection<Course> CoursesTeaching { get; set; }
        [JsonIgnore]
        public ICollection<Course> CoursesStuding { get; set; }
        [JsonIgnore]
        public List<StudentCourse> StudentCourses { get; set; }
    }
}
