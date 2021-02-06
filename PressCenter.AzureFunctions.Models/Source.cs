using System;
using System.Collections.Generic;

namespace PressCenter.AzureFunctions.Models
{
    public class Source
    {
        public string TypeName { get; set; }

        public string ShortName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string EntryPointUrl { get; set; }

        public string Url { get; set; }

        public string DefaultImageUrl { get; set; }
    }
}
