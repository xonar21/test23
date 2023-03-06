 Bizon360.CRM   
===========

The information system is intended to automate the beneficiary's sales processes by means of storage, management and reporting modules for the beneficiary's customers.

Managing the activity of sales managers, monitoring their efficiency through KPI and a notification function for all actors to increase their productivity and efficiency. As well as providing the necessary information for the exercise of verification and control duties by responsible persons.

# Environment settings

  # Development

  Jenkins CI/CD pipeline: http://192.168.1.51:8080/job/dotnet-dev-bizon360crm/

  SonarQube: http://192.168.1.50/dashboard?id=dotnet-dev-bizon360crm

  URL: https://bizon360crm.dev.indrivo.com

# Tehnologies
.NET CORE 2.2, C#, MVC, JQuery, PostgreSql database, PgAdmin, javascript native, javascript Prototype, SweetAlert2, Libman, Razor View Libraries, Modular Arhitecture, Entity Framework, Bootstrap 4, .Net Standard, Nginx, IIS, Kestrel

# Compatibility
The framework is cross platform, thanks .net core, is compatible with Windows, Linux distributive and Mac OS.

### Windows OS Requirements
To start the app you need the following packages installed:
- [.Net Core SDK 2.2.402] - C# cross platform framework
- [Redis] - Distributed cache soft
- [Pg Admin 4] - Postgres manager for interact with database
- [PostgreSql] - Database provider
- [Libman] - UI packages provider

