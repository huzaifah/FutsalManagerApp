using FutsalManager.Domain.Dtos;
using FutsalManager.Domain.Entity;
using FutsalManager.Domain.Enum;
using FutsalManager.Domain.Exceptions;
using FutsalManager.Domain.Helpers;
using FutsalManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain
{
    public class TournamentService
    {
        private readonly ITournamentRepository tournamentRepo;
        private List<PlayerDto> _players;

        public TournamentService(ITournamentRepository repo)
        {
            tournamentRepo = repo;
            RefreshPlayerCache();
        }

        public IReadOnlyList<PlayerDto> Players
        {
            get
            {
                if (_players == null)
                    _players = new List<PlayerDto>();

                return _players;
            }
        }

        public void RefreshPlayerCache()
        {
            if (_players == null)
                _players = new List<PlayerDto>();

            _players.Clear();
            _players = tournamentRepo.GetAllPlayers().ToList();

            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].ListItemId = i + 1;
            }
        }

        public PlayerDto GetPlayerByItemId(long itemId)
        {
            return _players.Single(x => x.ListItemId == itemId);
        }

        public void UpdatePlayerByTournament(PlayerDto player)
        {
            tournamentRepo.UpdatePlayerByTournament(player);
        }

        public string AddEditTournament(Tournament tournament)
        {
            var tournamentId = tournamentRepo.AddEdit(tournament.ConvertToDto());
            return tournamentId;
        }

        public string CreateTeam(Team team)
        {
            return tournamentRepo.AddEditTeam(team.ConvertToDto());
        }

        public void AssignTeam(Tournament tournament, Team team)
        {
            var teamCount = tournamentRepo.GetTotalTeamsByTournament(tournament.Id);

            if (teamCount >= tournament.TotalTeam)
                throw new ExceedTotalTeamsException();
                        
            tournamentRepo.AssignTeam(tournament.Id, team.ConvertToDto());            
        }

        public void AssignPlayer(Tournament tournament, Team team, Player player)
        {
            var teamList = RetrieveTeams(tournament);

            if (teamList.SingleOrDefault(t => t.Name == team.Name) == null)
                throw new TeamNotFoundException();

            tournamentRepo.AssignPlayer(player.ConvertToDto());
        }

        public string AddMatch(Tournament tournament, Team home, Team away)
        {
            var teamList = RetrieveTeams(tournament);

            //var matchTeam = from t in teamList
            //            where t.Name == home.Name || t.Name == away.Name
            //            select t;

            if (teamList.Where(t => t.Name == home.Name || t.Name == away.Name).Count() != 2)
                throw new TeamNotFoundException();

            var match = new Match(home, away);
                        
            var matchId = tournamentRepo.AddMatch(tournament.Id, match.ConvertToDto());
            return matchId;
        }

        public string AddMatch(Tournament tournament, Match match)
        {
            var teamList = RetrieveTeams(tournament);

            if (teamList.FirstOrDefault(x => x.Name == match.HomeTeam.Name)!=null
                || 
                teamList.FirstOrDefault(x => x.Name == match.AwayTeam.Name)!=null)
                throw new TeamNotFoundException();

            var matchId = tournamentRepo.AddMatch(tournament.Id, match.ConvertToDto());
            return matchId;
        }

        public IEnumerable<Tournament> RetrieveTournament()
        {
            var tournamentList = tournamentRepo.GetAll();
            var tournaments = tournamentList.ToList().ConvertAll(t => t.ConvertToEntity());
            return tournaments;
        }

        public Tournament RetrieveTournamentByDate(DateTime tournamentDate)
        {
            var tournament = tournamentRepo.GetByDate(tournamentDate);
            return tournament.ConvertToEntity();
        }

        public Tournament RetrieveTournamentById(string tournamentId)
        {
            var tournament = tournamentRepo.GetById(tournamentId);
            return tournament.ConvertToEntity();
        }

        public void AddScore(Tournament tournament, Match match, TeamSide scoredSide, string playerId, string remark = "")
        {
            var scoredTeam = scoredSide == TeamSide.Home ? match.HomeTeam : match.AwayTeam;

            var playerList = tournamentRepo.GetPlayersByTeam(tournament.Id, scoredTeam.Id);
            var player = playerList.SingleOrDefault(x => x.Id == playerId);

            if (player == null)
                if (String.IsNullOrEmpty(remark))
                    throw new PlayerNotFoundException();
                else
                    player = tournamentRepo.GetPlayerById(playerId);

            tournamentRepo.AddMatchScore(tournament.Id, match.Id, scoredTeam.Id, player.Id, remark);
        }

        public IEnumerable<Player> RetrievePlayers(Tournament tournament, Team team)
        {
            var players = tournamentRepo.GetPlayersByTeam(tournament.Id, team.Id);
            return players.ToList().ConvertAll(player => player.ConvertToEntity());
        }

        public IEnumerable<Match> RetrieveMatches(Tournament tournament)
        {
            var tournaments = tournamentRepo.GetMatches(tournament.Id);
            return tournaments.ToList().ConvertAll(t => t.ConvertToEntity());
        }

        public IEnumerable<Team> RetrieveTeams(Tournament tournament)
        {
            var teamList = tournamentRepo.GetTeamsByTournament(tournament.Id);
            return teamList.ToList().ConvertAll(t => t.ConvertToEntity());
        }

        public PlayerDto GetPlayerById(string playerId)
        {
            return tournamentRepo.GetPlayerById(playerId);
        }

        public string AddEditPlayer(PlayerDto player)
        {
            var playerId = tournamentRepo.AddEditPlayer(player);

            if (String.IsNullOrEmpty(player.Id))
            {
                player.Id = playerId;
                player.ListItemId = _players.Count + 1;

                _players.Add(player);
            }

            return playerId;
        }
    }
}
