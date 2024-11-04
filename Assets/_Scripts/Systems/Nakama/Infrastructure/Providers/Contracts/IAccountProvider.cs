using Nakama;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IAccountProvider
{
    Task<IApiAccount> GetAccountAsync();

}