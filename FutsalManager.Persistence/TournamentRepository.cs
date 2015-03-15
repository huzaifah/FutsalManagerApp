using FutsalManager.Domain.Dtos;
using FutsalManager.Domain.Exceptions;
using FutsalManager.Domain.Interfaces;
using FutsalManager.Persistence.Entities;
using FutsalManager.Persistence.Helpers;
using Android.Database.Sqlite;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Persistence
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly SQLiteConnection db;

        public TournamentRepository(string databasePath)
        {
            db = new SQLiteConnection(databasePath);

            if (!db.GetTableInfo("Players").Any()) 
                CreateNewTables();
        }

        public void CreateNewTables()
        {
            db.CreateTable<Tournaments>();
            db.CreateTable<Players>();
            db.CreateTable<PlayerAssignments>();
            db.CreateTable<Teams>();
            db.CreateTable<TeamAssignments>();
            db.CreateTable<Matchs>();
            db.CreateTable<Scores>();
        }
        
        public IEnumerable<PlayerDto> GetAllPlayers()
        {
            var players = db.Query<Players>("Select * from Players order by Name");
            var playerList = players.ConvertAll(p => p.ConvertToDto());

            for (int i = 0; i < playerList.Count; i++)            
            {
                var player = playerList[i];

                var playerId = Guid.Parse(player.Id);

                var scores = db.Table<Scores>().Count(s => s.PlayerId == playerId);                
                player.TotalGoals = Convert.ToInt64(scores);
                
            }

            return playerList;
        }

        public IEnumerable<TournamentDto> GetAll()
        {
            var tournaments = db.Query<Tournaments>("Select * from Tournament");
            return tournaments.ConvertAll(t => t.ConvertToDto());            
        }

        public TournamentDto GetByDate(DateTime tournamentDate)
        {
            var tournament = db.Table<Tournaments>().FirstOrDefault(x => x.Date == tournamentDate);
            return tournament.ConvertToDto();
        }

        public TournamentDto GetById(string tournamentId)
        {
            var tournament = db.Table<Tournaments>().FirstOrDefault(x => x.Id == Guid.Parse(tournamentId));
            return tournament.ConvertToDto();
        }

        public string AddEdit(TournamentDto tournament)
        {
            if (String.IsNullOrEmpty(tournament.Id))
            {
                tournament.Id = Guid.NewGuid().ToString();
                db.Insert(tournament.ConvertToDb(), typeof(Tournaments));
            }
            else
            {
                db.Update(tournament.ConvertToDb(), typeof(Tournaments));
            }
                
            return tournament.Id;
        }

        public IEnumerable<PlayerDto> GetPlayersByName(string playerName)
        {
            playerName = "%" + playerName + "%";
            var players = db.Table<Players>().Where(p => p.Name == playerName).ToList();
            return players.ConvertAll(player => player.ConvertToDto());
        }

        public PlayerDto GetPlayerById(string playerId)
        {
            Guid playerGuid;

            if (!Guid.TryParse(playerId, out playerGuid))
                throw new ArgumentException("Player id is invalid");
            
            var player = db.Table<Players>().Where(p => p.Id == playerGuid).SingleOrDefault();
            return player.ConvertToDto();
        }

        public PlayerDto GetPlayerById(Guid playerGuid)
        {
            var player = db.Table<Players>().Where(p => p.Id == playerGuid).Single();
            return player.ConvertToDto();
        }

        public void AssignPlayer(PlayerDto player)
        {
            Guid playerId, tournamentId, teamId;

            if (Guid.TryParse(player.Id, out playerId) && Guid.TryParse(player.TeamId, out teamId) && Guid.TryParse(player.TournamentId, out tournamentId))
                db.Insert(new PlayerAssignments { PlayerId = playerId, TournamentId = tournamentId, TeamId = teamId }, typeof(PlayerAssignments));
        }

        public string AddEditPlayer(PlayerDto player)
        {
            if (GetPlayerById(player.Id) == null)
            {
                player.Id = Guid.NewGuid().ToString();
                db.Insert(player.ConvertToDb(), typeof(Players));
            }
            else
            {
                db.Update(player.ConvertToDb(), typeof(Players));
            }

            return player.Id;
        }

        public void UpdatePlayerByTournament(PlayerDto player)
        {
            var playerAssignment = db.Table<PlayerAssignments>()
                .SingleOrDefault(p => p.PlayerId == Guid.Parse(player.Id) && p.TournamentId == Guid.Parse(player.TournamentId));

            if (playerAssignment == null)
                throw new ApplicationException("Player " + player.Id + " not found");

            playerAssignment.Attendance = player.Attendance;
            playerAssignment.Paid = player.Paid;

            db.Update(playerAssignment, typeof(PlayerAssignments));
        }

        public IEnumerable<PlayerDto> GetPlayersByTeam(string tournamentId, string teamId)
        {
            Guid tournamentGuid, teamGuid = Guid.Empty;

            if (Guid.TryParse(tournamentId, out tournamentGuid) && Guid.TryParse(teamId, out teamGuid))
                throw new ArgumentException("Tournament id or team id is invalid");

            var teamPlayers = db.Table<PlayerAssignments>().Where(x => x.TournamentId == tournamentGuid && x.TeamId == teamGuid);
            var playerList = teamPlayers.ToList().ConvertAll(player => new PlayerDto
                {
                    Id = player.PlayerId.ToString(),
                    Name = GetPlayerById(player.PlayerId).Name,
                    TeamId = player.TeamId.ToString(),
                    TournamentId = player.TournamentId.ToString(),
                    Attendance = player.Attendance,
                    Paid = player.Paid
                });

            return playerList;
        }

        public int GetTotalPlayerByTeam(string tournamentId, string teamId)
        {
            Guid tournamentGuid, teamGuid = Guid.Empty;

            if (Guid.TryParse(tournamentId, out tournamentGuid) && Guid.TryParse(teamId, out teamGuid))
                throw new ArgumentException("Tournament id or team id is invalid");

            return db.Table<PlayerAssignments>().Count(x => x.TournamentId == tournamentGuid && x.TeamId == teamGuid);
        }

        public string AddEditTeam(TeamDto team)
        {
            if (!String.IsNullOrEmpty(team.Id))
            {   
                team.Id = Guid.NewGuid().ToString();
                db.Insert(new Teams { Id = Guid.NewGuid(), Name = team.Name }, typeof(Teams));
            }
            else
            {
                db.Update(team.ConvertToDb(), typeof(Teams));
            }

            return team.Id;
        }

        public void AssignTeam(string tournamentId, TeamDto team)
        {
            Guid teamGuid;

            if (!Guid.TryParse(team.Id, out teamGuid))
                throw new ArgumentException("Team id is invalid");

            if (GetTeamById(teamGuid) == null)
                throw new TeamNotFoundException();

            Guid tournamentGuid;

            if (!Guid.TryParse(tournamentId, out tournamentGuid))
                throw new ArgumentException("Tournament id is invalid");

            db.Insert(new TeamAssignments { TeamId = teamGuid, TournamentId = tournamentGuid }, typeof(TeamAssignments));
        }

        public TeamDto GetTeamByName(string teamName)
        {
            var team = db.Table<Teams>().First(t => t.Name == teamName);
            return team.ConvertToDto();
        }

        public TeamDto GetTeamById(Guid teamId)
        {
            var team = db.Table<Teams>().First(t => t.Id == teamId);
            return team.ConvertToDto();
        }

        public IEnumerable<TeamDto> GetTeamsByTournament(string tournamentId)
        {
            Guid tournamentGuid;

            if (!Guid.TryParse(tournamentId, out tournamentGuid))
                throw new ArgumentException("Tournament id is invalid");

            var teams = db.Table<TeamAssignments>().Where(x => x.TournamentId == tournamentGuid);

            IEnumerable<TeamDto> teamsWithNames = null;
            
            if (teams.Any())
            {
                teamsWithNames = teams.ToList().ConvertAll(team =>
                    new TeamDto
                    {
                        Id = team.TeamId.ToString(),
                        Name = GetTeamById(team.TeamId).Name,
                        TournamentId = team.TournamentId.ToString()
                    });
            }

            return teamsWithNames;
        }

        public int GetTotalTeamsByTournament(string tournamentId)
        {
            Guid tournamentGuid;

            if (!Guid.TryParse(tournamentId, out tournamentGuid))
                throw new ArgumentException("Tournament id is invalid");

            return db.Table<TeamAssignments>().Count(x => x.TournamentId == tournamentGuid);
        }

        public string AddMatch(string tournamentId, MatchDto match)
        {
            var matchToSave = match.ConvertToDb();

            if (String.IsNullOrEmpty(match.Id))
                matchToSave.Id = Guid.NewGuid();

            matchToSave.TournamentId = Guid.Parse(tournamentId);

            db.Insert(matchToSave, typeof(Matchs));

            return matchToSave.Id.ToString();
        }

        public IEnumerable<MatchDto> GetMatches(string tournamentId)
        {
            Guid tournamentGuid;

            if (!Guid.TryParse(tournamentId, out tournamentGuid))
                throw new ArgumentException("Tournament id is invalid");

            var matches = db.Table<Matchs>().Where(m => m.TournamentId == tournamentGuid);

            return matches.ToList().ConvertAll(m => m.ConvertToDto());
        }

        public void AddMatchScore(string tournamentId, string matchId, string teamId, string playerId, string remark = "")
        {
            var score = new Scores
            {
                TournamentId = Guid.Parse(tournamentId),
                MatchId = Guid.Parse(matchId),
                TeamId = Guid.Parse(teamId),
                PlayerId = Guid.Parse(playerId),
                Remark = remark
            };

            db.Insert(score, typeof(Scores));
        }

        public int GetTotalScoresByMatchTeam(string tournamentId, string matchId, string teamId)
        {
            Guid tournamentGuid = Guid.Parse(tournamentId);
            Guid matchGuid = Guid.Parse(matchId);
            Guid teamGuid = Guid.Parse(teamId);

            var scores = db.Table<Scores>().Count(m => m.TournamentId == tournamentGuid && m.MatchId == matchGuid && m.TeamId == teamGuid);

            return scores;
        }

        public IEnumerable<ScoreDto> GetScoresByMatch(string tournamentId, string matchId)
        {
            Guid tournamentGuid = Guid.Parse(tournamentId);
            Guid matchGuid = Guid.Parse(matchId);

            var scoreList = db.Table<Scores>().Where(m => m.TournamentId == tournamentGuid && m.MatchId == matchGuid);
            return scoreList.ToList().ConvertAll(s => s.ConvertToDto());
        }

        /*
        IEnumerable<TournamentDto> GetAll();
        TournamentDto GetByDate(DateTime tournamentDate);
        string Add(TournamentDto tournament);
        IEnumerable<PlayerDto> GetPlayersByName(string playerName);
        PlayerDto GetPlayerById(string playerId);
        string AddEditTeam(string teamName);
        void AssignTeam(string tournamentId, TeamDto team);
        string AddEditPlayer(string playerName);
        void AssignPlayer(PlayerDto player);
        string AddMatch(string tournamentId, Match match);
        void AddMatchScore(string tournamentId, string matchId, string teamId, string playerId);
        IEnumerable<TeamDto> GetTeamsByTournament(string tournamentId);
        int GetTotalTeamsByTournament(string tournamentId);
        IEnumerable<PlayerDto> GetPlayersByTeam(string tournamentId, string teamId);
        int GetTotalPlayerByTeam(string tournamentId, string teamId);
        IEnumerable<Match> GetMatches(string tournamentId);
         */
    }
}
