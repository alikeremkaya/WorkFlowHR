WorkFlowHR, bir bilişim firmasının insan kaynakları süreçlerini yönetmek için geliştirilmiş, ASP.NET Core 9 tabanlı bir projedir. Modern yazılım prensipleri ve katmanlı mimari yapısı ile geliştirilmiş olan bu proje, güçlü ve genişletilebilir bir altyapıya sahiptir.

 Kullanılan Teknolojiler
ASP.NET Core 9 (MVC)

Entity Framework Core (Code First yaklaşımı)

MSSQL

Repository Pattern (Generic + Özel Repository)

Fluent API

Dependency Injection

DTO ve Service Pattern

Azure Active Directory (Kurumsal kimlik doğrulama)

Proje Mimarisi
Proje katmanlı mimari ile tasarlanmıştır:

1. WorkFlowHR.UI
Kullanıcı arayüzü (MVC Controller’lar, View’lar)

Giriş, yetkilendirme ve dashboard ekranları

2. WorkFlowHR.Application
Business Logic (İş mantığı)

DTO’lar (AdminDTO, ManagerDTO, MailDTO)

Servisler (AccountService, AdminService, ManagerService)

DependencyInjection.cs ile servislerin IoC Container’a eklenmesi

3. WorkFlowHR.Domain
Temel entity sınıfları (Admin, Manager, Employee vb.)

4. WorkFlowHR.Infrastructure
AppDbContext ile EF Core yapılandırması

Fluent API konfigürasyonları (AdminConfiguration, ManagerConfiguration)

Repository Pattern yapısı:

Generic Repository → EFBaseRepository<T>

Repository interface’leri → IAsyncRepository, IAsyncInsertableRepository, vb.

Özel repository’ler → AdminRepository, ManagerRepository

 Kullanılan Tasarım Pattern’leri
Projede aşağıdaki tasarım desenleri uygulanmıştır:

Pattern	Amaç
Repository Pattern	Veri erişim katmanını soyutlamak ve tekrar kullanılabilir hale getirmek
Dependency Injection	Katmanlar arası bağımlılıkları azaltmak, test edilebilirlik sağlamak
DTO Pattern	Veri transferini optimize etmek
Service Pattern	İş mantığını yönetmek için servis tabanlı yapı
Fluent API	EF Core entity yapılandırmalarını düzenli hale getirmek

 Veritabanı Yaklaşımı
Code First yaklaşımı kullanılmıştır.

DbContext yönetimi AppDbContext ile sağlanır.

Fluent API konfigürasyonu mevcuttur.

SeedData klasöründe örnek veriler (AdminSeed.cs) bulunmaktadır.
