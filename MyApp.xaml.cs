using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;


namespace Microsoft.Samples.Kinect.DiscreteGestureBasics
{
    /// <summary>
    /// Interaction logic for MyApp.xaml
    /// </summary>
    public partial class MyApp : Window
    {
        //Kinect
        private KinectSensor kinectSensor = null;
        private Body[] bodies = null;
        private BodyFrameReader bodyFrameReader = null;
        private string statusText = null;
        private KinectBodyView kinectBodyView = null;
        private List<GestureDetector> gestureDetectorList = null;

        //DB data
        int userId;
        string username = "";
        int sessionId;

        //right
        double handLengthR = 0.0;
        double upperArmLengthR = 0.0;
        double foreArmLengthR = 0.0;
        double shoulderLengthR = 0.0;
        double hipLengthR = 0.0;
        double upperLegLengthR = 0.0;
        double shinLengthR = 0.0;
        double footLengthR = 0.0;

        //left
        double handLengthL = 0.0;
        double upperArmLengthL = 0.0;
        double foreArmLengthL = 0.0;
        double shoulderLengthL = 0.0;
        double hipLengthL = 0.0;
        double upperLegLengthL = 0.0;
        double shinLengthL = 0.0;
        double footLengthL = 0.0;

        //general
        double neckLength = 0.0;
        double backboneLength = 0.0;
        double lowerBackLength = 0.0;

        //angles
        double minHWEAngleR = 360.0;
        double meanHWEAngleR = 0.0;
        double maxHWEAngleR = 0.0;
        double minHWEAngleL = 360.0;
        double meanHWEAngleL = 0.0;
        double maxHWEAngleL = 0.0;

        double minWEShAngleR = 360.0;
        double meanWEShAngleR = 0.0;
        double maxWEShAngleR = 0.0;
        double minWEShAngleL = 360.0;
        double meanWEShAngleL = 0.0;
        double maxWEShAngleL = 0.0;

        //accumilators
        //right
        double handLengthRAcc = 0.0;
        double upperArmLengthRAcc = 0.0;
        double foreArmLengthRAcc = 0.0;
        double shoulderLengthRAcc = 0.0;
        double hipLengthRAcc = 0.0;
        double upperLegLengthRAcc = 0.0;
        double shinLengthRAcc = 0.0;
        double footLengthRAcc = 0.0;

        //left
        double handLengthLAcc = 0.0;
        double upperArmLengthLAcc = 0.0;
        double foreArmLengthLAcc = 0.0;
        double shoulderLengthLAcc = 0.0;
        double hipLengthLAcc = 0.0;
        double upperLegLengthLAcc = 0.0;
        double shinLengthLAcc = 0.0;
        double footLengthLAcc = 0.0;

        //general
        double neckLengthAcc = 0.0;
        double backboneLengthAcc = 0.0;
        double lowerBackLengthAcc = 0.0;

        //angles

        double meanHWEAngleRAcc = 0.0;
        double meanHWEAngleLAcc = 0.0;
        double meanWEShAngleRAcc = 0.0;
        double meanWEShAngleLAcc = 0.0;

        //counters and flags
        Boolean uniqueUsername = false;
        int frameCounter = 0;
        Boolean startClicked = false;
        int startClickedCounter = 10;

        public MyApp()
        {
            this.kinectSensor = KinectSensor.GetDefault();
            this.kinectSensor.Open();
            this.statusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            this.kinectBodyView = new KinectBodyView(this.kinectSensor);
            int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
            InitializeComponent();
            enter_label.Visibility = Visibility.Hidden;
            notRegistered_btn.Visibility = Visibility.Hidden;
            start_btn.Visibility = Visibility.Hidden;
        }

        public double getDistanceBetweenJoints(CameraSpacePoint a, CameraSpacePoint b)
        {
            double result = 0;
            double xInter = Math.Pow(b.X - a.X, 2);
            double yInter = Math.Pow(b.Y - a.Y, 2);
            double zInter = Math.Pow(b.Z - a.Z, 2);

            result = Math.Sqrt(xInter + yInter + zInter);
            return result;
        }

