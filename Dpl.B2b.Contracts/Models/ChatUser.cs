using System;
using System.Collections.Generic;
using System.Text;

namespace Dpl.B2b.Contracts.Models
{
    public class ChatUser
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Language { get; set; }

        public string Token { get; set; }
    }

    public class ChatUsersSearchRequest
    {
        public string[] Emails { get; set; }
    }
}
