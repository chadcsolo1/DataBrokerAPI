namespace DataBrokerAPI.Entities
{
    public class Customer
    {
        public int CustomerId
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        } = string.Empty;
        public string LastName
        {
            get;
            set;
        } = string.Empty;

        public string Username
        {
            get;
            set;
        } = string.Empty;
        public string PasswordHash
        {
            get;
            set;
        } = string.Empty;

        public string MemberShip
        {
            get;
            set;
        } = string.Empty;
    }
}
