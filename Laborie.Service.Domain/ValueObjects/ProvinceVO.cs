using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborie.Service.Domain.Entities.Mongo.Shop;

namespace Laborie.Service.Domain.ValueObjects
{
    public class ProvinceVO : FulfillmentProvinces
    {
        public required List<DistrictVO> Districts { get; set; }
    }
}