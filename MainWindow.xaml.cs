using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab2_dpgi
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Команда Save
        private ICommand _saveCommand;

        // Обробник події CanExecute для команди Save
        void CanSaveExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (textBox.Text.Trim().Length > 0) e.CanExecute = true; else e.CanExecute = false;
        }

        // Обробник події Execute для команди Save
        void SaveExecute(object sender, ExecutedRoutedEventArgs e)
        {
            System.IO.File.WriteAllText("d:\\myFile.txt", textBox.Text);
            MessageBox.Show("The file was saved!");
        }


        void CanOpenExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true; // Завжди дозволяємо відкривати файл
        }

        void OpenExecute(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    // Читання вмісту файлу і відображення його у текстовому полі
                    string fileContent = File.ReadAllText(filePath);
                    textBox.Text = fileContent;
                    MessageBox.Show("Файл успішно відкрито!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при відкритті файлу: {ex.Message}");
                }
            }
        }


        void CanClearExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(textBox.Text); // Дозволяємо стерти, якщо текстове поле не пусте
        }

        void ClearExecute(object sender, ExecutedRoutedEventArgs e)
        {
            textBox.Clear(); // Очищуємо текстове поле
        }


        void CanCopyExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(textBox.SelectedText);
        }

        void CopyExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (textBox.SelectedText.Length > 0) // Перевіряємо, чи є щось виділене
            {
                textBox.Copy(); // Копіюємо виділений текст
            }
        }


        void CanPasteExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Clipboard.ContainsText(); // Завжди дозволяємо вставку тексту
        }

        void PasteExecute(object sender, ExecutedRoutedEventArgs e)
        {
            textBox.Paste(); // Вставляємо текст з буфера обміну
        }

        public MainWindow()
        {
            InitializeComponent();

            // Ініціалізуємо команду Save з вказанням обробників подій
            _saveCommand = new RoutedCommand("Save", typeof(MainWindow));
            CommandBindings.Add(new CommandBinding(_saveCommand, SaveExecute, CanSaveExecute));

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenExecute, CanOpenExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, ClearExecute, CanClearExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, CopyExecute, CanCopyExecute));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, PasteExecute, CanPasteExecute));


            // Прив'язуємо команду до кнопок
            btnSave.Command = _saveCommand;
            btnOpen.Command = ApplicationCommands.Open;
            btnClear.Command = ApplicationCommands.Delete;
            btnCopy.Command = ApplicationCommands.Copy;
            btnPaste.Command = ApplicationCommands.Paste;
        }
    }
}
