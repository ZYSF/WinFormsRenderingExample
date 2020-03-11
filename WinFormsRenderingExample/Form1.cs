using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsRenderingExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // In a more advanced app you might want to add handles for the form designer to pick this class up.
            // For this, I've just added the control manually:
            RendererControl rc = new RendererControl();
            rc.Location = new Point(0, 0);
            rc.MinimumSize = new Size(300, 300);
            Controls.Add(rc);
        }
    }
}
