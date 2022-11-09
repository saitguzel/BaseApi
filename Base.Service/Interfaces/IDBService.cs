using Base.Common;

namespace Base.Service.Interfaces
{
    public interface IDBService
    {
        Task<BaseResultModel> GetUserList();


    }
}
