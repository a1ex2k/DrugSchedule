namespace DrugSchedule.BusinessLogic.Models;

public class UserContactsCollectionModel
{
    public required long UserId { get; set; }

    public List<UserContactModel>? Contacts { get; set; }
}

public class UserPublicCollectionModel
{
    public List<PublicUserModel>? Users { get; set; }
}