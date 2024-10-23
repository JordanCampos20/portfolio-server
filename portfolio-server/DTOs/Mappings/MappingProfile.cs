using AutoMapper;
using portfolio_server.Models;

namespace portfolio_server.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contato, ContatoDTO>().ReverseMap();
        }
    }
}