using President.BLL.Dtos;
using President.BLL.Game;
using President.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace President.BLL.Services
{
    public interface IGameService
    {
        string GetUserName(ClaimsPrincipal claimsPrincipal);
        void AddGame(OnlineGame game);
        bool UserIsInAnyGame(string userName);
        OnlineGame ThrowCard(CardDto cardDto, string userName);
        OnlineGame Pass(string userName);
        OnlineGame Switch(string userName, List<CardDto> cardDtos);
        IEnumerable<PlayerStatistics> SaveStats(OnlineGame game, IEnumerable<string> winners, IEnumerable<string> userNames);
        List<string> UserLoggedOff(string userName);
    }
}
