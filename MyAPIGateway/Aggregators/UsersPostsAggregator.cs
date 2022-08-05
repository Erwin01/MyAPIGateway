using Microsoft.AspNetCore.Http;
using MyAPIGateway.Dtos;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyAPIGateway.Aggregators
{
    public class UsersPostsAggregator : IDefinedAggregator
    {

        /// <summary>
        /// Method Receive Request Responses / Creation Aggregate
        /// </summary>
        /// <param name="responses"></param>
        /// <returns> response </returns>
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            // Indexes by user and posts
            var userResponseContent = await responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();
            var postResponseContent = await responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();

            // Call list of UserDto and PostDto
            var users = JsonConvert.DeserializeObject<List<UserDto>>(userResponseContent);
            var posts = JsonConvert.DeserializeObject<List<PostDto>>(postResponseContent);

            // Browse user list
            foreach (var user in users)
            {
                // For each user find PostDto that belongs to that UserDto
                var userPosts = posts.Where(p => p.UserId == user.Id).ToList();
                // Add user list
                user.Posts.AddRange(userPosts);

            }

            // Is re-serialized
            var postByUserString = JsonConvert.SerializeObject(users);

            // Build Media-Type
            var stringContent = new StringContent(postByUserString)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            // Return your response
            return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");


        }
    }
}
