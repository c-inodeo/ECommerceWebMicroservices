﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }
        ICartRepository Cart { get; }
        Task Save();
    }
}
