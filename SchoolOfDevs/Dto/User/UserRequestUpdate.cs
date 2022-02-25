using SchoolOfDevs.Enuns;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolOfDevs.Dto.User
{
    public class UserRequestUpdate : UserRequest
    {
        public string CurrentPassword { get; set; }
    }
}
