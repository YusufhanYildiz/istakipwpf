# 📋 Implementation Plan: İkon Tasarımı, Paketleme ve Velopack Güncelleme Sistemi

## Faz 1: Görsel Tasarım ve Markalama (Assets & Branding)
- [x] Task: Nano Banana ile Logo ve İkon Üretimi
    - [x] Müşteri ilişkileri ve iş takibini temsil eden modern logo tasarımı.
    - [x] Yüksek çözünürlüklü PNG çıktısının alınması.
- [x] Task: İkon Setinin Hazırlanması
    - [x] PNG görselinin standart Windows ICO formatına dönüştürülmesi.
    - [x] İkonun proje dizinine (Resources) eklenmesi.
- [x] Task: Proje İkon Entegrasyonu
    - [x] Visual Studio proje özelliklerinde 'Application Icon' güncellenmesi.
    - [x] MainWindow ve LoginWindow ikonlarının set edilmesi.
- [ ] Task: Conductor - User Manual Verification 'Görsel Tasarım ve Markalama' (Protocol in workflow.md)

## Faz 2: Velopack Altyapısı ve Güncelleme Kontrolü
- [x] Task: Velopack Kütüphanesinin Entegrasyonu
    - [x] `Velopack` NuGet paketinin projeye eklenmesi.
    - [x] Uygulama başlangıç akışının (`VelopackApp.Build().Run()`) düzenlenmesi.
- [x] Task: UpdateManager Servisi Geliştirme
    - [x] Sürüm kontrolü, indirme ve yükleme adımlarını yöneten `IUpdateService` yazımı.
    - [x] "İndirme tamamlandı, yüklemeye hazır" durum bilgisinin ViewModel'e aktarılması.
- [x] Task: Ayarlar Ekranı Güncellemesi (UI)
    - [x] "Güncellemeleri Denetle" butonu ve durum göstergeleri (ProgressBar, Text).
    - [x] "Şimdi Yükle ve Yeniden Başlat" butonu (Yalnızca indirme bitince görünür).
- [ ] Task: Conductor - User Manual Verification 'Güncelleme Kontrolü' (Protocol in workflow.md)

## Faz 3: Paketleme ve Dağıtım (Packaging & Release)
- [ ] Task: Velopack Build Sürecinin Yapılandırılması
    - [ ] Proje sürüm numarasının (v1.0.0) ayarlanması.
    - [ ] Velopack paketleme komutlarının (CLI) hazırlanması.
- [ ] Task: İlk Release Paketinin Oluşturulması
    - [ ] v1.0.0 Setup.exe ve nupkg dosyalarının üretilmesi.
- [ ] Task: Kurulum ve Manuel Güncelleme Akış Testi
    - [ ] v1.0.1 paketi oluşturularak güncelleme akışının (Denetle -> İndir -> Yükle) doğrulanması.
- [ ] Task: Conductor - User Manual Verification 'Paketleme ve Dağıtım' (Protocol in workflow.md)
