using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Ccr.Core;

namespace bt
{

    public partial class WebcamForm : Form
    {
        /// <summary>
        /// A bitmap to hold the camera image
        /// </summary>
        private Bitmap cameraImage;
        private List<Point> _path;
        private Point? _tracking;
        private bool _connected = false;

        //private Point? _point = null;
        private Pen _pen = new Pen(new SolidBrush(Color.Gray), 5);
        private Brush _ptBrush = new SolidBrush(Color.Gray);
        private Brush _ptFirstBrush = new SolidBrush(Color.Red);
        private Brush _rightBrush = new SolidBrush(Color.Green);
        private Brush _leftBrush = new SolidBrush(Color.Blue);
        private Color? _rgbPoint = null;
        private DateTime _lastSecond = DateTime.Now;
        private int frames = 0;

        private btService _parent;
        private btOperations _eventsPort;

        public WebcamForm(btOperations eventsPort, btService parent)
        {
            _parent = parent;
            _eventsPort = eventsPort;
            InitializeComponent();
        }

        public void SetState(FormState state)
        {
            this.cameraImage = state.Bitmap;
            this._path = state.Path;

            Image old = this.cameraView.Image;
            this.cameraView.Image = state.Bitmap;

            if (state.Path.Count > 0 || state.RightSide.HasValue || state.LeftSide.HasValue)
            {
                Graphics g = Graphics.FromImage(cameraImage);
                if (state.Path.Count > 0)
                {
                    Point? last = null;
                    for (int i = 0; i < state.Path.Count; i++)
                    {
                        var p = state.Path[state.Path.Count - i - 1];
                        if (last != null)
                        {
                            g.DrawLine(_pen, last.Value, p);
                        }
                        if (i == state.Path.Count - 1)
                        {
                            g.FillEllipse(_ptFirstBrush, p.X - 10, p.Y - 10, 20, 20);
                        }
                        else
                        {
                            g.FillEllipse(_ptBrush, p.X - 10, p.Y - 10, 20, 20);
                        }
                        last = p;
                    }
                }
                if (state.RightSide.HasValue)
                {
                    g.FillEllipse(_rightBrush, state.RightSide.Value.X - 10, state.RightSide.Value.Y - 10, 20, 20);
                }
                if (state.LeftSide.HasValue)
                {
                    g.FillEllipse(_leftBrush, state.LeftSide.Value.X - 10, state.LeftSide.Value.Y - 10, 20, 20);
                }
            }
            frames++;
            TimeSpan ts = DateTime.Now - _lastSecond;
            if (ts.TotalMilliseconds >= 1000)
            {
                fpsLabel.Text = string.Format("FPS: {0}", frames);
                _lastSecond = DateTime.Now;
                frames = 0;
            }


            // Dispose of the old bitmap to save memory
            // (It will be garbage collected eventually, but this is faster)
            if (old != null)
            {
                old.Dispose();
            }
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            statusMsg.Text = "Connecting...";
            _eventsPort.Post(new Connect(comPorts.SelectedText));
        }

        private void cameraView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point p = ScreenToImage(e.X, e.Y);
                _eventsPort.Post(new AddToPath(p));
                _tracking = p;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Point p = ScreenToImage(e.X, e.Y);
                _rgbPoint = cameraImage.GetPixel(p.X, p.Y);
                rgbLabel.Text = string.Format("R: {0} G: {1} B: {2}", _rgbPoint.Value.R, _rgbPoint.Value.G, _rgbPoint.Value.B);
            }
        }

        private void cameraView_MouseMove(object sender, MouseEventArgs e)
        {
            if ((sender as Control).Capture && _tracking.HasValue)
            {
                Point p = ScreenToImage(e.X, e.Y);
                _eventsPort.Post(new MoveLastPoint(p));
                _tracking = p;
            }
        }

        private void cameraView_MouseUp(object sender, MouseEventArgs e)
        {
            _tracking = null;
        }

        private Point ScreenToImage(int eX, int eY)
        {
            float iw = cameraImage.Width;
            float ih = cameraImage.Height;
            float x = 0;
            float y = 0;
            if (ih * cameraView.Width > iw * cameraView.Height)
            {
                float k = cameraView.Height / ih;
                float filler = Math.Abs(cameraView.Width - iw * k) / 2;
                x = (eX - filler) / k;
                y = eY / k;
            }
            else
            {
                float k = cameraView.Width / iw;
                float filler = Math.Abs(cameraView.Height - ih * k) / 2;
                x = eX / k;
                y = (eY - filler) / k;
            }
            return new Point((int)x, (int)y);
        }

        private void WebcamForm_Load(object sender, EventArgs e)
        {
            //_eventsPort.Post(new OnLoad(this));
        }


        public void ChangeStatus(bool connected, string msg)
        {
            _connected = connected;
            statusMsg.Text = msg;
        }

        private void cameraView_Paint(object sender, PaintEventArgs e)
        {
        }

        private void resetPathBtn_Click(object sender, EventArgs e)
        {
            _eventsPort.Post(new ResetPath());
        }

        private void trainLeftBtn_Click(object sender, EventArgs e)
        {
            if (!_rgbPoint.HasValue)
            {
                MessageBox.Show("Select point first");
            }
            else
            {
                _eventsPort.Post(new TrainKeypoint(KeypointType.LEFT, _rgbPoint.Value));
            }
        }

        private void trainRightBtn_Click(object sender, EventArgs e)
        {
            if (!_rgbPoint.HasValue)
            {
                MessageBox.Show("Select point first");
            }
            else
            {
                _eventsPort.Post(new TrainKeypoint(KeypointType.RIGHT, _rgbPoint.Value));
            }
        }

        private void activeChk_CheckedChanged(object sender, EventArgs e)
        {
            _eventsPort.Post(new SetActive(new SetActiveRequest(activeChk.Checked)));
        }
    }
}
