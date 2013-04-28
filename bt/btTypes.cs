using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using Microsoft.Dss.ServiceModel.DsspServiceBase;
using W3C.Soap;
using System.Drawing;

namespace bt
{
	public sealed class Contract
	{
		[DataMember]
		public const string Identifier = "http://schemas.tempuri.org/2013/03/bt.html";
	}
	
	[DataContract]
	public class btState
	{
        public bool Processing { get; set; }
        public bool Connected { get; set; }
        public bool Running { get; set; }
        public bool Active { get; set; }

        public Point? LeftSide { get; set; }
        public Point? RightSide { get; set; }
        public List<Point> Path { get; set; }

        public DateTime LastUpdated { get; set; }

        //public DateTime Watchdog { get; set; }
        //public int LastLeftDistance { get; set; }
        //public int LastRightDistance { get; set; }
        public int TryCount { get; set; }

        public btState()
        {
            Path = new List<Point>();
            Running = false;
            Active = false;
            Connected = false;
        }
	}

    public struct FormState
    {
        public Point? LeftSide { get; set; }
        public Point? RightSide { get; set; }
        public List<Point> Path { get; set; }
        public Bitmap Bitmap { get; set; }
    }

    public class ControlPacket
    {
        public int LeftPower { get; set; }
        public int LeftDuration { get; set; }

        public int RightPower { get; set; }
        public int RightDuration { get; set; }

        /// <summary>
        /// default ctor stops all drives
        /// </summary>
        public ControlPacket()
        {
            LeftPower = 0;
            RightPower = 0;
            LeftDuration = 0;
            RightDuration = 0;
        }

        public byte[] GetPacket()
        {
            // data packet is 10 bytes:
            // L<power>0<duration>0
            // R<power>0<duration>0

            // power is calculated as: abs(given_power) * 2 + sign(given_power)
            int l = CalcPower(LeftPower);
            int r = CalcPower(RightPower);

            byte[] bytes = new byte[10];
            bytes[0] = 76; //'L'
            bytes[1] = (byte)(l % 256);
            bytes[2] = 0;
            bytes[3] = (byte)((LeftDuration / 10) % 256);
            bytes[4] = 0;
            bytes[5] = 82; //'R'
            bytes[6] = (byte)(r % 256);
            bytes[7] = 0;
            bytes[8] = (byte)((RightDuration / 10) % 256);
            bytes[9] = 0;

            return bytes;
        }

        private int CalcPower(int v)
        {
            if (v >= 0)
            {
                return v * 2;
            }
            return -v * 2 + 1;
        }
    }

    public class ColorBin
    {
        private int _red = 0;
        private int _green = 0;
        private int _blue = 0;

        public string Name { get; set; }
        public Color Target
        {
            get
            {
                return Color.FromArgb(_red, _green, _blue);
            }
            set
            {
                _red = value.R;
                _green = value.G;
                _blue = value.B;
            }
        }

        public bool Test(Color c)
        {
            return Test(c.R, c.G, c.B);
        }

        public bool Test(int r, int g, int b)
        {
            int fr = (r - _red);
            int fg = (g - _green);
            int fb = (b - _blue);
            return (fr * fr + fg * fg + fb * fb) < 1600;
        }
    }

    public class FoundBlob
    {
        public ColorBin Bin { get; private set; }

        public double MeanX { get; set; }
        public double MeanY { get; set; }
        public double Area { get; set; }

        public int MeanRed { get; set; }
        public int MeanGreen { get; set; }
        public int MeanBlue { get; set; }

        public Color MeanColor
        {
            get
            {
                return Color.FromArgb(MeanRed, MeanGreen, MeanBlue);
            }
        }

        public FoundBlob(ColorBin bin)
        {
            Bin = bin;
            MeanX = 0;
            MeanY = 0;
            Area = 0;
        }

        public void AddPixel(int r, int g, int b, int x, int y)
        {
            MeanX += x;
            MeanY += y;
            Area++;
            MeanRed += r;
            MeanGreen += g;
            MeanBlue += b;
        }

        public bool CalcMeans()
        {
            if (Area == 0)
            {
                return false;
            }
            MeanX = MeanX / Area;
            MeanY = MeanY / Area;
            MeanRed = (int)(MeanRed / Area);
            MeanGreen = (int)(MeanGreen / Area);
            MeanBlue = (int)(MeanBlue / Area);
            return true;
        }
    }

    [DataContract]
    public class UpdateProcessingRequest
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UpdateProcessingRequest()
        {
        }

        /// <summary>
        /// Creates a new UpdateProcessingRequest object.
        /// </summary>
        /// <param name="processing"></param>
        public UpdateProcessingRequest(bool processing)
        {
            _processing = processing;
        }

