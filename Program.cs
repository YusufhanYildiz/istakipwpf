using System;
using Velopack;

namespace IsTakipWpf
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // Velopack: Uygulama olaylarını yönet
                VelopackApp.Build()
                    .OnBeforeUninstallFastCallback(onAppUninstall)
                    .Run();

                // WPF Uygulamasını başlat
                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception ex)
            {
                // Kritik hata loglama
                System.Windows.MessageBox.Show("Uygulama başlatılamadı: " + ex.Message);
            }
        }

        private static void onAppUninstall(NuGet.Versioning.SemanticVersion version)
        {
            if (!Infrastructure.DatabaseBootstrap.IsPortable)
            {
                var result = System.Windows.MessageBox.Show(
                    "Uygulama kaldırılıyor. Kayıtlı tüm verileriniz ve yedekleriniz kalıcı olarak silinsin mi?",
                    "Verileri Temizle",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    try
                    {
                        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        string appDataRoot = System.IO.Path.Combine(localAppData, "IsTakipApp");

                        if (System.IO.Directory.Exists(appDataRoot))
                        {
                            System.IO.Directory.Delete(appDataRoot, true);
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
