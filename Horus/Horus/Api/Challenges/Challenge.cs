using System;

namespace Horus.Api.Challenges
{
    public class Challenge
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double CurrentPoints { get; set; }
        public double TotalPoints { get; set; }
    }
}