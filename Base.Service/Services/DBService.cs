using Base.Common;
using Base.Model.Models;
using Base.Service.Helpers;
using Base.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Base.Service.Services
{
    public class DBService : IDBService
    {
        private readonly IMemoryCache _cache;
        public DBService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<BaseResultModel> GetUserList()
        {
            var result = new BaseResultModel();
            try
            {
                var dt = await DatabaseHelper.ListFromStoredProcedure("PRC_User_List");
                var listResult = new List<User>();
                if (dt != null)
                {
                    listResult = ConvertHelper.ConvertDataTable<User>(dt);
                }
                result.AddResultObject(listResult);
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }



    }
}
