using Identity.Domain.Common;

namespace Identity.Domain.Entities
{
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
