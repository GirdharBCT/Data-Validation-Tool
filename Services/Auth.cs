using Data_Validation_Tool.Interface;
using Microsoft.EntityFrameworkCore;
using Data_Validation_Tool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data_Validation_Tool.Data;

namespace Data_Validation_Tool.Services
{
    public class Auth : IAuth
    {
        private readonly prd_phyndContext _context;
        public Auth(prd_phyndContext context)
        {
            _context = context;
        }
        public async Task<DataanalysisUser> LoginUser(string username, string password)
        {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return null;
                }
                return await _context.DataanalysisUsers.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }
    }
}
