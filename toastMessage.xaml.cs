using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace appAsistencia
{
    /// <summary>
    /// Lógica de interacción para toastMessage.xaml
    /// </summary>
    public partial class ToastMessage : Window
    {
        public ToastMessage(string mensaje)
        {
            InitializeComponent();
            txtMensaje.Text = mensaje;

            // Cerrar automáticamente luego de 5 segundos
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                Close();
            };
            timer.Start();
        }
    }
}