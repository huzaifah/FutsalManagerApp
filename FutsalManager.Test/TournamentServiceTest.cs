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
        private string _tournamentId = "784224da-b26e-4304-bf06-07040b8645dd";

        [SetUp]
        public void Setup() 
        {
            string storagePath =
                Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path,
                "Bobai", "futsal.db");

            tournamentRepo = new TournamentRepository(storagePath);
            tournamentService = new TournamentService(tournamentRepo);
        }
        
        //[TearDown]
        //public void Tear() { }

        //[Test]
        public void CreateNewTournament_FirstTournament_TournamentCreated()
        {
            var tournament = new Tournament(DateTime.Now.AddDays(1), 4, 8);
            var tournamentId = tournamentService.AddEditTournament(tournament);

            tournament.Id = tournamentId;
            Assert.IsNotNull(tournamentId);
            _tournamentId = tournamentId;
        }

        //[Test]
        //public void CreateTeam_FourTeams_TeamsCreated()
        //{
        //    var tournament = tournamentService.RetrieveTournamentById(_tournamentId);

        //    var team = new Team
        //    {
        //        Name = "White"
        //    };

        //    team.Id = tournamentService.CreateTeam(team);
        //    tournamentService.AssignTeam(tournament, team);

        //    team = new Team
        //    {
        //        Name = "Black"
        //    };

        //    team.Id = tournamentService.CreateTeam(team);
        //    tournamentService.AssignTeam(tournament, team);

        //    team = new Team
        //    {
        //        Name = "Red"
        //    };

        //    team.Id = tournamentService.CreateTeam(team);
        //    tournamentService.AssignTeam(tournament, team);

        //    team = new Team
        //    {
        //        Name = "Blue"
        //    };

        //    team.Id = tournamentService.CreateTeam(team);
        //    tournamentService.AssignTeam(tournament, team);

        //    var teams = tournamentService.RetrieveTeams(tournament);
        //    Assert.AreEqual(teams.Count(), 4);
        //}

        [Test]
        public void CreateOneTeam_WhiteTeam_TeamCreated()
        {
            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);

            var whiteTeam = new Team
            {
                Id = Guid.NewGuid().ToString(),
                Name = "White"
            };

            tournamentService.CreateTeam(whiteTeam);

            var teams = tournamentService.RetrieveTeams(tournament);
            Assert.AreEqual(teams.Count(), 1);
        }

        [Test]
        public void CreateAssignTeam_WhiteTeams_PlayerAssign()
        {
            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);

            var teams = tournamentService.RetrieveTeams(tournament);
            var whiteTeam = teams.Single(t => t.Name == "White");

            var player = new Domain.Dtos.PlayerDto { Name = "Nathan", BirthDate = new DateTime(1960, 1, 1), Position = "Outfield" };
            player.Id = tournamentService.AddEditPlayer(player);
            tournamentService.AssignPlayer(tournament, whiteTeam, player.ConvertToEntity());

            player = new Domain.Dtos.PlayerDto { Name = "Bas", BirthDate = new DateTime(1960, 1, 1), Position = "Outfield" };
            player.Id = tournamentService.AddEditPlayer(player);
            tournamentService.AssignPlayer(tournament, whiteTeam, player.ConvertToEntity());

            player = new Domain.Dtos.PlayerDto { Name = "Geoff", BirthDate = new DateTime(1960, 1, 1), Position = "Outfield" };
            player.Id = tournamentService.AddEditPlayer(player);
            tournamentService.AssignPlayer(tournament, whiteTeam, player.ConvertToEntity());

            player = new Domain.Dtos.PlayerDto { Name = "Peter", BirthDate = new DateTime(1960, 1, 1), Position = "Outfield" };
            player.Id = tournamentService.AddEditPlayer(player);
            tournamentService.AssignPlayer(tournament, whiteTeam, player.ConvertToEntity());

            player = new Domain.Dtos.PlayerDto { Name = "Ernest", BirthDate = new DateTime(1960, 1, 1), Position = "Goalkeeper" };
            player.Id = tournamentService.AddEditPlayer(player);
            tournamentService.AssignPlayer(tournament, whiteTeam, player.ConvertToEntity());

            player = new Domain.Dtos.PlayerDto { Name = "Bernie", BirthDate = new DateTime(1960, 1, 1), Position = "Outfield" };
            player.Id = tournamentService.AddEditPlayer(player);
            tournamentService.AssignPlayer(tournament, whiteTeam, player.ConvertToEntity());

            var players = tournamentService.RetrievePlayers(tournament, whiteTeam);
            
            Assert.AreEqual(teams.Count(), 6);
        }

        [Test]
        public void UpdateAttendancePayment_SinglePlayer_Updated()
        {
            var tournament = tournamentService.RetrieveTournamentById(_tournamentId);

            var teams = tournamentService.RetrieveTeams(tournament);
            var team = teams.Single(t => t.Name == "White");

            var player = tournamentService.RetrievePlayers(tournament, team).Single(p => p.Name == "Nathan");

            player.Attendance = "On Time";
            player.Paid = true;

            tournamentService.UpdatePlayerByTournament(player.ConvertToDto());

            var updPlayer = tournamentService.GetPlayerById(player.Id);
            Assert.AreEqual("On Time", updPlayer.Attendance);
            Assert.IsTrue(updPlayer.Paid);
        }
        //[Test]
        //public void Pass()
        //{
        //    Console.WriteLine("test1");
        //    Assert.True(true);
        //}

        //[Test]
        //public void Fail()
        //{
        //    Assert.False(true);
        //}

        //[Test]
        //[Ignore("another time")]
        //public void Ignore()
        //{
        //    Assert.True(false);
        //}

        //[Test]
        //public void Inconclusive()
        //{
        //    Assert.Inconclusive("Inconclusive");
        //}
    }
}