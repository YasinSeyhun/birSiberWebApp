# BirSiber Danışmanlık Web Uygulaması

<div align="center">
  <img src="wwwroot/images/logo.png" alt="BirSiber Logo" width="200"/>
  <br/>
  <p><em>Siber Güvenlik ve IT Danışmanlık Çözümleri</em></p>
</div>

## 📋 Proje Hakkında
BirSiber Danışmanlık, siber güvenlik ve IT danışmanlık hizmetleri sunan modern bir web uygulamasıdır. Kullanıcılar sisteme kayıt olabilir, giriş yapabilir, randevu alabilir ve hizmetlerimiz hakkında detaylı bilgi edinebilirler.

### 🌟 Temel Özellikler
- **Kullanıcı Yönetimi**
  - Güvenli kayıt ve giriş sistemi
  - Google ile tek tıkla giriş/kayıt
  - Profil yönetimi ve şifre değiştirme
  - İki faktörlü kimlik doğrulama (2FA)

- **Randevu Sistemi**
  - Online randevu oluşturma
  - Randevu takvimi görüntüleme
  - Randevu geçmişi
  - Otomatik hatırlatma e-postaları

- **Hizmet Kataloğu**
  - Detaylı hizmet açıklamaları
  - Fiyat listesi
  - Hizmet kategorileri
  - Özel teklifler

- **Yönetim Paneli**
  - Kullanıcı yönetimi
  - Randevu takibi
  - Hizmet yönetimi
  - Raporlama ve analiz

## 🛠️ Teknoloji Stack'i

### Backend
- **ASP.NET Core MVC 7.0**
  - Razor Pages
  - Tag Helpers
  - View Components
  - Partial Views

- **Entity Framework Core**
  - Code First yaklaşımı
  - Migration yönetimi
  - LINQ sorguları
  - Repository pattern

- **SQL Server 2019**
  - Stored Procedures
  - Views
  - Indexes
  - Full-text search

### Frontend
- **Bootstrap 5**
  - Responsive grid system
  - Custom components
  - Utility classes
  - Dark mode desteği

- **JavaScript/jQuery**
  - AJAX calls
  - Form validations
  - Dynamic content loading
  - Interactive UI elements

- **Font Awesome 6**
  - Modern ikonlar
  - Animasyonlar
  - Custom styling

### Güvenlik
- **Identity Framework**
  - Role-based authorization
  - Claims-based authentication
  - Password hashing
  - Account lockout

- **Google OAuth 2.0**
  - Secure authentication
  - Profile information
  - Email verification

## 📦 Detaylı Kurulum Adımları

### 1. Sistem Gereksinimleri
- **İşletim Sistemi**
  - Windows 10/11
  - macOS 10.15+
  - Linux (Ubuntu 20.04+)

- **Geliştirme Ortamı**
  - .NET 7.0 SDK
  - Visual Studio 2022 veya VS Code
  - Git 2.30+
  - Node.js 16+

- **Veritabanı**
  - SQL Server 2019+
  - SQL Server Management Studio
  - Azure Data Studio (opsiyonel)

### 2. Projeyi Klonlama ve Hazırlık
```bash
# Projeyi klonla
git clone https://github.com/kullaniciadi/birSiberDanismanlik.git

# Proje dizinine git
cd birSiberDanismanlik

# Bağımlılıkları yükle
dotnet restore

# Geliştirme sertifikasını oluştur
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### 3. Veritabanı Kurulumu
1. **SQL Server Kurulumu**
   - SQL Server Management Studio'yu açın
   - `create_database.sql` dosyasını açın
   - Dosyayı çalıştırın (F5 veya Execute butonu)
   - Bu script otomatik olarak:
     - Veritabanını oluşturacak
     - Tüm tabloları ve ilişkileri oluşturacak
     - Gerekli indeksleri ekleyecek
     - Örnek verileri ekleyecek (admin kullanıcısı ve temel roller)

2. **Bağlantı Ayarları**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=BirSiberDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
     }
   }
   ```

