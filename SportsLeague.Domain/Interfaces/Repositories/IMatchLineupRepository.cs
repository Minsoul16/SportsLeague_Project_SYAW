using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface IMatchLineupRepository : IGenericRepository<MatchLineup>
{
    Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);
    Task<IEnumerable<MatchLineup>> GetByTeamAndMatchAsync(int teamId, int matchId);
    Task<MatchLineup?> ExistsMatchLineupAsync(int matchId, int playerId);
    Task<int?> IdMatchLineupAsync(int matchId, int playerId);
    Task<int> CountStartersByMatchAndTeamAsync(int matchId,int teamId);
}