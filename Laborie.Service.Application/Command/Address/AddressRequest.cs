namespace Laborie.Service.Application.Command.Address;

public sealed record AddressAddRequest(string Name
    , string Phone
    , string Address
    , string ProvinceId
    , string DistrictId
    , string WardId
    , bool IsHome
    , bool IsDefault
);

public sealed record AddressUpdateRequest(string Name
    , string Phone
    , string Address
    , string ProvinceId
    , string DistrictId
    , string WardId
    , bool IsHome
    , bool IsDefault
);