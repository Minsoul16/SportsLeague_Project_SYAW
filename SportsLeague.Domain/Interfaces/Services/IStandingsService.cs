namespace SportsLeague.Domain.Interfaces.Services;

public interface IStandingsService
{//El keyword object permite que estos métodos reciban cualquier tipo y forma de dato y lo devuelva
    Task<object> GetStandingsAsync(int tournamentId); //Para obtener tabla de posiciones
    Task<object> GetTopScorersAsync(int tournamentId); //Obtener máximos goleadores
    Task<object> GetCardStatsAsync(int tournamentId); //Obtener estadísticas de tarjetas para un torneo
}