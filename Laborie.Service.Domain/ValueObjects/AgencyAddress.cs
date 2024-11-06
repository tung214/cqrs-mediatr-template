using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborie.Service.Domain.Entities.Mongo.Laborie;

namespace Laborie.Service.Domain.ValueObjects
{
    public class AgencyAddressVO : LaborieAgency
    {
        public List<LaborieAgencyAddress>? Addresses { get; set; }
    }
}