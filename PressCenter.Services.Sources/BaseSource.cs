using System;
using System.Collections.Generic;
using System.Text;
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
        public IBrowsingContext Context { get; }

        public abstract IEnumerable<RemoteNews> GetLatestPublications();

        public abstract IEnumerable<RemoteNews> GetAllPublicationsAsync();
    }
}
