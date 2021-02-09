namespace PressCenter.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using PressCenter.Data.Common.Repositories;
    using PressCenter.Data.Models;

    public class TopNewsSourceService : ITopNewsSourceService
    {
        private readonly IDeletableEntityRepository<TopNewsSource> topNewsSourceRepository;

        public TopNewsSourceService(IDeletableEntityRepository<TopNewsSource> topNewsSourceRepository)
        {
            this.topNewsSourceRepository = topNewsSourceRepository;
        }

        public IEnumerable<TopNewsSource> GetAll()
        {
            var sources = this.topNewsSourceRepository.All().ToList();
            return sources;
        }
    }
}
