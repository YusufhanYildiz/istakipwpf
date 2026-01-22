# 📋 Implementation Plan: Login, Şifre Yönetimi ve Optimizasyon

## Faz 1: Kimlik Doğrulama Altyapısı (Infrastructure)
- [ ] Task: Veritabanı Şeması Güncelleme
    - [ ] Settings tablosuna AdminPasswordHash ve AdminPasswordSalt ekle.
    - [ ] Varsayılan şifreyi (admin) oluştur.
- [x] Task: Kimlik Doğrulama Servisi (AuthService)
    - [ ] IAuthService oluştur ve hashleme mantığını yaz.
- [x] Task: AuthService Birim Testleri (TDD)
- [ ] Task: Conductor - User Manual Verification 'Kimlik Doğrulama Altyapısı' (Protocol in workflow.md)

## Faz 2: Giriş Ekranı (Login UI)
- [ ] Task: Görsel Varlıkların Üretimi (Nano Banana)
- [ ] Task: LoginViewModel ve LoginWindow Geliştirme
- [ ] Task: Login UI Entegrasyon Testleri
- [ ] Task: Conductor - User Manual Verification 'Giriş Ekranı' (Protocol in workflow.md)

## Faz 3: Şifre Yönetimi Sayfası
- [ ] Task: PasswordChangeViewModel ve Görünüm Oluşturma
- [ ] Task: Navigasyon Güncellemesi
- [ ] Task: Şifre Değiştirme Testleri
- [ ] Task: Conductor - User Manual Verification 'Şifre Yönetimi Sayfası' (Protocol in workflow.md)

## Faz 4: Performans Analizi ve Optimizasyon
- [ ] Task: Bellek Kullanım Analizi (Profilleme)
- [ ] Task: View ve ViewModel Temizliği (Optimization)
- [ ] Task: Stabilite ve RAM Testi
- [ ] Task: Conductor - User Manual Verification 'Performans ve Optimizasyon' (Protocol in workflow.md)