3. **Veritabanı Şeması**
   - `create_database.sql` dosyası aşağıdaki tabloları oluşturur:
     - AspNetUsers (Kullanıcılar)
     - AspNetRoles (Roller)
     - AspNetUserRoles (Kullanıcı-Rol İlişkileri)
     - Appointments (Randevular)
     - Services (Hizmetler)
   - Tüm tablolar arasındaki ilişkiler ve kısıtlamalar otomatik olarak oluşturulur
   - Örnek veriler eklenir:
     - Admin kullanıcısı (admin@admin.com)
     - Temel roller (Admin, User, Çalışan)
     - Örnek randevular ve hizmetler

### 4. Google OAuth Yapılandırması
1. **Google Cloud Console Ayarları**
   - Proje oluştur
   - OAuth 2.0 kimlik bilgilerini yapılandır
   - İzin verilen URI'leri ekle:
     ```
     https://localhost:7098/signin-google
     https://localhost:7098/Account/ExternalLoginCallback
     ```

2. **Uygulama Ayarları**
   ```json
   {
     "Authentication": {
       "Google": {
         "ClientId": "your-client-id",
         "ClientSecret": "your-client-secret",
         "CallbackPath": "/signin-google"
       }
     }
   }
   ```

### 5. Projeyi Çalıştırma
```bash
# Geliştirme modunda çalıştır
dotnet run --environment Development

# Production modunda çalıştır
dotnet run --environment Production
```

## 📁 Detaylı Proje Yapısı
```
birSiberDanismanlik/
├── Controllers/
│   ├── AccountController.cs          # Kullanıcı işlemleri
│   ├── AdminController.cs            # Admin paneli işlemleri
│   ├── ApiAuthController.cs          # API kimlik doğrulama
│   ├── AppointmentApiController.cs   # Randevu API endpoints
│   ├── AppointmentController.cs      # Randevu yönetimi
│   ├── CacheTestController.cs        # Cache test işlemleri
│   ├── HomeController.cs             # Ana sayfa
│   └── ServicesController.cs         # Hizmet yönetimi
├── Models/
│   ├── Appointment.cs                # Randevu modeli
│   ├── ErrorViewModel.cs             # Hata sayfası modeli
│   ├── Service.cs                    # Hizmet modeli
│   └── User.cs                       # Kullanıcı modeli
├── Services/
│   ├── AppointmentService.cs         # Randevu iş mantığı
│   ├── EmailService.cs               # E-posta gönderimi
│   ├── IAppointmentService.cs        # Randevu servis arayüzü
│   ├── IServiceService.cs            # Hizmet servis arayüzü
│   ├── ReminderBackgroundService.cs  # Hatırlatma servisi
│   ├── ServiceService.cs             # Hizmet iş mantığı
│   └── SmsService.cs                 # SMS gönderimi
├── ViewModels/                       # View modelleri
├── Views/                            # Razor görünümleri
├── wwwroot/                          # Statik dosyalar
├── Data/                             # Veritabanı context
├── Migrations/                       # EF Core migrations
├── Properties/                       # Proje özellikleri
├── .github/                          # GitHub yapılandırması
├── .vscode/                          # VS Code ayarları
├── .vs/                              # Visual Studio ayarları
├── Program.cs                        # Uygulama başlangıç noktası
├── appsettings.json                  # Uygulama ayarları
├── appsettings.Development.json      # Geliştirme ayarları
├── BirSiberDanismanlik.csproj        # Proje dosyası
├── databasescript.sql                # Veritabanı şeması
├── dockerfile                        # Docker yapılandırması
└── requirements.txt                  # Bağımlılıklar
```

## 🔐 Güvenlik Özellikleri

### Kimlik Doğrulama
- JWT tabanlı authentication
- OAuth 2.0 entegrasyonu
- İki faktörlü kimlik doğrulama
- Oturum yönetimi

### Yetkilendirme
- Role-based access control (RBAC)
- Policy-based authorization
- Resource-based authorization
- Custom authorization handlers

