using System;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;

namespace Notepad
{
    public partial class MainNotepadForm : Form
    {
        private string fileName;
        private Encoding encoding = Encoding.UTF8;

        public MainNotepadForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(ClosingApplication);
        }
        private void OpenFile(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                        {
                            MainTextBox.Text = sr.ReadToEnd();
                        }
                        fileName = openFileDialog.FileName;
                        Text = "Notepad | Текущий файл: " + Path.GetFileName(openFileDialog.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно открыть файл данного типа!", "Возникла ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
        private void CreateFile(object sender, EventArgs e)
        {
            if (MainTextBox.Modified)
            {
                DialogResult result = MessageBox.Show("Хотите сохранить изменения перед созданием нового файла?", "Сохранение файла", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    if (fileName == null)
                    {
                        SaveFileAs(sender, e);
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(fileName))
                        {
                            sw.Write(MainTextBox.Text);
                        }
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
            MainTextBox.Clear();
            fileName = null;
            Text = "Notepad | Текущий файл не сохранён";

        }
        private void SaveFileAs(object sender, EventArgs e)
        {
            if (fileName == null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                            {
                                sw.Write(MainTextBox.Text);
                            }
                            Text = "Notepad | Текущий файл: " + Path.GetFileName(saveFileDialog.FileName);
                        }
                        catch
                        {
                            MessageBox.Show("Невозможно сохранить файл!", "Возникла ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write(MainTextBox.Text);
                }
            }
        }
        private void PrintMethod(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            PrintDialog printDialog = new PrintDialog();
            printDocument.PrintPage += PrintPage;
            printDialog.Document = printDocument;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDialog.Document.Print();
            }
        }
        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(MainTextBox.Text, MainTextBox.Font, Brushes.Black, 100, 100);
        }
        private void ClosingApplication(object sender, FormClosingEventArgs e)
        {
            if (MainTextBox.Modified)
            {
                DialogResult result = MessageBox.Show("Хотите сохранить изменения перед закрытием?", "Сохранение файла", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    SaveFileAs(sender, e);
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
        private void ExitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void CopyText(object sender, EventArgs e)
        {
            MainTextBox.Copy();
        }
        private void PasteText(object sender, EventArgs e)
        {
            MainTextBox.Paste();
        }
        private void CutText(object sender, EventArgs e)
        {
            MainTextBox.Cut();
        }

        private void ChangeFont(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    MainTextBox.SelectionFont = fontDialog.Font;
                }
            }
        }
        private void ChangeFontColor(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    MainTextBox.SelectionColor = colorDialog.Color;
                }
            }
        }
        private void ShowDataAboutApp(object sender, EventArgs e)
        {
            MessageBox.Show("Version: 1.0\nPublisher: c0nf1dxnt Inc.\nDeveloper: Farkhat Tlyaumbetov 09-322","О программе");
        }
    }
}
