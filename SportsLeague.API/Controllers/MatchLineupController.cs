using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;
using SportsLeague.Domain.Services;

namespace SportsLeague.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchLineupController : ControllerBase
{
    private readonly IMatchLineupService _matchLineupService;
    private readonly IMapper _mapper;

    public MatchLineupController(
        IMatchLineupService matchLineupService,
        IMapper mapper)
    {
        _matchLineupService = matchLineupService;
        _mapper = mapper;
    }

    [HttpPost("{matchId}/lineup")]
    public async Task<ActionResult<MatchLineupResponseDTO>> Create(int matchId, MatchLineupRequestDTO dto)
    {
        try
        {
            var matchLineup = _mapper.Map<MatchLineup>(dto);
            matchLineup.MatchId = matchId;
            var newMatchLineup = await _matchLineupService.CreateAsync(matchLineup);
            return Ok(_mapper.Map<MatchLineupResponseDTO>(newMatchLineup));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpGet("{matchId}/lineup")]
    public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetByMatch(int matchId)
    {
        try
        {
            var matchLineups = await _matchLineupService.GetAllByMatchAsync(matchId);
            return Ok(_mapper.Map<IEnumerable<MatchLineupResponseDTO>>(matchLineups));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{matchId}/lineup/team/{teamId}")]
    public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetByTeamAndMatch(
        int teamId,int matchId)
    {
        try
        {
            var matchLineups = await _matchLineupService.GetAllByTeamAndMatchAsync(teamId,matchId);
            return Ok(_mapper.Map<IEnumerable<MatchLineupResponseDTO>>(matchLineups));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{matchId}/lineup/{id}")]
    public async Task<IActionResult> DeleteAsync(int matchId, int id)
    {
        try
        {
            await _matchLineupService.DeleteAsync(id, matchId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}