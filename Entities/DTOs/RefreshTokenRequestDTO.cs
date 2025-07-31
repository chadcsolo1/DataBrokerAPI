namespace DataBrokerAPI.Entities.DTOs
{
    public class RefreshTokenRequestDTO
    {
        public int CustomerId
        {
            get;
            set;
        }

        public required string RefreshToken
        {
            get;
            set;
        } 
    }
}
