namespace WebAPI.Models.Responses
{
    public class RegisterResponse
    {
        public Guid CustomerId { get; set; }
        public string JwtToken { get; set; }

    }
}
