using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Colours
{
    public class ColorBox : UserControl
    {
        public ColorBox()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }
        private Color _color;
        private bool _selected = false;
        private bool _hover = false;

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                Invalidate();
            }
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //draw or erase the selection
            if (_selected && (!_hover))
            {
                DrawBox(e.Graphics, new Font(Font, FontStyle.Bold | FontStyle.Underline), _hover);
            }
            else
            {
                DrawBox(e.Graphics, Font, _hover);
            }
            base.OnPaint(e);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            //draw border
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Clipboard.SetText(FormatDigit(_color.R.ToString("x")) +
                FormatDigit(_color.G.ToString("x")) +
                FormatDigit(_color.B.ToString("x")));
            //Invalidate();
            _selected = true;
            base.OnMouseDown(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            //erase border
            _hover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }
        private void DrawBox(Graphics grfx, Font fnt, bool border)
        {
            string str = _color.Name;
            if (border)
            {
                str = _color.Name + "\n[" +
                    FormatDigit(_color.R.ToString("x")) + FormatDigit(_color.G.ToString("x")) + FormatDigit(_color.B.ToString("x") + "]");
            }
            StringFormat strfrmt = new StringFormat();
            strfrmt.Alignment = StringAlignment.Center;
            strfrmt.LineAlignment = StringAlignment.Center;
            grfx.FillRectangle(new SolidBrush(_color), 0, 0, ClientSize.Width, ClientSize.Height);

            grfx.DrawString(
               str,
                fnt,
                GetAppropriateBrush(_color),
                (ClientSize.Width / 2),
                (ClientSize.Height / 2),
                strfrmt
                );
            if (border)
            {
                grfx.DrawRectangle(new Pen(GetAppropriateBrush(_color)), new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1));
            }
        }
        protected SolidBrush GetAppropriateBrush(SolidBrush brush)
        {//gets a contrasting brush for a given brush.
            if ((brush.Color.R + brush.Color.G + brush.Color.B) > 254)
            {
                return (SolidBrush)Brushes.Black;//for light colors
            }
            else
            {
                return (SolidBrush)Brushes.White;//dark colors
            }
        }
        protected SolidBrush GetAppropriateBrush(Color color)
        {
            return GetAppropriateBrush(new SolidBrush(color));
        }
        string FormatDigit(string s)
        {
            if (s.Length == 1)
            {
                return "0" + s;
            }
            return s;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ColorBox
            // 
            this.Name = "ColorBox";
            this.ResumeLayout(false);

        }
    }
}