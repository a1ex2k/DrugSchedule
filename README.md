# DrugSchedule
![Static Badge](https://img.shields.io/badge/.NET-8.0.1-blue)
![Static Badge](https://img.shields.io/badge/EF_Core-8.0.1-8A2BE2) 
**Do not forget taking your medicine with .NET 8**

## App Features
### User features
- **Scheduling of taking drugs** with different patterns
- **Viewing calendar** with all the medicine you've scheduled to take 
- **Confirmation** of following schedules with text message and/or photo 
- **Searching for drug** info in existing library with filters
- **Forming own drug** info based on library data
- **Sharing schedules** with other app users to let them control you
### API Features
- JWT Auth
- Signed URLs for downloading files
- Simplified and Extended DTOs, collection
- Human readable error messages with 404 and 400 status codes  
### Code Features
- OneOf<> as return type of serevices' methods
- IQueryable extensions based on custum expressions for filters applying and more
- Custom Expressions for
  
  - Users
  - Registration (with email confirmation in future)
  - Logging in
  - Check for available usernames
  - Search for users, add to contacts
  - ...

- Storage Service Contracts
  - Data classes and interfaces for repos
    - Medicaments
    - Schedule
    - Users
    - File storage
  - Filters for quering lists of smth.
  - Paging for repos (skip/take)
  - Custom Exceptions for repos
  - ...

- Sql Server Data Access
   - IQueryable ext for filters applying
   - Get list with filter and single by Id. With pagination.
   - Create/update with sublists check, delete.
   - Basic info of uploaded files here, 
   - ...

- Medicament Collection (Medicament, ManuFacturer, RealeseForm)
  - Separate app to parse 103.by for filling up Medicament Library
  - Only read API endpoints
  - ...

- File service
  - Direct save / delete
  - Returns Signed Url for uploading and downloading by client
  - GUID as public file identificator
  - ...

- Scheduling features
  - Reusable parts of schedule
  - ...

- Busines layer common features
  - Creating copy of entities with specifying only modified fields
  - ...
  
- Additional
  - OneOf<> as return type of controller actions
  - Cancellation request checks