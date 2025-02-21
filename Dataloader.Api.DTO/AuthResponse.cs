using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataloader.Api.DTO
{
   public class AuthResponse
    {
        public string AccesToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
