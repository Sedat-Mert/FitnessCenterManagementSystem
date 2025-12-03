# FitnessCenterManagementSystem
Projenin amacı, spor salonlarında verilen hizmetlerin, antrenörlerin uzmanlık alanlarının, üyelerin randevularının ve yapay zekâ tabanlı egzersiz/diyet önerilerinin tek bir sistem üzerinden yönetilmesini sağlamaktır.

##Projenin Amacı

Spor salonu süreçlerinin dijital ortamda yönetilmesi
Üye – Antrenör – Hizmet arasındaki ilişkilerin yönetilmesi
Randevu alma süreçlerinin otomatik olarak kontrol edilmesi
Yapay zekâ kullanarak kişiye özel öneri oluşturulması
Modern web teknolojileri ile tam fonksiyonel bir MVC uygulaması geliştirme
##Kullanılan Teknolojiler

ASP.NET Core MVC (Güncel LTS)
C#
Entity Framework Core (ORM), LINQ
SQL Server / PostgreSQL
Bootstrap 5
HTML5, CSS3, JavaScript, jQuery
Yapay zekâ API entegrasyonu (OpenAI veya benzeri)
##Proje Özellikleri

Spor salonu tanımlama, hizmet ekleme ve ücret belirleme
Antrenör ekleme, uzmanlık alanı ve müsaitlik saatlerinin yönetilmesi
Rol bazlı yetkilendirme (Admin / Üye)
Üyelerin uygun antrenöre randevu alabilmesi
Randevu uygunluk kontrolü
REST API ile veri listeleme (ör. antrenör filtreleme, randevu sorgulama)
Yapay zekâ tabanlı egzersiz veya diyet öneri sistemi
##Klasör Yapısı (Örnek)

/Controllers – İş mantığının bulunduğu controller sınıfları
/Models – Veritabanı modelleri
/Views – Razor View dosyaları
/wwwroot – CSS, JS, resimler
appsettings.json – Veritabanı bağlantı ayarları
Program.cs / Startup.cs – Uygulama başlangıç ayarları
