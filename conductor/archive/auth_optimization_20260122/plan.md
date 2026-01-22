# 📋 Implementation Plan: Login, Şifre Yönetimi ve Optimizasyon

## Faz 1: Kimlik Doğrulama Altyapısı (Infrastructure) [checkpoint: 3820b19]
- [x] Task: Veritabanı Şeması Güncelleme [97e59d0]
    - [x] Settings tablosuna AdminPasswordHash ve AdminPasswordSalt ekle.
    - [x] Varsayılan şifreyi (admin) oluştur.
- [x] Task: Kimlik Doğrulama Servisi (AuthService) [a6e87b4]
    - [x] IAuthService oluştur ve hashleme mantığını yaz.
- [x] Task: AuthService Birim Testleri (TDD) [a6e87b4]
- [x] Task: Conductor - User Manual Verification 'Kimlik Doğrulama Altyapısı' [a6e87b4]

## Faz 2: Giriş Ekranı (Login UI) [checkpoint: ae53599]
- [x] Task: Görsel Varlıkların Üretimi (Nano Banana) [a6e87b4]
- [x] Task: LoginViewModel ve LoginWindow Geliştirme [a6e87b4]
- [x] Task: Login UI Entegrasyon Testleri [a6e87b4]
- [x] Task: Conductor - User Manual Verification 'Giriş Ekranı' [a6e87b4]

## Faz 3: Şifre Yönetimi Sayfası [checkpoint: 06f407f]

- [x] Task: PasswordChangeViewModel ve Görünüm Oluşturma [da3e039]

- [x] Task: Navigasyon Güncellemesi [da3e039]

- [x] Task: Şifre Değiştirme Testleri [da3e039]

- [x] Task: Conductor - User Manual Verification 'Şifre Yönetimi Sayfası' [06f407f]



## Faz 4: Tema Yönetimi (Dark Mode) [checkpoint: 51f1071]



- [x] Task: ThemeService ve Altyapı [0a1561a]



    - [x] IThemeService oluştur.



    - [x] Sistem temasını algılama.



    - [x] Ayarları kaydetme.



- [x] Task: Ayarlar Sayfası Güncellemesi [0a1561a]



    - [x] Koyu/Açık/Sistem seçeneği ekle.



- [x] Task: UI Kontrolleri ve Stil Uyarlaması [0a1561a]



    - [x] DataGrid ve Butonların koyu modda görünürlüğünü test et.



- [x] Task: Conductor - User Manual Verification 'Tema Yönetimi' [51f1071]







## Faz 5: Performans Analizi ve Optimizasyon [checkpoint: e0b2d98]







- [x] Task: Bellek Kullanım Analizi (Profilleme) [e0b2d98]







    - [x] Transient View/ViewModel köklenmesi (rooting) sorunu tespit edildi.







- [x] Task: View ve ViewModel Temizliği (Optimization) [e0b2d98]







    - [x] Ana görünümler Singleton'a çekildi.







    - [x] IRefreshable ile navigasyon bazlı yenileme eklendi.







- [x] Task: Stabilite ve RAM Testi







    - [x] Kullanıcı testi: 110 MB seviyesinde stabilizasyon (İlk yüklemede artış, sonrası sabit).







- [ ] Task: Conductor - User Manual Verification 'Performans ve Optimizasyon' (Protocol in workflow.md)




















