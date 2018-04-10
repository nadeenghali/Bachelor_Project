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
using LibSVMsharp;

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
        private BodyFrameReader bodyFrameReader1 = null;
        private string statusText = null;
        private KinectBodyView kinectBodyView = null;
        
        //DB data
        int userId;
        string username = "";
        int sessionId;
        DateTime startFrameTime;
        DateTime signInStartFrameTime;
        bool inserted = true;
        bool compared = true;

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
        double wristRelativeSpineShoulderRx = 0.0;
        double elbowRelativeSpineShoulderRx = 0.0;

        double wristRelativeSpineShoulderLx = 0.0;
        double elbowRelativeSpineShoulderLx = 0.0;

        double wristRelativeSpineShoulderRy = 0.0;
        double elbowRelativeSpineShoulderRy = 0.0;

        double wristRelativeSpineShoulderLy = 0.0;
        double elbowRelativeSpineShoulderLy = 0.0;

        double wristRelativeSpineShoulderRz = 0.0;
        double elbowRelativeSpineShoulderRz = 0.0;

        double wristRelativeSpineShoulderLz = 0.0;
        double elbowRelativeSpineShoulderLz = 0.0;


        //velocity and acceleration
        double wristVelocity = 0.0;
        double handVelocity = 0.0;
        double wristAcceleration = 0.0;

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
        double wristRelativeSpineShoulderRxAcc = 0.0;
        double elbowRelativeSpineShoulderRxAcc = 0.0;

        double wristRelativeSpineShoulderLxAcc = 0.0;
        double elbowRelativeSpineShoulderLxAcc = 0.0;

        double wristRelativeSpineShoulderRyAcc = 0.0;
        double elbowRelativeSpineShoulderRyAcc = 0.0;

        double wristRelativeSpineShoulderLyAcc = 0.0;
        double elbowRelativeSpineShoulderLyAcc = 0.0;

        double wristRelativeSpineShoulderRzAcc = 0.0;
        double elbowRelativeSpineShoulderRzAcc = 0.0;

        double wristRelativeSpineShoulderLzAcc = 0.0;
        double elbowRelativeSpineShoulderLzAcc = 0.0;

        //velocity
        double wristVelocityAcc = 0.0;
        double handVelocityAcc = 0.0;
        double wristAccelerationAcc = 0.0;

        double oldWristVelocity = 0.0;
        double newWristVelocity = 0.0;

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

        double wristRelativeSpineShoulderRxUserAcc;
        double elbowRelativeSpineShoulderRxUserAcc;
        double wristRelativeSpineShoulderLxUserAcc;
        double elbowRelativeSpineShoulderLxUserAcc;

        double wristRelativeSpineShoulderRyUserAcc;
        double elbowRelativeSpineShoulderRyUserAcc;
        double wristRelativeSpineShoulderLyUserAcc;
        double elbowRelativeSpineShoulderLyUserAcc;

        double wristRelativeSpineShoulderRzUserAcc;
        double elbowRelativeSpineShoulderRzUserAcc;
        double wristRelativeSpineShoulderLzUserAcc;
        double elbowRelativeSpineShoulderLzUserAcc;

        double wristVelocityUserAcc;
        double handVelocityUserAcc;
        double wristAccelerationUserAcc;
        
        //counters and flags
        bool uniqueUsername = false;
        int frameCounter = 0;
        bool startClicked = false;
        bool signInStartClicked = false;
        int startClickedCounter = 5;

        //velocity attributes
        CameraSpacePoint oldWristPos;
        CameraSpacePoint oldHandPos;
        DateTime oldFrameTime;

        public MyApp()
        {
            this.kinectSensor = KinectSensor.GetDefault();
            this.kinectSensor.Open();
            this.statusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            this.bodyFrameReader1 = this.kinectSensor.BodyFrameSource.OpenReader();
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
            this.wristRelativeSpineShoulderRxAcc = 0.0;
            this.elbowRelativeSpineShoulderRxAcc = 0.0;

            this.wristRelativeSpineShoulderLxAcc = 0.0;
            this.elbowRelativeSpineShoulderLxAcc = 0.0;

            this.wristRelativeSpineShoulderRyAcc = 0.0;
            this.elbowRelativeSpineShoulderRyAcc = 0.0;

            this.wristRelativeSpineShoulderLyAcc = 0.0;
            this.elbowRelativeSpineShoulderLyAcc = 0.0;

            this.wristRelativeSpineShoulderRzAcc = 0.0;
            this.elbowRelativeSpineShoulderRzAcc = 0.0;

            this.wristRelativeSpineShoulderLzAcc = 0.0;
            this.elbowRelativeSpineShoulderLzAcc = 0.0;

            //velocity
            this.wristVelocityAcc = 0.0;
            this.handVelocityAcc = 0.0;
            this.wristAccelerationAcc = 0.0;
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

            this.wristRelativeSpineShoulderRxUserAcc = 0.0;
            this.elbowRelativeSpineShoulderRxUserAcc = 0.0;
            this.wristRelativeSpineShoulderLxUserAcc = 0.0;
            this.elbowRelativeSpineShoulderLxUserAcc = 0.0;

            this.wristRelativeSpineShoulderRyUserAcc = 0.0;
            this.elbowRelativeSpineShoulderRyUserAcc = 0.0;
            this.wristRelativeSpineShoulderLyUserAcc = 0.0;
            this.elbowRelativeSpineShoulderLyUserAcc = 0.0;

            this.wristRelativeSpineShoulderRzUserAcc = 0.0;
            this.elbowRelativeSpineShoulderRzUserAcc = 0.0;
            this.wristRelativeSpineShoulderLzUserAcc = 0.0;
            this.elbowRelativeSpineShoulderLzUserAcc = 0.0;

            this.wristVelocityUserAcc = 0.0;
            this.handVelocityUserAcc = 0.0;
            this.wristAccelerationUserAcc = 0.0;
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
            this.wristRelativeSpineShoulderRx = 0.0;
            this.elbowRelativeSpineShoulderRx = 0.0;

            this.wristRelativeSpineShoulderLx = 0.0;
            this.elbowRelativeSpineShoulderLx = 0.0;


            this.wristRelativeSpineShoulderRy = 0.0;
            this.elbowRelativeSpineShoulderRy = 0.0;

            this.wristRelativeSpineShoulderLy = 0.0;
            this.elbowRelativeSpineShoulderLy = 0.0;


            this.wristRelativeSpineShoulderRz = 0.0;
            this.elbowRelativeSpineShoulderRz = 0.0;

            this.wristRelativeSpineShoulderLz = 0.0;
            this.elbowRelativeSpineShoulderLz = 0.0;

            this.wristVelocity = 0.0;
            this.handVelocity = 0.0;
            this.wristAcceleration = 0.0;
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
            string connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=GestureAuthenticationDB1;Integrated Security=True;Pooling=False";
            this.frameCounter = 0;
            this.startClickedCounter = 5;
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
               message += "Oops database connection failed! -" + ex.ToString();
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

            this.frameCounter = 0;
            startClicked = true;
            startClickedCounter -= 1;
            startFrameTime = DateTime.Now;
            inserted = false;

            resetAttributeValues();
            resetAccumilators();
            
            // set the BodyFramedArrived event notifier
            this.bodyFrameReader.FrameArrived += this.register_Reader_BodyFrameArrived;
        }

        private void register_Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
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

                                this.handLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.WristRight].Position,
                                    joints[JointType.HandTipRight].Position);
                                this.foreArmLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ElbowRight].Position,
                                    joints[JointType.WristRight].Position);
                                this.upperArmLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ShoulderRight].Position,
                                    joints[JointType.ElbowRight].Position);
                                this.shoulderLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ShoulderRight].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.hipLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.HipRight].Position,
                                    joints[JointType.SpineBase].Position);
                                this.upperLegLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.HipRight].Position,
                                    joints[JointType.KneeRight].Position);
                                this.shinLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.AnkleRight].Position,
                                    joints[JointType.KneeRight].Position);
                                this.footLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.AnkleRight].Position,
                                    joints[JointType.FootRight].Position);


                                this.handLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.WristLeft].Position,
                                    joints[JointType.HandTipLeft].Position);
                                this.foreArmLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ElbowLeft].Position,
                                    joints[JointType.WristLeft].Position);
                                this.upperArmLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ShoulderLeft].Position,
                                    joints[JointType.ElbowLeft].Position);
                                this.shoulderLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ShoulderLeft].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.hipLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.HipLeft].Position,
                                    joints[JointType.SpineBase].Position);
                                this.upperLegLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.HipLeft].Position,
                                    joints[JointType.KneeLeft].Position);
                                this.shinLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.AnkleLeft].Position,
                                    joints[JointType.KneeLeft].Position);
                                this.footLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.AnkleLeft].Position,
                                    joints[JointType.FootLeft].Position);

                                this.neckLengthAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.Head].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.backboneLengthAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.SpineMid].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.lowerBackLengthAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.SpineMid].Position,
                                    joints[JointType.SpineBase].Position);

                                this.wristRelativeSpineShoulderRxAcc += joints[JointType.WristRight].Position.X -
                                    joints[JointType.SpineShoulder].Position.X;
                                this.elbowRelativeSpineShoulderRxAcc += joints[JointType.ElbowRight].Position.X -
                                    joints[JointType.SpineShoulder].Position.X;

                                this.wristRelativeSpineShoulderLxAcc += joints[JointType.WristLeft].Position.X -
                                    joints[JointType.SpineShoulder].Position.X;
                                this.elbowRelativeSpineShoulderLxAcc += joints[JointType.ElbowLeft].Position.X -
                                    joints[JointType.SpineShoulder].Position.X;

                                this.wristRelativeSpineShoulderRyAcc += joints[JointType.WristRight].Position.Y -
                                    joints[JointType.SpineShoulder].Position.Y;
                                this.elbowRelativeSpineShoulderRyAcc += joints[JointType.ElbowRight].Position.Y -
                                    joints[JointType.SpineShoulder].Position.Y;

                                this.wristRelativeSpineShoulderLyAcc += joints[JointType.WristLeft].Position.Y -
                                    joints[JointType.SpineShoulder].Position.Y;
                                this.elbowRelativeSpineShoulderLyAcc += joints[JointType.ElbowLeft].Position.Y -
                                    joints[JointType.SpineShoulder].Position.Y;

                                this.wristRelativeSpineShoulderRzAcc += joints[JointType.WristRight].Position.Z -
                                    joints[JointType.SpineShoulder].Position.Z;
                                this.elbowRelativeSpineShoulderRzAcc += joints[JointType.ElbowRight].Position.Z -
                                    joints[JointType.SpineShoulder].Position.Z;

                                this.wristRelativeSpineShoulderLzAcc += joints[JointType.WristLeft].Position.Z -
                                    joints[JointType.SpineShoulder].Position.Z;
                                this.elbowRelativeSpineShoulderLzAcc += joints[JointType.ElbowLeft].Position.Z -
                                    joints[JointType.SpineShoulder].Position.Z;

                                double HWEAngleL = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.ElbowLeft].Position,
                                    joints[JointType.WristLeft].Position,
                                    joints[JointType.ThumbLeft].Position);
                                
                                double WEShAngleL = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.WristLeft].Position,
                                    joints[JointType.ElbowLeft].Position,
                                    joints[JointType.ShoulderLeft].Position);

                                double HWEAngleR = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.ElbowRight].Position,
                                    joints[JointType.WristRight].Position,
                                    joints[JointType.ThumbRight].Position);

                                double WEShAngleR = HelperMethods.getAngleAtMiddleJoint
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
                                
                                if(frameCounter == 1)
                                {
                                    this.wristVelocityAcc = 0;
                                    this.handVelocityAcc = 0;
                                    this.oldWristVelocity = 0;
                                }
                                else
                                {
                                    this.newWristVelocity = HelperMethods.getVelocity(this.oldWristPos, joints[JointType.WristRight].Position, DateTime.Now - oldFrameTime);
                                    this.handVelocityAcc += HelperMethods.getVelocity(this.oldHandPos, joints[JointType.HandRight].Position, DateTime.Now - oldFrameTime);
                                    this.wristVelocityAcc += this.newWristVelocity;
                                    this.wristAccelerationAcc += HelperMethods.getAcceleration(this.oldWristVelocity, this.newWristVelocity, DateTime.Now - oldFrameTime);

                                    this.oldWristVelocity = this.newWristVelocity;
                                }

                                this.oldFrameTime = DateTime.Now;
                                this.oldWristPos = joints[JointType.WristRight].Position;
                                this.oldHandPos = joints[JointType.HandRight].Position;

                                if (((DateTime.Now) - startFrameTime).Seconds == 3 && !inserted)
                                {
                                    inserted = true;
                                    this.handLengthR = handLengthRAcc / frameCounter;
                                    this.upperArmLengthR = upperArmLengthRAcc / frameCounter;
                                    this.foreArmLengthR = foreArmLengthRAcc / frameCounter;
                                    this.shoulderLengthR = shoulderLengthRAcc / frameCounter;
                                    this.hipLengthR = hipLengthRAcc / frameCounter;
                                    this.upperLegLengthR = upperLegLengthRAcc / frameCounter;
                                    this.shinLengthR = shinLengthRAcc / frameCounter;
                                    this.footLengthR = footLengthRAcc / frameCounter;
                                    
                                    this.handLengthL = handLengthLAcc / frameCounter;
                                    this.upperArmLengthL = upperArmLengthLAcc / frameCounter;
                                    this.foreArmLengthL = foreArmLengthLAcc / frameCounter;
                                    this.shoulderLengthL = shoulderLengthLAcc / frameCounter;
                                    this.hipLengthL = hipLengthLAcc / frameCounter;
                                    this.upperLegLengthL = upperLegLengthLAcc / frameCounter;
                                    this.shinLengthL = shinLengthLAcc / frameCounter;
                                    this.footLengthL = footLengthLAcc / frameCounter;

                                    this.neckLength = neckLengthAcc / frameCounter;
                                    this.backboneLength = backboneLengthAcc / frameCounter;
                                    this.lowerBackLength = lowerBackLengthAcc / frameCounter;

                                    this.meanHWEAngleL = meanHWEAngleLAcc / frameCounter;
                                    this.meanHWEAngleR = meanHWEAngleRAcc / frameCounter;
                                    this.meanWEShAngleL = meanWEShAngleLAcc / frameCounter;
                                    this.meanWEShAngleR = meanWEShAngleRAcc / frameCounter;

                                    this.elbowRelativeSpineShoulderRx = elbowRelativeSpineShoulderRxAcc / frameCounter;
                                    this.wristRelativeSpineShoulderRx = wristRelativeSpineShoulderRxAcc / frameCounter;
                                    this.elbowRelativeSpineShoulderLx = elbowRelativeSpineShoulderLxAcc / frameCounter;
                                    this.wristRelativeSpineShoulderLx = wristRelativeSpineShoulderLxAcc / frameCounter;

                                    this.elbowRelativeSpineShoulderRy = elbowRelativeSpineShoulderRyAcc / frameCounter;
                                    this.wristRelativeSpineShoulderRy = wristRelativeSpineShoulderRyAcc / frameCounter;
                                    this.elbowRelativeSpineShoulderLy = elbowRelativeSpineShoulderLyAcc / frameCounter;
                                    this.wristRelativeSpineShoulderLy = wristRelativeSpineShoulderLyAcc / frameCounter;

                                    this.elbowRelativeSpineShoulderRz = elbowRelativeSpineShoulderRzAcc / frameCounter;
                                    this.wristRelativeSpineShoulderRz = wristRelativeSpineShoulderRzAcc / frameCounter;
                                    this.elbowRelativeSpineShoulderLz = elbowRelativeSpineShoulderLzAcc / frameCounter;
                                    this.wristRelativeSpineShoulderLz = wristRelativeSpineShoulderLzAcc / frameCounter;

                                    this.wristVelocity = (wristVelocityAcc / (frameCounter - 1));
                                    this.handVelocity = (handVelocityAcc / (frameCounter - 1));

                                    this.wristAcceleration = (wristAccelerationAcc / (frameCounter - 1));

                                    resetAccumilators();

                                    string connectionString = null;
                                    connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=GestureAuthenticationDB1;Integrated Security=True;Pooling=False";
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
                                                        "','" + this.wristRelativeSpineShoulderRx *100 +
                                                        "','" + this.elbowRelativeSpineShoulderRx *100 + 
                                                        "','" + this.wristRelativeSpineShoulderLx * 100 +
                                                        "','" + this.elbowRelativeSpineShoulderLx * 100 +
                                                        "','" + this.wristRelativeSpineShoulderRy * 100 +
                                                        "','" + this.elbowRelativeSpineShoulderRy * 100 +
                                                        "','" + this.wristRelativeSpineShoulderLy * 100 +
                                                        "','" + this.elbowRelativeSpineShoulderLy * 100 +
                                                        "','" + this.wristRelativeSpineShoulderRz * 100 +
                                                        "','" + this.elbowRelativeSpineShoulderRz * 100 +
                                                        "','" + this.wristRelativeSpineShoulderLz * 100 +
                                                        "','" + this.elbowRelativeSpineShoulderLz * 100 +
                                                        "','" + this.wristVelocity +
                                                        "','" + this.handVelocity +
                                                        "','" + this.wristAcceleration + "')", conn))
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
                                                        this.wristRelativeSpineShoulderRxUserAcc += Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Rx"].ToString());
                                                        this.elbowRelativeSpineShoulderRxUserAcc += Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Rx"].ToString());
                                                        this.wristRelativeSpineShoulderLxUserAcc += Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Lx"].ToString());
                                                        this.elbowRelativeSpineShoulderLxUserAcc += Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Lx"].ToString());
                                                        this.wristRelativeSpineShoulderRyUserAcc += Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Ry"].ToString());
                                                        this.elbowRelativeSpineShoulderRyUserAcc += Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Ry"].ToString());
                                                        this.wristRelativeSpineShoulderLyUserAcc += Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Ly"].ToString());
                                                        this.elbowRelativeSpineShoulderLyUserAcc += Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Ly"].ToString());
                                                        this.wristRelativeSpineShoulderRzUserAcc += Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Rz"].ToString());
                                                        this.elbowRelativeSpineShoulderRzUserAcc += Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Rz"].ToString());
                                                        this.wristRelativeSpineShoulderLzUserAcc += Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Lz"].ToString());
                                                        this.elbowRelativeSpineShoulderLzUserAcc += Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Lx"].ToString());

                                                        this.wristVelocityUserAcc += Convert.ToDouble(reader["Wrist_Velocity_R"].ToString());
                                                        this.handVelocityUserAcc += Convert.ToDouble(reader["Hand_Velocity_R"].ToString());
                                                        this.wristAccelerationUserAcc += Convert.ToDouble(reader["Wrist_Acceleration_R"].ToString());
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
                                                        + "','" + this.minWEShAngleLUserAcc / userRecords + "','" + this.meanWEShAngleLUserAcc / userRecords + "','" + this.maxWEShAngleLUserAcc / userRecords 
                                                        + "','" + this.wristRelativeSpineShoulderRxUserAcc / userRecords 
                                                        + "','" + this.elbowRelativeSpineShoulderRxUserAcc / userRecords 
                                                        + "','" + this.wristRelativeSpineShoulderLxUserAcc / userRecords 
                                                        + "','" + this.elbowRelativeSpineShoulderLxUserAcc / userRecords
                                                        + "','" + this.wristRelativeSpineShoulderRyUserAcc / userRecords
                                                        + "','" + this.elbowRelativeSpineShoulderRyUserAcc / userRecords
                                                        + "','" + this.wristRelativeSpineShoulderLyUserAcc / userRecords
                                                        + "','" + this.elbowRelativeSpineShoulderLyUserAcc / userRecords
                                                        + "','" + this.wristRelativeSpineShoulderRzUserAcc / userRecords
                                                        + "','" + this.elbowRelativeSpineShoulderRzUserAcc / userRecords
                                                        + "','" + this.wristRelativeSpineShoulderLzUserAcc / userRecords
                                                        + "','" + this.elbowRelativeSpineShoulderLzUserAcc / userRecords
                                                        + "','" + this.wristVelocityUserAcc / userRecords
                                                        + "','" + this.handVelocityUserAcc / userRecords
                                                        + "','" + this.wristAccelerationUserAcc / userRecords
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
                                    this.bodyFrameReader.FrameArrived -= this.register_Reader_BodyFrameArrived;
                                    this.bodyFrameReader.Dispose();
                                    this.bodyFrameReader = null;
                                    // break;
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
            this.bodyFrameReader1 = this.kinectSensor.BodyFrameSource.OpenReader();

            // set the BodyFramedArrived event notifier
            this.frameCounter = 0;
            signInStartClicked = true;
            signInStartFrameTime = DateTime.Now;
            compared = false;
            resetAttributeValues();
            resetAccumilators();

            this.bodyFrameReader1.FrameArrived += this.signIn_Reader_BodyFrameArrived;

        }

        private void signIn_Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
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

                                this.handLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.WristRight].Position,
                                    joints[JointType.HandTipRight].Position);
                                this.foreArmLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ElbowRight].Position,
                                    joints[JointType.WristRight].Position);
                                this.upperArmLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ShoulderRight].Position,
                                    joints[JointType.ElbowRight].Position);
                                this.shoulderLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ShoulderRight].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.hipLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.HipRight].Position,
                                    joints[JointType.SpineBase].Position);
                                this.upperLegLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.HipRight].Position,
                                    joints[JointType.KneeRight].Position);
                                this.shinLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.AnkleRight].Position,
                                    joints[JointType.KneeRight].Position);
                                this.footLengthRAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.AnkleRight].Position,
                                    joints[JointType.FootRight].Position);


                                this.handLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.WristLeft].Position,
                                    joints[JointType.HandTipLeft].Position);
                                this.foreArmLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ElbowLeft].Position,
                                    joints[JointType.WristLeft].Position);
                                this.upperArmLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ShoulderLeft].Position,
                                    joints[JointType.ElbowLeft].Position);
                                this.shoulderLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.ShoulderLeft].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.hipLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.HipLeft].Position,
                                    joints[JointType.SpineBase].Position);
                                this.upperLegLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.HipLeft].Position,
                                    joints[JointType.KneeLeft].Position);
                                this.shinLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.AnkleLeft].Position,
                                    joints[JointType.KneeLeft].Position);
                                this.footLengthLAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.AnkleLeft].Position,
                                    joints[JointType.FootLeft].Position);

                                this.neckLengthAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.Head].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.backboneLengthAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.SpineMid].Position,
                                    joints[JointType.SpineShoulder].Position);
                                this.lowerBackLengthAcc += HelperMethods.getDistanceBetweenJoints(joints[JointType.SpineMid].Position,
                                    joints[JointType.SpineBase].Position);
                                this.wristRelativeSpineShoulderRxAcc += joints[JointType.WristRight].Position.X -
                                                                   joints[JointType.SpineShoulder].Position.X;
                                this.elbowRelativeSpineShoulderRxAcc += joints[JointType.ElbowRight].Position.X -
                                    joints[JointType.SpineShoulder].Position.X;

                                this.wristRelativeSpineShoulderLxAcc += joints[JointType.WristLeft].Position.X -
                                    joints[JointType.SpineShoulder].Position.X;
                                this.elbowRelativeSpineShoulderLxAcc += joints[JointType.ElbowLeft].Position.X -
                                    joints[JointType.SpineShoulder].Position.X;

                                this.wristRelativeSpineShoulderRyAcc += joints[JointType.WristRight].Position.Y -
                                    joints[JointType.SpineShoulder].Position.Y;
                                this.elbowRelativeSpineShoulderRyAcc += joints[JointType.ElbowRight].Position.Y -
                                    joints[JointType.SpineShoulder].Position.Y;

                                this.wristRelativeSpineShoulderLyAcc += joints[JointType.WristLeft].Position.Y -
                                    joints[JointType.SpineShoulder].Position.Y;
                                this.elbowRelativeSpineShoulderLyAcc += joints[JointType.ElbowLeft].Position.Y -
                                    joints[JointType.SpineShoulder].Position.Y;

                                this.wristRelativeSpineShoulderRzAcc += joints[JointType.WristRight].Position.Z -
                                    joints[JointType.SpineShoulder].Position.Z;
                                this.elbowRelativeSpineShoulderRzAcc += joints[JointType.ElbowRight].Position.Z -
                                    joints[JointType.SpineShoulder].Position.Z;

                                this.wristRelativeSpineShoulderLzAcc += joints[JointType.WristLeft].Position.Z -
                                    joints[JointType.SpineShoulder].Position.Z;
                                this.elbowRelativeSpineShoulderLzAcc += joints[JointType.ElbowLeft].Position.Z -
                                    joints[JointType.SpineShoulder].Position.Z;

                                double HWEAngleL = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.ElbowLeft].Position,
                                    joints[JointType.WristLeft].Position,
                                    joints[JointType.ThumbLeft].Position);


                                double WEShAngleL = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.WristLeft].Position,
                                    joints[JointType.ElbowLeft].Position,
                                    joints[JointType.ShoulderLeft].Position);

                                double HWEAngleR = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.ElbowRight].Position,
                                    joints[JointType.WristRight].Position,
                                    joints[JointType.ThumbRight].Position);

                                double WEShAngleR = HelperMethods.getAngleAtMiddleJoint
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

                                if (frameCounter == 1)
                                {
                                    this.wristVelocityAcc = 0;
                                    this.handVelocityAcc = 0;
                                    this.oldWristVelocity = 0;
                                }
                                else
                                {
                                    this.newWristVelocity = HelperMethods.getVelocity(this.oldWristPos, joints[JointType.WristRight].Position, DateTime.Now - oldFrameTime);
                                    this.handVelocityAcc += HelperMethods.getVelocity(this.oldHandPos, joints[JointType.HandRight].Position, DateTime.Now - oldFrameTime);
                                    this.wristVelocityAcc += this.newWristVelocity;
                                    this.wristAccelerationAcc += HelperMethods.getAcceleration(this.oldWristVelocity, this.newWristVelocity, DateTime.Now - oldFrameTime);

                                    this.oldWristVelocity = this.newWristVelocity;
                                }

                                this.oldFrameTime = DateTime.Now;
                                this.oldWristPos = joints[JointType.WristRight].Position;
                                this.oldHandPos = joints[JointType.HandRight].Position;

                                if (((DateTime.Now) - signInStartFrameTime).Seconds == 3 && !compared)
                                {
                                    compared = true;
                                    this.handLengthR = handLengthRAcc / frameCounter;
                                    this.upperArmLengthR = upperArmLengthRAcc / frameCounter;
                                    this.foreArmLengthR = foreArmLengthRAcc / frameCounter;
                                    this.shoulderLengthR = shoulderLengthRAcc / frameCounter;
                                    this.hipLengthR = hipLengthRAcc / frameCounter;
                                    this.upperLegLengthR = upperLegLengthRAcc / frameCounter;
                                    this.shinLengthR = shinLengthRAcc / frameCounter;
                                    this.footLengthR = footLengthRAcc / frameCounter;

                                    this.handLengthL = handLengthLAcc / frameCounter;
                                    this.upperArmLengthL = upperArmLengthLAcc / frameCounter;
                                    this.foreArmLengthL = foreArmLengthLAcc / frameCounter;
                                    this.shoulderLengthL = shoulderLengthLAcc / frameCounter;
                                    this.hipLengthL = hipLengthLAcc / frameCounter;
                                    this.upperLegLengthL = upperLegLengthLAcc / frameCounter;
                                    this.shinLengthL = shinLengthLAcc / frameCounter;
                                    this.footLengthL = footLengthLAcc / frameCounter;

                                    this.neckLength = neckLengthAcc / frameCounter;
                                    this.backboneLength = backboneLengthAcc / frameCounter;
                                    this.lowerBackLength = lowerBackLengthAcc / frameCounter;

                                    this.meanHWEAngleL = meanHWEAngleLAcc / frameCounter;
                                    this.meanHWEAngleR = meanHWEAngleRAcc / frameCounter;
                                    this.meanWEShAngleL = meanWEShAngleLAcc / frameCounter;
                                    this.meanWEShAngleR = meanWEShAngleRAcc / frameCounter;

                                    this.elbowRelativeSpineShoulderRx = elbowRelativeSpineShoulderRxAcc / frameCounter;
                                    this.wristRelativeSpineShoulderRx = wristRelativeSpineShoulderRxAcc / frameCounter;
                                    this.elbowRelativeSpineShoulderLx = elbowRelativeSpineShoulderLxAcc / frameCounter;
                                    this.wristRelativeSpineShoulderLx = wristRelativeSpineShoulderLxAcc / frameCounter;

                                    this.elbowRelativeSpineShoulderRy = elbowRelativeSpineShoulderRyAcc / frameCounter;
                                    this.wristRelativeSpineShoulderRy = wristRelativeSpineShoulderRyAcc / frameCounter;
                                    this.elbowRelativeSpineShoulderLy = elbowRelativeSpineShoulderLyAcc / frameCounter;
                                    this.wristRelativeSpineShoulderLy = wristRelativeSpineShoulderLyAcc / frameCounter;

                                    this.elbowRelativeSpineShoulderRz = elbowRelativeSpineShoulderRzAcc / frameCounter;
                                    this.wristRelativeSpineShoulderRz = wristRelativeSpineShoulderRzAcc / frameCounter;
                                    this.elbowRelativeSpineShoulderLz = elbowRelativeSpineShoulderLzAcc / frameCounter;
                                    this.wristRelativeSpineShoulderLz = wristRelativeSpineShoulderLzAcc / frameCounter;


                                    this.wristVelocity = wristVelocityAcc / (frameCounter - 1);
                                    this.handVelocity = handVelocityAcc / (frameCounter - 1);
                                    this.wristAcceleration = wristAccelerationAcc / (frameCounter - 1);

                                    resetAccumilators();

                                    string connectionString = null;
                                    connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=GestureAuthenticationDB1;Integrated Security=True;Pooling=False";
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

                                                        //Math.Pow(Convert.ToDouble(reader["Hip_Length_R"].ToString()) - this.hipLengthR, 2) +
                                                        //Math.Pow(Convert.ToDouble(reader["Upper_Leg_Length_R"].ToString()) - this.upperLegLengthR, 2) +
                                                        //Math.Pow(Convert.ToDouble(reader["Shin_Length_R"].ToString()) - this.shinLengthR, 2) +
                                                        //Math.Pow(Convert.ToDouble(reader["Foot_Length_R"].ToString()) - this.footLengthR, 2) +
                                                        //Math.Pow(Convert.ToDouble(reader["Hip_Length_L"].ToString()) - this.hipLengthL, 2) +
                                                        //Math.Pow(Convert.ToDouble(reader["Upper_Leg_Length_L"].ToString()) - this.upperLegLengthL, 2) +
                                                        //Math.Pow(Convert.ToDouble(reader["Shin_Length_L"].ToString()) - this.shinLengthL, 2) +
                                                        //Math.Pow(Convert.ToDouble(reader["Foot_Length_L"].ToString()) - this.footLengthL, 2) +

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
                                                        Math.Pow(Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Rx"].ToString()) - this.wristRelativeSpineShoulderRx, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Rx"].ToString()) - this.elbowRelativeSpineShoulderRx, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Lx"].ToString()) - this.wristRelativeSpineShoulderLx, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Lx"].ToString()) - this.elbowRelativeSpineShoulderLx, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Ry"].ToString()) - this.wristRelativeSpineShoulderRy, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Ry"].ToString()) - this.elbowRelativeSpineShoulderRy, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Ly"].ToString()) - this.wristRelativeSpineShoulderLy, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Ly"].ToString()) - this.elbowRelativeSpineShoulderLy, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Rz"].ToString()) - this.wristRelativeSpineShoulderRz, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Rz"].ToString()) - this.elbowRelativeSpineShoulderRz, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Wrist_Relative_SpineShoulder_Lz"].ToString()) - this.wristRelativeSpineShoulderLz, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Lz"].ToString()) - this.elbowRelativeSpineShoulderLz, 2) +
                                                        Math.Pow((Convert.ToDouble(reader["Wrist_Velocity_R"].ToString())) - this.wristVelocity, 2) +
                                                        Math.Pow((Convert.ToDouble(reader["Hand_Velocity_R"].ToString())) - this.handVelocity, 2)+
                                                        Math.Pow((Convert.ToDouble(reader["Wrist_Acceleration_R"].ToString())) - this.wristAcceleration, 2)
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
                                                        MessageBox.Show(reader1["User_Name"].ToString()
                                                            +"\n Wrist Velocity:"
                                                            +this.wristVelocity 
                                                            + "\n Wrist Acceleration:"
                                                            + this.wristAcceleration);
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
                                    this.bodyFrameReader1.FrameArrived -= this.signIn_Reader_BodyFrameArrived;
                                    this.bodyFrameReader1.Dispose();
                                    this.bodyFrameReader1 = null;

                                    signInStart_btn.Visibility = Visibility.Visible;
                                    notRegistered_btn.Visibility = Visibility.Visible;
                                }
                           }
                        }
                    }
                }
            }
        }                        
    }
}