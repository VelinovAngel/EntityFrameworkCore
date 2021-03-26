namespace VaporStore
{
	using AutoMapper;
    using VaporStore.Data.Models;
    using VaporStore.ImportResults;

    public class VaporStoreProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public VaporStoreProfile()
		{
			this.CreateMap<ImportGameDTO, Game>();
		}
	}
}