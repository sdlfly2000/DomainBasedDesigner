dotnet ef dbcontext scaffold `
"server=homeserver2;database=DomainDrivenDesigner;uid=sdlfly2000;password=sdl@1215;TrustServerCertificate=True" "Microsoft.EntityFrameworkCore.SqlServer" `
--use-database-names `
--project ./Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.csproj `
--context DomainDbContext `
--context-dir ./Context `
--output-dir ./Entities `
--force `
`
--table dbo.T_PROJECT 