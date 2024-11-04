using Nakama;
using System;
using System.Collections.Generic;

namespace Networking.Nakama.Infrastructure.NakamaAdapters
{
    public interface INakamaSession
    {
        string AuthToken { get; }
        bool Created { get; }
        long CreateTime { get; }
        long ExpireTime { get; }
        bool IsExpired { get; }
        bool IsRefreshExpired { get; }
        long RefreshExpireTime { get; }
        string RefreshToken { get; }
        IDictionary<string, string> Vars { get; }
        string Username { get; }
        string UserId { get; }
        bool HasExpired(DateTime offset);
        bool HasRefreshExpired(DateTime offset);
        ISession GetInstance();
    }
}
