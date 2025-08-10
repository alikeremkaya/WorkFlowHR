

namespace WorkFlowHR.Domain.Core.Interfaces
{
    public interface ICreatableEntity:IEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
