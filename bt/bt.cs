using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using serial = Microsoft.Robotics.Services.SerialComService.Proxy;
//using webcam = Microsoft.Robotics.Services.MultiDeviceWebCam.Proxy;
using webcam = Microsoft.Robotics.Services.WebCam.Proxy;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Ccr.Adapters.WinForms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

namespace bt
{
	[Contract(Contract.Identifier)]
	[DisplayName("bt")]
	[Description("bt service (no description provided)")]
	public class btService : DsspServiceBase
	{
		[ServiceState]
		btState _state = new btState();
		
		[ServicePort("/bt", AllowMultipleInstances = true)]
		btOperations _mainPort = new btOperations();

        [Partner("SerialCOMService", CreationPolicy = PartnerCreationPolicy.UseExisting, Contract = serial.Contract.Identifier)]
        private serial.SerialCOMServiceOperations _serialPort = new serial.SerialCOMServiceOperations();

        [Partner("WebCam", CreationPolicy = PartnerCreationPolicy.UseExistingOrCreate, Contract = webcam.Contract.Identifier)]
        private webcam.WebCamOperations _webcamPort = new webcam.WebCamOperations();

        private webcam.WebCamOperations _webcamNotify = new webcam.WebCamOperations();

        private WebcamForm _cameraForm;

        private ColorBin _leftBin = new ColorBin() { Name = "Left", Target = Color.FromArgb(100, 134, 80) };
        private ColorBin _rightBin = new ColorBin() { Name = "Right", Target = Color.FromArgb(132, 148, 0) };

        //private ColorBin _leftBin = new ColorBin() { Name = "Left", RedMin = -1, RedMax = 30, GreenMin = 140, GreenMax = 170, BlueMin = 226, BlueMax = 256 };
        //private ColorBin _rightBin = new ColorBin() { Name = "Right", RedMin = 30, RedMax = 60, GreenMin = 150, GreenMax = 180, BlueMin = 120, BlueMax = 150 };

		public btService(DsspServiceCreationPort creationPort)
			: base(creationPort)
		{
		}
		
		protected override void Start()
		{
			base.Start();

            _state.Processing = false;

            //_bins.Add(new ColorBin() { RedMin = 80, RedMax = 100, GreenMin = 150, GreenMax = 200, BlueMin = -1, BlueMax = 120 });
            //_bins.Add(new ColorBin() { RedMin = -1, RedMax = 50, GreenMin = 120, GreenMax = 160, BlueMin = 150, BlueMax = 256 });

            //_bins.Add(new ColorBin() { Name = "Left", RedMin = 120, RedMax = 140, GreenMin = 150, GreenMax = 180, BlueMin = -1, BlueMax = 10 });
            //_bins.Add(new ColorBin() { Name = "Right", RedMin = 80, RedMax = 120, GreenMin = 140, GreenMax = 190, BlueMin = 50, BlueMax = 100 });

            SpawnIterator(Setup);
		}

        /// <summary>
        /// Perform the initial setup
        /// </summary>
        /// <returns>An iterator</returns>
        private IEnumerator<ITask> Setup()
        {
            MainPortInterleave.CombineWith(Arbiter.Interleave(new TeardownReceiverGroup(),
                new ExclusiveReceiverGroup(
                    //Arbiter.ReceiveWithIterator<OnLoad>(true, _boardEvents, OnLoadFormHandler),
                    //Arbiter.ReceiveWithIterator<OnConnect>(true, _boardEvents, OnConnectFormHandler),
                    //Arbiter.ReceiveWithIterator<OnManualControl>(true, _boardEvents, OnManualControlFormHandler),
                    //Arbiter.Receive<OnSetPath>(true, _boardEvents, OnSetPathFormHandler),
                    //Arbiter.Receive<OnResetPath>(true, _boardEvents, OnResetPathFormHandler),
                    Arbiter.Receive<webcam.UpdateFrame>(true, _webcamNotify, CameraUpdateFrameHandler)
                    ),
                    new ConcurrentReceiverGroup()
                    ));

            SpawnIterator(this.ConnectWebCamHandler);

            //WinFormsServicePort.Post(new RunForm(this.CreateForm));

            yield break;
        }

