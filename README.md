# Introduction 
Single Sign-on is a open source project for everyone. Every member can create new client, api resource, identity resource and login this with SSO. 

# Migration
- Add-Migration Initial -OutputDir Data/Migrations
- Add-Migration InitialPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
- Add-Migration InitialConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb

#Technology Stack
1. ASP.NET Core 3.1
2. Angular 9.x
3. Identity Server 4
5. SQL Server 2019

# How to run this Project
1. Clone this source code from Repository
2. Build solution to restore all Nuget Packages
2. Set startup project is SSO.Backend
4. Set startup project to multiple projects include: SSO.Backend

# References
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-3.1)
- [Visual Studio](https://visualstudio.microsoft.com/)
- [IdentityServer4](https://identityserver.io/)

- https://medium.com/@matthew.bajorek/configuring-serilog-in-asp-net-core-2-2-web-api-5e0f4d89749c
- https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-3.1

# Angular Installation
- NPM (https://nodejs.org/en/)
- https://git-scm.com/downloads
- npm install -g @angular/cli
- Visual Studio Code (https://code.visualstudio.com/)
- Run command: git clone https://github.com/start-angular/SB-Admin-BS4-Angular-8.git admin-app

# Deployment
1. Publish source code to package (BackendServer, WebPortal, Admin App)
2. Install server environment (SQL Server, .NET Core Runtime, ASP.NET Core Runtime)
3. Copy artifacts to Server
4. Install IIS (Internet information service)
5. Create IIS Web App (Configure Pool IIS No managed)
6. Config connection string appsettings.Production.json
7. Install Certificate for IIS (friendly name)
8. Setup security IIS_IUSR permission for web app, enable 32 bit in pool, enable stdout log in webconfig.
9. Instal rewrite URL Module for IIS (https://www.iis.net/downloads/microsoft/url-rewrite)
10. Create webconfig
```xml

<configuration>
      <system.webServer>
        <rewrite>
          <rules>
            <rule name="AngularJS Routes" stopProcessing="true">
              <match url=".*" />
              <conditions logicalGrouping="MatchAll">
                <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
                <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />   
              </conditions>
              <action type="Rewrite" url="/" />
            </rule>
          </rules>
        </rewrite>
      </system.webServer>
</configuration>
```
11. Check url in setting

# Reference
1. https://jakeydocs.readthedocs.io/en/latest/client-side/using-gulp.html
