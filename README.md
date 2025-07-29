
# WorkFlowHR

WorkFlowHR, İnsan Kaynakları süreçlerini yönetmek için **.NET Core 9** kullanılarak oluşturulmuş, **katmanlı mimari (Clean Architecture)** prensiplerine dayalı bir projedir.  
Şu anda proje **altyapı hazırlığı** aşamasındadır ve üç temel katmandan oluşmaktadır.

---

##  Kullanılan Teknolojiler
- **.NET Core 9**
- **Entity Framework Core** (Planlanıyor)
- **MSSQL Server** (Planlanıyor)
- **Clean Architecture Yaklaşımı**
- **Dependency Injection** (Varsayılan olarak mevcut)

---

##  Proje Yapısı
```
WorkFlowHR
├── WorkFlowHR.Application      # Uygulama katmanı (iş mantığı, servisler)
├── WorkFlowHR.Domain           # Domain katmanı (entityler, core modeller)
├── WorkFlowHR.Infrastructure   # Altyapı katmanı (veritabanı, repository)
└── WorkFlowHR.sln              # Çözüm dosyası
```

---

##  Katmanlar
- **Domain:**  
  Temel iş kuralları ve entity modelleri burada yer alacak.
  
- **Application:**  
  Domain ile etkileşen servisler, business logic, UseCase yapısı bu katmanda olacak.
  
- **Infrastructure:**  
  Veritabanı bağlantısı, Entity Framework Core konfigürasyonu, Repository pattern bu katmanda bulunacak.


