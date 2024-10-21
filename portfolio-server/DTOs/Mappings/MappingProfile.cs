using AutoMapper;
using finance_control_server.Models;

namespace finance_control_server.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contato, ContatoDTO>().ReverseMap();
        }
    }
}