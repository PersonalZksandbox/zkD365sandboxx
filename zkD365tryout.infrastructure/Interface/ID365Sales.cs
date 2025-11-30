using Microsoft.Crm.Sdk.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zkD365tryout.infrastructure.Interface
{
    public interface ID365Sales
    {
        Task<string> GetAccessTokenAsync();
        Task<string> CreateD365SalesAccountName(string token, string name);
    }
}
