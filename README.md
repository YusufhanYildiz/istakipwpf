# İş Takip Sistemi (WPF)

Müşteri yönetimi, iş takibi ve raporlama süreçlerini dijitalleştiren, gizlilik odaklı ve tamamen yerel (offline) çalışan bir masaüstü uygulaması.

![Logo](IsTakipWpf/logo.png)

## 🚀 Özellikler
- **Müşteri Yönetimi:** Detaylı müşteri kaydı, arama ve filtreleme.
- **İş Takibi:** Beklemede, Devam Ediyor, Tamamlandı statüleriyle iş lifecycle yönetimi.
- **Şehir/İlçe Entegrasyonu:** Türkiye geneli 81 il ve bağlı ilçelerle lokasyon bazlı takip.
- **Güvenli Kimlik Doğrulama:** Admin hesabı ile şifreli giriş sistemi.
- **Yedekleme & Geri Yükleme:** Veri kaybını önlemek için otomatik ve manuel yedekleme mekanizması.
- **Raporlama:** Excel import/export ve profesyonel PDF rapor çıktısı.
- **Modern UI:** Material Design standartlarında, koyu/açık tema destekli kullanıcı arayüzü.
- **Otomatik Güncelleme:** Velopack altyapısı ile kullanıcı kontrollü sürüm yönetimi.

## 🛠️ Teknoloji Yığını
- **Framework:** .NET Framework 4.8 / WPF
- **Veritabanı:** SQLite & Dapper (Micro-ORM)
- **UI:** Material Design In XAML Toolkit
- **Paketleme:** Velopack
- **Raporlama:** ClosedXML & iTextSharp

## 📦 Kurulum ve Çalıştırma
Uygulamanın en güncel sürümünü [Releases](../../releases) sayfasından `IsTakipWpfSetup.exe` dosyasını indirerek tek tıkla kurabilirsiniz.

Geliştirici modunda çalıştırmak için:
1. Projeyi klonlayın.
2. Visual Studio 2022 ile `.sln` dosyasını açın.
3. NuGet paketlerini geri yükleyin.
4. `F5` ile projeyi başlatın.

## 🔄 Güncelleme Yapısı
Uygulama, yeni bir sürüm yayınlandığında **Ayarlar > Uygulama Güncelleme** menüsü üzerinden sizi bilgilendirir. Güncellemeyi indirip tek tıkla kurulumu tamamlayabilirsiniz.

## 📝 Lisans
Bu proje eğitim ve kurumsal kullanım amacıyla geliştirilmiştir. Tüm hakları saklıdır.
