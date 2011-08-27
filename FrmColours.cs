using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Colours
{
    public class FrmColours : Form
    {
        public FrmColours()
        {
            szClr = new Size(32, 32);
            CreateBoxes();
            //set our properties
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(392, 366);//these call OnResize() 
            this.MinimumSize = new System.Drawing.Size(125, 60);//this should be after the call.
            this.Name = "FrmColours";
            this.Text = "Colours 2";
        }
        private IList<ColorBox> _clrBoxes;
        private const int space = 5;
        private Size szClr;
        int rows = 0, cols = 0;
        bool bAboutToCall = false;//prevent recursive calls
        //overidded
        protected override void OnResize(EventArgs e)
        {
            this.SuspendLayout();
            if (bAboutToCall) return;
            if (ClientSize.Width == 0) return; //incase the form is minimised.
            cols = (int)Convert.ToDecimal(this.ClientSize.Width) / (space + szClr.Width);//the cols that can fit
            if (cols == 0) cols = 1; //somehow Form.MinimumSize does'nt work! So i added this.
            rows = (int)Math.Ceiling(Convert.ToDecimal(_clrBoxes.Count) / cols);//the number of rows NEEDED
            bAboutToCall = true;
            this.AutoScrollMinSize = new Size(this.ClientSize.Width, rows * (space + szClr.Height));
            bAboutToCall = false;
            //reposition all boxes
            Point pt = this.AutoScrollPosition;
            int b = 0;//box count
            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= cols; j++)
                {
                    if (b == _clrBoxes.Count) return; //we've done the last brush
                    _clrBoxes[b].Location = new Point(pt.X + space, pt.Y + space);
                    pt.X += space + szClr.Width;
                    b++;
                    //"covers" what *we* painted, and point the the egde of the nxt box
                }
                pt.X = this.AutoScrollPosition.X;//back to 0,0
                pt.Y += (space + szClr.Height);
            }
            this.ResumeLayout(true);
            base.OnResize(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            //am i missing something here
            //yep the new stuff to show pretty color codes...
            base.OnMouseDown(e);
        }
        //functions
        private void CreateBoxes()
        {
            Size maxStrSz = new Size(); ; //max size of the colors names;
            SizeF szStr = new SizeF(); ; //size *per string
            IList<ColorBox> lstClrBoxes = new List<ColorBox>();
            Type cType = typeof(Color);
            PropertyInfo[] cProps = cType.GetProperties();
            foreach (PropertyInfo prClr in cProps)
            {
                if (prClr.CanRead && prClr.PropertyType.Name == "Color")//make sure we dont get unwanted props
                {
                    Color clr = (Color)prClr.GetValue(cType, null);
                    ColorBox clrBox = new ColorBox();
                    clrBox.Color = clr;
                    this.Controls.Add(clrBox);
                    lstClrBoxes.Add(clrBox);
                    using (Graphics grfx = this.CreateGraphics())
                    {
                        szStr = grfx.MeasureString(prClr.Name.ToString(),
                          new Font(this.Font, FontStyle.Bold | FontStyle.Underline)); //the size of the name
                    }
                    if (szStr.Width > maxStrSz.Width) maxStrSz.Width = (int)szStr.Width; //check if its the widest
                    if (szStr.Height > maxStrSz.Height) maxStrSz.Height = (int)szStr.Height; //check for "tallest"
                }
            }
            if (szClr.Width < szStr.Width) szClr.Width = maxStrSz.Width;
            if (szClr.Height < szStr.Height) szClr.Height = maxStrSz.Height;
            //set sizes
            foreach (ColorBox clrbx in lstClrBoxes)
                clrbx.Size = szClr;

            _clrBoxes = lstClrBoxes;
        }
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmColours());
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FrmColours
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Name = "FrmColours";
            this.ResumeLayout(false);

        }
    }
}