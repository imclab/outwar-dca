using System.Collections.Generic;
using DCT.Parsing;
using DCT.Protocols.Http;

namespace DCT.Outwar
{
    internal class AccountsEngine
    {
        private List<Account> mAccounts;
        internal List<Account> Accounts
        {
            get { return mAccounts; }
        }

        private int mMainIndex;

        internal Account MainAccount
        {
            get
            {
                if (mAccounts.Count < 1)
                {
                    return null;
                }
                else
                {
                    return mAccounts[mMainIndex];
                }
            }
        }

        internal AccountsEngine()
        {
            mAccounts = new List<Account>();
            mMainIndex = 0;
        }

        internal Account this[int i]
        {
            get { return mAccounts[i]; }
        }

        internal int Count
        {
            get { return mAccounts.Count; }
        }

        internal void SetMain(Account a)
        {
            mMainIndex = mAccounts.IndexOf(a);
        }

        internal void SetMain(int index)
        {
            mMainIndex = index;
        }

        internal Account Last
        {
            get { return mAccounts[mAccounts.Count - 1]; }
        }

        internal Account Add(string name, int id, ServerName server)
        {
            OutwarHttpSocket socket = new OutwarHttpSocket();
            Account a = new Account(socket, name, id, server);
            mAccounts.Add(a);
            return a;
        }

        internal void Remove(Account a)
        {
            mAccounts.Remove(a);
        }

        internal void Remove(int index)
        {
            mAccounts.RemoveAt(index);
        }

        internal void Login(string user, string pass, bool remember)
        {
            HttpSocket.DefaultInstance.Get("http://outwar.com/myaccount.php?login_username=" + user
                                           + "&login_password=" + pass
                                           + (remember ? "&remember=1" : string.Empty));

            for (int i = 1; i <= Server.NUM_SERVERS; i++)
            {
                string svrlist = HttpSocket.DefaultInstance.Get("http://outwar.com/accounts.php?ac_serverid=" + i);
                AddAccountsFromSource(svrlist);
            }
        }

        private void AddAccountsFromSource(string src)
        {
            // No accounts
            if (!src.Contains("PLAY!"))
            {
                return;
            }

            int i = src.IndexOf("Trustee Access");
            if (i > -1)
            {
                src = src.Substring(0, i);
            }
            Parser m1 = new Parser(src);
            int serverid;
            if (!int.TryParse(m1.Parse("serverid=", "\""), out serverid))
            {
                return;
            }
            foreach (
                string t in
                    m1.MultiParse("<font color=\"#FFFF00\" face=\"Verdana, Arial, Helvetica, sans-serif\" size=\"1\">",
                                  "PLAY!"))
            {
                Parser p = new Parser(t);
                string name = p.Parse("<b>", "</b>");
                int id;
                if (!int.TryParse(p.Parse("suid=", "&"), out id))
                {
                    continue;
                }

                Add(name, id, Server.IdToName(serverid));
                Last.Socket.Cookie = HttpSocket.DefaultInstance.Cookie + " ow_userid=" + id
                                     + "; ow_serverid=" + serverid;
            }
        }
    }
}