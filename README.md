# DrugSchedule
Do not forget your medicine **with .NET 8**


### Planning features
- **Scheduling of taking drugs** with different reusable patterns
- **Viewing calendar** with all the medicine you've scheduled to take 
- **Confirmation** of following schedules with text message and/or photo 
- **Searching for drug** info in existing library with filters
- **Forming own drug** info based on library data
- **Sharing schedules** with other app users to let them control you



### Features plan and whot to do

- Users
  - Registration (with email confirmation in future)
  - Logging in
  - Check for available usernames
  - Search for usesrs, add to contacts
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
   - IQueryable ext. for filters applying
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