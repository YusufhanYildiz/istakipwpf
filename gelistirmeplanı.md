# 🎯 Müşteri ve İş Takip Sistemi - Geliştirme Planı

## Proje Özeti
WPF tabanlı, modern ve minimalist bir müşteri yönetimi ve iş takip sistemi geliştiriyoruz. Sistem, müşteri bilgilerini kaydetme, iş süreçlerini takip etme, raporlama ve yedekleme özelliklerini içerecek.

---

## 🛠️ Teknoloji Stack

| Bileşen | Teknoloji |
|---------|-----------|
| Framework | .NET Framework 4.8 |
| UI | WPF + MaterialDesignInXAML |
| Veritabanı | SQLite |
| ORM | Dapper |
| Excel İşlemleri | ClosedXML |
| PDF İşlemleri | iTextSharp |
| DI Container | Microsoft.Extensions.DependencyInjection |
| MVVM | CommunityToolkit.Mvvm |

---

## 📁 Proje Yapısı

```
IsTakipWPF/
├── App.xaml
├── MainWindow.xaml
├── Models/
│   ├── Musteri.cs
│   ├── IsTakip.cs
│   └── YedeklemeAyarlari.cs
├── Data/
│   ├── DatabaseContext.cs
│   ├── Repositories/
│   │   ├── IMusteriRepository.cs
│   │   ├── MusteriRepository.cs
│   │   ├── IIsTakipRepository.cs
│   │   └── IsTakipRepository.cs
├── Services/
│   ├── IExcelService.cs
│   ├── ExcelService.cs
│   ├── IPdfService.cs
│   ├── PdfService.cs
│   ├── IYedeklemeService.cs
│   └── YedeklemeService.cs
├── ViewModels/
│   ├── MainViewModel.cs
│   ├── MusteriViewModel.cs
│   ├── IsTakipViewModel.cs
│   └── YedeklemeViewModel.cs
├── Views/
│   ├── MusteriPage.xaml
│   ├── IsTakipPage.xaml
│   └── YedeklemePage.xaml
├── Converters/
│   └── StatusToColorConverter.cs
└── Resources/
    └── Styles.xaml
```

---

## 📊 Veritabanı Şeması

### Musteriler Tablosu
```sql
CREATE TABLE Musteriler (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Ad NVARCHAR(100) NOT NULL,
    Soyad NVARCHAR(100) NOT NULL,
    Telefon NVARCHAR(20),
    Adres NVARCHAR(500),
    OlusturmaTarihi DATETIME DEFAULT CURRENT_TIMESTAMP,
    GuncellemeTarihi DATETIME
);
```

### IsTakip Tablosu
```sql
CREATE TABLE IsTakip (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MusteriId INTEGER NOT NULL,
    IsAciklamasi NVARCHAR(1000),
    Durum NVARCHAR(50) DEFAULT 'Bekliyor',
    BaslangicTarihi DATETIME,
    BitisTarihi DATETIME,
    Notlar NVARCHAR(2000),
    OlusturmaTarihi DATETIME DEFAULT CURRENT_TIMESTAMP,
    GuncellemeTarihi DATETIME,
    FOREIGN KEY (MusteriId) REFERENCES Musteriler(Id)
);
```

### YedeklemeAyarlari Tablosu
```sql
CREATE TABLE YedeklemeAyarlari (
    Id INTEGER PRIMARY KEY,
    OtomatikYedekleme INTEGER DEFAULT 0,
    YedeklemeSikligi NVARCHAR(50),
    YedeklemeDizini NVARCHAR(500),
    CikistaSor INTEGER DEFAULT 1,
    SonYedeklemeTarihi DATETIME
);
```

---

## 🎨 UI/UX Gereksinimleri

### Genel Tasarım
- MaterialDesign tema kullanılacak (Dark veya Light seçilebilir)
- Sol tarafta Navigation Drawer ile sayfa geçişleri
- Minimalist ve modern görünüm
- Responsive tasarım (pencere boyutuna uyum)

### Müşteriler Sayfası
- DataGrid ile müşteri listesi
- Arama/Filtreleme alanı (isim, soyisim, telefon)
- Ekleme/Düzenleme için Dialog veya Side Panel
- Silme onayı için Dialog
- Excel'den içe aktarma butonu
- Excel'e dışa aktarma butonu
- Toplu silme özelliği

