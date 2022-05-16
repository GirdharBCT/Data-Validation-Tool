using Data_Validation_Tool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_Validation_Tool.Interface
{
    public interface IAuth
    {
        Task<DataanalysisUser> LoginUser(string username, string password);
    }
}
