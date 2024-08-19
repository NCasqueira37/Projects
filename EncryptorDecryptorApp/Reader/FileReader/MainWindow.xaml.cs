using System;
using System.IO;
using System.Text;
using System.Windows;
using Win32 = Microsoft.Win32;
using System.Windows.Forms;

namespace FileReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum EncryptionType
        {
            CaesarCipher,
            ROT13,
            AES
        }
        private EncryptionType encryptionType;

        Win32.OpenFileDialog openFileDialog = new Win32.OpenFileDialog();

        private int shift;
        public MainWindow()
        {
            InitializeComponent();
            
            radioButton_CaesarCipher.Checked += RadioButton_CaesarCipher_Checked;
            radioButton_Rot13.Checked += RadioButton_Rot13_Checked;
        }

        private void RadioButton_Rot13_Checked(object sender, RoutedEventArgs e)
        {
            encryptionType = EncryptionType.ROT13;
            Console.WriteLine($"Changed Encryption Type to: {encryptionType}");
        }

        private void RadioButton_CaesarCipher_Checked(object sender, RoutedEventArgs e)
        {
            encryptionType = EncryptionType.CaesarCipher;
            Console.WriteLine($"Changed Encryption Type to: {encryptionType}");
        }

        private void OpenFileDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string filePath = openFileDialog.FileName;

            StreamReader sr = new StreamReader(filePath);
            string output = sr.ReadToEnd();
            textbox_Input.Text = output;
        }

        private void button_FilePicker_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.FileOk += OpenFileDialog_FileOk;
            openFileDialog.Filter = "Text Files|*.txt";
            openFileDialog.ShowDialog();
        }

        private void button_Encryptor_Click(object sender, RoutedEventArgs e)
        {
            char[] chars = textbox_Input.Text.ToCharArray();
            string result = "";

            switch (encryptionType)
            {
                case EncryptionType.CaesarCipher:
                    shift = 5;
                    for (int i = 0; i < chars.Length; i++)
                    {
                        char c = (char)(chars[i] + shift);
                        result += c;
                    }
                    textbox_Output.Text = result;
                    break;

                case EncryptionType.ROT13:
                    shift = 13;
                    for (int i = 0; i < chars.Length; i++)
                    {
                        char c = (char)(chars[i] + shift);
                        result += c;
                    }
                    textbox_Output.Text = result;
                    break;
            }
            Console.WriteLine("Encrypted Text");
        }

        private void button_Decryptor_Click(object sender, RoutedEventArgs e)
        {
            char[] chars = textbox_Input.Text.ToCharArray();
            string result = "";

            switch (encryptionType)
            {
                case EncryptionType.CaesarCipher:
                    shift = 5;
                    for (int i = 0; i < chars.Length; i++)
                    {
                        char c = (char)(chars[i] - shift);
                        result += c;
                    }
                    textbox_Output.Text = result;
                    break;

                case EncryptionType.ROT13:
                    shift = 13;
                    for (int i = 0; i < chars.Length; i++)
                    {
                        char c = (char)(chars[i] - shift);
                        result += c;
                    }
                    textbox_Output.Text = result;
                    break;
            }
            Console.WriteLine("Decrypted Text");
        }

        private void button_SaveFile_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBroserDialog = new FolderBrowserDialog();
            folderBroserDialog.ShowNewFolderButton = true;
            DialogResult result = folderBroserDialog.ShowDialog();
            string selectedPath = folderBroserDialog.SelectedPath;

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                FileStream fileStream = File.Create(selectedPath + "/NewFile.txt");
                byte[] buffer = new byte[4096];
                buffer = Encoding.UTF8.GetBytes(textbox_Output.Text);
                fileStream.Write(buffer, 0, buffer.Length);
                Console.WriteLine($"Saved file to: {selectedPath + "/NewFile.txt"}");
                System.Windows.MessageBox.Show($"Saved file to: {selectedPath + "/NewFile.txt"}", "File Saved!");
            }
            
        }

        private void button_NewText_Click(object sender, RoutedEventArgs e)
        {
            string outputString = textbox_Output.Text;

            textbox_Output.Text = "";
            textbox_Input.Text = outputString;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Shutdown the application
            System.Windows.Application.Current.Shutdown();
        }
    }
}
