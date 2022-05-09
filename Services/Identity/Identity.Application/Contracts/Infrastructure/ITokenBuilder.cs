namespace Identity.Application.Contracts.Infrastructure
{
    public interface ITokenBuilder
    {
        string BuildToken(string username);
    }
}
