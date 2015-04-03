using System;
using NUnit.Framework;
using FutsalManager.Domain.Interfaces;
using FutsalManager.Domain;
using System.IO;
using FutsalManager.Persistence;
using FutsalManager.Domain.Entity;
using FutsalManager.Domain.Helpers;
using System.Linq;

namespace FutsalManager.Test
{
    [TestFixture]
    public class TournamentServiceTest
    {
        private ITournamentRepository tournamentRepo;
        private TournamentService tournamentService;
        private string _tournamentId = "ed61ff06-c11f-41af-b02c-5dfb0ed4c1c7";

        [SetUp]
        public void Setup() 
        {
            string folderPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path,
                "Bobai");

            //string storagePath =
            //    Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path,
            //    "Bobai", "futsal.db");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string storagePath = Path.Combine(folderPath, "futsal.db");

            tournamentRepo = new TournamentRepository(storagePath);
            tournamentService = new TournamentService(tournamentRepo);
        }
        
        [Test]
        [Ignore("another time")]
        public void CreateNewTournament_FirstTournament_TournamentCreated()
        {
            var tournament = new Tournament(DateTime.Now.AddDays(1), 4, 8);
            var tournamentId = tournamentService.AddEditTournament(tournament);

            tournament.Id = tournamentId;
            Console.WriteLine("=====> " + tournamentId);
            Assert.IsNotNull(tournamentId);        
        }
                        
        [Test]
        [Ignore("another time")]
        public void RetrieveTournament_AllTournaments()
        {
            var tournaments = tournamentService.RetrieveTournament();
            Assert.IsTrue(tournaments.Count() > 0);
        }

        [Test]
        [Ignore("another time")]
        public void CreateTeam_FourTeams_TeamsCreated()
        {
            tournamentRepo.DeleteAllTeams();

            var wteam = new Team
            {
                Name = "White"
            };

            wteam.Id = tournamentService.CreateTeam(wteam);

            var bteam = new Team
            {
                Name = "Black"
            };

            bteam.Id = tournamentService.CreateTeam(bteam);

            var rteam = new Team
            {
                Name = "Red"
            };

            rteam.Id = tournamentService.CreateTeam(rteam);

            var blteam = new Team
            {
                Name = "Blue"
            };

            blteam.Id = tournamentService.CreateTeam(blteam);

            var teams = tournamentRepo.GetAllTeams();
            Assert.AreEqual(4, teams.Count());
        }

        [Test]     
        [Ignore("another time")]
        public void AssignTeam_FourTeams_TeamAssign()
        {
            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);

            tournamentRepo.DeleteAllPlayerAssignments();

            var teams = tournamentService.RetrieveTeams(tournament);
            var blueTeam = teams.Single(t => t.Name.Equals("Blue", StringComparison.OrdinalIgnoreCase));

            var playerNathan = new Domain.Dtos.PlayerDto { Name = "Nathan", BirthDate = new DateTime(1960, 1, 1), Position = "Outfield" };
            playerNathan.Id = tournamentService.AddEditPlayer(playerNathan);
            tournamentService.AssignPlayer(tournament, blueTeam, playerNathan.ConvertToEntity());

            var playerBas = new Domain.Dtos.PlayerDto { Name = "Bas", BirthDate = new DateTime(1960, 1, 1), Position = "Outfield" };
            playerBas.Id = tournamentService.AddEditPlayer(playerBas);
            tournamentService.AssignPlayer(tournament, blueTeam, playerBas.ConvertToEntity());

            var playerGe = new Domain.Dtos.PlayerDto { Name = "Geoff", BirthDate = new DateTime(1960, 1, 1), Position = "Outfield" };
            playerGe.Id = tournamentService.AddEditPlayer(playerGe);
            tournamentService.AssignPlayer(tournament, blueTeam, playerGe.ConvertToEntity());

            var playerPet = new Domain.Dtos.PlayerDto { Name = "Peter", BirthDate = new DateTime(1960, 1, 1), Position = "Outfield" };
            playerPet.Id = tournamentService.AddEditPlayer(playerPet);
            tournamentService.AssignPlayer(tournament, blueTeam, playerPet.ConvertToEntity());

            var playerErn = new Domain.Dtos.PlayerDto { Name = "Ernest", BirthDate = new DateTime(1960, 1, 1), Position = "Goalkeeper" };
            playerErn.Id = tournamentService.AddEditPlayer(playerErn);
            tournamentService.AssignPlayer(tournament, blueTeam, playerErn.ConvertToEntity());

            var playerBern = new Domain.Dtos.PlayerDto { Name = "Bernie", BirthDate = new DateTime(1960, 1, 1), Position = "Outfield" };
            playerBern.Id = tournamentService.AddEditPlayer(playerBern);
            tournamentService.AssignPlayer(tournament, blueTeam, playerBern.ConvertToEntity());

            var players = tournamentService.RetrievePlayers(tournament, blueTeam);

