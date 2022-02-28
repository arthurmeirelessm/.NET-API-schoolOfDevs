using AutoMapper;
using SchoolOfDevs.Dto.Course;
using SchoolOfDevs.Dto.Note;
using SchoolOfDevs.Dto.User;
using SchoolOfDevs.Entities;

namespace SchoolOfDevs.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseRequest>();
            CreateMap<Course, CourseResponse>();

            CreateMap<CourseRequest, Course>();
            CreateMap<CourseResponse, Course>();
        }
    }
}
