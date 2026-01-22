# 📝 Specification: Özel İkon Tasarımı, Paketleme ve Velopack Güncelleme Sistemi

## 1. Genel Bakış (Overview)
Bu track, uygulamanın kurumsal kimliğini tamamlamak için Nano Banana ile özel bir ikon seti oluşturulmasını, uygulamanın Velopack kütüphanesi kullanılarak profesyonel bir şekilde paketlenmesini ve kullanıcı kontrollü otomatik güncelleme altyapısının kurulmasını kapsar.

## 2. Fonksiyonel Gereksinimler (Functional Requirements)

### 2.1. Görsel Kimlik ve İkon Tasarımı
- **Konsept:** Müşteri ilişkilerini (CRM) ve modern iş disiplinini temsil eden soyut bir tasarım.
- **Stil:** Material Design standartlarına uygun, derinlikli ve gölgeli (Modern & Professional).
- **Üretim:** Nano Banana MCP kullanılarak yüksek çözünürlüklü ikonlar üretilecek.
- **Entegrasyon:** Üretilen ikon .ico formatına dönüştürülerek Taskbar, Pencere başlığı, Masaüstü kısayolu ve Setup dosyası ikonu olarak kullanılacak.

### 2.2. Paketleme (Packaging)
- **Altyapı:** Velopack kütüphanesi kullanılacak.
- **Kurulum:** Tek tıkla kurulum sağlayan modern bir Setup exe'si oluşturulacak.
- **Kısayollar:** Kurulum sonrası Masaüstü ve Başlat menüsüne otomatik kısayol eklenecek.

### 2.3. Güncelleme Sistemi (Auto-Update)
- **Entegrasyon:** Velopack altyapısı kod seviyesinde entegre edilecek.
- **Kullanıcı Kontrolü:** Güncellemeler arka planda indirilebilir ancak kurulum işlemi kullanıcının Ayarlar menüsünden tetikleyeceği bir butonla ("Şimdi Yükle ve Yeniden Başlat") gerçekleştirilecek.
- **Görüntüleme:** Ayarlar sayfasında mevcut sürüm bilgisi ve güncelleme durumu (ProgressBar, durum metni) yer alacak.

## 3. Kabul Kriterleri (Acceptance Criteria)
- Uygulama ikonu tüm platform noktalarında tutarlı bir şekilde görülmeli.
- Velopack ile başarılı bir şekilde "Release" paketi oluşturulabilmeli.
- Kullanıcı manuel olarak güncellemeleri denetleyebilmeli ve yüklemeyi başlatabilmeli.
- Setup dosyası uygulamayı hatasız kurmalı ve güncellemeler versiyon geçişlerini (v1.0.0 -> v1.0.1) desteklemeli.

## 4. Kapsam Dışı (Out of Scope)
- Uzak sunucu (CDN) barındırma maliyetleri veya yönetimi (Sadece yerel veya test URL altyapısı hazır edilecek).
