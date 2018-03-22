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

        //relative distances
        double wristRelativeSpineShoulderR = 0.0;
        double elbowRelativeSpineShoulderR = 0.0;

        double wristRelativeSpineShoulderL = 0.0;
        double elbowRelativeSpineShoulderL = 0.0;

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

        //relative distances
        double wristRelativeSpineShoulderRAcc = 0.0;
        double elbowRelativeSpineShoulderRAcc = 0.0;

        double wristRelativeSpineShoulderLAcc = 0.0;
        double elbowRelativeSpineShoulderLAcc = 0.0;


        //user accumilatiors

        double handLengthRUserAcc;
        double upperArmLengthRUserAcc;
        double foreArmLengthRUserAcc;
        double shoulderLengthRUserAcc;
        double handLengthLUserAcc;
        double upperArmLengthLUserAcc;
        double foreArmLengthLUserAcc;
        double shoulderLengthLUserAcc;
        double neckLengthUserAcc;
        double backboneLengthUserAcc;
        double lowerBackLengthUserAcc;
        double hipLengthRUserAcc;
        double upperLegLengthRUserAcc;
        double shinLengthRUserAcc;
        double footLengthRUserAcc;
        double hipLengthLUserAcc;
        double upperLegLengthLUserAcc;
        double shinLengthLUserAcc;
        double footLengthLUserAcc;
        double minHWEAngleRUserAcc;
        double meanHWEAngleRUserAcc;
        double maxHWEAngleRUserAcc;

        double minWEShAngleRUserAcc;
        double meanWEShAngleRUserAcc;
        double maxWEShAngleRUserAcc;

        double meanHWEAngleLUserAcc;
        double maxHWEAngleLUserAcc;
        double minHWEAngleLUserAcc;

        double minWEShAngleLUserAcc;
        double meanWEShAngleLUserAcc;
        double maxWEShAngleLUserAcc;

        double wristRelativeSpineShoulderRUserAcc;
        double elbowRelativeSpineShoulderRUserAcc;
        double wristRelativeSpineShoulderLUserAcc;
        double elbowRelativeSpineShoulderLUserAcc;

        //counters and flags
        Boolean uniqueUsername = false;
        int frameCounter = 0;
        Boolean startClicked = false;
        Boolean signInStartClicked = false;
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

        private void resetAccumilators()
        {

            this.handLengthRAcc = 0.0;
            this.upperArmLengthRAcc = 0.0;
            this.foreArmLengthRAcc = 0.0;
            this.shoulderLengthRAcc = 0.0;
            this.hipLengthRAcc = 0.0;
            this.upperLegLengthRAcc = 0.0;
            this.shinLengthRAcc = 0.0;
            this.footLengthRAcc = 0.0;

            //left
            this.handLengthLAcc = 0.0;
            this.upperArmLengthLAcc = 0.0;
            this.foreArmLengthLAcc = 0.0;
            this.shoulderLengthLAcc = 0.0;
            this.hipLengthLAcc = 0.0;
            this.upperLegLengthLAcc = 0.0;
            this.shinLengthLAcc = 0.0;
            this.footLengthLAcc = 0.0;

            //general
            this.neckLengthAcc = 0.0;
            this.backboneLengthAcc = 0.0;
            this.lowerBackLengthAcc = 0.0;

            //angles

            this.meanHWEAngleRAcc = 0.0;
            this.meanHWEAngleLAcc = 0.0;
            this.meanWEShAngleRAcc = 0.0;
            this.meanWEShAngleLAcc = 0.0;

            //relative distances
            this.wristRelativeSpineShoulderRAcc = 0.0;
            this.elbowRelativeSpineShoulderRAcc = 0.0;

            this.wristRelativeSpineShoulderLAcc = 0.0;
            this.elbowRelativeSpineShoulderLAcc = 0.0;
        }

        private void resetUserAccumilators()
        {

            this.handLengthRUserAcc = 0.0;
            this.upperArmLengthRUserAcc = 0.0;
            this.foreArmLengthRUserAcc = 0.0;
            this.shoulderLengthRUserAcc = 0.0;
            this.handLengthLUserAcc = 0.0;
            this.upperArmLengthLUserAcc = 0.0;
            this.foreArmLengthLUserAcc = 0.0;
            this.shoulderLengthLUserAcc = 0.0;
            this.neckLengthUserAcc = 0.0;
            this.backboneLengthUserAcc = 0.0;
            this.lowerBackLengthUserAcc = 0.0;
            this.hipLengthRUserAcc = 0.0;
            this.upperLegLengthRUserAcc = 0.0;
            this.shinLengthRUserAcc = 0.0;
            this.footLengthRUserAcc = 0.0;
            this.hipLengthLUserAcc = 0.0;
            this.upperLegLengthLUserAcc = 0.0;
            this.shinLengthLUserAcc = 0.0;
            this.footLengthLUserAcc = 0.0;
            this.minHWEAngleRUserAcc = 0.0;
            this.meanHWEAngleRUserAcc = 0.0;
            this.maxHWEAngleRUserAcc = 0.0;

            this.minWEShAngleRUserAcc = 0.0;
            this.meanWEShAngleRUserAcc = 0.0;
            this.maxWEShAngleRUserAcc = 0.0;

            this.meanHWEAngleLUserAcc = 0.0;
            this.maxHWEAngleLUserAcc = 0.0;
            this.minHWEAngleLUserAcc = 0.0;

            this.minWEShAngleLUserAcc = 0.0;
            this.meanWEShAngleLUserAcc = 0.0;
            this.maxWEShAngleLUserAcc = 0.0;

            this.wristRelativeSpineShoulderRUserAcc = 0.0;
            this.elbowRelativeSpineShoulderRUserAcc = 0.0;
            this.wristRelativeSpineShoulderLUserAcc = 0.0;
            this.elbowRelativeSpineShoulderLUserAcc = 0.0;

        }

        private void resetAttributeValues()
        {
            this.handLengthR = 0.0;
            this.upperArmLengthR = 0.0;
            this.foreArmLengthR = 0.0;
            this.shoulderLengthR = 0.0;
            this.hipLengthR = 0.0;
            this.upperLegLengthR = 0.0;
            this.shinLengthR = 0.0;
            this.footLengthR = 0.0;

            //left
            this.handLengthL = 0.0;
            this.upperArmLengthL = 0.0;
            this.foreArmLengthL = 0.0;
            this.shoulderLengthL = 0.0;
            this.hipLengthL = 0.0;
            this.upperLegLengthL = 0.0;
            this.shinLengthL = 0.0;
            this.footLengthL = 0.0;

            //general
            this.neckLength = 0.0;
            this.backboneLength = 0.0;
            this.lowerBackLength = 0.0;

            //angles
            this.minHWEAngleR = 360.0;
            this.meanHWEAngleR = 0.0;
            this.maxHWEAngleR = 0.0;
            this.minHWEAngleL = 360.0;
            this.meanHWEAngleL = 0.0;
            this.maxHWEAngleL = 0.0;

            this.minWEShAngleR = 360.0;
            this.meanWEShAngleR = 0.0;
            this.maxWEShAngleR = 0.0;
            this.minWEShAngleL = 360.0;
            this.meanWEShAngleL = 0.0;
            this.maxWEShAngleL = 0.0;

            //relative distances
            this.wristRelativeSpineShoulderR = 0.0;
            this.elbowRelativeSpineShoulderR = 0.0;

            this.wristRelativeSpineShoulderL = 0.0;
            this.elbowRelativeSpineShoulderL = 0.0;

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
            signInStart_btn.Visibility = Visibility.Visible;
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
            signInStart_btn.Visibility = Visibility.Hidden;
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

            resetAttributeValues();
            resetAccumilators();
            
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

                                this.wristRelativeSpineShoulderRAcc += getDistanceBetweenJoints(joints[JointType.WristRight].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.elbowRelativeSpineShoulderRAcc += getDistanceBetweenJoints(joints[JointType.ElbowRight].Position,
                                    joints[JointType.SpineShoulder].Position);

                                this.wristRelativeSpineShoulderLAcc += getDistanceBetweenJoints(joints[JointType.WristLeft].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.elbowRelativeSpineShoulderLAcc += getDistanceBetweenJoints(joints[JointType.ElbowLeft].Position,
                                    joints[JointType.SpineShoulder].Position);

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

                                    this.elbowRelativeSpineShoulderR = elbowRelativeSpineShoulderRAcc / 150;
                                    this.wristRelativeSpineShoulderR = wristRelativeSpineShoulderRAcc / 150;
                                    this.elbowRelativeSpineShoulderL = elbowRelativeSpineShoulderLAcc / 150;
                                    this.wristRelativeSpineShoulderL = wristRelativeSpineShoulderLAcc / 150;

                                    resetAccumilators();

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
                                            //register
                                            if (uniqueUsername && !this.username.Equals("")) 
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
                                                        "','" + this.maxWEShAngleL + 
                                                        "','" + this.wristRelativeSpineShoulderR *100 +
                                                        "','" + this.elbowRelativeSpineShoulderR *100 + 
                                                        "','" + this.wristRelativeSpineShoulderL * 100 +
                                                        "','" + this.elbowRelativeSpineShoulderL * 100 + "')", conn))
                                                {
                                                    SqlDataReader reader = command.ExecuteReader();
                                                    reader.Close();
                                                }

                                            }
                                            conn.Close();
                                        }

                                        if (startClickedCounter == 0)
                                        {
                                            int userRecords = 0;
                                            using (SqlConnection conn = new SqlConnection(connectionString))
                                            {
                                                conn.Open();
                                                SqlDataReader reader;
                                                using (SqlCommand command = new SqlCommand("SELECT *" +
                                                    "FROM Extracted_Kinect_Data " + "WHERE User_Id=" + this.userId,conn))
                                                {
                                                    reader = command.ExecuteReader();
                                                    while(reader.Read())
                                                    {
                                                        this.handLengthRUserAcc += Convert.ToDouble(reader["Hand_Length_R"].ToString());
                                                        this.upperArmLengthRUserAcc += Convert.ToDouble(reader["Upper_Arm_Length_R"].ToString());
                                                        this.foreArmLengthRUserAcc += Convert.ToDouble(reader["Fore_Arm_Length_R"].ToString());
                                                        this.shoulderLengthRUserAcc += Convert.ToDouble(reader["Shoulder_Length_R"].ToString());
                                                        this.handLengthLUserAcc += Convert.ToDouble(reader["Hand_Length_L"].ToString());
                                                        this.upperArmLengthLUserAcc += Convert.ToDouble(reader["Upper_Arm_Length_L"].ToString());
                                                        this.foreArmLengthLUserAcc += Convert.ToDouble(reader["Fore_Arm_Length_L"].ToString());
                                                        this.shoulderLengthLUserAcc += Convert.ToDouble(reader["Shoulder_Length_L"].ToString());
                                                        this.neckLengthUserAcc += Convert.ToDouble(reader["Neck_Length"].ToString());
                                                        this.backboneLengthUserAcc += Convert.ToDouble(reader["Backbone_Length"].ToString());
                                                        this.lowerBackLengthUserAcc += Convert.ToDouble(reader["Lower_Back_Length"].ToString());
                                                        this.hipLengthRUserAcc += Convert.ToDouble(reader["Hip_Length_R"].ToString());
                                                        this.upperLegLengthRUserAcc += Convert.ToDouble(reader["Upper_Leg_Length_R"].ToString());
                                                        this.shinLengthRUserAcc += Convert.ToDouble(reader["Shin_Length_R"].ToString());
                                                        this.footLengthRUserAcc += Convert.ToDouble(reader["Foot_Length_R"].ToString());
                                                        this.hipLengthLUserAcc += Convert.ToDouble(reader["Hip_Length_L"].ToString());
                                                        this.upperLegLengthLUserAcc += Convert.ToDouble(reader["Upper_Leg_Length_L"].ToString());
                                                        this.shinLengthLUserAcc += Convert.ToDouble(reader["Shin_Length_L"].ToString());
                                                        this.footLengthLUserAcc += Convert.ToDouble(reader["Foot_Length_L"].ToString());
                                                        this.minHWEAngleRUserAcc += Convert.ToDouble(reader["Min_HWE_Ang_R"].ToString());
                                                        this.meanHWEAngleRUserAcc += Convert.ToDouble(reader["Mean_HWE_Ang_R"].ToString());
                                                        this.maxHWEAngleRUserAcc += Convert.ToDouble(reader["Max_HWE_Ang_R"].ToString());
                                                        this.minWEShAngleRUserAcc += Convert.ToDouble(reader["Min_WESh_Ang_R"].ToString());
                                                        this.meanWEShAngleRUserAcc += Convert.ToDouble(reader["Mean_WESh_Ang_R"].ToString());
                                                        this.maxWEShAngleRUserAcc += Convert.ToDouble(reader["Max_WESh_Ang_R"].ToString());
                                                        this.minHWEAngleLUserAcc += Convert.ToDouble(reader["Min_HWE_Ang_L"].ToString());
                                                        this.meanHWEAngleLUserAcc += Convert.ToDouble(reader["Mean_HWE_Ang_L"].ToString());
                                                        this.maxHWEAngleLUserAcc += Convert.ToDouble(reader["Max_HWE_Ang_L"].ToString());
                                                        this.minWEShAngleLUserAcc += Convert.ToDouble(reader["Min_WESh_Ang_L"].ToString());
                                                        this.meanWEShAngleLUserAcc += Convert.ToDouble(reader["Mean_WESh_Ang_L"].ToString());
                                                        this.maxWEShAngleLUserAcc += Convert.ToDouble(reader["Max_WESh_Ang_L"].ToString());
                                                        this.wristRelativeSpineShoulderRUserAcc += Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_R"].ToString());
                                                        this.elbowRelativeSpineShoulderRUserAcc += Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_R"].ToString());
                                                        this.wristRelativeSpineShoulderLUserAcc += Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_L"].ToString());
                                                        this.elbowRelativeSpineShoulderLUserAcc += Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_L"].ToString());

                                                    }
                                                    reader.Close();

                                                    using (SqlCommand command1 = new SqlCommand("SELECT COUNT('Id') FROM  Extracted_Kinect_Data" +
                                                        " WHERE User_Id='" + this.userId+"'",conn))
                                                    {
                                                        userRecords = (Int32)command1.ExecuteScalar();
                                                    }
                                                    
                                                }

                                                using (SqlCommand command = new SqlCommand
                                                        ("INSERT INTO Templates VALUES ( 1,'" + this.userId 
                                                        + "','" + this.handLengthRUserAcc / userRecords + "','" + this.upperArmLengthRUserAcc / userRecords + "','" +this.foreArmLengthRUserAcc/ userRecords + "','" + this.shoulderLengthRUserAcc / userRecords
                                                        + "','" + this.handLengthLUserAcc / userRecords + "','" + this.upperArmLengthLUserAcc / userRecords + "','" + this.foreArmLengthLUserAcc / userRecords + "','" + this.shoulderLengthLUserAcc / userRecords
                                                        + "','" + this.neckLengthUserAcc / userRecords + "','" + this.backboneLengthUserAcc / userRecords + "','" + this.lowerBackLengthUserAcc / userRecords + "','" + this.hipLengthRUserAcc / userRecords
                                                        + "','" + this.upperLegLengthRUserAcc / userRecords + "','" + this.shinLengthRUserAcc / userRecords + "','" + this.footLengthRUserAcc / userRecords + "','" + this. hipLengthLUserAcc / userRecords + "','" + this.upperLegLengthLUserAcc / userRecords + "','" + this.shinLengthLUserAcc / userRecords
                                                        + "','" + this.footLengthLUserAcc / userRecords + "','" + this.minHWEAngleRUserAcc / userRecords + "','" + this.meanHWEAngleRUserAcc / userRecords + "','" + this.maxHWEAngleRUserAcc / userRecords + "','" + this.minWEShAngleRUserAcc / userRecords + "','" + this.meanWEShAngleRUserAcc / userRecords
                                                        + "','" + this.maxWEShAngleRUserAcc / userRecords + "','" + this.minHWEAngleLUserAcc / userRecords + "','" + this.meanHWEAngleLUserAcc / userRecords + "','" + this.maxHWEAngleLUserAcc / userRecords
                                                        + "','" + this.minWEShAngleLUserAcc / userRecords + "','" + this.meanWEShAngleLUserAcc / userRecords + "','" + this.maxWEShAngleLUserAcc / userRecords + "','" + this.wristRelativeSpineShoulderRUserAcc / userRecords + "','" + this.elbowRelativeSpineShoulderRUserAcc / userRecords + "','" + this.wristRelativeSpineShoulderLUserAcc / userRecords + "','" + this.elbowRelativeSpineShoulderLUserAcc / userRecords
                                                        + "')", conn))
                                                {
                                                    SqlDataReader reader1 = command.ExecuteReader();
                                                    reader1.Close();
                                                }
                                                reader.Close();
                                                conn.Close();
                                            }
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
                                            resetUserAccumilators();
                                            resetAttributeValues();
                                            resetAccumilators();
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

        private void signInStart_btn_Click(object sender, RoutedEventArgs e)
        {

            signInStart_btn.Visibility = Visibility.Hidden;
            notRegistered_btn.Visibility = Visibility.Hidden;
            // open the reader for the body frames
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // set the BodyFramedArrived event notifier
            this.frameCounter = 0;
            signInStartClicked = true;

            resetAttributeValues();
            resetAccumilators();

            this.bodyFrameReader.FrameArrived += this.SignIn_Reader_BodyFrameArrived;

        }

        private void SignIn_Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
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
                if (signInStartClicked)
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

                                //System.Diagnostics.Debug.WriteLine("body frame triggered");
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

                                this.wristRelativeSpineShoulderRAcc += getDistanceBetweenJoints(joints[JointType.WristRight].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.elbowRelativeSpineShoulderRAcc += getDistanceBetweenJoints(joints[JointType.ElbowRight].Position,
                                    joints[JointType.SpineShoulder].Position);

                                this.wristRelativeSpineShoulderLAcc += getDistanceBetweenJoints(joints[JointType.WristLeft].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.elbowRelativeSpineShoulderLAcc += getDistanceBetweenJoints(joints[JointType.ElbowLeft].Position,
                                    joints[JointType.SpineShoulder].Position);

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

                                    this.elbowRelativeSpineShoulderR = elbowRelativeSpineShoulderRAcc / 150;
                                    this.wristRelativeSpineShoulderR = wristRelativeSpineShoulderRAcc / 150;
                                    this.elbowRelativeSpineShoulderL = elbowRelativeSpineShoulderLAcc / 150;
                                    this.wristRelativeSpineShoulderL = wristRelativeSpineShoulderLAcc / 150;

                                    resetAccumilators();

                                    string connectionString = null;
                                    connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=BachelorProject;Integrated Security=True;Pooling=False";
                                    try
                                    {

                                        using (SqlConnection conn = new SqlConnection(connectionString))
                                        {
                                            conn.Open();
                                            SqlDataReader reader;
                                            SqlDataReader reader1;
                                            double minError = 999999;
                                            double currentError;
                                            int currentId = 0;
                                            using (SqlCommand command = new SqlCommand("SELECT * FROM Templates", conn))
                                            {
                                                reader = command.ExecuteReader();
                                                while (reader.Read())
                                                {
                                                    currentError = Math.Sqrt(Math.Pow(Convert.ToDouble(reader["Hand_Length_R"].ToString()) - this.handLengthR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Upper_Arm_Length_R"].ToString()) - this.upperArmLengthR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Fore_Arm_Length_R"].ToString()) - this.foreArmLengthR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Shoulder_Length_R"].ToString()) - this.shoulderLengthR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Hand_Length_L"].ToString()) - this.handLengthL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Upper_Arm_Length_L"].ToString()) - this.upperArmLengthL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Fore_Arm_Length_L"].ToString()) - this.foreArmLengthL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Shoulder_Length_L"].ToString()) - this.upperArmLengthL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Neck_Length"].ToString()) - this.neckLength, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Backbone_Length"].ToString()) - this.backboneLength, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Lower_Back_Length"].ToString()) - this.lowerBackLength, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Hip_Length_R"].ToString()) - this.hipLengthR, 2) +

                                                        Math.Pow(Convert.ToDouble(reader["Upper_Leg_Length_R"].ToString()) - this.upperLegLengthR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Shin_Length_R"].ToString()) - this.shinLengthR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Foot_Length_R"].ToString()) - this.footLengthR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Hip_Length_L"].ToString()) - this.hipLengthL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Upper_Leg_Length_L"].ToString()) - this.upperLegLengthL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Shin_Length_L"].ToString()) - this.shinLengthL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Foot_Length_L"].ToString()) - this.footLengthL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Min_HWE_Ang_R"].ToString()) - this.minHWEAngleR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Mean_HWE_Ang_R"].ToString()) - this.meanHWEAngleR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Max_HWE_Ang_R"].ToString()) - this.maxHWEAngleR, 2) +

                                                        Math.Pow(Convert.ToDouble(reader["Upper_Arm_Length_R"].ToString()) - this.upperArmLengthR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Upper_Arm_Length_R"].ToString()) - this.upperArmLengthR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Upper_Arm_Length_R"].ToString()) - this.upperArmLengthR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Min_WESh_Ang_R"].ToString()) - this.minWEShAngleR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Mean_WESh_Ang_R"].ToString()) - this.meanWEShAngleR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Max_WESh_Ang_R"].ToString()) - this.maxWEShAngleR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Min_HWE_Ang_L"].ToString()) - this.minHWEAngleL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Mean_HWE_Ang_L"].ToString()) - this.meanHWEAngleL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Max_HWE_Ang_L"].ToString()) - this.maxHWEAngleL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Min_WESh_Ang_L"].ToString()) - this.minWEShAngleL, 2) +

                                                        Math.Pow(Convert.ToDouble(reader["Mean_WESh_Ang_L"].ToString()) - this.meanWEShAngleL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Max_WESh_Ang_L"].ToString()) - this.maxWEShAngleL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_R"].ToString()) - this.wristRelativeSpineShoulderR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_R"].ToString()) - this.elbowRelativeSpineShoulderR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_L"].ToString()) - this.wristRelativeSpineShoulderL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_L"].ToString()) - this.elbowRelativeSpineShoulderL, 2)
                                                        );
                                                    if (currentError < minError)
                                                    {
                                                        minError = currentError;
                                                        currentId = Convert.ToInt32(reader["User_Id"].ToString());
                                                    }

                                                }
                                                reader.Close();
                                            }

                                            if (minError < 150)
                                            {
                                                using (SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Id=" + currentId, conn))
                                                {
                                                    reader1 = command.ExecuteReader();
                                                    if(reader1.Read())
                                                    {
                                                        MessageBox.Show(reader1["User_Name"].ToString());
                                                    }
                                                    reader1.Close();
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("No match found!");
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                    }
                                }
                           }
                        }
                    }
                }
            }
        }

                                
    }
}