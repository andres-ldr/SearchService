using AutoMapper;
using Contracts;

namespace SearchService;

/*
    This class is used to map the properties of the AuctionCreatedConsumer to the Item class.
*/
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AuctionCreated, Item>();
        CreateMap<AuctionUpdated, Item>();
    }

}
