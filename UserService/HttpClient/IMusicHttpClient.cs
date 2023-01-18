using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UserService.HttpClient
{
    public interface IMusicHttpClient
    {
        public Task<HttpStatusCode> AddFavoritePlaylist(int userId);
    }

}
