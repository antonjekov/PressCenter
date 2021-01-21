using PressCenter.Data.Common.Repositories;
using PressCenter.Data.Models;
using PressCenter.Services.Mapping;
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

        public IEnumerable<T> GetAll<T>()
        {
            return this.sourceRepository.AllAsNoTracking()
                .To<T>()
                .ToList();
        }
    }
}
