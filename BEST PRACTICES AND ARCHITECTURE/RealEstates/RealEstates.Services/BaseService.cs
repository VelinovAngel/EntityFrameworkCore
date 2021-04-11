namespace RealEstates.Services
{
    using AutoMapper;

    using RealEstates.Services.Profiler;

    public abstract class BaseService
    {
        public BaseService()
        {
            InitializeAutoMapper();
        }

        protected IMapper Mapper { get; private set; }

        private void InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RealEstatesProfiler>();
            });

            this.Mapper = config.CreateMapper();
        }
    }
}
