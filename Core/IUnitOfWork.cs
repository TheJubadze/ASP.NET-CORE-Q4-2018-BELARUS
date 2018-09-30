using System;
using Core.Repositories;

namespace Core
{
    public interface IUnitOfWork : IDisposable
    {
        ISupplierRepository Suppliers { get; }
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        int Complete();
    }
}
