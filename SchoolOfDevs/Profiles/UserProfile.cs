using AutoMapper;
using SchoolOfDevs.Dto.User;
using SchoolOfDevs.Entities;

namespace SchoolOfDevs.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserRequest>();
            CreateMap<User, UserRequestUpdate>();
            CreateMap<User, UserResponse>();
        }
    }
}
