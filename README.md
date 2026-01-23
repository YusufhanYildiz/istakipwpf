# İş Takip ve Müşteri Yönetim Sistemi (WPF)

Modern, performanslı ve kullanıcı dostu bir Müşteri ve İş Takip çözümüdür. Bu uygulama, işletmelerin müşteri portföylerini yönetmelerini, iş süreçlerini takip etmelerini ve profesyonel raporlar oluşturmalarını sağlar.

## 🚀 Öne Çıkan Özellikler

### 👥 Müşteri Yönetimi
- Detaylı müşteri kayıtları (Ad, Soyad, Telefon, Adres, İl/İlçe).
- Gelişmiş arama ve lokasyon bazlı filtreleme.
- Excel üzerinden toplu müşteri içe/dışa aktarma.

### 💼 İş Takip Sistemi
- Müşterilere bağlı iş süreçleri oluşturma ve yönetme.
- İş durum takibi (Bekliyor, Devam Ediyor, Tamamlandı).
- Başlangıç ve bitiş tarihleri ile zaman yönetimi.
- Dinamik iş listesi arama ve filtreleme.

### 📊 Modern Raporlama ve Excel Entegrasyonu
- **QuestPDF Altyapısı:** Kurumsal logolu, modern ve şık PDF rapor çıktıları.
- **Yüksek Performanslı Excel:** Saniyeler içinde yüzlerce kaydı işleyebilen Transaction tabanlı içe aktarma sistemi.
- **Otomatik Rehber:** Ana panelde yer alan interaktif Excel hazırlama kılavuzu.

### 🛠️ Teknik Avantajlar
- **Performans:** SQLite veritabanı indeksleme ve toplu işlem (Transaction) desteği ile anlık tepki süresi.
- **Düşük Donanım Uyumluluğu:** UI Virtualization (Sanallaştırma) sayesinde binlerce kayıtta bile kasmadan çalışma.
- **Güvenlik:** Şifrelenmiş kimlik doğrulama ve "Beni Hatırla" özelliği.
- **Güncelleme:** Velopack entegrasyonu ile tek tıkla kurulum ve otomatik güncelleme desteği.

## 💻 Teknoloji Yığını

- **Framework:** .NET 4.8 / WPF
- **UI:** Material Design In XAML Toolkit
- **Veritabanı:** SQLite & Dapper (ORM)
- **Raporlama:** QuestPDF (PDF) & ClosedXML (Excel)
- **Kurulum:** Velopack

## 📦 Kurulum ve Yayınlama

Uygulamayı bir kurulum paketi (setup) haline getirmek için:

1. Uygulamayı yayınlayın:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained
   ```
2. Velopack ile paketleyin:
   ```bash
   vpk pack -u IsTakipWpf -v 1.0.0 -p bin\Release\net48\win-x64\publish -e IsTakipWpf.exe --icon app_icon.ico
   ```

## 📝 Kullanım Notları
- Excel aktarımlarında ilk satırın başlık olması zorunludur.
- Müşteri eşleştirmesi için Excel'deki isimlerin sistemdeki kayıtlarla birebir aynı olması gerekir.
- PDF raporlarında kurumsal logonuzun görünmesi için `logo.png` dosyasının uygulama dizininde yer alması önerilir.

---
*Bu proje modern yazılım mimarileri ve yüksek performans standartları gözetilerek geliştirilmiştir.*