### Veri Güvenliği
- HTTPS zorunluluğu
- SQL injection koruması
- XSS koruması
- CSRF token doğrulama
- Input validasyonu
- Output encoding

### Şifreleme
- Password hashing (BCrypt)
- Veri şifreleme
- SSL/TLS
- Secure cookie handling

## 👥 Kullanıcı Rolleri ve İzinler

### Admin
- Tüm sisteme tam erişim
- Kullanıcı yönetimi
- Rol yönetimi
- Sistem ayarları
- Raporlama

### Çalışan
- Randevu yönetimi
- Hizmet yönetimi
- Müşteri görüşmeleri
- Randevu onaylama/reddetme
- Randevu notları ekleme

### User
- Profil yönetimi
- Randevu oluşturma
- Randevu görüntüleme
- Hizmet görüntüleme

## 📝 Veritabanı Şeması Detayları

### Users Tablosu
```sql
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    LastLoginAt DATETIME NULL
);
```

### Appointments Tablosu
```sql
CREATE TABLE Appointments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    ServiceType NVARCHAR(50) NOT NULL,
    AppointmentDate DATETIME NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
```

## 🐛 Hata Ayıklama ve Sorun Giderme

### Visual Studio Debug
1. Breakpoint'ler ekle
2. Watch window kullan
3. Immediate window ile değerleri kontrol et
4. Call stack'i incele

### Tarayıcı Araçları
1. Network sekmesi
2. Console hataları
3. Application storage
4. Performance profiling

### Veritabanı Debug
1. SQL Server Profiler
2. Execution plan analizi
3. Index kullanımı
4. Query performansı

## 📈 Performans Optimizasyonu

### Backend
- Async/await kullanımı
- Caching stratejileri
- Query optimizasyonu
- Connection pooling

### Frontend
- Bundle ve minify
- Lazy loading
- Image optimization
- Browser caching

### Veritabanı
- Index oluşturma
- Query optimizasyonu
- Partitioning
- Maintenance planları

## 🔄 Bakım ve Güncelleme

### Güvenlik Güncellemeleri
1. NuGet paket güncellemeleri
2. Güvenlik yamaları
3. Dependency scanning
4. Vulnerability testing

### Veritabanı Bakımı
1. Yedekleme stratejisi
2. Index maintenance
3. Statistics güncelleme
4. Log yönetimi

### Monitoring
1. Application insights
2. Error logging
3. Performance monitoring
4. User analytics

## 👥 Katkıda Bulunma
1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'feat: Add amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 🚀 Deploy (Docker ile Uygulama ve Veritabanı Kurulumu)

Bu bölümde, uygulamanın Docker ile nasıl çalıştırılacağı ve veritabanı bağlantısının nasıl sağlanacağı adım adım anlatılmaktadır.  
Aşağıdaki adımlar, hem kendi bilgisayarınızda SQL Server ile hem de Docker içinde SQL Server ile çalışmak isteyenler için uygundur.

---

### 1. **Docker Kurulumu**

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) uygulamasını indirip kurun.
- Kurulumdan sonra Docker'ın çalıştığından emin olun.

---

### 2. **Seçenek 1: Host Makinedeki SQL Server'a Bağlanmak**

#### a) **SQL Server Ayarları**
- SQL Server'ın kurulu ve çalışır durumda olduğundan emin olun.
- **SQL Server Configuration Manager** ile:
  - `SQL Server Network Configuration > Protocols for [InstanceAdı] > TCP/IP` > **Enabled** olmalı.
  - `TCP/IP > Properties > IPAll > TCP Port` kısmında **1433** yazmalı.
- **SQL Server'ı yeniden başlatın.**
- **Windows Güvenlik Duvarı** üzerinden 1433 portunu açın.

#### b) **SQL Kullanıcısı Oluşturma**
```sql
CREATE LOGIN testuser WITH PASSWORD = 'Docker123!';
USE BirSiberDB;
CREATE USER testuser FOR LOGIN testuser;
ALTER ROLE db_owner ADD MEMBER testuser;
```