        #region Service handlers

        /// <summary>
        /// OnUpdateProcessing Handler
        /// </summary>
        /// <param name="update"></param>
        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public void OnUpdateProcessing(UpdateProcessing update)
        {
            _state.Processing = update.Body.Processing;
            update.ResponsePort.Post(DefaultUpdateResponseType.Instance);

        }

        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public void OnUpdateDirection(UpdateDirection update)
        {
            if (_state.Connected && _state.Active)
            {
                SpawnIterator(UpdateDirection);
            }
            update.ResponsePort.Post(DefaultUpdateResponseType.Instance);
        }

        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public void OnConnect(Connect c)
        {
            SpawnIterator(c.Body, OnConnectFormHandler);
            c.ResponsePort.Post(DefaultUpdateResponseType.Instance);
        }

        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public void OnAddToPath(AddToPath path)
        {
            lock (_state)
            {
                _state.Path.Add(path.Body.Target);
            }
            path.ResponsePort.Post(DefaultUpdateResponseType.Instance);
        }

        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public void OnMoveLastPoint(MoveLastPoint path)
        {
            // TODO: check concurrency
            lock (_state)
            {
                if (_state.Path.Count == 0)
                {
                    _state.Path.Add(path.Body.Target);
                }
                else
                {
                    _state.Path[_state.Path.Count - 1] = path.Body.Target;
                }
            }
            path.ResponsePort.Post(DefaultUpdateResponseType.Instance);
        }

        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public void OnResetPath(ResetPath reset)
        {
            lock (_state)
            {
                _state.Path.Clear();
            }
            reset.ResponsePort.Post(DefaultUpdateResponseType.Instance);
        }

        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public void OnGoPath(SetActive msg)
        {
            _state.Active = msg.Body.Active;
            msg.ResponsePort.Post(DefaultUpdateResponseType.Instance);
        }

        [ServiceHandler(ServiceHandlerBehavior.Exclusive)]
        public void OnTrainKeypoint(TrainKeypoint tk)
        {
            Color c = tk.Body.Color;
            if (tk.Body.Keypoint == KeypointType.LEFT)
            {
                SetBinWindow(_leftBin, c);
            }
            else if (tk.Body.Keypoint == KeypointType.RIGHT)
            {
                SetBinWindow(_rightBin, c);
            }
            tk.ResponsePort.Post(DefaultUpdateResponseType.Instance);
        }

        #endregion

        private void SetBinWindow(ColorBin bin, Color c)
        {
            bin.Target = c;
        }

