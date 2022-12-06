using System;
using Refit;

namespace Horus.Api.Account
{
    public interface IAccountApi
    {
        [Post("/UserSignIn")]
        IObservable<SignInResponse> SignIn([Body] SignInRequest request);
    }
}