using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class MatchLineupRepository : GenericRepository<MatchLineup>, IMatchLineupRepository
{
    public MatchLineupRepository(LeagueDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId)
    {
        return await _dbSet
            .Where(ml => ml.MatchId == matchId)
            .ToListAsync();
    }

    public async Task<IEnumerable<MatchLineup>> GetByTeamAsync(int teamId)
    {
        return await _dbSet
            .Include(ml => ml.Player)
            .ThenInclude(p => p.TeamId)
            .Where(ml => ml.Player.TeamId == teamId)
            .ToListAsync();
    }

    public async Task<MatchLineup?> ExistsMatchLineupAsync(int matchId, int playerId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(ml => ml.MatchId == matchId && ml.PlayerId == playerId);
    }

    public async Task<int> CountStartersByMatchAndTeamAsync(int matchId,int teamId)
    {
        return await _dbSet
            .Include(ml => ml.Player)
            .CountAsync(ml =>
                ml.MatchId == matchId &&
                ml.IsStarter &&
                ml.Player.TeamId == teamId);
    }
}