        private IEnumerator<ITask> UpdateDirection()
        {
            ControlPacket cp = null;

            DateTime dt = DateTime.Now;
            if ((dt - _state.LastUpdated).TotalMilliseconds < 60)
            {
                yield break;
            }
            _state.LastUpdated = dt;

            Point? tp = null;
            lock (_state)
            {
                if (_state.Path.Count > 0)
                {
                    tp = _state.Path[0];
                }
            }

            if (tp.HasValue)
            {
                Point p = tp.Value;
                int distLeft = (int)Distance(_state.LeftSide.Value, p);
                int distRight = (int)Distance(_state.RightSide.Value, p);
                int dist = (int)Distance(_state.RightSide.Value, _state.LeftSide.Value);
                //Point middle = GetMiddlePoint(_state.LeftSide.Value, _state.RightSide.Value);
                // check if target is reached
                if (distLeft > (dist) || distRight > (dist))
                {
                    cp = new ControlPacket();
                    cp.LeftDuration = 100;
                    cp.RightDuration = 100;

                    // algorithm:
                    // find normal
                    Point l = _state.LeftSide.Value;
                    Point r = _state.RightSide.Value;
                    Point m = GetMiddlePoint(l, r);
                    // translate left point and rotate by 90 deg
                    Point n = new Point(m.Y - l.Y, l.X - m.X);
                    // translate target point to the middle
                    Point ps = p;
                    ps.X -= m.X;
                    ps.Y -= m.Y;
                    // detect angle to the target point
                    double cos = (n.X * ps.X + n.Y * ps.Y) / (Math.Sqrt(n.X * n.X + n.Y * n.Y) * Math.Sqrt(ps.X * ps.X + ps.Y * ps.Y));
                    int round = (int)Math.Round(1000 * cos);
                    double angle = Math.Acos(cos);

                    // calc speed: 500px = 127 (power value)
                    //           <= 2 * dist = 40  (power value)
                    int dm = (int)Distance(m, p);

                    int maxpower = (127 * dm) / (500 - 2 * dist);
                    if (dm < 2 * dist)
                    {
                        maxpower = 40;
                    }
                    else if (dm > 500)
                    {
                        maxpower = 127;
                    }

                    if (maxpower < 40)
                    {
                        maxpower = 40;
                    }
                    if (maxpower > 127)
                    {
                        maxpower = 127;
                    }

                    bool process = true;
                    // update watchdog each second
                    //if ((dt - _state.Watchdog).TotalMilliseconds >= 1000)
                    //{
                    //    // check progress: if no move last second then get back and remove current target point
                    //    if (Math.Abs(_state.LastLeftDistance - distLeft) < 20 && Math.Abs(_state.LastRightDistance - distRight) < 20)
                    //    {
                    //        // some cheating here: it prevents commands by this function next 200 ms
                    //        _state.LastUpdated += TimeSpan.FromMilliseconds(200);
                    //        cp.LeftDuration = 200;
                    //        cp.RightDuration = 200;
                    //        cp.LeftPower = -maxpower;
                    //        cp.RightPower = -maxpower;
                    //        process = false;
                    //        if (_state.TryCount > 3)
                    //        {
                    //            // remove target point
                    //            lock (_state)
                    //            {
                    //                _state.Path.RemoveAt(0);
                    //            }
                    //            _state.TryCount = 0;
                    //        }
                    //        else
                    //        {
                    //            _state.TryCount++;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        _state.TryCount = 0;
                    //    }
                    //    _state.LastLeftDistance = distLeft;
                    //    _state.LastRightDistance = distRight;
                    //    _state.Watchdog = dt;
                    //}
                    if (process)
                    {
                        if (process)//round > 0)
                        {
                            // target is in front of the robot
                            double ratio = (Math.PI / 2 - angle) / (Math.PI / 2 + angle);
                            if (distLeft < distRight)
                            {
                                cp.LeftPower = (int)(maxpower * ratio);
                                cp.RightPower = maxpower;
                            }
                            else if (distLeft > distRight)
                            {
                                cp.LeftPower = maxpower;
                                cp.RightPower = (int)(maxpower * ratio);
                            }
                            else
                            {
                                cp.LeftPower = maxpower;
                                cp.RightPower = maxpower;
                            }
                        }
                        else // if (round <= 0)
                        {
                            // target is on the back side
                            if (distLeft < distRight)
                            {
                                cp.LeftPower = -maxpower;
                                cp.RightPower = maxpower;
                            }
                            else
                            {
                                cp.LeftPower = maxpower;
                                cp.RightPower = -maxpower;
                            }
                        }
                        //else
                        //{
                        //    // target is on 9 or 3 o'clock
                        //    if (distLeft < distRight)
                        //    {
                        //        cp.LeftPower = 0;
                        //        cp.RightPower = maxpower;
                        //    }
                        //    else
                        //    {
                        //        cp.LeftPower = maxpower;
                        //        cp.RightPower = 0;
                        //    }
                        //}
                    }

                    _state.Running = true;
                }
                else
                {
                    // Got point, remove it from queue
                    lock (_state)
                    {
                        _state.Path.RemoveAt(0);
                    }
                }
            }
            else if (_state.Running)
            {
                // no target points, stop
                cp = new ControlPacket();

                _state.Running = false;
            }
            if (cp != null)
            {
                Fault fault = null;

                yield return Arbiter.Choice(_serialPort.SendPacket(new serial.Packet(cp.GetPacket())), EmptyHandler, x => fault = x);

                if (fault != null)
                {
                    _cameraForm.ChangeStatus(false, "Failed to send command");
                    yield break;
                }
                _cameraForm.ChangeStatus(true, "Connected");
            }
            yield break;
        }