### İş Takip Sayfası
- DataGrid ile iş listesi
- Gelişmiş filtreleme:
  - Müşteri adına göre (ComboBox veya AutoComplete)
  - Tarihe göre (DatePicker aralığı)
  - Duruma göre (Bekliyor, Devam Ediyor, Tamamlandı, İptal)
- Durum değiştirme için inline ComboBox veya Chip
- Renk kodlaması (durumlara göre satır renklendirme)
- PDF'e aktarma (filtreli veya tüm kayıtlar)
- Excel'e aktarma
- Excel'den içe aktarma

### Yedekleme Sayfası
- Manuel yedekleme butonu
- Yedekleme geçmişi listesi
- Otomatik yedekleme ayarları:
  - Açık/Kapalı toggle
  - Sıklık seçimi (Günlük, Haftalık, Her açılışta)
  - Yedekleme dizini seçimi
- "Çıkışta otomatik kaydet" checkbox
- Yedekten geri yükleme özelliği

---

## 🔧 Modül Detayları

### 1. Müşteri Modülü (CRUD)

**Model - Musteri.cs:**
```csharp
public class Musteri
{
    public int Id { get; set; }
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public string Telefon { get; set; }
    public string Adres { get; set; }
    public DateTime OlusturmaTarihi { get; set; }
    public DateTime? GuncellemeTarihi { get; set; }
    
    public string TamAd => $"{Ad} {Soyad}";
}
```

**Repository İşlemleri:**
- `GetAllAsync()` - Tüm müşterileri getir
- `GetByIdAsync(int id)` - ID ile müşteri getir
- `AddAsync(Musteri musteri)` - Yeni müşteri ekle
- `UpdateAsync(Musteri musteri)` - Müşteri güncelle
- `DeleteAsync(int id)` - Müşteri sil
- `SearchAsync(string searchTerm)` - Müşteri ara

### 2. İş Takip Modülü

**Model - IsTakip.cs:**
```csharp
public class IsTakip
{
    public int Id { get; set; }
    public int MusteriId { get; set; }
    public string IsAciklamasi { get; set; }
    public string Durum { get; set; } // Bekliyor, Devam Ediyor, Tamamlandı, İptal
    public DateTime? BaslangicTarihi { get; set; }
    public DateTime? BitisTarihi { get; set; }
    public string Notlar { get; set; }
    public DateTime OlusturmaTarihi { get; set; }
    public DateTime? GuncellemeTarihi { get; set; }
    
    // Navigation Property
    public Musteri Musteri { get; set; }
}
```

**Durum Seçenekleri:**
- ⏳ Bekliyor (Sarı)
- 🔄 Devam Ediyor (Mavi)
- ✅ Tamamlandı (Yeşil)
- ❌ İptal (Kırmızı)

**Repository İşlemleri:**
- `GetAllWithMusteriAsync()` - İşleri müşteri bilgisiyle getir
- `GetByMusteriIdAsync(int musteriId)` - Müşteriye göre işler
- `GetByDateRangeAsync(DateTime start, DateTime end)` - Tarih aralığına göre
- `GetByDurumAsync(string durum)` - Duruma göre filtrele
- `AddAsync(IsTakip is)` - Yeni iş ekle
- `UpdateAsync(IsTakip is)` - İş güncelle
- `UpdateDurumAsync(int id, string durum)` - Sadece durum güncelle
- `DeleteAsync(int id)` - İş sil

### 3. Excel Servisi

**IExcelService:**
```csharp
public interface IExcelService
{
    Task<List<Musteri>> ImportMusterilerAsync(string filePath);
    Task ExportMusterilerAsync(List<Musteri> musteriler, string filePath);
    Task<List<IsTakip>> ImportIsTakipAsync(string filePath);
    Task ExportIsTakipAsync(List<IsTakip> isler, string filePath);
}
```

### 4. PDF Servisi

