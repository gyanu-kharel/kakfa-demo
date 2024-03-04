namespace Shared;

public class UserSignUpMessage
{
    public UserSignUpMessage(string name, string email)
    {
        Name = name;
        Email = email;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Email { get; set; }
}