        public double getAngleBetweenTwoVectors(double vx, double vy, double vz, double ux, double uy, double uz)
        {
            double result = 0;
            Vector3D v = new Vector3D(vx, vy, vz);
            Vector3D u = new Vector3D(ux, uy, uz);

            result = Vector3D.AngleBetween(v, u);
            return result;
        }
        public double getAngleAtMiddleJoint(CameraSpacePoint a, CameraSpacePoint b, CameraSpacePoint c)
        {
            double result = 0;
            Vector3D v = new Vector3D(b.X - a.X, b.Y - a.Y, b.Z - a.Z);
            Vector3D u = new Vector3D(b.X - c.X, b.Y - c.Y, b.Z - c.Z);

            result = Vector3D.AngleBetween(v, u);
            return result;
        }

        private void username_txtbx_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void username_txtbx_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

            TextBox txt = sender as TextBox;
            if (txt.Text.Equals("Username"))
            {
                txt.Text = string.Empty;
            }
        }

        private void register_btn_Click(object sender, RoutedEventArgs e)
        {
            string message = "";
            string connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=BachelorProject;Integrated Security=True;Pooling=False";
            this.frameCounter = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    try {
                        this.username = username_txtbx.Text;
                        // System.Diagnostics.Debug.WriteLine(this.username);
                        using (SqlCommand command =
                            new SqlCommand("SELECT * FROM Users WHERE Users.User_Name =\'" + this.username + "\'", conn))
                        {
                            uniqueUsername = false;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    uniqueUsername = true;
                                    register_btn.Visibility = Visibility.Hidden;
                                    username_txtbx.Visibility = Visibility.Hidden;
                                    register_label.Visibility = Visibility.Hidden;
                                    signIn_btn.Visibility = Visibility.Hidden;
                                    enter_label.Visibility = Visibility.Visible;
                                    start_btn.Visibility = Visibility.Visible;

                                }
                            }
                        }
                    }
                    catch (Exception ex1)
                    {
                        message += "Could not validate username! \n"+ ex1.ToString();
                    }


                    if (uniqueUsername)
                    {
                        message += "Valid Username \n";
                        try
                        {
                            using (SqlCommand command1 = new SqlCommand
                            ("INSERT INTO Users VALUES ('" + this.username + "')", conn))
                            {
                                SqlDataReader reader1 = command1.ExecuteReader();
                                reader1.Close();
                            }
                        }
                        catch (Exception ex0)
                        {
                            message += "Could not create user!" + ex0.ToString();
                        }

                        try
                        {
                            using (SqlCommand command =
                                new SqlCommand("SELECT * FROM Users WHERE Users.User_Name =\'" + this.username + "\'", conn))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();
                                    this.userId = Convert.ToInt32(reader["Id"].ToString());
                                }
                            }

                            using (SqlCommand command2 = 
                                new SqlCommand ("INSERT INTO Sessions VALUES ('" + this.userId + "')", conn))
                            {
                                SqlDataReader reader2 = command2.ExecuteReader();
                                reader2.Close();
                            }
                            using (SqlCommand command =
                                new SqlCommand("SELECT Top 1 * FROM Sessions WHERE Sessions.User_Id =\'" + this.userId + "\' ORDER BY Sessions.Id DESC", conn))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    reader.Read();
                                    this.sessionId = Convert.ToInt32(reader["Id"].ToString());
                                }
                            }
                        }
                        catch (Exception ex3)
                        {
                            message += "Could not create session!" + ex3.ToString();
                        }

                    }
                    else
                    {
                        message += "Username already taken!";
                    }
                    
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
               message += "Oops database connection failed!";
            }

            MessageBox.Show(message);
        }

        private void signIn_btn_Click(object sender, RoutedEventArgs e)
        {
            register_btn.Visibility = Visibility.Hidden;
            username_txtbx.Visibility = Visibility.Hidden;
            register_label.Visibility = Visibility.Hidden;
            signIn_btn.Visibility = Visibility.Hidden;
            enter_label.Visibility = Visibility.Visible;
            notRegistered_btn.Visibility = Visibility.Visible;
            start_btn.Visibility = Visibility.Visible;
        }

        private void notRegistered_btn_Click(object sender, RoutedEventArgs e)
        {
            register_btn.Visibility = Visibility.Visible;
            username_txtbx.Visibility = Visibility.Visible;
            register_label.Visibility = Visibility.Visible;
            signIn_btn.Visibility = Visibility.Visible;
            enter_label.Visibility = Visibility.Hidden;
            notRegistered_btn.Visibility = Visibility.Hidden;
            start_btn.Visibility = Visibility.Hidden;
        }

        private void start_btn_Click(object sender, RoutedEventArgs e)
        {
            start_btn.Visibility = Visibility.Hidden;
            // open the reader for the body frames
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // set the BodyFramedArrived event notifier
            this.frameCounter = 0;
            startClicked = true;
            startClickedCounter -= 1;
            this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;
            //enter_label.Content = this.kinectSensor.BodyFrameSource.BodyCount;
        }

        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            string message = "";
            bool dataReceived = false;
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                if(startClicked && startClickedCounter >= 0)
                {
                    // we may have lost/acquired bodies, so update the corresponding gesture detectors
                    if (this.bodies != null)
                    {
                        // loop through all bodies
                        int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;

                        for (int i = 0; i < maxBodies; ++i)
                        {
                            Body body = this.bodies[i];

                            if (body.IsTracked)
                            {

                                System.Diagnostics.Debug.WriteLine("body frame triggered");
                                IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
                                // convert the joint points to depth (display) space
                                Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                                frameCounter += 1;

                                this.handLengthRAcc += getDistanceBetweenJoints(joints[JointType.WristRight].Position,
                                    joints[JointType.HandTipRight].Position);
                                this.foreArmLengthRAcc += getDistanceBetweenJoints(joints[JointType.ElbowRight].Position,
                                    joints[JointType.WristRight].Position);
                                this.upperArmLengthRAcc += getDistanceBetweenJoints(joints[JointType.ShoulderRight].Position,
                                    joints[JointType.ElbowRight].Position);
                                this.shoulderLengthRAcc += getDistanceBetweenJoints(joints[JointType.ShoulderRight].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.hipLengthRAcc += getDistanceBetweenJoints(joints[JointType.HipRight].Position,
                                    joints[JointType.SpineBase].Position);
                                this.upperLegLengthRAcc += getDistanceBetweenJoints(joints[JointType.HipRight].Position,
                                    joints[JointType.KneeRight].Position);
                                this.shinLengthRAcc += getDistanceBetweenJoints(joints[JointType.AnkleRight].Position,
                                    joints[JointType.KneeRight].Position);
                                this.footLengthRAcc += getDistanceBetweenJoints(joints[JointType.AnkleRight].Position,
                                    joints[JointType.FootRight].Position);


                                this.handLengthLAcc += getDistanceBetweenJoints(joints[JointType.WristLeft].Position,
                                    joints[JointType.HandTipLeft].Position);
                                this.foreArmLengthLAcc += getDistanceBetweenJoints(joints[JointType.ElbowLeft].Position,
                                    joints[JointType.WristLeft].Position);
                                this.upperArmLengthLAcc += getDistanceBetweenJoints(joints[JointType.ShoulderLeft].Position,
                                    joints[JointType.ElbowLeft].Position);
                                this.shoulderLengthLAcc += getDistanceBetweenJoints(joints[JointType.ShoulderLeft].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.hipLengthLAcc += getDistanceBetweenJoints(joints[JointType.HipLeft].Position,
                                    joints[JointType.SpineBase].Position);
                                this.upperLegLengthLAcc += getDistanceBetweenJoints(joints[JointType.HipLeft].Position,
                                    joints[JointType.KneeLeft].Position);
                                this.shinLengthLAcc += getDistanceBetweenJoints(joints[JointType.AnkleLeft].Position,
                                    joints[JointType.KneeLeft].Position);
                                this.footLengthLAcc += getDistanceBetweenJoints(joints[JointType.AnkleLeft].Position,
                                    joints[JointType.FootLeft].Position);

                                this.neckLengthAcc += getDistanceBetweenJoints(joints[JointType.Head].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.backboneLengthAcc += getDistanceBetweenJoints(joints[JointType.SpineMid].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.lowerBackLengthAcc += getDistanceBetweenJoints(joints[JointType.SpineMid].Position,
                                    joints[JointType.SpineBase].Position);

                                double HWEAngleL = getAngleAtMiddleJoint
                                    (joints[JointType.ElbowLeft].Position,
                                    joints[JointType.WristLeft].Position,
                                    joints[JointType.ThumbLeft].Position);


                                double WEShAngleL = getAngleAtMiddleJoint
                                    (joints[JointType.WristLeft].Position,
                                    joints[JointType.ElbowLeft].Position,
                                    joints[JointType.ShoulderLeft].Position);

                                double HWEAngleR = getAngleAtMiddleJoint
                                    (joints[JointType.ElbowRight].Position,
                                    joints[JointType.WristRight].Position,
                                    joints[JointType.ThumbRight].Position);

                                double WEShAngleR = getAngleAtMiddleJoint
                                    (joints[JointType.WristRight].Position,
                                    joints[JointType.ElbowRight].Position,
                                    joints[JointType.ShoulderRight].Position);

                                this.meanHWEAngleLAcc += HWEAngleL;
                                this.meanWEShAngleLAcc += WEShAngleL;

                                this.meanHWEAngleRAcc += HWEAngleR;
                                this.meanWEShAngleRAcc += WEShAngleR;


                                if (this.minHWEAngleL > HWEAngleL)
                                {
                                    this.minHWEAngleL = HWEAngleL;
                                }
                                if (this.maxHWEAngleL < HWEAngleL)
                                {
                                    this.maxHWEAngleL = HWEAngleL;
                                }

                                if (this.minHWEAngleR > HWEAngleR)
                                {
                                    this.minHWEAngleR = HWEAngleR;
                                }
                                if (this.maxHWEAngleR < HWEAngleR)
                                {
                                    this.maxHWEAngleR = HWEAngleR;
                                }

                                if (this.minWEShAngleL > HWEAngleL)
                                {
                                    this.minWEShAngleL = WEShAngleL;
                                }
                                if (this.maxWEShAngleL < WEShAngleL)
                                {
                                    this.maxWEShAngleL = WEShAngleL;
                                }

                                if (this.minWEShAngleR > WEShAngleR)
                                {
                                    this.minWEShAngleR = WEShAngleR;
                                }
                                if (this.maxWEShAngleR < WEShAngleR)
                                {
                                    this.maxWEShAngleR = WEShAngleR;
                                }


                                if (frameCounter == 150)
                                {
                                    this.handLengthR = handLengthRAcc / 150;
                                    this.upperArmLengthR = upperArmLengthRAcc / 150;
                                    this.foreArmLengthR = foreArmLengthRAcc / 150;
                                    this.shoulderLengthR = shoulderLengthRAcc / 150;
                                    this.hipLengthR = hipLengthRAcc / 150;
                                    this.upperLegLengthR = upperLegLengthRAcc / 150;
                                    this.shinLengthR = shinLengthRAcc / 150;
                                    this.footLengthR = footLengthRAcc / 150;

                                    this.handLengthL = handLengthLAcc / 150;
                                    this.upperArmLengthL = upperArmLengthLAcc / 150;
                                    this.foreArmLengthL = foreArmLengthLAcc / 150;
                                    this.shoulderLengthL = shoulderLengthLAcc / 150;
                                    this.hipLengthL = hipLengthLAcc / 150;
                                    this.upperLegLengthL = upperLegLengthLAcc / 150;
                                    this.shinLengthL = shinLengthLAcc / 150;
                                    this.footLengthL = footLengthLAcc / 150;

                                    this.neckLength = neckLengthAcc / 150;
                                    this.backboneLength = backboneLengthAcc / 150;
                                    this.lowerBackLength = lowerBackLengthAcc / 150;

                                    this.meanHWEAngleL = meanHWEAngleLAcc / 150;
                                    this.meanHWEAngleR = meanHWEAngleRAcc / 150;
                                    this.meanWEShAngleL = meanWEShAngleLAcc / 150;
                                    this.meanWEShAngleR = meanWEShAngleRAcc / 150;


                                    this.handLengthRAcc = 0.0;
                                    this.upperArmLengthRAcc = 0.0;
                                    this.foreArmLengthRAcc = 0.0;
                                    this.shoulderLengthRAcc = 0.0;
                                    this.hipLengthRAcc = 0.0;
                                    this.upperLegLengthRAcc = 0.0;
                                    this.shinLengthRAcc = 0.0;
                                    this.footLengthRAcc = 0.0;

                                    this.handLengthLAcc = 0.0;
                                    this.upperArmLengthLAcc = 0.0;
                                    this.foreArmLengthLAcc = 0.0;
                                    this.shoulderLengthLAcc = 0.0;
                                    this.hipLengthLAcc = 0.0;
                                    this.upperLegLengthLAcc = 0.0;
                                    this.shinLengthLAcc = 0.0;
                                    this.footLengthLAcc = 0.0;

                                    this.neckLengthAcc = 0.0;
                                    this.backboneLengthAcc = 0.0;
                                    this.lowerBackLengthAcc = 0.0;
                                    this.meanHWEAngleLAcc = 0.0;
                                    this.meanHWEAngleRAcc = 0.0;
                                    this.meanWEShAngleLAcc = 0.0;
                                    this.meanWEShAngleRAcc = 0.0;

                                    string connectionString = null;
                                    connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=BachelorProject;Integrated Security=True;Pooling=False";
                                    try
                                    {

                                        using (SqlConnection conn = new SqlConnection(connectionString))
                                        {
                                            conn.Open();

                                            this.username = username_txtbx.Text;
                                            // System.Diagnostics.Debug.WriteLine(this.username);
                                            using (SqlCommand command = new SqlCommand
                                                        ("SELECT Id FROM Users WHERE Users.User_Name =\'" + this.username + "\'", conn))
                                            {
                                                uniqueUsername = false;
                                                using (SqlDataReader reader = command.ExecuteReader())
                                                {
                                                    if (reader.Read() && !this.username.Equals(""))
                                                    {
                                                        this.userId = Convert.ToInt32(reader["Id"].ToString());
                                                        uniqueUsername = true;
                                                        message = "Valid username \n";
                                                    }
                                                    else
                                                    {
                                                        this.userId = 0;
                                                    }
                                                    reader.Close();
                                                }
                                            }

                                            if (uniqueUsername && !this.username.Equals("")) //register
                                            {
                                                using (SqlCommand command = new SqlCommand
                                                        ("INSERT INTO Extracted_Kinect_Data VALUES ( 1,'"
                                                        + this.userId + "','" + this.sessionId + "','" +
                                                        this.handLengthR * 100 + "','" + this.upperArmLengthR * 100 +
                                                        "','" + this.foreArmLengthR * 100 + "','" + this.shoulderLengthR * 100 +
                                                        "','" + this.handLengthL * 100 + "','" + this.upperArmLengthL * 100 +
                                                        "','" + this.foreArmLengthL * 100 + "','" + this.shoulderLengthL * 100 +
                                                        "','" + this.neckLength * 100 + "','" + this.backboneLength * 100 +
                                                        "','" + this.lowerBackLength * 100 + "','" + this.hipLengthR * 100 +
                                                        "','" + this.upperLegLengthR * 100 + "','" + this.shinLengthR * 100 +
                                                        "','" + this.footLengthR * 100 + "','" + this.hipLengthL * 100 +
                                                        "','" + this.upperLegLengthL * 100 + "','" + this.shinLengthL * 100 +
                                                        "','" + this.footLengthL * 100 + "','" + this.minHWEAngleR +
                                                        "','" + this.meanHWEAngleR + "','" + this.maxHWEAngleR +
                                                        "','" + this.minWEShAngleR + "','" + this.meanWEShAngleR +
                                                        "','" + this.maxWEShAngleR + "','" + this.minHWEAngleL +
                                                        "','" + this.meanHWEAngleL + "','" + this.maxHWEAngleL +
                                                        "','" + this.minWEShAngleL + "','" + this.meanWEShAngleL +
                                                        "','" + this.maxWEShAngleL + "')", conn))
                                                {
                                                    SqlDataReader reader = command.ExecuteReader();
                                                    reader.Close();
                                                }

                                            }
                                            conn.Close();
                                        }

                                        if (startClickedCounter == 0)
                                        {
                                            message = "Registered Successfully! \n";
                                            this.username = "";
                                            this.uniqueUsername = false;
                                            register_btn.Visibility = Visibility.Visible;
                                            username_txtbx.Visibility = Visibility.Visible;
                                            register_label.Visibility = Visibility.Visible;
                                            signIn_btn.Visibility = Visibility.Visible;
                                            enter_label.Visibility = Visibility.Hidden;
                                            start_btn.Visibility = Visibility.Hidden;
                                            MessageBox.Show(message);
                                            this.startClickedCounter = 10;
                                            this.frameCounter = 0;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Oops an error has occured!\n" + ex.ToString());
                                        return;
                                    }
                                   
                                    startClicked = false;
                                    if (this.startClickedCounter > 0)
                                    {
                                        start_btn.Visibility = Visibility.Visible;
                                    }
                                    this.bodyFrameReader.FrameArrived -= this.Reader_BodyFrameArrived;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}