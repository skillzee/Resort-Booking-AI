namespace OnlineResortBooking.Domain.Entities;

public class GuestProfile
{
    public Guid Id { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Email { get; private set; }

    public int NumberOfGuests { get; private set; }

    public GuestProfile(
        Guid id,
        string firstName,
        string lastName,
        string email,
        int numberOfGuests)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        NumberOfGuests = numberOfGuests;
    }
}