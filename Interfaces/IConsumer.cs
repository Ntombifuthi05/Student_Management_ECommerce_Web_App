using ASPNETCore_DB.Models;

namespace ASPNETCore_DB.Interfaces
{
    public interface IConsumer
    {
        IQueryable<Consumer> GetConsumers(string searchString, string sortOrder);
        Consumer Details(string id);
        Consumer Create(Consumer consumer);
        Consumer Edit(Consumer consumer);
        bool Delete(Consumer consumer);
        bool IsExist(string id);
    }
}