        /// <summary>
        /// Is processing?
        /// </summary>
        [DataMember, DataMemberConstructor]
        public bool Processing
        {
            get { return _processing; }
            set { _processing = value; }
        }
        private bool _processing;
    }

    [DataContract]
    public class AddToPathRequest
    {
        public Point Target { get; private set; }

        public AddToPathRequest()
        {
        }

        public AddToPathRequest(Point p)
        {
            Target = p;
        }
    }

    [DataContract]
    public class ConnectRequest
    {
        public string PortName { get; private set; }

        public ConnectRequest()
        {
        }

        public ConnectRequest(string portName)
        {
            PortName = portName;
        }
    }

    public enum KeypointType
    {
        LEFT = 1,
        RIGHT = 2
    }

    [DataContract]
    public class TrainKeypointRequest
    {
        public KeypointType Keypoint { get; private set; }
        public Color Color { get; private set; }

        public TrainKeypointRequest()
        {
        }

        public TrainKeypointRequest(KeypointType kp, Color c)
        {
            Keypoint = kp;
            Color = c;
        }
    }

    [DataContract]
    public class SetActiveRequest
    {
        public bool Active { get; private set; }

        public SetActiveRequest()
        {
        }

        public SetActiveRequest(bool v)
        {
            Active = v;
        }
    }
	
	[ServicePort]
	public class btOperations : PortSet<DsspDefaultLookup,
        DsspDefaultDrop,
        Get,
        Connect,
        SetActive,
        UpdateProcessing,
        UpdateDirection,
        AddToPath,
        MoveLastPoint,
        ResetPath,
        TrainKeypoint>
	{
	}
	
	public class Get : Get<GetRequestType, PortSet<btState, Fault>>
	{
		public Get()
		{
		}
		
		public Get(GetRequestType body)
			: base(body)
		{
		}
		
		public Get(GetRequestType body, PortSet<btState, Fault> responsePort)
			: base(body, responsePort)
		{
		}
	}

    public class UpdateProcessing : Update<UpdateProcessingRequest, DsspResponsePort<DefaultUpdateResponseType>>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UpdateProcessing()
        {
        }

        /// <summary>
        /// Creates a new UpdateProcessing message with the specified body.
        /// </summary>
        /// <param name="processing"></param>
        public UpdateProcessing(bool processing)
            : base(new UpdateProcessingRequest(processing))
        {
        }
    }

    public class UpdateDirection : Update<GetRequestType, DsspResponsePort<DefaultUpdateResponseType>>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UpdateDirection()
        {
        }

        public UpdateDirection(GetRequestType body)
            : base(body)
        {
        }

        public UpdateDirection(GetRequestType body, DsspResponsePort<DefaultUpdateResponseType> responsePort)
            : base(body, responsePort)
        {
        }
    }

    public class ResetPath : Update<GetRequestType, DsspResponsePort<DefaultUpdateResponseType>>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ResetPath()
        {
        }

        public ResetPath(GetRequestType body)
            : base(body)
        {
        }

        public ResetPath(GetRequestType body, DsspResponsePort<DefaultUpdateResponseType> responsePort)
            : base(body, responsePort)
        {
        }
    }

    public class AddToPath : Update<AddToPathRequest, DsspResponsePort<DefaultUpdateResponseType>>
    {
        public AddToPath()
        {
        }

        public AddToPath(Point newPoint)
            : base(new AddToPathRequest(newPoint))
        {
        }
    }

    public class MoveLastPoint : Update<AddToPathRequest, DsspResponsePort<DefaultUpdateResponseType>>
    {
        public MoveLastPoint()
        {
        }

        public MoveLastPoint(Point newPoint)
            : base(new AddToPathRequest(newPoint))
        {
        }
    }

    public class Connect : Update<ConnectRequest, PortSet<DefaultUpdateResponseType, Fault>>
    {
        public Connect()
        {
        }

        public Connect(string portName)
            : this(new ConnectRequest(portName))
        {
        }

        public Connect(ConnectRequest body)
            : base(body)
        {
        }

        public Connect(ConnectRequest body, PortSet<DefaultUpdateResponseType, Fault> responsePort)
            : base(body, responsePort)
        {
        }
    }

    public class SetActive : Update<SetActiveRequest, PortSet<DefaultUpdateResponseType, Fault>>
    {
        public SetActive()
        {
        }

        public SetActive(SetActiveRequest body)
            : base(body)
        {
        }

        public SetActive(SetActiveRequest body, PortSet<DefaultUpdateResponseType, Fault> responsePort)
            : base(body, responsePort)
        {
        }
    }

    public class TrainKeypoint : Update<TrainKeypointRequest, DsspResponsePort<DefaultUpdateResponseType>>
    {
        public TrainKeypoint()
        {
        }

        public TrainKeypoint(KeypointType keypoint, Color c)
            : base(new TrainKeypointRequest(keypoint, c))
        {
        }
    }


}


