using PressCenter.Data.Common.Repositories;
using PressCenter.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PressCenter.Services.Data
{
    public class SourceService : ISourceService
    {
        private IDeletableEntityRepository<Source> sourceRepository;

        public SourceService(IDeletableEntityRepository<Source> sourceRepository)
        {
            this.sourceRepository = sourceRepository;
        }

        public IEnumerable<Source> GetAll()
        {
            var sources = this.sourceRepository.All().ToList();
            return sources;
        }
    }
}
