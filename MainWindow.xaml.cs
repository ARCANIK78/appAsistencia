using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using appAsistencia.api;
using ZXing;
using ZXing.QrCode;
using ZXing.Common;
using System.Drawing;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Collections.Generic;


namespace appAsistencia
{
    public partial class MainWindow : Window
    {
        private VideoCaptureDevice videoSource;
        private Bitmap currentFrame;
        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }
        private void BtnIniciarCamara_Click(object sender, RoutedEventArgs e)
        {
            var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No se encontró cámara.");
                return;
            }
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSource.Start();
            timer.Start();
        }
        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (currentFrame != null)
                currentFrame.Dispose();
            currentFrame = (Bitmap)eventArgs.Frame.Clone();

            // Mostrar en el Imagen
            if (!videoImagen.Dispatcher.HasShutdownStarted && !videoImagen.Dispatcher.HasShutdownFinished)
            {
                videoImagen.Dispatcher.Invoke(() =>
                {
                    videoImagen.Source = BitmapToImageSource(currentFrame);
                });
            }
        }
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (currentFrame != null)
            {
                try
                {
                    Bitmap frameCopy;
                    lock (currentFrame)
                    {
                        frameCopy = (Bitmap)currentFrame.Clone();
                    }

                    var lector = new BarcodeReader(); // ZXing.Net
                    var resultado = lector.Decode(frameCopy);

                    if (resultado != null)
                    {
                        string ci = resultado.Text.Trim();
                        timer.Stop();
                        lblCI.Text = ci;
                        ApiService api = new ApiService();
                        bool exito = await api.RegistrarAsistencia(ci);

                        lblResultadoQR.Text = exito
                            ? $"✅ Asistencia registrada para {ci}"
                            : "❌ Error al registrar asistencia";
                        videoSource.SignalToStop();
                    }
                    frameCopy.Dispose(); // liberar la copia
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error leyendo QR: " + ex.Message);
                }
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
                videoSource.SignalToStop();
        }
    }
}