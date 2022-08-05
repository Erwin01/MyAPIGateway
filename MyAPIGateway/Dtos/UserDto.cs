using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAPIGateway.Dtos
{
    public class UserDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public List<PostDto> Posts { get; set; } = new List<PostDto>();

    }
}
