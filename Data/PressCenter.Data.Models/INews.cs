namespace PressCenter.Data.Models
{
    using System;

    public interface INews : ITopNews
    {
        string Content { get; set; }

        DateTime Date { get; set; }
    }
}