**IPdfService:**
```csharp
public interface IPdfService
{
    Task ExportIsTakipToPdfAsync(List<IsTakip> isler, string filePath, string baslik);
    Task ExportMusterilerToPdfAsync(List<Musteri> musteriler, string filePath);
}
```

### 5. Yedekleme Servisi

**IYedeklemeService:**
```csharp
public interface IYedeklemeService
{
    Task<bool> CreateBackupAsync(string targetPath);
    Task<bool> RestoreBackupAsync(string backupPath);
    Task<List<BackupInfo>> GetBackupHistoryAsync();
    Task SaveSettingsAsync(YedeklemeAyarlari ayarlar);
    Task<YedeklemeAyarlari> GetSettingsAsync();
    Task DeleteOldBackupsAsync(int keepCount);
}
```

---

## 📋 Geliştirme Aşamaları

### Faz 1: Proje Altyapısı
1. WPF projesi oluştur (.NET Framework 4.8)
2. NuGet paketlerini yükle:
   - MaterialDesignThemes
   - System.Data.SQLite
   - Dapper
   - ClosedXML
   - QuestPDF veya iTextSharp
   - CommunityToolkit.Mvvm
   - Microsoft.Extensions.DependencyInjection
3. Proje klasör yapısını oluştur
4. SQLite veritabanını ve tabloları oluştur
5. DI Container konfigürasyonu
6. MVVM altyapısını kur

### Faz 2: Müşteri Modülü
1. Musteri modelini oluştur
2. IMusteriRepository ve MusteriRepository yaz
3. MusteriViewModel oluştur
4. MusteriPage.xaml tasarla
5. CRUD işlemlerini implement et
6. Excel import/export özelliğini ekle

### Faz 3: İş Takip Modülü
1. IsTakip modelini oluştur
2. IIsTakipRepository ve IsTakipRepository yaz
3. IsTakipViewModel oluştur
4. IsTakipPage.xaml tasarla
5. CRUD işlemlerini implement et
6. Filtreleme özelliklerini ekle
7. Durum değiştirme özelliğini ekle
8. PDF ve Excel export özelliklerini ekle
9. Excel import özelliğini ekle

### Faz 4: Yedekleme Modülü
1. YedeklemeAyarlari modelini oluştur
2. IYedeklemeService ve YedeklemeService yaz
3. YedeklemeViewModel oluştur
4. YedeklemePage.xaml tasarla
5. Manuel yedekleme özelliğini implement et
6. Otomatik yedekleme scheduler'ı ekle
7. Geri yükleme özelliğini ekle
8. Çıkışta yedekleme sorgusu ekle

### Faz 5: Ana Pencere ve Navigasyon
1. MainWindow.xaml'ı tasarla
2. Navigation Drawer ekle
3. Sayfa geçişlerini ayarla
4. Tema değiştirme özelliğini ekle

### Faz 6: Son Rötuşlar
1. Hata yönetimi ve loglama
2. Input validasyonu
3. Loading göstergeleri
4. Bildirim sistemi (Snackbar)
5. Performans optimizasyonu
6. Test ve bug fix

---

## 🚀 Başlangıç Komutu

Bu planı Gemini CLI Conductor ile çalıştırmak için:

```
Bu planda belirtilen Müşteri ve İş Takip Sistemini geliştir. 
Faz 1'den başlayarak sırasıyla tüm fazları tamamla.
Her modül için:
1. Önce modelleri oluştur
2. Repository'leri yaz (Dapper ile)
3. ViewModel'leri oluştur (CommunityToolkit.Mvvm kullanarak)
4. XAML sayfalarını tasarla (MaterialDesign kullanarak)
5. Tüm CRUD işlemlerini implement et

MaterialDesign tema kurulumunu ve DI container yapılandırmasını unutma.
Her aşamada kodun çalıştığından emin ol.
```

---

## ⚠️ Önemli Notlar

- Tüm async metotlar için proper exception handling yapılmalı
- SQLite bağlantı string'i App.config'de saklanmalı
- Yedekleme dosyaları tarih damgası ile isimlendirilmeli
- Excel şablonları kullanıcı dostu olmalı (başlık satırları vs.)
- PDF raporları profesyonel görünümde olmalı
- Türkçe karakter desteği tüm import/export işlemlerinde sağlanmalı
