using Uqs.AppointmentBooking.Domain.DomainObjects;

namespace Uqs.AppointmentBooking.Domain.Services;

public interface IServicesService
{
    Task<IEnumerable<Service>> GetActiveServices();
    Task<Service?> GetService(int id);
}