### Ubuntu 18.04 or another OS Requirements
To start the app you need the following packages installed:
- [.Net Core SDK 2.2.402](https://docs.microsoft.com/en-us/dotnet/core/install/linux-package-manager-ubuntu-1804) - C# cross platform framework, [optional link](https://www.techrepublic.com/article/how-to-install-dotnet-core-on-ubuntu-18-04/)
- [Redis](https://www.digitalocean.com/community/tutorials/how-to-install-and-secure-redis-on-ubuntu-18-04) - Distributed cache soft
- [Pg Admin 4](https://www.howtoforge.com/tutorial/how-to-install-postgresql-and-pgadmin4-on-ubuntu-1804-lts/) - Postgres manager for interact with database
- [PostgreSql](https://www.digitalocean.com/community/tutorials/how-to-install-and-use-postgresql-on-ubuntu-18-04) - Database provider
- [Libman] - UI packages provider

`Note`: For other OS supported by .net core, consult internet sources

# Installation

The app can be started using 3 environments:
- `Development` - is used for development purpose
- `Stage` - is used on pre- production
- `Production` - is used for clients, on server side

`NOTE`: For development use Development env
`NOTE`: Each configuration corresponds to a configuration file, like this: `appsettings.{Env}.json`

Structure of appssetings file: 
```json
{
  "SystemConfig": {
    "MachineIdentifier": ".GR.Prod"
  },
  "ConnectionStrings": {
    "Provider": "Npgsql.EntityFrameworkCore.PostgreSQL",
    "ConnectionString": "Host=127.0.0.1;Port=5432;Username=postgres;Password=bizon360crm;Persist Security Info=true;Database=Bizon360Crm;MaxPoolSize=1000;"
  },
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Error"
    }
  },
  "HealthCheck": {
    "Timeout": 3,
    "Path": "/health"
  },
  "LocalizationConfig": {
    "Languages": [
      {
        "IsDisabled": false,
        "Identifier": "en",
        "Name": "English"
      },
      {
        "IsDisabled": false,
        "Identifier": "ro",
        "Name": "Romanian"
      },
      {
        "IsDisabled": true,
        "Identifier": "ru",
        "Name": "Russian"
      },
      {
        "IsDisabled": true,
        "Identifier": "it",
        "Name": "Italian"
      },
      {
        "IsDisabled": true,
        "Identifier": "fr",
        "Name": "French"
      },
      {
        "IsDisabled": true,
        "Identifier": "de",
        "Name": "German"
      },
      {
        "IsDisabled": true,
        "Identifier": "uk",
        "Name": "Ukrainian"
      },
      {
        "IsDisabled": true,
        "Identifier": "ja",
        "Name": "Japanese"
      },
      {
        "IsDisabled": true,
        "Identifier": "zh",
        "Name": "Chinese"
      },
      {
        "IsDisabled": true,
        "Identifier": "el",
        "Name": "Greek"
      },
      {
        "IsDisabled": true,
        "Identifier": "nl",
        "Name": "Dutch"
      },
      {
        "IsDisabled": true,
        "Identifier": "pl",
        "Name": "Polish"
      },
      {
        "IsDisabled": true,
        "Identifier": "es",
        "Name": "Spanish"
      }
    ],
    "Path": "Localization",
    "SessionStoreKeyName": "lang",
    "DefaultLanguage": "en"
  },
  "IsConfigured": true,
  "LdapSettings": {
    "ServerName": "",
    "ServerPort": 389,
    "UseSSL": false,
    "Credentials": {
      "DomainUserName": "",
      "Password": ""
    },
    "SearchBase": "",
    "ContainerName": "",
    "DomainName": "",
    "DomainDistinguishedName": ""
  },
  "WebClients": {
    "CORE": {
      "uri": "http://159.69.195.160:6969"
    },
    "BPMApi": {
      "uri": "http://159.69.195.160:6969"
    }
  },
  "RedisConnection": {
    "Host": "127.0.0.1",
    "Port": "6379"
  },
  "BackupSettings": {
    "Enabled": false,
    "UsePostGreSql": false,
    "UseMsSql": false,
    "BackupFolder": "ISODMS",
    "Interval": "24",
    "PostGreSqlBackupSettings": {
      "PgDumpPath": "C:\\Program Files\\PostgreSQL\\11\\bin\\pg_dump.exe",
      "Host": "localhost",
      "Port": "5432",
      "User": "postgres",
      "Password": "1111",
      "Database": "ISODMS.PROD",
      "FileExtension": "pgbackup"
    },
    "MsSqlBackupSettings": {
    }
  },
  "EmailSettings": {
    "Enabled": true,
    "Host": "smtp.office365.com",
    "Port": 587,
    "Timeout": 5000,
    "EnableSsl": true,
    "NetworkCredential": {
      "Email": "iso_dms.mail@indrivo.com",
      "Password": "I50_dm5.M@!1"
    }
  },
  "Sentry": {
    "Dsn": "https://a898fb5130514f2485704835f8109591@sentry.io/1547729",
    "IncludeRequestPayload": true,
    "SendDefaultPii": true,
    "MinimumBreadcrumbLevel": "Debug",
    "MinimumEventLevel": "Warning",
    "AttachStackTrace": true,
    "Debug": true,
    "DiagnosticsLevel": "Error"
  }
}
```

### Explanation of appsettings blocks: 
- `SystemConfig` - represent a general section that provide some global info:
    - MachineIdentifier - is used for identify the app id, if is installed multiple GEAR apps, after app installation it is ovverided by generated string
- `ConnectionStrings` - represent databases providers configuration
    - Provider - default is postgres
        - Npgsql.EntityFrameworkCore.PostgreSQL - postgres ,enabled and default
        - Microsoft.EntityFrameworkCore.SqlServer - enabled
        - Microsoft.EntityFrameworkCore.Sqlite - for the future
        - Microsoft.EntityFrameworkCore.InMemory - for the future
        - Microsoft.EntityFrameworkCore.Cosmos - for the future
        - Pomelo.EntityFrameworkCore.MySql - for the future
        - Pomelo.EntityFrameworkCore.MyCat - for the future
        - EntityFrameworkCore.SqlServerCompact40 - for the future
        - EntityFrameworkCore.SqlServerCompact35 - for the future
        - EntityFrameworkCore.Jet - for the future
        - MySql.Data.EntityFrameworkCore - for the future
        - FirebirdSql.EntityFrameworkCore.Firebird - for the future
        - EntityFrameworkCore.FirebirdSql - for the future
        - IBM.EntityFrameworkCore - for the future
        - EntityFrameworkCore.OpenEdge - for the future
- [Logging] - see microsoft docs
- `LocalizationConfig` - language configurations
- `IsConfigured` - This property determines whether the app has been installed or not, if set to true then the configurations set in the database are taken, otherwise when accessing any page, it will be redirected to the installer
- `LdapSettings` - This involves configuring the AD mode
- `RedisConnection` - configurations for distributed cache
    - Host - represent the ip address of redis connection
    - Port - represent the port where is bind redis service, default: 6379
- `BackupSettings` - this section is used for backup module, now is developed only for postgres provider
- `EmailSettings` - this section is used for email client
    - Enabled - set active or inactivity of service
    - Host - the smptp host
    - Port - the port of smtp
    - Timeout - represents the time allowed for the service to wait for the message to be successfully sent
    - EnableSsl - represent usage of smtp with ssl
    - NetworkCredential
        - Email - existent smtp email
        - Password - the password of smtp email
- `Sentry` - consult [sentry documentation](https://docs.sentry.io/platforms/dotnet/aspnetcore/) for .net core 

## App run
To start the app, you need:
1. Restore ui packages on all razor projects (is optional step because, they are restored on build)
    ```shell
    libman restore
    ```
2. Restore C# nuget packages by typing 
    ```shell
    dotnet restore
    ```
3. Build. To build, you must navigate the explorer to the path: `./src/GR.WebHosts/GR.Cms` or 
    ```shell
    cd ./src/GR.WebHosts/GR.Cms
    ```
    after it execute the following command: 
    ```shell
    dotnet build
    ```
4. If build has run successfully, it is the green wave to start the project
    ```shell
    dotnet run 
    ```
    `optional` for change exposed port 
    ```shell
    dotnet run --urls=http://localhost:5001/ 
    ```

## Install steps
`Note`: Be sure that in appsettings{Env}.json, the IsConfigured property is set to false
1. Start the application
You will be met by the following message describing the platform
![Welcome board](https://i.ibb.co/5GWdW6N/welcome-gear.png)
Click on `Go to installation`

2. Configure admin profile
![Profile tab](https://i.ibb.co/nQw9kHK/profile-gear.png)
Settings:
- `User Name` - adminstrator user name
- `Email` - your email address to receive emails on system events
- `Password` and `Confirm Password` - the administrator password
- `First Name` - admin first name
- `Last Name` - admin last name
- `Organization Name` - represent the default organization name
3. Set up database provider
![Configuration of database provider](https://i.ibb.co/hMnP7y6/db-gear.png)
`Note`: Use postgres default, because MsSql has not been tested for a long time, we plan support for other providers
`Connection String example`: Host=127.0.0.1;Port=5432;Username=postgres;Password=Gear2020;Persist Security Info=true;Database=Gear.PROD;MaxPoolSize=1000;
4. Press `Install` button and wait until the system is installed

# Modules
The framework has developed the following modules:
- Module for managing dynamic entities
- Workflow manager and builder module 
- Task Management Module
- Calendar module
- Synchronization component with external calendar
- Notifications module
- Document Management Module (DMS)
- User Management Module
- Role and permissions management module
- Report and Statistics Module
- The chat module
- Content management module
- Page Management module
- Forms management module
- Menu management module
- The localization module
- Authentication and authorization module

 [.net Core SDK 2.2.402]: <https://dotnet.microsoft.com/download/dotnet-core/2.2>
 [Redis]: <https://github.com/MicrosoftArchive/redis/releases/download/win-3.2.100/Redis-x64-3.2.100.msi>
 [Pg Admin 4]: <https://www.pgadmin.org/download/pgadmin-4-windows/>
 [PostgreSql]: <https://www.enterprisedb.com/downloads/postgres-postgresql-downloads>
 [Libman]: <https://docs.microsoft.com/en-us/aspnet/core/client-side/libman/libman-cli?view=aspnetcore-3.1>
 [Logging]: <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1>
