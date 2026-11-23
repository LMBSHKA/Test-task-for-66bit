using Application.Interfaces.RepoIntrefaces;
using Application.Interfaces.ServiceInterface;
using Models.Entities;

namespace Application.Services
{
    public class ManufacturerService : GenericService<Manufacturer>, IManufacturerService
    {
        public ManufacturerService(IWarehouseUnitOfWork uow, IGenericRepo<Manufacturer> repo) : base(uow, repo)
        {
        }
    }
}
