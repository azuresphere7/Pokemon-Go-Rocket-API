﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using PokemonGo.RocketAPI.Extensions;
using PokemonGo.RocketAPI.Helpers;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;

namespace PokemonGo.RocketAPI.Rpc
{
    public class Player : BaseRpc
    {
        public Player(Client client) : base(client)
        {
            _client = client;
        }

        public async Task<PlayerUpdateResponse> UpdatePlayerLocation(double latitude, double longitude, double altitude)
        {
            SetCoordinates(latitude, longitude, altitude);
            var message = new PlayerUpdateMessage
            {
                Latitude = _client.CurrentLatitude,
                Longitude = _client.CurrentLongitude
            };

            var updatePlayerLocationRequestEnvelope = RequestBuilder.GetRequestEnvelope(
                new Request
                {
                    RequestType = RequestType.PlayerUpdate,
                    RequestMessage = message.ToByteString()
                });

            return await _client.PokemonHttpClient.PostProtoPayload<Request, PlayerUpdateResponse>(ApiUrl, updatePlayerLocationRequestEnvelope);
        }

        internal void SetCoordinates(double lat, double lng, double altitude)
        {
            _client.CurrentLatitude = lat;
            _client.CurrentLongitude = lng;
            _client.CurrentAltitude = altitude;
        }

        public async Task<GetPlayerResponse> GetOwnProfile()
        {
            var request = RequestBuilder.GetRequestEnvelope(RequestType.GetPlayer, new GetPlayerMessage());
            return await _client.PokemonHttpClient.PostProtoPayload<Request, GetPlayerResponse>(ApiUrl, request);
        }
    }
}