            Assert.AreEqual(6, players.Count());
        }

        [Test]
        [Ignore("another time")]
        public void AssignTeamToTournament_FourTeam_UpdateAssignment()
        {
            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);
            
            tournamentRepo.DeleteAllTeamsAssignment();

            var blueTeam = new Team("Blue");
            tournamentService.AssignTeam(tournament, blueTeam);

            var whiteTeam = new Team("White");
            tournamentService.AssignTeam(tournament, whiteTeam);

            var blackTeam = new Team("Black");
            tournamentService.AssignTeam(tournament, blackTeam);

            var redTeam = new Team("Red");
            tournamentService.AssignTeam(tournament, redTeam);

            var teams = tournamentService.RetrieveTeams(tournament);

            Assert.AreEqual(4, teams.Count());
        }

        [Test]
        [Ignore("another time")]
        public void UpdateAttendancePayment_SinglePlayer_Updated()
        {
            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);

            var teams = tournamentService.RetrieveTeams(tournament);
            var blueTeam = teams.Single(t => t.Name.Equals("Blue", StringComparison.OrdinalIgnoreCase));

            var player = tournamentService.RetrievePlayers(tournament, blueTeam).Single(p => p.Name == "Nathan");

            player.Attendance = "On Time";
            player.Paid = true;

            tournamentService.UpdatePlayerByTournament(player.ConvertToDto());

            var updPlayer = tournamentService.GetPlayerByTournament(player.Id, tournament.Id);
            Assert.AreEqual("On Time", updPlayer.Attendance);
            Assert.IsTrue(updPlayer.Paid);
        }

        [Test]
        [Ignore("another time")]
        public void GenerateMatches_OneTournament_ReturnTotalMatches()
        {
            tournamentRepo.DeleteAllMatchesByTournament(_tournamentId);

            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);
            tournamentService.GenerateMatches(tournament);

            var matches = tournamentService.RetrieveMatches(tournament);
            Assert.AreEqual(12, matches.Count());
        }

        [Test]
        [Ignore]
        public void AddMatchScore_BlueVsBlack_ScoreUpdated()
        {
            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);
            var matches = tournamentService.RetrieveMatches(tournament);
            var teams = tournamentService.RetrieveTeams(tournament);
            
            var match = matches.Single(m => m.HomeTeam.Name == "Blue" && m.AwayTeam.Name == "Black");
            var team = teams.Single(t => t.Name == "Blue");
            var players = tournamentService.RetrievePlayers(tournament, team);
            var player = players.Single(p => p.Name == "Nathan");

            tournamentService.AddScore(tournament, match, Domain.Enum.TeamSide.Home, player.Id, "");

            var scores = tournamentService.GetTotalScoresByTeam(tournament, match, team);

            Assert.AreEqual(1, scores);
        }

        [Test]
        [Ignore("another time")]
        public void CompleteMatch_SingleMatch_MatchUpdated()
        {
            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);
            var matches = tournamentService.RetrieveMatches(tournament);

            var match = matches.Single(m => m.HomeTeam.Name == "Blue" && m.AwayTeam.Name == "Black");

            tournamentService.CompleteMatch(match);

            var updMatch = tournamentService.RetrieveMatches(tournament);
            Assert.IsTrue(updMatch.Single(m => m.HomeTeam.Name == "Blue" && m.AwayTeam.Name == "Black").IsCompleted == true);
        }

        [Test]
        [Ignore("another time")]
        public void GenerateStanding_SingleTournament()
        {
            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);
            var standings = tournamentService.GenerateStanding(tournament);

            Assert.AreEqual(4, standings.Count());
        }

        [Test]
        [Ignore("another time")]
        public void GenerateDeciderMatch_SingleTournament_RetrieveDeciderMatch()
        {
            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);
            var standings = tournamentService.GenerateStanding(tournament).ToList();

            tournamentService.GenerateDeciderMatch(tournament, standings);

            var deciderMatches = tournamentService.RetrieveDeciderMatch(tournament);
            var final = deciderMatches.Single(m => m.Type == "Final");
            var thirdPlacing = deciderMatches.Single(m => m.Type == "3rdPlacing");

            Assert.IsTrue(final.HomeTeam.Name == standings[0].Team.Name || final.AwayTeam.Name == standings[0].Team.Name);
            Assert.IsTrue(final.HomeTeam.Name == standings[1].Team.Name || final.AwayTeam.Name == standings[1].Team.Name);

            Assert.IsTrue(thirdPlacing.HomeTeam.Name == standings[2].Team.Name || thirdPlacing.AwayTeam.Name == standings[2].Team.Name);
            Assert.IsTrue(thirdPlacing.HomeTeam.Name == standings[3].Team.Name || thirdPlacing.AwayTeam.Name == standings[3].Team.Name);
        }

        [Test]
        public void EndTournament_SingleTournament_RetrieveTournament()
        {
            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);
            tournamentService.EndTournament(tournament);

            tournament = tournamentService.RetrieveTournamentById(_tournamentId);
            Assert.IsTrue(tournament.Completed);
        }
    }
}