﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Core.Base;
using WorkFlowHR.Domain.Core.Interfaces;

namespace WorkFlowHR.Infrastructure.DataAccess.Interfaces
{
    public interface IAsyncUpdatableRepository<TEntity> : IAsyncRepository where TEntity : BaseEntity
    {/// <summary>
     /// Belirtilen varlığı asenkron olarak veritabanında günceller.
     /// </summary>
     /// <param name="entity">
     /// Güncellenecek varlık. Varlığın türü <typeparamref name="TEntity"/> olmalıdır.
     /// </param>
     /// <returns>
     /// Asenkron operasyonun sonucunu temsil eden bir <see cref="Task{TResult}"/>. 
     /// Görev tamamlandığında, güncellenen varlığı döner.
     /// </returns>
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
