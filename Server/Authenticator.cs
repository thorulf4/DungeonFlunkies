using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Shared;

namespace Server
{
    public class Authenticator
    {
        private Dictionary<string, UserConnection> sessions = new Dictionary<string, UserConnection>();

        public string CreateSession(string name, int id)
        {
            sessions.Remove(name);

            var expiryTime = DateTime.Now.AddHours(5);
            var token = CreateSessionHash(name, expiryTime);

            sessions.Add(name, new UserConnection
            {
                Id = id,
                Character = name,
                Expiry = expiryTime,
                Token = token
            });

            return token;
        }

        public int VerifySession(string name, string token)
        {
            UserConnection user = sessions[name];

            if(user.Token == token && DateTime.Compare(user.Expiry, DateTime.Now) > 0)
            {
                return user.Id;
            }

            throw new Exception("Session not valid");
        }

        private string CreateSessionHash(string name, DateTime expiryTime)
        {
            byte[] data = Encoding.UTF8.GetBytes(name + expiryTime.ToString());

            using (SHA256 sha256 = SHA256.Create())
            {
                return Encoding.ASCII.GetString(sha256.ComputeHash(data));
            }
        }

        private class UserConnection
        {
            public int Id { get; set; }
            public string Character { get; set; }
            public DateTime Expiry { get; set; }
            public string Token { get; set; }
        }
    }

    
}
