# DrugSchedule
![Static Badge](https://img.shields.io/badge/.NET%208-blue?style=for-the-badge)
![Static Badge](https://img.shields.io/badge/EF%20Core-8A2BE2?style=for-the-badge)
![Static Badge](https://img.shields.io/badge/SQL%20Server-d38712?style=for-the-badge)
![Static Badge](https://img.shields.io/badge/Blazor_WASM-592c8c?style=for-the-badge)    
     
**Do not forget taking your medicine!**    
ASP.NET WebAPI project that allows users to create timetable of taking medicine, confirm and control themselves and contacts     
    
## App Features
### User features
- **Scheduling of taking drugs** with different patterns
- **Viewing calendar** with all the medicine you've scheduled to take 
- **Confirmation** of following schedules with text message and/or photo 
- **Searching for drug** info in existing library (we don't provede WebAPI to fill library)
- **Forming own drug** info based on library data
- **Sharing schedules** with other app users to let them control you

### API Features
- JWT Auth with refresh tokens
- Signed URLs for downloading files
- Simplified and Extended DTOs, collections, filters
- Human readable error messages with 404 and 400 status codes

### Code Features
- Discriminated Unions as return type of serevices
- IQueryable extensions for filters applying and more
- Custom Expressions for data projection and filtering
- Separate project with DTOs for referencing from client app project
- Options pattern for various parameters
- Libraries in use: [OneOf](https://github.com/mcintyre321/OneOf), [Mapster](https://github.com/MapsterMapper/Mapster), [ImageSharp](https://github.com/SixLabors/ImageSharp), etc.     
     
## Blazor Client
Blazor client project is an example of using API and provides the main functionality:
- Explore medicament library
- Manage user medicaments
- Manage user profile and contacts
- Create schedules

Look through the source code to find out basic principals and run the project to check the API functionality using the GUI. We recommend to **fill medicament library** with provided sample data from [103.by](https://apteka.103.by/).    
Made with [Blazorise](https://github.com/Megabit/Blazorise) component library.     
     
     
## How to host
### Run WebAPI
1. Clone repository
1. Apply migrations (app tested with SQL Server 2019). In `DrugSchedule.Storage` directory, run
   ```
   dotnet ef database update --connection "<YourConnectionString>"
   ```
1. Fill `appsettings.json` of `DrugSchedule.Api`
   - Specify correct values where `<env. specific>`. Predefined ones can be left default.
     Probably, connection string might have `TrustServerCertificate=True; Trusted_Connection=True;`
   - Mind to specify correct URL under `ValidAudience`, `ValidIssuer`, `CorsOrigins` to make auth work.    
     *Default launch URL is `http://localhost:5126` and `http://localhost:5127` is for Blazor client.* 
1. Build `DrugSchedule.Api` and run.   
1. If `EnableSwagger` set to `true`, open in browser
   ```
   http://localhost:5126/swagger/index.html
   ```
        
### Run Blazor client
1. Set multiple startup projects in Visual Studio: `DrugSchedule.Api` and `DrugSchedule.Client`
1. Build and run. Client will be available on
   ```
   http://localhost:5127/
   ```
1. If you want use different address/port, update respectively:
   - `CorsOrigins` property in `appsettings.json` of `DrugSchedule.Api`
   - `ApiBaseUri` property in `wwwroot/appsettings.json` of `DrugSchedule.Client`
        
### Fill medicament library
1. Download [DrugScheduleSampleData.7z](https://drive.google.com/file/d/1jQhQEunzjMeytzffdpDUFravuY0pUqrG/view?usp=sharing) and extract its content.
1. Ensure migrations are already applied. 
1. Build `Utils/DrugScheduleFill` and run it with parameters:
   ```
   DrugScheduleFill.exe -s "<path/to/extracted>" -c "<YourConnectionString>" -o "<storage/directory>"
   ```
   where `<storage/directory>` is `FileStorageOptions:DirectoryPath` of API project appsettings     
        
## Planning features
- Make Blazor WASM client better
- Email confirmation
