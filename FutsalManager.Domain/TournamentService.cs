using FutsalManager.Domain.Dtos;
using FutsalManager.Domain.Entity;
using FutsalManager.Domain.Enums;
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
        private List<TournamentDto> _tournaments;

        public TournamentService(ITournamentRepository repo)
        {
            tournamentRepo = repo;
            RefreshPlayerCache();
            RefreshTournamentCache();
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
            _players = tournamentRepo.GetAllPlayers(false).ToList();

            _players.Add(new PlayerDto { Name = "0" });
            _players.OrderBy(p => p.Name);

            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].ListItemId = i + 1;
            }
        }

        public PlayerDto GetPlayerByItemId(long itemId)
        {
            return _players.Single(x => x.ListItemId == itemId);
        }

        public IReadOnlyList<TournamentDto> Tournaments
        {
            get
            {
                if (_tournaments == null)
                    _tournaments = new List<TournamentDto>();

                return _tournaments;
            }
        }

        public void RefreshTournamentCache()
        {
            if (_tournaments == null)
                _tournaments = new List<TournamentDto>();

            _tournaments.Clear();
            _tournaments = tournamentRepo.GetAll().ToList();

            _tournaments.Add(new TournamentDto { Id = "" });
            _tournaments.OrderByDescending(p => p.Date);

            for (int i = 0; i < _tournaments.Count; i++)
            {
                _tournaments[i].ListItemId = i + 1;
            }
        }

        public TournamentDto GetTournamentByItemId(long itemId)
        {
            return _tournaments.Single(x => x.ListItemId == itemId);
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

            var teams = tournamentRepo.GetAllTeams();
            var teamToAssign = teams.Single(t => t.Name.Equals(team.Name, StringComparison.OrdinalIgnoreCase));

            tournamentRepo.AssignTeam(tournament.Id, teamToAssign);            
        }

        public IEnumerable<TeamDto> GetAllTeams()
        {
            var teams = tournamentRepo.GetAllTeams();
            return teams;
        }

        public void AssignPlayer(Tournament tournament, Team team, Player player)
        {
            var teamList = RetrieveTeams(tournament);

            if (teamList.SingleOrDefault(t => t.Name == team.Name) == null)
                throw new TeamNotFoundException();

            player.TeamId = team.Id;
            player.TournamentId = tournament.Id;

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

            if (teamList == null)
                return new List<Team>();

            return teamList.ToList().ConvertAll(t => t.ConvertToEntity());
        }

        public PlayerDto GetPlayerById(string playerId)
        {
            return tournamentRepo.GetPlayerById(playerId);
        }

        public PlayerDto GetPlayerByTournament(string playerId, string tournamentId)
        {
            return tournamentRepo.GetPlayerStatusByTournament(playerId, tournamentId);
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

        public void GenerateMatches(Tournament tournament)
        {
            // find all assigned teams
            var teams = RetrieveTeams(tournament);

            foreach (var hometeam in teams)
            {
                var teamsToPlay = teams.Where(t => t.Name != hometeam.Name);

                foreach (var awayteam in teamsToPlay)
                {
                    AddMatch(tournament, hometeam, awayteam);
                }
            }
        }

        public IEnumerable<TeamStanding> GenerateStanding(Tournament tournament)
        {
            var teamStandings = new List<TeamStanding>();

            // find all completed matches
            var matches = RetrieveMatches(tournament).Where(m => m.IsCompleted == true);

            // find all teams
            var teams = RetrieveTeams(tournament);

            // iterate each team
            foreach (var team in teams)
            {
                var teamStanding = new TeamStanding();
                teamStanding.Team = team;

                // find home match
                var totalHome = matches.Count(m => m.HomeTeam.Name == team.Name);

                // find away match
                var totalAway = matches.Count(m => m.AwayTeam.Name == team.Name);

                // calculate total matches
                teamStanding.TotalMatches = totalHome + totalAway;

                var teamMatches = matches.Where(m => m.HomeTeam.Name == team.Name || m.AwayTeam.Name == team.Name);
                int totalWins = 0;
                int totalDraws = 0;
                int totalLoses = 0;

                foreach (var match in teamMatches)
                {
                    // retrieve scores
                    var scores = GetAllScoresByMatch(tournament, match);

                    var goals = scores.Count(s => s.TeamId == team.Id);
                    var concedes = scores.Count(s => s.TeamId != team.Id);
                    teamStanding.TotalScores += goals;
                    teamStanding.TotalConceded += concedes;

                    if (goals > concedes)
                        totalWins++;
                    else if (goals == concedes)
                        totalDraws++;
                    else
                        totalLoses++;
                }

                teamStanding.GoalDifference = teamStanding.TotalScores - teamStanding.TotalConceded;
                teamStanding.TotalWins = totalWins;
                teamStanding.TotalLoses = totalLoses;
                teamStanding.TotalDraws = totalDraws;

                // calculate points
                teamStanding.TotalPoints = (totalWins * 3) + (totalDraws);

                teamStandings.Add(teamStanding);
            }

            // sort teamStandings by total points and goal difference
            teamStandings.OrderByDescending(s => s.TotalPoints).ThenByDescending(s => s.GoalDifference);

            return teamStandings;
        }

        public int GetTotalScoresByTeam(Tournament tournament, Match match, Team team)
        {
            var scores = tournamentRepo.GetTotalScoresByMatchTeam(tournament.Id, match.Id, team.Id);
            return scores;
        }

        public IEnumerable<ScoreDto> GetAllScoresByMatch(Tournament tournament, Match match)
        {
            var scores = tournamentRepo.GetScoresByMatch(tournament.Id, match.Id);
            return scores;
        }

        public void GenerateDeciderMatch(Tournament tournament, List<TeamStanding> teamStandings)
        {
            // validate total matches are equal
            var teams = teamStandings.Count;
            var minMatch = (teams - 1) * 2;

            foreach (var standing in teamStandings)
            {
                if (standing.TotalMatches < minMatch)
                    throw new ApplicationException("Incomplete total matches for team " + standing.Team.Name);
            }

            // sort teamStandings by total points and goal difference
            teamStandings.OrderByDescending(s => s.TotalPoints).ThenByDescending(s => s.GoalDifference);

            // assign rank to each team
            int i = 0;
            teamStandings.ForEach(t => t.Ranking = i++);

            // add final match
            if (teamStandings[0] != null && teamStandings[1] != null)
            {
                var finalMatch = new Match
                {
                    HomeTeam = teamStandings.Single(t => t.Ranking == 1).Team,
                    AwayTeam = teamStandings.Single(t => t.Ranking == 2).Team,
                    Type = "Final"
                };

                AddMatch(tournament, finalMatch);
            }

            // add third placing
            if (teamStandings[2] != null && teamStandings[3] != null)
            {
                var thirdPlacing = new Match
                {
                    HomeTeam = teamStandings.Single(t => t.Ranking == 3).Team,
                    AwayTeam = teamStandings.Single(t => t.Ranking == 4).Team,
                    Type = "3rdPlacing"
                };

                AddMatch(tournament, thirdPlacing);
            }
        }

        public IEnumerable<MatchDto> RetrieveDeciderMatch(Tournament tournament)
        {
            var matches = RetrieveMatches(tournament);

            var final = matches.SingleOrDefault(m => m.Type == "Final");
            var thirdPlacing = matches.SingleOrDefault(m => m.Type == "3rdPlacing");

            if (final == null || thirdPlacing == null)
                throw new ApplicationException("One or all decider matches are not found");

            var deciderMatches = new List<MatchDto>();
            deciderMatches.Add(final.ConvertToDto());
            deciderMatches.Add(thirdPlacing.ConvertToDto());

            return deciderMatches;
        }

        public void CompleteMatch(Match match)
        {
            match.IsCompleted = true;
            tournamentRepo.UpdateMatch(match.ConvertToDto());
        }

        public void EndTournament(Tournament tournament)
        {
            // validate if there's any incomplete matches
            var incompleteMatches = RetrieveMatches(tournament).Where(m => m.IsCompleted == false).Count();
            
            if (incompleteMatches != 0)
                throw new IncompleteMatchFoundException(String.Format("{0} matches are incomplete", incompleteMatches));

            tournament.Status = TournamentStatus.Completed;
            AddEditTournament(tournament);
        }

        public void StartTournament(Tournament tournament)
        {
            // change tournament status
            tournament.Status = Domain.Enums.TournamentStatus.InProgress;            
            AddEditTournament(tournament);
        }

        public void DeletePlayer(string playerId)
        {
            var player = tournamentRepo.GetPlayerById(playerId);

            if (player == null)
                throw new PlayerNotFoundException();

            tournamentRepo.DeletePlayer(player);
        }

        public void DeleteTournament(string tournamentId)
        {
            var tournament = RetrieveTournamentById(tournamentId);

            if (tournament.Status != TournamentStatus.NotStarted)
                throw new ApplicationException("This tournament is in progress or ended. Therefore, it cannot be removed.");

            tournamentRepo.DeleteTournament(tournamentId);
        }
    }
}
