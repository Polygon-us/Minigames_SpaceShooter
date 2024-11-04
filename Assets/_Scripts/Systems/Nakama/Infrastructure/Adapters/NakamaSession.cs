using System;
using System.Collections.Generic;
using Nakama;

namespace Networking.Nakama.Infrastructure.NakamaAdapters
{
    public class NakamaSession : INakamaSession
    {
        private readonly ISession _session;

        public NakamaSession(ISession session)
        {
            _session = session;
        }

        public ISession GetInstance()
        {
            return _session;
        }

        public string AuthToken => _session.AuthToken;
        public bool Created => _session.Created;
        public long CreateTime => _session.CreateTime;
        public long ExpireTime => _session.ExpireTime;
        public bool IsExpired => _session.IsExpired;
        public bool IsRefreshExpired => _session.IsRefreshExpired;
        public long RefreshExpireTime => _session.RefreshExpireTime;
        public string RefreshToken => _session.RefreshToken;
        public IDictionary<string, string> Vars => _session.Vars;
        public string Username => _session.Username;
        public string UserId => _session.UserId;

        public bool HasExpired(DateTime offset)
        {
            return _session.HasExpired(offset);
        }

        public bool HasRefreshExpired(DateTime offset)
        {
            return _session.HasRefreshExpired(offset);
        }

        public override string ToString()
        {
            return $"NakamaSession:\n" +
                   $"  UserId: {UserId}\n" +
                   $"  Username: {Username}\n" +
                   $"  AuthToken: {AuthToken}\n" +
                   $"  Created: {Created}\n" +
                   $"  CreateTime: {CreateTime}\n" +
                   $"  ExpireTime: {ExpireTime}\n" +
                   $"  IsExpired: {IsExpired}\n" +
                   $"  IsRefreshExpired: {IsRefreshExpired}\n" +
                   $"  RefreshToken: {RefreshToken}\n" +
                   $"  RefreshExpireTime: {RefreshExpireTime}\n" +
                   $"  Vars: {FormatVars(Vars)}";
        }

        private string FormatVars(IDictionary<string, string> vars)
        {
            if (vars == null || vars.Count == 0)
                return "None";

            var formattedVars = new List<string>();
            foreach (var kvp in vars)
            {
                formattedVars.Add($"{kvp.Key}: {kvp.Value}");
            }
            return string.Join(", ", formattedVars);
        }
    }
}
