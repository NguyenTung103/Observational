using ES_CapDien.AppCode.Interface;
using ES_CapDien.Models;
using ES_CapDien.Models.Entity;
using Qi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ES_CapDien.AppCode
{
    public class DataObservationMongoData : MogoRepository<ES_CapDien.Models.Entity.Data>
    {                      
        public DataObservationMongoData()
        {            
            
        }        
    }
}