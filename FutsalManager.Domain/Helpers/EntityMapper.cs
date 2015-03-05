using FutsalManager.Domain.Dtos;
using FutsalManager.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Helpers
{
    public static class EntityMapper
    {
        public static TournamentDto ConvertToDto(this Tournament tournament)
        {
            return new TournamentDto
            {
                Id = tournament.Id,
                Date = tournament.Date,
                TotalTeam = tournament.TotalTeam,
                MaxPlayerPerTeam = tournament.MaxPlayerPerTeam
            };
        }

        public static Tournament ConvertToEntity(this TournamentDto tournament)
        {
            return new Tournament
            {
                Id = tournament.Id,
                Date = tournament.Date,
                TotalTeam = tournament.TotalTeam,
                MaxPlayerPerTeam = tournament.MaxPlayerPerTeam
            };
        }

        public static PlayerDto ConvertToDto(this Player player)
        {
            return new PlayerDto
            {
                Id = player.Id,
                Name = player.Name,
                TeamId = player.TeamId,
                TournamentId = player.TournamentId
            };
        }

        public static Player ConvertToEntity(this PlayerDto player)
        {
            return new Player
            {
                Id = player.Id,
                Name = player.Name,
                TeamId = player.TeamId,
                TournamentId = player.TournamentId
            };
        }

        public static TeamDto ConvertToDto(this Team team)
        {
            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                TournamentId = team.TournamentId
            };
        }

        public static Team ConvertToEntity(this TeamDto team)
        {
            return new Team
            {
                Id = team.Id,
                Name = team.Name,
                TournamentId = team.TournamentId
            };
        }

        public static MatchDto ConvertToDto(this Match match)
        {
            return new MatchDto
            {
                Id = match.Id,
                HomeTeam = match.HomeTeam.ConvertToDto(),
                AwayTeam = match.AwayTeam.ConvertToDto(),
                IsCompleted = match.IsCompleted,
                TournamentId = match.TournamentId
            };
        }

        public static Match ConvertToEntity(this MatchDto match)
        {
            return new Match
            {
                Id = match.Id,
                HomeTeam = match.HomeTeam.ConvertToEntity(),
                AwayTeam = match.AwayTeam.ConvertToEntity(),
                IsCompleted = match.IsCompleted,
                TournamentId = match.TournamentId
            };
        }
    }
}
