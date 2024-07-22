using AutoMapper;
using Models.Customer;
using Models.Registration;
using System.Text;

namespace WebAPI.MapProfiles
{
    public class MapProfiles : Profile
    {
        public MapProfiles()
        {
            // converte a password de string para byte[]
            CreateMap<RegisterRequest, UserRegistration>()
                .ForMember(dest => dest.Password, 
                act => act.MapFrom(src => Encoding.UTF8.GetBytes(src.Password)));
        }
    }
}
