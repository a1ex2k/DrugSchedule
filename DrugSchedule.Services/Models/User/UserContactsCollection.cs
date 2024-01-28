namespace DrugSchedule.BusinessLogic.Models;

public class UserContactsCollection
{
    public required long UserId { get; set; }

    public List<UserContact>? Contacts { get; set; }
}