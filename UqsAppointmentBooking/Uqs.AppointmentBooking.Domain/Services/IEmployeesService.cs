using Uqs.AppointmentBooking.Domain.DomainObjects;

namespace Uqs.AppointmentBooking.Domain.Services;

public interface IEmployeesService
{
    Task<IEnumerable<Employee>> GetEmployees();
}