using DataBrokerAPI.Entities.DTOs;

namespace DataBrokerAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<string> GenerateToken(CustomerDTO request);
        Task<TokenResponseDTO> GenerateRefreshToken(CustomerDTO request);
    }
}
