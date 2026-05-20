using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Interfaces.Services;

public interface IMatchLineupService
{
    Task<Match> CreateAsync(MatchLineup matchLineup);
    Task DeleteAsync(int id);
    Task<IEnumerable<MatchLineup>> GetAllByMatchAsync(int matchId);
    Task<IEnumerable<MatchLineup>> GetAllByTeamAsync(int teamId);
}
