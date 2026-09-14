using OnlineResortBooking.Domain.Entities;

namespace OnlineResortBooking.Application.Interfaces;

public interface IResortRepository
{
    IReadOnlyCollection<Resort> GetAll();

    Resort? GetById(Guid id);

    IEnumerable<Resort> Search(
        string? location,
        string? roomType,
        bool? hasPrivatePool);
}