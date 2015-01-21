using System.Configuration;

namespace Iridium.Server
{
    public class IridiumConfig : ConfigurationSection
    {
        private static class Key
        {
            public const string ServerProperties = "serverProperties";
            public const string MySqlPushNotifications = "mysqlIridium";
        }


        [ConfigurationProperty(Key.ServerProperties, IsRequired = true)]
        public ServerPropertiesConfigs ServerProperties
        {
            get { return (ServerPropertiesConfigs)this[Key.ServerProperties]; }
            set { this[Key.ServerProperties] = value; }
        }


        /// <summary>
        /// Gets or sets mysql connection settings iridium database
        /// </summary>
        [ConfigurationProperty(Key.MySqlPushNotifications, IsRequired = true)]
        public Database MysqlIridium
        {
            get { return (Database)this[Key.MySqlPushNotifications]; }
            set { this[Key.MySqlPushNotifications] = value; }
        }

        public class ServerPropertiesConfigs : ConfigurationElement
        {
            private static class Key
            {
                public const string Host = "host";
                public const string Port = "port";
            }
            
            [ConfigurationProperty(Key.Host, IsRequired = true)]
            public string Host
            {
                get { return (string)this[Key.Host]; }
                set { this[Key.Host] = value; }
            }

            [ConfigurationProperty(Key.Port, IsRequired = false, DefaultValue = 3306)]
            [IntegerValidator(ExcludeRange = false, MinValue = 1, MaxValue = ushort.MaxValue)]
            public int Port
            {
                get { return (int)this[Key.Port]; }
                set { this[Key.Port] = value; }
            }
        }

        /// <summary>
        /// Represents Database configuration
        /// </summary>
        public class Database : ConfigurationElement
        {
            private static class Key
            {
                public const string Host = "host";
                public const string Port = "port";
                public const string User = "user";
                public const string Password = "password";
                public const string Schema = "schema";
                public const string DefaultHoursToLive = "default_hours_to_live";
            }

            [ConfigurationProperty(Key.Host, IsRequired = true)]
            public string Host
            {
                get { return (string)this[Key.Host]; }
                set { this[Key.Host] = value; }
            }

            [ConfigurationProperty(Key.Port, IsRequired = false, DefaultValue = 3306)]
            [IntegerValidator(ExcludeRange = false, MinValue = 1, MaxValue = ushort.MaxValue)]
            public int Port
            {
                get { return (int)this[Key.Port]; }
                set { this[Key.Port] = value; }
            }

            [ConfigurationProperty(Key.User, IsRequired = true)]
            public string User
            {
                get { return (string)this[Key.User]; }
                set { this[Key.User] = value; }
            }

            [ConfigurationProperty(Key.Password, IsRequired = true)]
            public string Password
            {
                get { return (string)this[Key.Password]; }
                set { this[Key.Password] = value; }
            }

            [ConfigurationProperty(Key.Schema, IsRequired = true)]
            public string Schema
            {
                get { return (string)this[Key.Schema]; }
                set { this[Key.Schema] = value; }
            }

            [ConfigurationProperty(Key.DefaultHoursToLive, IsRequired = true)]
            public int DefaultHoursToLive
            {
                get { return (int)this[Key.DefaultHoursToLive]; }
                set { this[Key.DefaultHoursToLive] = value; }
            }

        }
    }
}
