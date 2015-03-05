using FutsalManager.Domain.Dtos;
using FutsalManager.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Interfaces
{
    public interface ITournamentRepository
    {
        IReadOnlyList<PlayerDto> Players { get; }
        IEnumerable<TournamentDto> GetAll();
        void RefreshPlayerCache();
        PlayerDto GetPlayerByItemId(long itemId);

        TournamentDto GetByDate(DateTime tournamentDate);
        string Add(TournamentDto tournament);
        IEnumerable<PlayerDto> GetPlayersByName(string playerName);
        PlayerDto GetPlayerById(string playerId);
        string AddEditTeam(TeamDto team);
        void AssignTeam(string tournamentId, TeamDto team);
        string AddEditPlayer(PlayerDto player);
        void AssignPlayer(PlayerDto player);
        string AddMatch(string tournamentId, MatchDto match);
        void AddMatchScore(string tournamentId, string matchId, string teamId, string playerId, string remark);
        IEnumerable<TeamDto> GetTeamsByTournament(string tournamentId);
        int GetTotalTeamsByTournament(string tournamentId);
        IEnumerable<PlayerDto> GetPlayersByTeam(string tournamentId, string teamId);
        int GetTotalPlayerByTeam(string tournamentId, string teamId);
        IEnumerable<MatchDto> GetMatches(string tournamentId);
    }
}
