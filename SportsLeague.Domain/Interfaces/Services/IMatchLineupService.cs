using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Interfaces.Services;

public interface IMatchLineupService
{
    Task<MatchLineup> CreateAsync(MatchLineup matchLineup);
    Task DeleteAsync(int playerId,int matchId);
    Task<IEnumerable<MatchLineup>> GetAllByMatchAsync(int matchId);
    Task<IEnumerable<MatchLineup>> GetAllByTeamAndMatchAsync(int teamId, int matchId);
}
