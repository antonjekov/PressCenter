using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;

namespace PressCenter.Services.Sources
{
    public abstract class BaseSource
    {
        protected BaseSource()
        {
            this.Context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        }

        public abstract string EntryPointUrl { get; }
        public abstract string BaseUrl { get; }
        public IBrowsingContext Context { get; }

        public abstract Task<IEnumerable<RemoteNews>> GetNewPublications(List<string> existingNewsRemoteIds);

        public abstract Task<IEnumerable<RemoteNews>> GetAllPublicationsAsync();
    }
}
