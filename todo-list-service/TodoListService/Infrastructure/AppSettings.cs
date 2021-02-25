using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListService.Infrastructure
{
    //Application Settings class to load appsettings.json file.
    public class AppSettings
    {
        public string AllowedHost { get; set; }
        public ConnectionString ConnectionString { get; set; }
        public Jwt Jwt { get; set; }
        public string DefaultUser { get; set; }
    }

    public class ConnectionString
    {
        public string TodoListEntity { get; set; }
    }

    public class Jwt
    {
        public string key { get; set; }
    }

    
}
