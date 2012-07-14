using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// adds tagging to inquiry
    /// </summary>
    public class TagEntity : IIdentifiable
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
