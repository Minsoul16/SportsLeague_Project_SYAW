using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface ITournamentTeamRepository : IGenericRepository<TournamentTeam>
{
    Task<TournamentTeam?> GetByTournamentAndTeamAsync(int tournamentId, int teamId); //Relación entre torneo y equipo
    Task<IEnumerable<TournamentTeam>> GetByTournamentAsync(int tournamentId); //Relaciones de equipos en un torneo especifico
    Task<IEnumerable<TournamentTeam>> GetByTeamAsync(int teamId); //Relaciones de torneos para un equipo especifico
}