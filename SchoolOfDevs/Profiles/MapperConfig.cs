using AutoMapper;

namespace SchoolOfDevs.Profiles
{
    public static class MapperConfig
    {
        public static MapperConfiguration GetMapperConfig()
        {
            return new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
            });
        }
    }
}
