using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserService.DTO;
using UserService.Exceptions;
using UserService.Models.EntityFramework;

namespace UserService.HttpClient
{
    public class MusicHttpClient : IMusicHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string BASE_PATH = "";
        private readonly string ADD_FAVORITE_PLAYLIST = "private/";

        public MusicHttpClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            BASE_PATH = configuration["Urls:MusicPlaylistService"];
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpStatusCode> AddFavoritePlaylist(int userId)
        {
            //UserDTO userDTO = _mapper.Map<UserDTO>(user);

            var httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.PostAsync(BASE_PATH + ADD_FAVORITE_PLAYLIST + userId.ToString(), null);
            //response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                return response.StatusCode;
            }

            throw new FailedHttpRequestException("HttpClient did not receive Id for User");
        }
    }
}
