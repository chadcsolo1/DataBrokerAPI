using DataBrokerAPI.Entities;

namespace DataBrokerAPI.Repositories
{
    public interface ICustomerRepo
    {
        Customer GetCustomerByUsername(string username);
    }
}
