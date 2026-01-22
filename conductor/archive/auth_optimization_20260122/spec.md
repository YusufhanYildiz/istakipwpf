# 📝 Specification: Giriş Sistemi, Şifre Yönetimi ve Performans Optimizasyonu

## 1. Genel Bakış (Overview)
Bu track, uygulamanın güvenliğini sağlamak için bir giriş sistemi eklemeyi, kullanıcıya şifre yönetimi imkanı tanımayı ve raporlanan bellek (RAM) artış sorunlarını analiz ederek optimize etmeyi amaçlar.

## 2. Fonksiyonel Gereksinimler (Functional Requirements)

### 2.1. Giriş Sistemi (Login)
- **Bağımsız Giriş Penceresi:** Uygulama açıldığında ana pencereden önce küçük, şık bir giriş penceresi açılacaktır.
- **Admin Yetkilendirmesi:** Tek bir ana kullanıcı (Admin) hesabı desteklenecektir.
- **Bileşenler:**
    - Kullanıcı adı ve şifre giriş alanları.
    - **Beni Hatırla:** Giriş bilgilerinin yerel olarak güvenli saklanması.
    - **Şifre Görünürlüğü:** Şifreyi göster/gizle ikonu.
    - **Görsel Tasarım:** Nano Banana ile oluşturulmuş profesyonel bir logo/görsel.
- **Doğrulama:** Hatalı girişlerde kullanıcıya Snackbar veya hata mesajı ile bilgi verilecektir.

### 2.2. Şifre Yönetimi
- **Yeni Sayfa:** Sol menüye ""Şifre Değiştir"" adında yeni bir sekme eklenecektir.
- **İşlem:** Mevcut şifre, yeni şifre ve yeni şifre tekrarı alanları ile güvenli şifre güncelleme sağlanacaktır.
- **Kalıcılık:** Yeni şifre SQLite veritabanında (hashlenmiş olarak) saklanacaktır.

### 2.3. Performans ve Optimizasyon
- **Bellek Analizi:** Programın başlangıçtaki 80 MB'lık kullanımının ardından neden sürekli arttığı analiz edilecektir.
- **Sızıntı Giderimi:** Sayfa geçişlerinde (Navigation) temizlenmeyen nesneler ve arka plan servisleri optimize edilecektir.
- **Kararlılık:** Uygulamanın uzun süreli kullanımda stabil bir RAM tüketiminde kalması sağlanacaktır.

## 3. Kabul Kriterleri (Acceptance Criteria)
- Uygulama giriş yapılmadan ana ekrana geçişe izin vermemelidir.
- Şifre değiştirme işlemi başarıyla tamamlanmalı ve yeni şifre bir sonraki girişte geçerli olmalıdır.
- ""Beni Hatırla"" seçeneği işaretlendiğinde uygulama kapatılıp açılınca giriş bilgileri hazır gelmelidir.
- RAM tüketimi, veri girişi olmayan boşta bekleme sürelerinde sürekli artış göstermemelidir.

## 4. Kapsam Dışı (Out of Scope)
- Çoklu kullanıcı rolleri ve yetkilendirme seviyeleri.
- E-posta yoluyla şifre sıfırlama.