﻿using FutsalManager.Domain.Dtos;
using FutsalManager.Domain.Entity;
using FutsalManager.Domain.Enums;
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
                MaxPlayerPerTeam = tournament.MaxPlayerPerTeam,
                Status = tournament.Status.ToString()
            };
        }

        public static Tournament ConvertToEntity(this TournamentDto tournament)
        {
            TournamentStatus status = TournamentStatus.NotStarted;

            if (!String.IsNullOrEmpty(tournament.Status))
                status = (TournamentStatus)Enum.Parse(typeof(TournamentStatus), tournament.Status);

            return new Tournament
            {
                Id = tournament.Id,
                Date = tournament.Date,
                TotalTeam = tournament.TotalTeam,
                MaxPlayerPerTeam = tournament.MaxPlayerPerTeam,
                Status = status
            };
        }

        public static PlayerDto ConvertToDto(this Player player)
        {
            return new PlayerDto
            {
                Id = player.Id,
                Name = player.Name,
                BirthDate = player.BirthDate,
                Position = player.Position,
                TeamId = player.TeamId,
                TournamentId = player.TournamentId,
                Attendance = player.Attendance,
                Paid = player.Paid,
                TotalGoals = player.TotalGoals                
            };
        }

        public static Player ConvertToEntity(this PlayerDto player)
        {
            return new Player
            {
                Id = player.Id,
                Name = player.Name,
                BirthDate = player.BirthDate,
                Position = player.Position,
                TeamId = player.TeamId,
                TournamentId = player.TournamentId,
                Attendance = player.Attendance,
                Paid = player.Paid,
                TotalGoals = player.TotalGoals
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
                TournamentId = match.TournamentId,
                Type = match.Type
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
                TournamentId = match.TournamentId,
                Type = match.Type
            };
        }
    }
}
