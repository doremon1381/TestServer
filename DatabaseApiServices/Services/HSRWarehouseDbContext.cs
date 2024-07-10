using Microsoft.EntityFrameworkCore;

namespace DatabaseApiServices.Services
{
    public class HSRWarehouseDbContext: DbContext, IHSRWarehouseDbContext
    {
    }

    public interface IHSRWarehouseDbContext
    {

    }
}
