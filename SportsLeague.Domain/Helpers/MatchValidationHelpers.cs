using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.Domain.Helpers;

public class MatchValidationHelper
{
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IMatchLineupRepository _matchLineupRepository;


    public MatchValidationHelper(
        IMatchRepository matchRepository,
        IPlayerRepository playerRepository,
        IMatchLineupRepository matchLineupRepository)
    {
        _matchRepository = matchRepository;
        _playerRepository = playerRepository;
        _matchLineupRepository = matchLineupRepository;
    }


    public async Task<Match> ValidateMatchForEventAsync(int matchId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException(
                $"No se encontró el partido con ID {matchId}");


        if (match.Status != MatchStatus.InProgress &&
            match.Status != MatchStatus.Finished)
            throw new InvalidOperationException(
                "Solo se pueden registrar eventos en partidos InProgress o Finished");


        return match;
    }

    public async Task<Match> ValidateMatchIsScheduledAsync(int matchId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
            throw new KeyNotFoundException(
                $"No se encontró el partido con ID {matchId}");


        if (match.Status != MatchStatus.Scheduled)
            throw new InvalidOperationException(
                "Solo se pueden registrar eventos en partidos Scheduled");


        return match;
    }

    public async Task<Player> ValidatePlayerInMatchAsync(
        int playerId, Match match)
    {
        var player = await _playerRepository.GetByIdAsync(playerId);
        if (player == null)
            throw new KeyNotFoundException(
                $"No se encontró el jugador con ID {playerId}");


        if (player.TeamId != match.HomeTeamId &&
            player.TeamId != match.AwayTeamId)
            throw new InvalidOperationException(
                "El jugador no pertenece a ninguno de los equipos del partido");


        return player;
    }


    public static void ValidateMinute(int minute)
    {
        if (minute < 1 || minute > 120)
            throw new InvalidOperationException(
                "El minuto debe estar entre 1 y 120");
    }

    public async Task ValidateMatchLineupForPlayerAsync(int playerId, int matchId, bool isStarter)
    {
        var match = await ValidateMatchIsScheduledAsync(matchId);

        var player = await ValidatePlayerInMatchAsync(playerId, match);

        var matchLineup = await _matchLineupRepository
            .ExistsMatchLineupAsync(matchId, playerId);

        if (matchLineup != null)
        {
            throw new InvalidOperationException(
                "El jugador ya está registrado en la alineación de este partido");
        }

        if (isStarter)
        {
            var starters = await _matchLineupRepository
                .CountStartersByMatchAndTeamAsync(matchId, player.TeamId);

            if (starters >= 11)
            {
                throw new InvalidOperationException(
                    "El equipo ya tiene 11 titulares registrados en este partido");
            }
        }
    }
}