        private void CameraUpdateFrameHandler(webcam.UpdateFrame update)
        {
            if (_state.Processing)
            {
                return;
            }
            _state.Processing = true;
            SpawnIterator(update, DoCameraUpdateFrameHandler);
        }

        /// <summary>
        /// Initialize the Web camera
        /// </summary>
        /// <returns>An iterator</returns>
        private IEnumerator<ITask> ConnectWebCamHandler()
        {
            Fault fault = null;

            yield return
                Arbiter.Choice(
                    this._webcamPort.Subscribe(this._webcamNotify),
                    EmptyHandler,
                    f => fault = f);

            if (fault != null)
            {
                LogError(null, "Failed to subscribe to webcam", fault);
                yield break;
            }

            var runForm = new RunForm(this.CreateWebCamForm);

            WinFormsServicePort.Post(runForm);

            yield return Arbiter.Choice(runForm.pResult, EmptyHandler, e => fault = Fault.FromException(e));

            if (fault != null)
            {
                LogError(null, "Failed to Create WebCam window", fault);
                yield break;
            }

            yield return Arbiter.Choice(
                _webcamPort.Subscribe(_webcamNotify, typeof(webcam.UpdateFrame)),
                delegate(SubscribeResponseType success) { },
                delegate(Fault f)
                {
                    fault = f;
                });

            if (fault != null)
            {
                LogError(null, "Failed to subscribe to webcam", fault);
                yield break;
            }

            yield break;
        }

        /// <summary>
        /// Create a form for the webcam 
        /// </summary>
        /// <returns>A Webcam Form</returns>
        private Form CreateWebCamForm()
        {
            this._cameraForm = new WebcamForm(_mainPort, this);
            return this._cameraForm;
        }

