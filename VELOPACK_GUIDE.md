# Velopack & GitHub Release Yönetimi Rehberi

Bu rehber, **İş Takip Sistemi** uygulamasının Velopack kullanılarak nasıl paketleneceğini ve GitHub üzerinden nasıl otomatik güncelleme sunacağını açıklar.

## 1. Hazırlık
Uygulamanın paketlenmesi için `vpro` (Velopack CLI) aracına ihtiyacınız vardır. Eğer yüklü değilse şu komutla yükleyin:
```bash
dotnet tool install -g vpro
```

## 2. GitHub Yapılandırması
GitHub üzerinden güncelleme sunmak için:
1. Projenizi GitHub'a yükleyin.
2. `IsTakipWpf/Services/UpdateService.cs` dosyasındaki `UpdateUrl` değişkenini kendi repo adresinizle değiştirin:
   - Örn: `https://github.com/kullanici_adi/is-takip-wpf`

## 3. Paketleme Adımları (Release Oluşturma)

### Adım 3.1: Projeyi Yayınla (Publish)
Önce uygulamayı bir klasöre yayınlamamız gerekir:
```bash
dotnet publish IsTakipWpf/IsTakipWpf.csproj -c Release -r win-x86 --self-contained -o ./publish
```
*Not: Velopack genellikle `--self-contained` (tüm çalışma zamanını içeren) paketleri tercih eder.*

### Adım 3.2: Velopack Paketi Oluştur (Pack)
Yayınladığınız dosyaları Velopack formatına dönüştürün:
```bash
vpro pack --id IsTakipWpf --version 1.0.0 --packDir ./publish --mainExe IsTakipWpf.exe --icon IsTakipWpf/app_icon.ico
```
- `--id`: Uygulamanın benzersiz kimliği.
- `--version`: Uygulamanın sürümü (her yeni sürümde artırılmalıdır).
- `--packDir`: Publish klasörünün yolu.
- `--mainExe`: Ana çalıştırılabilir dosya adı.
- `--icon`: Setup ve uygulama ikonu.

Bu komut sonrası `./releases` klasöründe şunlar oluşacaktır:
- `IsTakipWpfSetup.exe` (Kurulum dosyası)
- `.nupkg` dosyaları (Güncelleme paketleri)
- `RELEASES` dosyası (Sürüm indeksi)

## 4. GitHub'a Yükleme
Güncellemelerin çalışması için:
1. GitHub reponuzda yeni bir **Release** oluşturun (Örn: `v1.0.0`).
2. `./releases` klasöründeki **tüm dosyaları** (Setup.exe, .nupkg ve RELEASES) bu release'e asset olarak yükleyin.
3. Release'i yayınlayın.

## 5. Yeni Sürüm Çıkma (v1.0.1)
Uygulamayı güncellediğinizde:
1. Proje dosyasındaki sürüm numarasını artırın.
2. Tekrar `dotnet publish` yapın.
3. `vpro pack` komutunu `--version 1.0.1` ile çalıştırın.
4. Yeni oluşan dosyaları GitHub'da yeni bir release (`v1.0.1`) olarak paylaşın.

## 6. Önemli Notlar
- **Geliştirme Ortamı:** Geliştirme (Debug) modunda çalışırken "Güncellemeleri Denetle" butonu hata verebilir veya "Güncel" diyebilir. Velopack tam olarak sadece kurulmuş (Installed) uygulamalarda aktif olur.
- **Dizin Yapısı:** Velopack, uygulamayı `%LocalAppData%/IsTakipWpf` klasörüne kurar.
