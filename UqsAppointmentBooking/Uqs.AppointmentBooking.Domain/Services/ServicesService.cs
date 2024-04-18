using Microsoft.EntityFrameworkCore;
using Uqs.AppointmentBooking.Contract;
using Uqs.AppointmentBooking.Domain.Database;
using Service = Uqs.AppointmentBooking.Domain.DomainObjects.Service;

namespace Uqs.AppointmentBooking.Domain.Services;

public class ServicesService : IServicesService
{
    private readonly ApplicationContext _context;

    public ServicesService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Service>> GetActiveServices() =>
        await _context.Services!.Where(x => x.IsActive).ToArrayAsync();

    public async Task<Service?> GetService(int id) =>
        await _context.Services!.SingleOrDefaultAsync(x => x.IsActive && x.Id == id);
}