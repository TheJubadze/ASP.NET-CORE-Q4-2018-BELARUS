using System;
using Core.Repositories;

namespace Core
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        int Complete();
    }
}
