using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsRenderingExample
{
    public class RendererControl : Control
    {


        public Bitmap CreateBitmap(int w, int h)
        {
            Bitmap result = new Bitmap(w, h);
            Graphics graphics = Graphics.FromImage(result);
            int i = 0;
            for (int y = 0; y < h; y += 64)
            {
                for (int x = 0; x < w; x += 64)
                {
                    if (i % 2 != 0)
                    {
                        graphics.FillRectangle(Brushes.Black, x, y, 64, 64);
                    } else
                    {
                        graphics.FillRectangle(Brushes.White, x, y, 64, 64);
                    }
                    i++;
                }
            }
            return result;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(CreateBitmap(300, 300),0,0);
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
