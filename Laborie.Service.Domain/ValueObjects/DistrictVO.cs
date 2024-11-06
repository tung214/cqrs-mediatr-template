using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborie.Service.Domain.Entities.Mongo.Shop;

namespace Laborie.Service.Domain.ValueObjects
{
    public class DistrictVO : FulfillmentDistricts
    {
        public required List<FulfillmentWards> Wards { get; set; }
    }
}