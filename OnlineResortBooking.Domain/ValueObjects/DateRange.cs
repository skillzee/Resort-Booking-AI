namespace OnlineResortBooking.Domain.ValueObjects;

public sealed record DateRange
{
    public DateOnly CheckIn { get; }

    public DateOnly CheckOut { get; }

    public int Nights => CheckOut.DayNumber - CheckIn.DayNumber;

    public DateRange(DateOnly checkIn, DateOnly checkOut)
    {
        if (checkOut <= checkIn)
        {
            throw new ArgumentException(
                "Check-out date must be after check-in date.");
        }

        CheckIn = checkIn;
        CheckOut = checkOut;
    }
}