using DataBrokerAPI.Entities;
using DataBrokerAPI.Entities.DTOs;

namespace DataBrokerAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<TokenResponseDTO> Login(CustomerDTO request);
        Task<string> Registar(RegistarCustomerReqeust request);
        Task<string> GenerateAndSaveRefreshToken(Customer request);

        Task<Customer> ValidateRefreshToken(RefreshTokenRequestDTO request);

        Task<TokenResponseDTO> RefreshTokensAsync(RefreshTokenRequestDTO request);
        //string GenerateRefreshToken(CustomerDTO request);
    }
}
