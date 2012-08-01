namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public class UserService:IIdentifiable
    {
        public int Id { get; set; }
        public IIdentifiable CurrentUser { get; set; }
    }
}
