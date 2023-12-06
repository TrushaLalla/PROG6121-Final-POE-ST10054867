using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trial.DatabaseContent
{
    public class DBConString
    {
        public static string conString = "Data Source=trushaserver.database.windows.net;Initial Catalog=PROG6212-p2-ProjectDB-ST10054867;User ID=TrushaLalla;Password=trushasqlserver1!";
        public static string ReturnConString()
        {
            return conString;
        }
    }
}
