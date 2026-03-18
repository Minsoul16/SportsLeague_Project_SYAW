using AutoMapper;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;

namespace SportsLeague.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Team mappings
        CreateMap<TeamRequestDTO, Team>();
        CreateMap<Team, TeamResponseDTO>();

        // Player mappings
        CreateMap<PlayerRequestDTO, Player>();
        CreateMap<Player, PlayerResponseDTO>()
        .ForMember( //Genera un relación con equipo (mapea automaticamente los campos requeridos de Team)
        dest => dest.TeamName,
        opt => opt.MapFrom(src => src.Team.Name));
    }

}