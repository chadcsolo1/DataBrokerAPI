using DataBrokerAPI.Entities;
using DataBrokerAPI.Entities.DTOs;

namespace DataBrokerAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<TokenResponseDTO> Login(CustomerDTO request);
        Task<string> Registar(RegistarCustomerReqeust request);
        Task<string> GenerateAndSaveRefreshToken(Customer request);
        //string GenerateRefreshToken(CustomerDTO request);
    }
}
