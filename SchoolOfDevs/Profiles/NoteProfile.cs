using AutoMapper;
using SchoolOfDevs.Dto.Note;
using SchoolOfDevs.Dto.User;
using SchoolOfDevs.Entities;

namespace SchoolOfDevs.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, NoteRequest>();
            CreateMap<Note, NoteResponse>();

            CreateMap<NoteRequest, Note>();
            CreateMap<NoteResponse, Note>();
        }
    }
}
