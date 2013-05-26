using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace MySocialNetwork
{
    public static class WebSecurityConfig
    {
        public static void RegisterWebSecurity()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfiles", "Id", "UserName", autoCreateTables: true);
        }
    }
}