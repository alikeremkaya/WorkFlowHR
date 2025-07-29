using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowHR.Infrastructure.DataAccess.Interfaces
{
    public interface IRepository
    { /// <summary>
      /// Yapılan değişiklikleri veritabanına kaydeder.
      /// </summary>
      /// <returns>
      /// Veritabanına kaydedilen değişikliklerin sayısını döner.
      /// Eğer bir hata oluşursa, hata kodu olarak negatif bir değer döner.
      /// </returns>
        int SaveChanges();
    }
}
