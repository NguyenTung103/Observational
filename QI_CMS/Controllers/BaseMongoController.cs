using ES_CapDien.AppCode.Interface;
using System.Linq;
using System.Web.Mvc;

namespace ES_CapDien.Controllers
{    
    public class BaseMongoController : ControllerBase
    {
        protected readonly IDataObservationService dataObservationMongoService;
        public BaseMongoController(IDataObservationService _dataObservationMongoService)
        {
            dataObservationMongoService = _dataObservationMongoService;
        }

        protected override void ExecuteCore()
        {
            throw new System.NotImplementedException();
        }
    }
}