#### c) **Connection String**
Uygulamanın connection string'i şu şekilde olmalı:
```
Server=host.docker.internal,1433;Database=BirSiberDB;User Id=testuser;Password=Docker123!;
```

#### d) **Docker Container'ını Çalıştırma**
```sh
docker run -d -e "ConnectionStrings__DefaultConnection=Server=host.docker.internal,1433;Database=BirSiberDB;User Id=testuser;Password=Docker123!;" -p 8080:8080 --name birsiber yasinseyhun01/birsiber:latest
```

#### e) **Sorun Giderme**
- Eğer bağlantı hatası alırsanız:
  - SQL Server'ın 1433 portunda dinlediğinden ve TCP/IP'nin açık olduğundan emin olun.
  - Firewall'da 1433 portunun açık olduğundan emin olun.
  - `Test-NetConnection -ComputerName localhost -Port 1433` ile portun açık olup olmadığını kontrol edin.

---

### 3. **Seçenek 2: Docker İçinde SQL Server ile Tamamen İzole Çalışmak (Tavsiye Edilen)**

#### a) **docker-compose.yml Dosyası Oluşturun**
Proje dizininize aşağıdaki içeriğe sahip bir `docker-compose.yml` dosyası ekleyin:

```yaml
version: '3.8'
services:
  db:
    image: mcr.microsoft.com/mssql/server:2019-latest
    container_name: sqlserver
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Docker123!
    ports:
      - "1433:1433"
    networks:
      - appnet

  web:
    image: yasinseyhun01/birsiber:latest
    container_name: birsiber
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqlserver,1433;Database=BirSiberDB;User Id=sa;Password=Docker123!;
    ports:
      - "8080:8080"
    depends_on:
      - db
    networks:
      - appnet

networks:
  appnet:
    driver: bridge
```

#### b) **Uygulama ve Veritabanını Başlatın**
```sh
docker compose up -d
```

#### c) **İlk Kurulum Sonrası**
- SQL Server container'ı ilk açıldığında veritabanı boş olur. Migration veya SQL script ile veritabanı şemasını oluşturmanız gerekir.
- Gerekirse migration komutlarını veya SQL scriptlerini çalıştırın.

#### d) **Logları Takip Etmek**
```sh
docker logs -f birsiber
```

#### e) **Durdurmak için**
```sh
docker compose down
```

---

### 4. **Genel Notlar ve İpuçları**

- **Connection string'deki kullanıcı adı ve şifreyi kendi belirlediğiniz değerlere göre güncelleyebilirsiniz.
- **SQL Server container'ı** ile çalışırken, uygulamanın connection string'inde `Server=sqlserver,1433` kullanılır (compose ile aynı network'te).
- **Host makinedeki SQL Server ile çalışırken** ise `Server=host.docker.internal,1433` kullanılır.
- **Veritabanı migration** işlemlerini unutmayın! (EF Core kullanıyorsanız: `dotnet ef database update` veya uygulama ilk açılışta otomatik migration.)

---

### 5. **Sorun Giderme**

- **Bağlantı hatası alırsanız:**
  - SQL Server'ın çalıştığından ve doğru portta dinlediğinden emin olun.
  - Firewall ve TCP/IP ayarlarını kontrol edin.
  - Gerekirse SQL Server'ı Docker ile çalıştırın.
- **Logları inceleyin:**  
  ```sh
  docker logs -f birsiber
  ```

---

> **Not:**  
> Docker ile hem uygulamanızı hem de veritabanınızı izole ve taşınabilir şekilde çalıştırmak, geliştirme ve test süreçlerinde büyük kolaylık sağlar.

---

**Herhangi bir adımda sorun yaşarsanız, logları ve hata mesajlarını inceleyerek yukarıdaki adımları tekrar gözden geçirin.  
Geliştirici topluluğundan veya dökümantasyondan destek alabilirsiniz.**
