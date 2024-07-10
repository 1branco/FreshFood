namespace SecurityAPI.Models.Responses
{
    public class JwtToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string TokenTTL { get; set; }
    }
}
