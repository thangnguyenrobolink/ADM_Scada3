﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADM_Scada.Cores.Respo
{
    public interface IDataRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetByName(string s);
        Task<int> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
        Task<T> Get(int id);
    }
}
