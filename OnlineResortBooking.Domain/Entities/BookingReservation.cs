using OnlineResortBooking.Domain.ValueObjects;

namespace OnlineResortBooking.Domain.Entities;

public class BookingReservation
{
    public Guid Id { get; private set; }

    public Guid ResortId { get; private set; }

    public Guid RoomTypeId { get; private set; }

    public GuestProfile Guest { get; private set; }

    public DateRange Stay { get; private set; }

    public MonetaryAmount TotalAmount { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public BookingReservation(
        Guid id,
        Guid resortId,
        Guid roomTypeId,
        GuestProfile guest,
        DateRange stay,
        MonetaryAmount totalAmount)
    {
        Id = id;
        ResortId = resortId;
        RoomTypeId = roomTypeId;
        Guest = guest;
        Stay = stay;
        TotalAmount = totalAmount;
        CreatedAt = DateTime.UtcNow;
    }
}