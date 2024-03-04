using Bogus;
using Shared.Models;

namespace Registration;

public static class Iterator
{
    public static IEnumerable<User> GetUsers()
    {
        var user = new User();
        for (int i = 0; i < 50; i++)
        {
            var faker = new Faker();
            
            user.Id = Guid.NewGuid();
            user.Name = faker.Person.FullName;
            user.Email = faker.Person.Email;
            
            Thread.Sleep(1000);
            yield return user;
        }
    }
}