using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
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


using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Microsoft.Win32;
using System.Windows.Threading;
using NAudio.Dsp;



namespace Grabacion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WaveIn waveIn;
        WaveFormat formato;
        WaveFileWriter writer;
        WaveOutEvent output;
        AudioFileReader reader;

        Stopwatch stopwatch;
        TimeSpan Timespan;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            waveIn = new WaveIn();
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);
            formato = waveIn.WaveFormat;



            waveIn.DataAvailable += OnDataAvailable;
            waveIn.RecordingStopped += OnRecordingStopped;

            writer =
                new WaveFileWriter("sonido2.wav", formato);

            waveIn.StartRecording();
        }

        void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            writer.Dispose();

        }

        void OnDataAvailable (object sender, WaveInEventArgs e)
        {

            byte[] buffer = e.Buffer;
            int bytesGrabados = e.BytesRecorded;


            double acumulador = 0;
            double numMuestras = bytesGrabados / 2;
            int exponente = 1;
            int numeroMuestrasComplejas = 0;
            int bitsMaximos = 0;

            do //1,200
            {
                bitsMaximos = (int) Math.Pow(2, exponente);
                exponente++;
            } while (bitsMaximos < numMuestras);

            //bitsMaximos = 2048
            //exponente = 12

            //numeroMuestrasComplejas = 1024
            //exponente = 10

            exponente -= 2;
            numeroMuestrasComplejas = bitsMaximos / 2;

            Complex[] muestrasComplejas =
                new Complex[numeroMuestrasComplejas];

            for (int i=0; i < bytesGrabados; i+=2)
            {
                // byte i =  0 1 1 0 0 1 1 1
                //byte i+1 = 0 0 0 0 0 0 0 0 0 1 1 0 0 1 1 1
                // or      = 0 1 1 0 0 1 1 1 0 1 1 0 0 1 1 1
                short muestra =
                        (short)Math.Abs((buffer[i + 1] << 8)|buffer[i]);
                //lblMuestra.Text = muestra.ToString();
                //sldVolumen.Value = (double)muestra;

                float muestra32bits = (float)muestra / 32768.0f;
                sldVolumen.Value = Math.Abs(muestra32bits);

                if (i / 2 < numeroMuestrasComplejas)
                {
                    muestrasComplejas[i / 2].X = muestra32bits;
                }
                //acumulador += muestra;
                //numMuestras++;
            }
            //double promedio = acumulador / numMuestras;
            //sldVolumen.Value = promedio;
            //writer.Write(buffer, 0, bytesGrabados);

            FastFourierTransform.FFT(true, exponente, muestrasComplejas);
            float[] valoresAbsolutos = 
                new float[muestrasComplejas.Length];
            for(int i=0; i <muestrasComplejas.Length; i++)
            {
                valoresAbsolutos[i] = (float)
                    Math.Sqrt((muestrasComplejas[i].X * muestrasComplejas[i].X) +
                    (muestrasComplejas[i].Y * muestrasComplejas[i].Y));

            }

            int indiceMaximo =
                valoresAbsolutos.ToList().IndexOf(
                    valoresAbsolutos.Max());

            float frecuenciaFundamental =
                (float)(indiceMaximo * waveIn.WaveFormat.SampleRate) / (float)valoresAbsolutos.Length;





            lblFrecuencia.Text =
                            frecuenciaFundamental.ToString() + " Hz";

            

            

            if (frecuenciaFundamental > 80 && frecuenciaFundamental < 120)
            {
                txtFrecuencias.Text += "A";
            }
            else if (frecuenciaFundamental > 180 && frecuenciaFundamental < 220)
            {
                txtFrecuencias.Text += "B";
            }
            else if (frecuenciaFundamental > 280 && frecuenciaFundamental < 320)
            {
                txtFrecuencias.Text += "C";
            }
            else if (frecuenciaFundamental > 380 && frecuenciaFundamental < 420)
            {
                txtFrecuencias.Text += "D";
            }
            else if (frecuenciaFundamental > 480 && frecuenciaFundamental < 520)
            {
                txtFrecuencias.Text += "E";
            }
            else if (frecuenciaFundamental > 580 && frecuenciaFundamental < 620)
            {
                txtFrecuencias.Text += "F";
            }
            else if (frecuenciaFundamental > 680 && frecuenciaFundamental < 720)
            {
                txtFrecuencias.Text += "G";
            }
            else if (frecuenciaFundamental > 780 && frecuenciaFundamental < 820)
            {
                txtFrecuencias.Text += "H";
            }
            else if (frecuenciaFundamental > 880 && frecuenciaFundamental < 920)
            {
                txtFrecuencias.Text += "I";
            }
            else if (frecuenciaFundamental > 980 && frecuenciaFundamental < 1020)
            {
                txtFrecuencias.Text += "J";
            }
            else if (frecuenciaFundamental > 1080 && frecuenciaFundamental < 1120)
            {
                
                txtFrecuencias.Text += "K";
                
            }    
            else if (frecuenciaFundamental > 1180 && frecuenciaFundamental < 1220)
            {
                txtFrecuencias.Text += "L";
            }
            else if (frecuenciaFundamental > 1280 && frecuenciaFundamental < 1320)
            {
                txtFrecuencias.Text += "M";
            }
            else if (frecuenciaFundamental > 1380 && frecuenciaFundamental < 1420)
            {
                txtFrecuencias.Text += "N";
            }
            else if (frecuenciaFundamental > 1480 && frecuenciaFundamental < 1520)
            {
                txtFrecuencias.Text += "O";
            }
            else if (frecuenciaFundamental > 1580 && frecuenciaFundamental < 1620)
            {
                txtFrecuencias.Text += "P";
            }
            else if (frecuenciaFundamental > 1680 && frecuenciaFundamental < 1720)
            {
                txtFrecuencias.Text += "Q";
            }
            else if (frecuenciaFundamental > 1780 && frecuenciaFundamental < 1820)
            {
                txtFrecuencias.Text += "R";
            }
            else if (frecuenciaFundamental > 1880 && frecuenciaFundamental < 1920)
            {
                txtFrecuencias.Text += "S";
            }
            else if (frecuenciaFundamental > 1980 && frecuenciaFundamental < 2020)
            {
                txtFrecuencias.Text += "T";
            }
            else if (frecuenciaFundamental > 2080 && frecuenciaFundamental < 2120)
            {
                txtFrecuencias.Text += "U";
            }
            else if (frecuenciaFundamental > 2180 && frecuenciaFundamental < 2220)
            {
                txtFrecuencias.Text += "V";
            }
            else if (frecuenciaFundamental > 2280 && frecuenciaFundamental < 2320)
            {
                txtFrecuencias.Text += "W";
            }
            else if (frecuenciaFundamental > 2380 && frecuenciaFundamental < 2420)
            {
                txtFrecuencias.Text += "X";
            }
            else if (frecuenciaFundamental > 2480 && frecuenciaFundamental < 2520)
            {
                txtFrecuencias.Text += "Y";
            }
            else if (frecuenciaFundamental > 2580 && frecuenciaFundamental < 2620)
            {
                txtFrecuencias.Text += "Z";
            }
            else if (frecuenciaFundamental > 2680 && frecuenciaFundamental < 2720)
            {
                txtFrecuencias.Text += " ";
            }
            




        }

        private void btnDetener_Click(object sender, RoutedEventArgs e)
        {
            waveIn.StopRecording();
        }

        

        

        private void button_Click(object sender, RoutedEventArgs e)
        {
            output = new WaveOutEvent();
            reader = new AudioFileReader("sonido2.wav");
            output.Init(reader);
            output.Play();

        }
    }
}
