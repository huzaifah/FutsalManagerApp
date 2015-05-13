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
        //IReadOnlyList<PlayerDto> Players { get; }
        IEnumerable<TournamentDto> GetAll();
        IEnumerable<PlayerDto> GetAllPlayers(bool includeDeleted);
        void UpdatePlayerByTournament(PlayerDto player);
        //void RefreshPlayerCache();
        //PlayerDto GetPlayerByItemId(long itemId);

        TournamentDto GetByDate(DateTime tournamentDate);
        TournamentDto GetById(string tournamentId);
        string AddEdit(TournamentDto tournament);
        IEnumerable<PlayerDto> GetPlayersByName(string playerName);
        PlayerDto GetPlayerById(string playerId);
        string AddEditTeam(TeamDto team);
        void AssignTeam(string tournamentId, TeamDto team);
        void DeleteAllTeamsAssignment();
        string AddEditPlayer(PlayerDto player);
        void AssignPlayer(PlayerDto player);
        void DeleteAllPlayerAssignments();
        string AddMatch(string tournamentId, MatchDto match);
        void AddMatchScore(string tournamentId, string matchId, string teamId, string playerId, string remark);
        IEnumerable<TeamDto> GetTeamsByTournament(string tournamentId);
        IEnumerable<TeamDto> GetAllTeams();
        int GetTotalTeamsByTournament(string tournamentId);
        IEnumerable<PlayerDto> GetPlayersByTeam(string tournamentId, string teamId);
        int GetTotalPlayerByTeam(string tournamentId, string teamId);
        IEnumerable<MatchDto> GetMatches(string tournamentId);
        int GetTotalScoresByMatchTeam(string tournamentId, string matchId, string teamId);
        IEnumerable<ScoreDto> GetScoresByMatch(string tournamentId, string matchId);
        void DeleteAllTeams();
        PlayerDto GetPlayerStatusByTournament(string playerId, string tournamentId);
        void DeleteAllMatchesByTournament(string tournamentId);
        void UpdateMatch(MatchDto match);
        void DeletePlayer(PlayerDto player);
        void RunSqlStatement(string sql);
        void DeleteTournament(string tournamentId);
    }
}
