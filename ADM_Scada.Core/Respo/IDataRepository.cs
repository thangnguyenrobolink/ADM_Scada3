using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADM_Scada.Core.Respo
{
    public interface IDataRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetByName(string s);
        Task<int> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
        Task<T> GetById(int id);
        Task<List<T>> GetFiltered(params Func<T, bool>[] filters);
        Task<int> Count();
        Task<bool> Exists(int id);
    }
}
