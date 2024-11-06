using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborie.Service.Domain.Entities.Mongo.Shop;

namespace Laborie.Service.Domain.ValueObjects
{
    public class AddressVO
    {
        public FulfillmentWards? Ward { get; set; }
        public FulfillmentDistricts? District { get; set; }
        public FulfillmentProvinces? Province { get; set; }
    }
}