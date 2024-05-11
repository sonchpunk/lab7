using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Win32;
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
using Microsoft.CognitiveServices.Speech;

namespace lab7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer synthesizer;
        private string audioFilePath = "synthesizedAudio.wav";
        public MainWindow()
        {
            InitializeComponent();
            synthesizer = new SpeechSynthesizer();
        }

        private void LoadTextButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = ".txt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                TextInput.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private async void SaveAudioButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextInput.Text))
            {
                var result = await synthesizer.SpeakTextAsync(TextInput.Text);
                using (var stream = AudioDataStream.FromResult(result))
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "WAV files (*.wav)|*.wav",
                        DefaultExt = ".wav",
                        FileName = audioFilePath
                    };
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        audioFilePath = saveFileDialog.FileName;
                        await stream.SaveToWaveFileAsync(audioFilePath);
                        MessageBox.Show($"Аудио сохранено: {audioFilePath}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Введите текст для синтеза речи.");
            }
        }
    }
}
