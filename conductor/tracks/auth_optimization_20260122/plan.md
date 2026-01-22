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

## Faz 3: Şifre Yönetimi Sayfası
- [~] Task: PasswordChangeViewModel ve Görünüm Oluşturma
- [ ] Task: Navigasyon Güncellemesi
- [ ] Task: Şifre Değiştirme Testleri
- [ ] Task: Conductor - User Manual Verification 'Şifre Yönetimi Sayfası' (Protocol in workflow.md)

## Faz 4: Performans Analizi ve Optimizasyon
- [ ] Task: Bellek Kullanım Analizi (Profilleme)
- [ ] Task: View ve ViewModel Temizliği (Optimization)
- [ ] Task: Stabilite ve RAM Testi
- [ ] Task: Conductor - User Manual Verification 'Performans ve Optimizasyon' (Protocol in workflow.md)