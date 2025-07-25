namespace DataBrokerAPI.Entities.DTOs
{
    public class TokenResponseDTO
    {
        //JWT
        public required string AccessToken
        {
            get;
            set;
        }

        //Refresh Token
        public required string  RefreshToken
        {
            get;
            set;
        }
    }
}
