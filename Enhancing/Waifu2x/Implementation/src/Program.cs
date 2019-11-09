//*************************************************************************************************
//* (C) ColorfulSoft, 2019. Все права защищены.
//*************************************************************************************************

//-> Определение метода инициализации приложения.

using System;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace NeuralEnhance
{

    public sealed partial class MainForm : Form
    {

        public Waifu2x Net
        {
            get;
            set;
        }

        ///<summary>Поток выполнения.</summary>
        private Thread Enhancer;

        ///<summary>Останавливает процесс обработки. Это - обработчик события.</summary>
        ///<param name="sender">Ссылка на объёкт(кнопку на форме), вызвавший метод.</param>
        ///<param name="E">Аргументы.</param>
        private void StopProcess(object sender, EventArgs E)
        {
            this.Enhancer.Abort();
            this.Enhancer = null;
            this.OpenOriginal.Enabled = true;
            this.GenerateOrSaveResult.Text = "Stop";
            this.GenerateOrSaveResult.Click -= this.StopProcess;
            this.GenerateOrSaveResult.Click += this.StartProcess;
            this.Progress.Value = 0;
        }

        ///<summary>Останавливает поток обработки, если он был запущен. Это - обработчик события.</summary>
        ///<param name="sender">Ссылка на форму, вызвавшую метод.</param>
        ///<param name="E">Аргументы.</param>
        private void CloseWindowHandler(object sender, CancelEventArgs E)
        {
            if(this.Enhancer != null)
            {
                this.Enhancer.Abort();
            }
        }

        ///<summary>Запускает процесс обработки. Это - обработчик события.</summary>
        ///<param name="sender">Ссылка на объёкт(кнопку на форме), вызвавший метод.</param>
        ///<param name="E">Аргументы.</param>
        private void StartProcess(object sender, EventArgs E)
        {
            this.Enhancer = new Thread(this.Enhance);
            this.GenerateOrSaveResult.Text = "Stop";
            this.GenerateOrSaveResult.Click -= this.StartProcess;
            this.GenerateOrSaveResult.Click += this.StopProcess;
            this.OpenOriginal.Enabled = false;
            this.Enhancer.Start();
        }

        ///<summary>Выполняет улучшение изображения. Метод выполняется в потоке Enhancer.</summary>
        private void Enhance()
        {
            var original = IOConverters.ImageToTensor(new Bitmap(this.Original.Image as Bitmap, this.Original.Image.Width * 2, this.Original.Image.Height * 2));
            this.Result.Image = IOConverters.TensorToImage(Net.Enhance(original));
            this.Progress.Value = 0;
            this.GenerateOrSaveResult.Text = "Save";
            this.GenerateOrSaveResult.Click -= this.StopProcess;
            this.GenerateOrSaveResult.Click += this.SaveResult;
            this.OpenOriginal.Enabled = true;
        }

        ///<summary>Открывает диалоговое окно сохранения результата. Это - обработчик события.</summary>
        ///<param name="sender">Ссылка на объёкт(кнопку на форме), вызвавший метод.</param>
        ///<param name="E">Аргументы.</param>
        private void SaveResult(object sender, EventArgs E)
        {
            var SFD = new SaveFileDialog();
            SFD.Title = "Save";
            SFD.Filter = "Images (*.bmp)|*.bmp|Images (*.emf)|*.emf|Images (*.exif)|*.exif|Images (*.gif)|*.gif|Images (*.ico)|*.ico|Images (*.jpg)|*.jpg|Images (*.png)|*.png|Images (*.tiff)|*.tiff|Images (*.wmf)|*.wmf";
            if(SFD.ShowDialog() == DialogResult.OK)
            {
                switch(SFD.FilterIndex)
                {
                    case 1:
                    {
                        this.Result.Image.Save(SFD.FileName, ImageFormat.Bmp);
                        break;
                    }
                    case 2:
                    {
                        this.Result.Image.Save(SFD.FileName, ImageFormat.Emf);
                        break;
                    }
                    case 3:
                    {
                        this.Result.Image.Save(SFD.FileName, ImageFormat.Exif);
                        break;
                    }
                    case 4:
                    {
                        this.Result.Image.Save(SFD.FileName, ImageFormat.Gif);
                        break;
                    }
                    case 5:
                    {
                        this.Result.Image.Save(SFD.FileName, ImageFormat.Icon);
                        break;
                    }
                    case 6:
                    {
                        this.Result.Image.Save(SFD.FileName, ImageFormat.Jpeg);
                        break;
                    }
                    case 7:
                    {
                        this.Result.Image.Save(SFD.FileName, ImageFormat.Png);
                        break;
                    }
                    case 8:
                    {
                        this.Result.Image.Save(SFD.FileName, ImageFormat.Tiff);
                        break;
                    }
                    case 9:
                    {
                        this.Result.Image.Save(SFD.FileName, ImageFormat.Wmf);
                        break;
                    }
                }
            }
        }

        public void FixStep(byte percent)
        {
            this.Progress.Value = percent;
        }

        ///<summary>Открывает диалоговое окно выбора контентного изображения. Это - обработчик события.</summary>
        ///<param name="sender">Ссылка на объёкт(кнопку на форме), вызвавший метод.</param>
        ///<param name="E">Аргументы.</param>
        private void OpenOriginalHandler(object sender, EventArgs E)
        {
            var OFD = new OpenFileDialog();
            OFD.Title = "Open";
            OFD.Filter = "Images (*.bmp; *.emf; *.exif; *.gif; *.ico; *.jpg; *.png; *.tiff; *.wmf)|*.bmp; *.emf; *.exif; *.gif; *.ico; *.jpg; *.png; *.tiff; *.wmf|All files|*.*";
            if(OFD.ShowDialog() == DialogResult.OK)
            {
                this.Original.Image = new Bitmap(OFD.FileName);
                if(GenerateOrSaveResult.Text != "Start")
                {
                    this.GenerateOrSaveResult.Text = "Start";
                    this.GenerateOrSaveResult.Click -= SaveResult;
                    this.GenerateOrSaveResult.Click += StartProcess;
                }
            }
        }

        public MainForm() : base()
        {
            this.Initialize();
            this.Closing += this.CloseWindowHandler;
            this.OpenOriginal.Click += this.OpenOriginalHandler;
            this.GenerateOrSaveResult.Click += this.StartProcess;
            this.Icon = Icon.FromHandle((new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("MainIcon.jpg"))).GetHicon());
            this.Net = new Waifu2x(Assembly.GetExecutingAssembly().GetManifestResourceStream("Waifu2x.model"));
            this.Net.Step += FixStep;
        }

    }

    public static class Program
    {

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

    }

}
