# 📝 Specification: İl ve İlçe Yönetimi Entegrasyonu

## 1. Genel Bakış
Uygulamaya müşterilerin adres detaylarını standartlaştırmak ve raporlamayı kolaylaştırmak amacıyla İl ve İlçe seçimi eklenecektir. Bu özellik hem Müşteri hem de İş Takip modüllerinde aktif olacaktır.

## 2. Fonksiyonel Gereksinimler

### 2.1. Veri Kaynağı (Location Service)
- Türkiye'nin 81 ili ve ilçeleri sisteme dahil edilecek.
- Veriler çevrimdışı (offline) çalışacak.
- Bir il seçildiğinde, ilçe listesi otomatik olarak o ile ait ilçelerle güncellenecek (Cascading Dropdown).

### 2.2. Müşteri Yönetimi
- **Veritabanı:** `Customers` tablosuna `City` ve `District` sütunları eklenecek.
- **Ekle/Düzenle:** Adres girilirken manuel yazım yerine ComboBox ile seçim yapılacak.
- **Liste:** Müşteri listesinde İl ve İlçe sütunları görülecek.
- **Filtreleme:** Listede İl ve İlçe bazlı arama yapılabilecek.

### 2.3. İş Takibi Entegrasyonu
- **Görüntüleme:** İş listesinde, işin ait olduğu müşterinin İl ve İlçe bilgisi görülecek.
- **Filtreleme:** İşler, müşterinin bulunduğu konuma göre (Örn: "Ankara'daki işler") filtrelenebilecek.

## 3. Teknik Gereksinimler
- Mevcut veritabanı yapısı korunarak yeni sütunlar "Migration" mantığıyla eklenecek (Veri kaybı olmadan).
- UI bileşenleri Material Design standartlarına uygun olacak.