        private IEnumerator<ITask> DoCameraUpdateFrameHandler(webcam.UpdateFrame update)
        {
            try
            {
                Fault fault = null;

                webcam.QueryFrameRequest req = new webcam.QueryFrameRequest();
                req.Format = Guid.Empty;
                webcam.QueryFrameResponse resp = null;

                //req.Format = ImageFormat.Bmp.Guid;

                yield return Arbiter.Choice(_webcamPort.QueryFrame(req),
                    x => resp = x,
                    x => fault = x);

                if (fault != null)
                {
                    yield break;
                }

                if (resp.Frame != null)
                {
                    FoundBlob leftBlob = new FoundBlob(_leftBin);
                    FoundBlob rightBlob = new FoundBlob(_rightBin);
                    int step = 4;
                    for (int y = 0; y < resp.Size.Height; y += step)
                    {
                        int offset = y * resp.Size.Width * 3;
                        for (int x = 0; x < resp.Size.Width; x += step)
                        {
                            int b = resp.Frame[offset];
                            int g = resp.Frame[offset + 1];
                            int r = resp.Frame[offset + 2];

                            offset += 3 * step;

                            if (leftBlob.Bin.Test(r, g, b))
                            {
                                leftBlob.AddPixel(r, g, b, x, y);
                            }
                            if (rightBlob.Bin.Test(r, g, b))
                            {
                                rightBlob.AddPixel(r, g, b, x, y);
                            }
                        }
                    }
                    if (leftBlob.CalcMeans() && rightBlob.CalcMeans())
                    {
                        _state.RightSide = new Point((int)rightBlob.MeanX, (int)rightBlob.MeanY);
                        _state.LeftSide = new Point((int)leftBlob.MeanX, (int)leftBlob.MeanY);

                        int ro = _state.RightSide.Value.Y * resp.Size.Width * 3 + _state.RightSide.Value.X * 3;
                        int lo = _state.LeftSide.Value.Y * resp.Size.Width * 3 + _state.LeftSide.Value.X * 3;
                        if (!rightBlob.Bin.Test(resp.Frame[ro + 2], resp.Frame[ro + 1], resp.Frame[ro]) ||
                            !leftBlob.Bin.Test(resp.Frame[lo + 2], resp.Frame[lo + 1], resp.Frame[lo]))
                        {
                            _state.RightSide = null;
                            _state.LeftSide = null;
                        }
                        else
                        {
                            // correct color
                            //_leftBin.Target = leftBlob.MeanColor;
                            //_rightBin.Target = rightBlob.MeanColor;
                            //_mainPort.Post(new TrainKeypoint(KeypointType.LEFT, leftBlob.MeanColor));
                            //_mainPort.Post(new TrainKeypoint(KeypointType.RIGHT, rightBlob.MeanColor));
                        }
                    }
                    else
                    {
                        _state.RightSide = null;
                        _state.LeftSide = null;
                    }

                    Bitmap bmp = new Bitmap(
                        resp.Size.Width,
                        resp.Size.Height,
                        PixelFormat.Format24bppRgb
                    );

                    BitmapData data = bmp.LockBits(
                        new Rectangle(0, 0, bmp.Width, bmp.Height),
                        ImageLockMode.WriteOnly,
                        PixelFormat.Format24bppRgb
                    );

                    Marshal.Copy(resp.Frame, 0, data.Scan0, resp.Frame.Length);

                    bmp.UnlockBits(data);

                    FormState st = new FormState();
                    st.Bitmap = bmp;
                    st.Path = _state.Path;
                    st.LeftSide = _state.LeftSide;
                    st.RightSide = _state.RightSide;

                    FormInvoke setImage = new FormInvoke(delegate()
                    {
                        if (!_cameraForm.IsDisposed)
                        {
                            _cameraForm.SetState(st);
                        }
                    });

                    WinFormsServicePort.Post(setImage);

                    Arbiter.Choice(
                        setImage.ResultPort,
                        delegate(EmptyValue success) { },
                        delegate(Exception e)
                        {
                            fault = Fault.FromException(e);
                        }
                    );

                    if (fault != null)
                    {
                        LogError(null, "Unable to set camera image on form", fault);
                        yield break;
                    }

                    _mainPort.Post(new UpdateDirection());
                }
            }
            finally
            {
                _mainPort.Post(new UpdateProcessing(false));
            }

            yield break;
        }

        //private IEnumerator<ITask> OnLoadFormHandler(OnLoad ev)
        //{
        //    yield break;
        //}

        private IEnumerator<ITask> OnConnectFormHandler(ConnectRequest ev)
        {
            Fault fault = null;

            yield return Arbiter.Choice(_serialPort.OpenPort(), EmptyHandler, x => fault = x);
            if (fault != null)
            {
                FormInvoke inv = new FormInvoke(() =>
                {
                    if (!_cameraForm.IsDisposed)
                    {
                        _cameraForm.ChangeStatus(false, "Open port failed");
                    }
                });
                WinFormsServicePort.Post(inv);
                yield break;
            }

            _state.Connected = true;
            FormInvoke success = new FormInvoke(() =>
            {
                if (!_cameraForm.IsDisposed)
                {
                    _cameraForm.ChangeStatus(true, "Connected");
                }
            });
            WinFormsServicePort.Post(success);

            yield break;
        }

        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private Point GetMiddlePoint(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }


        //private IEnumerator<ITask> OnManualControlFormHandler(OnManualControl ev)
        //{
        //    serial.Packet pack = new serial.Packet(ev.Data.GetPacket());

        //    Fault fault = null;

        //    yield return Arbiter.Choice(_serialPort.SendPacket(pack), EmptyHandler, x => fault = x);

        //    if (fault != null)
        //    {
        //        ev.Form.ChangeStatus(false, "Failed to send command");
        //        yield break;
        //    }
        //    ev.Form.ChangeStatus(true, "Connected");

        //}
    }
}


