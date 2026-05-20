using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface IMatchLineupRepository : IGenericRepository<MatchLineup>
{
    Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);
    Task<IEnumerable<MatchLineup>> GetByTeamAsync(int teamId);
}