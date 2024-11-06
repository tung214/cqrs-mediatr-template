using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborie.Service.Domain.ValueObjects;

namespace Laborie.Service.Domain.Repositories.Mongo
{
    public interface IAgencyAddressRepository
    {
        Task<AgencyAddressVO?> GetAgencyAddress(string agencyId);
        Task<AddressVO?> GetAddress(string provinceId, string districtId, string wardId);
    }
}