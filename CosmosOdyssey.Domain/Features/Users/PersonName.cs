namespace CosmosOdyssey.Domain.Features.Users;

public class Customer
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Customer(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    protected Customer()
    {
    }
};