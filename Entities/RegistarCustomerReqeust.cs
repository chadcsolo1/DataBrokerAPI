namespace DataBrokerAPI.Entities
{
    public class RegistarCustomerReqeust
    {
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
        public string Password
        {
            get;
            set;
        } = string.Empty;
    }
}
