# 📋 Implementation Plan: İl ve İlçe Yönetimi

## Faz 1: Altyapı ve Veritabanı [checkpoint: current]
- [x] Task: LocationService ve Veri Setinin Oluşturulması
    - [x] 81 İl ve İlçe verisini içeren JSON veya Static Data sınıfı.
    - [x] ILocationService ve LocationService implementasyonu.
- [x] Task: Veritabanı Migrasyonu
    - [x] DatabaseBootstrap güncellemesi (Customers tablosuna City ve District sütunlarını ekle).
    - [x] Customer modelini güncelle.

## Faz 2: Müşteri Yönetimi Entegrasyonu
- [x] Task: CustomerRepository ve Service Güncellemesi
    - [x] CRUD işlemlerine City ve District alanlarını dahil et.
- [x] Task: Müşteri Ekle/Düzenle Ekranı (UI & VM)
    - [x] ViewModel'de İl/İlçe seçim mantığı (Cascading).
    - [x] Dialog tasarımına ComboBox'ların eklenmesi.
- [x] Task: Müşteri Listesi (UI & VM)
    - [x] DataGrid'e sütunların eklenmesi.
    - [x] Filtreleme alanlarının (ComboBox) eklenmesi ve backend mantığı.

## Faz 3: İş Takibi Entegrasyonu
- [x] Task: JobRepository Güncellemesi
    - [x] İşleri çekerken Müşteri tablosundan City/District bilgisini Join ile al.
    - [x] Job modeline (veya DTO) bu alanları ekle.
- [x] Task: İş Listesi (UI & VM)
    - [x] DataGrid'e sütunların eklenmesi.
    - [x] Filtreleme alanlarının eklenmesi.

## Faz 4: Doğrulama
- [x] Task: Manuel Test ve Kontrol
    - [x] Veri tutarlılığı, filtreleme doğruluğu ve UI testi.
