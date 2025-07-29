using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowHR.Infrastructure.DataAccess.Interfaces
{
    public interface IAsyncRepository
    { /// <summary>
      /// Yapılan değişiklikleri asenkron olarak veritabanına kaydeder.
      /// </summary>
      /// <returns>
      /// Asenkron operasyonun sonucunu temsil eden bir <see cref="Task{TResult}"/>. 
      /// Görev tamamlandığında, veritabanına kaydedilen değişikliklerin sayısını döner.
      /// Eğer bir hata oluşursa, hata kodu olarak negatif bir değer döner.
      /// </returns>
        Task<int> SaveChangesAsync();
    }
}
