using System;
using System.Collections.Generic;
using Horus.Api.Challenges;
using Refit;

namespace Horus.Api
{
    public interface IApi
    {
        [Get("/Challenges")]
        IObservable<IEnumerable<Challenge>> GetChallenges();
    }
}