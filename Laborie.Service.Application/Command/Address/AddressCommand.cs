using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;

namespace Laborie.Service.Application.Command.Address;

public sealed record AddressAdd(string UserId
    , string Name
    , string Phone
    , string Address
    , string ProvinceId
    , string DistrictId
    , string WardId
    , bool IsHome
    , bool IsDefault
) : ICommand<Response>;

public sealed record AddressUpdate(string UserId
    , string AddressId    
    , string Name
    , string Phone
    , string Address
    , string ProvinceId
    , string DistrictId
    , string WardId
    , bool IsHome
    , bool IsDefault
) : ICommand<Response>;

public sealed record AddressDelete(string UserId, string AddressId) : ICommand<Response>;