# DrugSchedule
![Static Badge](https://img.shields.io/badge/.NET%208-blue?style=for-the-badge)
![Static Badge](https://img.shields.io/badge/EF%20Core-8A2BE2?style=for-the-badge)
![Static Badge](https://img.shields.io/badge/SQL%20Server-d38712?style=for-the-badge)    
**Do not forget taking your medicine with .NET 8**    
ASP.NET WebAPI project that allows user to create timetable of taking medicine, confirm and control themselves and contacts
    
## App Features
### User features
- **Scheduling of taking drugs** with different patterns
- **Viewing calendar** with all the medicine you've scheduled to take 
- **Confirmation** of following schedules with text message and/or photo 
- **Searching for drug** info in existing library (we don't provede WebAPI to fill library)
- **Forming own drug** info based on library data
- **Sharing schedules** with other app users to let them control you

### API Features
- JWT Auth
- Signed URLs for downloading files
- Simplified and Extended DTOs, collections, filters
- Human readable error messages with 404 and 400 status codes

### Code Features
- OneOf<> as return type of serevices' methods
- IQueryable extensions for filters applying and more
- Custom Expressions for data projection and filtering
- Separate project with DTOs for referencing from client app project
- Options pattern for 
- Libraries in use: [ImageSharp](https://github.com/SixLabors/ImageSharp), [Mapster](https://github.com/MapsterMapper/Mapster), etc.

## How to run
1. Clone repository
1. Apply migrations. In `DrugSchedule.Storage` directory, run
   ```
   dotnet ef database update --connection "<YourConnectionsString>"
   ```
1. Fill `appsettings.json` of `DrugSchedule.Api`
   - Specify correct values where `<env. specific>`. Predifined ones cam be left default.    
   - Mind to specify correct URL under `ValidAudience` and `ValidIssuer` to make auth work. *Default launch URL is `http://localhost:5126`*    
1. Build `DrugSchedule.Api` and run.   
1. If `EnableSwagger` set to `true`, open in browser
   ```
   http://localhost:5126/swagger/index.html
   ```
   
## Planning features
- Sample data collection for Drug Library *(under development)*
- Blazor WASM client *(under development)*
- Email confirmation...
