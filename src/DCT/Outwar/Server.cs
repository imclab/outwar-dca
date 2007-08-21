using System;
using System.Collections.Generic;

namespace DCT.Outwar
{
    public enum ServerName
    {
        sigil,
        torax,
        fabar,
        zimbob
    }

    public class Server
    {
        public const int NUM_SERVERS = 4;

        public static List<string> NamesList
        {
            get
            {
                List<string> ret = new List<string>();
                ret.Add("Sigil");
                ret.Add("Torax");
                ret.Add("Fabar");
                ret.Add("Zimbob");
                return ret;
            }
        }

        public static int NameToId(ServerName n)
        {
            switch (n)
            {
                case ServerName.sigil:
                    return 1;
                case ServerName.torax:
                    return 2;
                case ServerName.fabar:
                    return 3;
                case ServerName.zimbob:
                    return 4;
            }
            throw new ArgumentException("No server id correlates with " + n);
        }

        public static ServerName IdToName(int id)
        {
            switch (id)
            {
                case 1:
                    return ServerName.sigil;
                case 2:
                    return ServerName.torax;
                case 3:
                    return ServerName.fabar;
                case 4:
                    return ServerName.zimbob;
            }
            throw new ArgumentException("No server name correlates with " + id);
        }
    }
}