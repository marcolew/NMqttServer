using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMqttServer.Model
{
    public class Device
    {
        public ObjectId Id { get; set; }
        public DateTime LastLogIn { get; set; }
        public string ClientId { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public bool VerifyCredentials(string u, string p)
        {
            if(u==User && p == Password)
            {
                return true;
            }
            return false;
        }
    }
}
