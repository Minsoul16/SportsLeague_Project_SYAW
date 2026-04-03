using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories;
public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
{
    Task<TournamentSponsor?> GetByTournamentAndSponsorAsync(int tournamentId, int sponsorId); //Relation beetwen sponsor and tournament
    Task<IEnumerable<TournamentSponsor>> GetByTournamentAsync(int tournamentId); //Sponsors in an specific tournament
    Task<IEnumerable<TournamentSponsor>> GetBySponsorAsync(int sponsorId); //Tournaments of an specific sponsor
}