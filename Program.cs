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
                // Velopack: Uygulama başlangıcında en üstte olmalı
                VelopackApp.Build().Run();

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
    }
}
