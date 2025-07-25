using DataBrokerAPI.Entities;
using DataBrokerAPI.Entities.DTOs;

namespace DataBrokerAPI.Repositories
{
    public interface ICustomerRepo
    {
        TokenResponseDTO GetCustomerByUsername(CustomerDTO request);
    }
}
