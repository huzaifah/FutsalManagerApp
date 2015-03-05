using FutsalManager.Domain.Dtos;
using FutsalManager.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Persistence.Helpers
{
    public static class DbMapper
    {
        public static TournamentDto ConvertToDto(this Tournaments tournament)
        {
            return new TournamentDto
            {
                Id = tournament.Id.ToString(),
                Date = tournament.Date,
                TotalTeam = tournament.TotalTeam,
                MaxPlayerPerTeam = tournament.MaxPlayerPerTeam
            };
        }

        public static Tournaments ConvertToDb(this TournamentDto tournament)
        {
            return new Tournaments
            {
                Id = Guid.Parse(tournament.Id),
                Date = tournament.Date,
                TotalTeam = tournament.TotalTeam,
                MaxPlayerPerTeam = tournament.MaxPlayerPerTeam
            };
        }

        public static PlayerDto ConvertToDto(this Players player)
        {
            if (player == null)
                return default(PlayerDto);

            return new PlayerDto
            {
                Id = player.Id.ToString(),
                Name = player.Name
                //TeamId = player.TeamId,
                //TournamentId = player.TournamentId
            };
        }

        public static Players ConvertToDb(this PlayerDto player)
        {
            return new Players
            {
                Id = Guid.Parse(player.Id),
                Name = player.Name
                //TeamId = player.TeamId,
                //TournamentId = player.TournamentId
            };
        }

        public static TeamDto ConvertToDto(this Teams team)
        {
            if (team == null)
                return default(TeamDto);

            return new TeamDto
            {
                Id = team.Id.ToString(),
                Name = team.Name
            };
        }

        public static Teams ConvertToDb(this TeamDto team)
        {
            Guid teamGuid;

            if (!Guid.TryParse(team.Id, out teamGuid))
                throw new ArgumentException("Team id is invalid");

            return new Teams
            {
                Id = teamGuid,
                Name = team.Name                
            };
        }

        public static MatchDto ConvertToDto(this Matchs match)
        {
            return new MatchDto
            {
                Id = match.Id.ToString(),
                IsCompleted = match.IsCompleted,
                TournamentId = match.TournamentId.ToString(),
                HomeTeam = new TeamDto { Id = match.HomeTeam.ToString() },
                AwayTeam = new TeamDto { Id = match.AwayTeam.ToString() },
            };
        }

        public static Matchs ConvertToDb(this MatchDto match)
        {
            Guid matchGuid = Guid.Empty, tournamentGuid = Guid.Empty, homeTeamGuid, awayTeamGuid;

            if (!String.IsNullOrEmpty(match.Id))
                if (!Guid.TryParse(match.Id, out matchGuid))
                    throw new ArgumentException("Match id is invalid");

            if (!String.IsNullOrEmpty(match.TournamentId))
                if (!Guid.TryParse(match.TournamentId, out tournamentGuid))
                    throw new ArgumentException("Tournament id is invalid");     
                
            if (!Guid.TryParse(match.HomeTeam.Id, out homeTeamGuid) || !Guid.TryParse(match.AwayTeam.Id, out awayTeamGuid))
                throw new ArgumentException("Team id is invalid");
            
            return new Matchs
            {
                Id = matchGuid,
                IsCompleted = match.IsCompleted,
                TournamentId = tournamentGuid,
                HomeTeam = homeTeamGuid,
                AwayTeam = awayTeamGuid,
            };
        }
    }
}
