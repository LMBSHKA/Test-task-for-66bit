using Application.Interfaces.RepoIntrefaces;
using Application.Interfaces.ServiceInterface;
using Models.Entities;

namespace Application.Services
{
    public class EquipmentTypeService : GenericService<EquipmentType>, IEquipmentTypeServce
    {
        public EquipmentTypeService(IWarehouseUnitOfWork uow, IGenericRepo<EquipmentType> repo) : base(uow, repo)
        {
        }
    }
}
