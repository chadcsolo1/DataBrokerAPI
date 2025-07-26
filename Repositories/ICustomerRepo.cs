using DataBrokerAPI.Entities;
using DataBrokerAPI.Entities.DTOs;

namespace DataBrokerAPI.Repositories
{
    public interface ICustomerRepo
    {
        Task<Customer> GetCustomerByUsername(CustomerDTO request);
    }
}
