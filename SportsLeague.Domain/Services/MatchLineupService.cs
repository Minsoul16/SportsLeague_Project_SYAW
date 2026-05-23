using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SportsLeague.Domain.Services;

public class MatchLineupService :IMatchLineupService
{
    private readonly IMatchLineupRepository _matchLineupRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IPlayerRepository _playerRepository;
    MatchValidationHelper _validationHelper;
    private readonly ILogger<MatchLineupService> _logger;

    public MatchLineupService(
        IMatchLineupRepository matchLineupRepository,
        IPlayerRepository playerRepository,
        ITeamRepository teamRepository,
        IMatchRepository matchRepository,
        MatchValidationHelper validationHelper,
        ILogger<MatchLineupService> logger)
    {
        _matchLineupRepository = matchLineupRepository;
        _teamRepository = teamRepository;
        _matchRepository = matchRepository;
        _playerRepository = playerRepository;
        _validationHelper = validationHelper;
        _logger = logger;
    }

    public async Task<IEnumerable<MatchLineup>> GetAllByMatchAsync(int matchId)
    {
        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
        {
            throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");
        }

        return await _matchLineupRepository.GetByMatchAsync(matchId);
    }

    public async Task<IEnumerable<MatchLineup>> GetAllByTeamAndMatchAsync(int teamId, int matchId)
    { 
        var team = await _teamRepository.GetByIdAsync(teamId);
        if(team == null)
        {
            throw new KeyNotFoundException($"No se encontró el equipo con ID {teamId}");
        }

        var match = await _matchRepository.GetByIdAsync(matchId);
        if (match == null)
        {
            throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");
        }
        
        var matchLineup = await _matchLineupRepository.GetByTeamAndMatchAsync(teamId, matchId);
        if (matchLineup == null)
        {
            throw new KeyNotFoundException($"No se encontraron alineaciones para ID de equipo {teamId} con ID de partido {matchId}");
        }

        return matchLineup;
    }

    public async Task<MatchLineup> CreateAsync(MatchLineup matchLineup)
    {
        var match = await _validationHelper.ValidateMatchForEventAsync(matchLineup.MatchId);
        await _validationHelper.ValidatePlayerInMatchAsync(matchLineup.PlayerId, match);
        await _validationHelper.ValidateMatchLineupForPlayerAsync(matchLineup.PlayerId, matchLineup.MatchId);

        _logger.LogInformation(
            "Registering MatchLineup: Match {MatchId}, Player {PlayerId}",
            matchLineup.MatchId, matchLineup.PlayerId);
        return await _matchLineupRepository.CreateAsync(matchLineup);
    }

    public async Task DeleteAsync(int playerId, int matchId)
    {
        var existing = await _matchLineupRepository.ExistsMatchLineupAsync(matchId, playerId);
        if (existing == null)
        {
            throw new KeyNotFoundException($"No se encontró la alineación para ID de jugador {playerId} con ID de partido {matchId}");
        }

        var IdMatchLineup = await _matchLineupRepository.IdMatchLineupAsync(matchId, playerId);
        if (IdMatchLineup == null)
        {
            throw new KeyNotFoundException($"No se encontró la alineación ni su ID");
        }

        _logger.LogInformation("Deleting matchLineup with player ID: {PlayerId} and match ID: {MatchId}", playerId, matchId);
        await _matchLineupRepository.DeleteAsync(IdMatchLineup.Value);
    }
}
