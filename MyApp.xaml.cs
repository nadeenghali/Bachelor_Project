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
        bool train = false;
        bool register = false;

        //DB data
        int userId;
        string username = "";
        string gestureName = "";
        int gestureId;
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

        double minEShSAngleR = 360.0;
        double meanEShSAngleR = 0.0;
        double maxEShSAngleR = 0.0;
        double minEShSAngleL = 360.0;
        double meanEShSAngleL = 0.0;
        double maxEShSAngleL = 0.0;

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
        double wristVelocityR = 0.0;
        double handVelocityR = 0.0;
        double wristVelocityL = 0.0;
        double handVelocityL = 0.0;

        double wristAccelerationR = 0.0;
        double wristAccelerationL = 0.0;


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
        double meanEShSAngleRAcc = 0.0;
        double meanEShSAngleLAcc = 0.0;

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
        double wristVelocityAccR = 0.0;
        double handVelocityAccR = 0.0;
        double wristVelocityAccL = 0.0;
        double handVelocityAccL = 0.0;

        double wristAccelerationAccR = 0.0;
        double wristAccelerationAccL = 0.0;

        double oldWristVelocityR = 0.0;
        double newWristVelocityR = 0.0;
        
        double oldWristVelocityL = 0.0;
        double newWristVelocityL = 0.0;

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

        double minEShSAngleRUserAcc;
        double meanEShSAngleRUserAcc;
        double maxEShSAngleRUserAcc;

        double meanHWEAngleLUserAcc;
        double maxHWEAngleLUserAcc;
        double minHWEAngleLUserAcc;

        double minWEShAngleLUserAcc;
        double meanWEShAngleLUserAcc;
        double maxWEShAngleLUserAcc;

        double minEShSAngleLUserAcc;
        double meanEShSAngleLUserAcc;
        double maxEShSAngleLUserAcc;

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

        double wristVelocityUserAccR;
        double handVelocityUserAccR;
        double wristVelocityUserAccL;
        double handVelocityUserAccL;

        double wristAccelerationUserAccR;
        double wristAccelerationUserAccL;

        //counters and flags
        bool uniqueUsername = false;
        int frameCounter = 0;
        bool startClicked = false;
        bool signInStartClicked = false;
        int startClickedCounter = 10;

        //velocity attributes
        CameraSpacePoint oldWristPosR;
        CameraSpacePoint oldHandPosR;


        CameraSpacePoint oldWristPosL;
        CameraSpacePoint oldHandPosL;

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

            gestureComboBox.Items.Add("1HUr");
            gestureComboBox.Items.Add("1HUl");
            gestureComboBox.Items.Add("2HU");
            gestureComboBox.Items.Add("1HRUr");
            gestureComboBox.Items.Add("1HRUl");
            gestureComboBox.Items.Add("HTW");
            gestureComboBox.Items.Add("HOH");
            gestureComboBox.Items.Add("Wr");
            gestureComboBox.Items.Add("Wl");
            gestureComboBox.Items.Add("T");
            gestureComboBox.Items.Add("PH");
            gestureComboBox.Items.Add("HC");

            //string connectionString = null;
            //connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=KinectDatabase;Integrated Security=True;Pooling=False";
            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    conn.Open();
            //    using (SqlCommand command2 = new SqlCommand
            //                                    ("SELECT * FROM Extracted_Kinect_Data", conn))
            //    {
            //        using (SqlDataReader reader2 = command2.ExecuteReader())
            //        {
            //            do
            //            {
            //                System.IO.File.AppendAllText("C:\\Users\\Nadeen\\Documents\\Visual Studio 2015\\Projects\\DiscreteGestureBasics-WPF\\KinectDataset.csv", HelperMethods.EKDtblReaderToCSV(reader2, false, ","));
            //            }
            //            while (!reader2.IsClosed && reader2.Read());
            //            reader2.Close();
            //            conn.Close();
            //        }
            //    }
            //}
            
            //connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=KinectDatabase1;Integrated Security=True;Pooling=False";
            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    conn.Open();
            //    using (SqlCommand command2 = new SqlCommand
            //                                    ("SELECT * FROM Extracted_Kinect_Data", conn))
            //    {
            //        using (SqlDataReader reader2 = command2.ExecuteReader())
            //        {
            //            do
            //            {
            //                System.IO.File.AppendAllText("C:\\Users\\Nadeen\\Documents\\Visual Studio 2015\\Projects\\DiscreteGestureBasics-WPF\\KinectDataset.csv", HelperMethods.EKDtblReaderToCSV(reader2, false, ","));
            //            }
            //            while (!reader2.IsClosed && reader2.Read());
            //            reader2.Close();
            //            conn.Close();
            //        }
            //    }
            //}

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
            this.meanEShSAngleRAcc = 0.0;
            this.meanEShSAngleLAcc = 0.0;

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
            this.wristVelocityAccR = 0.0;
            this.handVelocityAccR = 0.0;
            this.wristVelocityAccL = 0.0;
            this.handVelocityAccL = 0.0;

            this.wristAccelerationAccR = 0.0;
            this.wristAccelerationAccL = 0.0;
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

            this.minEShSAngleRUserAcc = 0.0;
            this.meanEShSAngleRUserAcc = 0.0;
            this.maxEShSAngleRUserAcc = 0.0;

            this.meanHWEAngleLUserAcc = 0.0;
            this.maxHWEAngleLUserAcc = 0.0;
            this.minHWEAngleLUserAcc = 0.0;

            this.minWEShAngleLUserAcc = 0.0;
            this.meanWEShAngleLUserAcc = 0.0;
            this.maxWEShAngleLUserAcc = 0.0;

            this.minEShSAngleLUserAcc = 0.0;
            this.meanEShSAngleLUserAcc = 0.0;
            this.maxEShSAngleLUserAcc = 0.0;

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

            this.wristVelocityUserAccR = 0.0;
            this.handVelocityUserAccR = 0.0;
            this.wristVelocityUserAccL = 0.0;
            this.handVelocityUserAccL = 0.0;

            this.wristAccelerationUserAccR = 0.0;
            this.wristAccelerationUserAccL = 0.0;
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

            this.minEShSAngleR = 360.0;
            this.meanEShSAngleR = 0.0;
            this.maxEShSAngleR = 0.0;
            this.minEShSAngleL = 360.0;
            this.meanEShSAngleL = 0.0;
            this.maxEShSAngleL = 0.0;

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

            this.wristVelocityR = 0.0;
            this.handVelocityR = 0.0;

            this.wristVelocityL = 0.0;
            this.handVelocityL = 0.0;

            this.wristAccelerationR = 0.0;
            this.wristAccelerationL = 0.0;
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
            string connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=KinectDatabase;Integrated Security=True;Pooling=False";
            this.frameCounter = 0;
            this.startClickedCounter = 10;
            train = false;
            register = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    try {
                        this.username = username_txtbx.Text;
                        this.gestureName = gestureComboBox.SelectedValue.ToString();
                        // System.Diagnostics.Debug.WriteLine(this.username);
                        using (SqlCommand command =
                            new SqlCommand("SELECT * FROM Users WHERE Users.User_Name =\'" + this.username + "\'", conn))
                        {
                            uniqueUsername = false;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.Read() && !(gestureName.Equals("") || gestureName==null))
                                {
                                    uniqueUsername = true;
                                    register_btn.Visibility = Visibility.Hidden;
                                    username_txtbx.Visibility = Visibility.Hidden;
                                    register_label.Visibility = Visibility.Hidden;
                                    gestureComboBox.Visibility = Visibility.Hidden;
                                    signIn_btn.Visibility = Visibility.Hidden;
                                    enter_label.Visibility = Visibility.Visible;
                                    start_btn.Visibility = Visibility.Visible;

                                }
                            }
                        }

                        using (SqlCommand command =
                            new SqlCommand("SELECT Id FROM Recognizable_Gestures WHERE Recognizable_Gestures.Gesture_Name =\'" + this.gestureName + "\'", conn))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if(reader.Read())
                                {
                                    this.gestureId = Convert.ToInt32(reader["Id"].ToString());
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

                            //using (SqlCommand command1 = new SqlCommand
                            //("Select * from Templates", conn))
                            //{
                            //    using (SqlDataReader reader = command1.ExecuteReader())
                            //    {
                            //        while (reader.Read())
                            //        {
                            //            MessageBox.Show(HelperMethods.ReaderToCSV(reader, false, " "));
                            //        }

                            //        reader.Close();
                            //    }
                            //}
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
            gestureComboBox.Visibility = Visibility.Hidden;
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
            gestureComboBox.Visibility = Visibility.Visible;
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

                                double EShSAngleL = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.ElbowLeft].Position,
                                    joints[JointType.ShoulderLeft].Position,
                                    joints[JointType.SpineShoulder].Position);

                                double HWEAngleR = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.ElbowRight].Position,
                                    joints[JointType.WristRight].Position,
                                    joints[JointType.ThumbRight].Position);

                                double WEShAngleR = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.WristRight].Position,
                                    joints[JointType.ElbowRight].Position,
                                    joints[JointType.ShoulderRight].Position);

                                double EShSAngleR = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.ElbowRight].Position,
                                    joints[JointType.ShoulderRight].Position,
                                    joints[JointType.SpineShoulder].Position);

                                this.meanHWEAngleLAcc += HWEAngleL;
                                this.meanWEShAngleLAcc += WEShAngleL;
                                this.meanEShSAngleLAcc += EShSAngleL;

                                this.meanHWEAngleRAcc += HWEAngleR;
                                this.meanWEShAngleRAcc += WEShAngleR;
                                this.meanEShSAngleRAcc += EShSAngleR;


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

                                if (this.minWEShAngleL > WEShAngleL)
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

                                if (this.minEShSAngleL > EShSAngleL)
                                {
                                    this.minEShSAngleL = EShSAngleL;
                                }
                                if (this.maxEShSAngleL < EShSAngleL)
                                {
                                    this.maxEShSAngleL = EShSAngleL;
                                }

                                if (this.minEShSAngleR > EShSAngleR)
                                {
                                    this.minEShSAngleR = EShSAngleR;
                                }
                                if (this.maxEShSAngleR < EShSAngleR)
                                {
                                    this.maxEShSAngleR = EShSAngleR;
                                }

                                if (frameCounter == 1)
                                {
                                    this.wristVelocityAccR = 0;
                                    this.handVelocityAccR = 0;
                                    this.oldWristVelocityR = 0;
                                    this.newWristVelocityL = 0;

                                    this.wristVelocityAccL = 0;
                                    this.handVelocityAccL = 0;
                                    this.oldWristVelocityL = 0;
                                    this.newWristVelocityL = 0;
                                }
                                else
                                {
                                    this.newWristVelocityR = HelperMethods.getVelocity(this.oldWristPosR, joints[JointType.WristRight].Position, DateTime.Now - oldFrameTime);
                                    this.handVelocityAccR += HelperMethods.getVelocity(this.oldHandPosR, joints[JointType.HandRight].Position, DateTime.Now - oldFrameTime);
                                    this.wristVelocityAccR += this.newWristVelocityR;
                                    this.wristAccelerationAccR += HelperMethods.getAcceleration(this.oldWristVelocityR, this.newWristVelocityR, DateTime.Now - oldFrameTime);

                                    this.oldWristVelocityR = this.newWristVelocityR;

                                    this.newWristVelocityL = HelperMethods.getVelocity(this.oldWristPosL, joints[JointType.WristLeft].Position, DateTime.Now - oldFrameTime);
                                    this.handVelocityAccL += HelperMethods.getVelocity(this.oldHandPosL, joints[JointType.HandLeft].Position, DateTime.Now - oldFrameTime);
                                    this.wristVelocityAccL += this.newWristVelocityL;
                                    this.wristAccelerationAccL += HelperMethods.getAcceleration(this.oldWristVelocityL, this.newWristVelocityL, DateTime.Now - oldFrameTime);

                                    this.oldWristVelocityL = this.newWristVelocityL;
                                }

                                this.oldFrameTime = DateTime.Now;
                                this.oldWristPosR = joints[JointType.WristRight].Position;
                                this.oldHandPosR = joints[JointType.HandRight].Position;
                                
                                this.oldWristPosL = joints[JointType.WristLeft].Position;
                                this.oldHandPosL = joints[JointType.HandLeft].Position;

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
                                    this.meanEShSAngleL = meanEShSAngleLAcc / frameCounter;
                                    this.meanEShSAngleR = meanEShSAngleRAcc / frameCounter;

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

                                    this.wristVelocityR = (wristVelocityAccR / (frameCounter - 1));
                                    this.handVelocityR = (handVelocityAccR / (frameCounter - 1));

                                    this.wristAccelerationR = (wristAccelerationAccR / (frameCounter - 1));

                                    this.wristVelocityL = (wristVelocityAccL / (frameCounter - 1));
                                    this.handVelocityL = (handVelocityAccL / (frameCounter - 1));

                                    this.wristAccelerationL = (wristAccelerationAccL / (frameCounter - 1));

                                    resetAccumilators();

                                    string connectionString = null;
                                    connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=KinectDatabase;Integrated Security=True;Pooling=False";
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
                                                        ("INSERT INTO Extracted_Kinect_Data VALUES ('" + this.gestureId + "','" +
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
                                                        "','" + this.maxWEShAngleR +
                                                        "','" + this.minEShSAngleR + "','" + this.meanEShSAngleR +
                                                        "','" + this.maxEShSAngleR + 
                                                        "','" + this.minHWEAngleL + "','" + this.meanHWEAngleL +
                                                        "','" + this.maxHWEAngleL +
                                                        "','" + this.minWEShAngleL + "','" + this.meanWEShAngleL +
                                                        "','" + this.maxWEShAngleL +
                                                        "','" + this.minEShSAngleL + "','" + this.meanEShSAngleL +
                                                        "','" + this.maxEShSAngleL +
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
                                                        "','" + this.wristVelocityR +
                                                        "','" + this.handVelocityR +
                                                        "','" + this.wristVelocityL +
                                                        "','" + this.handVelocityL +
                                                        "','" + this.wristAccelerationR +
                                                        "','" + this.wristAccelerationL + "')", conn))
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
                                                        this.minEShSAngleRUserAcc += Convert.ToDouble(reader["Min_EShS_Ang_R"].ToString());
                                                        this.meanEShSAngleRUserAcc += Convert.ToDouble(reader["Mean_EShS_Ang_R"].ToString());
                                                        this.maxEShSAngleRUserAcc += Convert.ToDouble(reader["Max_EShS_Ang_R"].ToString());
                                                        this.minHWEAngleLUserAcc += Convert.ToDouble(reader["Min_HWE_Ang_L"].ToString());
                                                        this.meanHWEAngleLUserAcc += Convert.ToDouble(reader["Mean_HWE_Ang_L"].ToString());
                                                        this.maxHWEAngleLUserAcc += Convert.ToDouble(reader["Max_HWE_Ang_L"].ToString());
                                                        this.minWEShAngleLUserAcc += Convert.ToDouble(reader["Min_WESh_Ang_L"].ToString());
                                                        this.meanWEShAngleLUserAcc += Convert.ToDouble(reader["Mean_WESh_Ang_L"].ToString());
                                                        this.maxWEShAngleLUserAcc += Convert.ToDouble(reader["Max_WESh_Ang_L"].ToString());
                                                        this.minEShSAngleLUserAcc += Convert.ToDouble(reader["Min_EShS_Ang_L"].ToString());
                                                        this.meanEShSAngleLUserAcc += Convert.ToDouble(reader["Mean_EShS_Ang_L"].ToString());
                                                        this.maxEShSAngleLUserAcc += Convert.ToDouble(reader["Max_EShS_Ang_L"].ToString());
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
                                                        this.elbowRelativeSpineShoulderLzUserAcc += Convert.ToDouble(reader["Elbow_Relative_SpineShoulder_Lz"].ToString());

                                                        this.wristVelocityUserAccR += Convert.ToDouble(reader["Wrist_Velocity_R"].ToString());
                                                        this.handVelocityUserAccR += Convert.ToDouble(reader["Hand_Velocity_R"].ToString());
                                                        this.wristAccelerationUserAccR += Convert.ToDouble(reader["Wrist_Acceleration_R"].ToString());

                                                        this.wristVelocityUserAccL += Convert.ToDouble(reader["Wrist_Velocity_L"].ToString());
                                                        this.handVelocityUserAccL += Convert.ToDouble(reader["Hand_Velocity_L"].ToString());
                                                        this.wristAccelerationUserAccL += Convert.ToDouble(reader["Wrist_Acceleration_L"].ToString());
                                                    }
                                                    reader.Close();

                                                    using (SqlCommand command1 = new SqlCommand("SELECT COUNT('Id') FROM  Extracted_Kinect_Data" +
                                                        " WHERE User_Id='" + this.userId+"'",conn))
                                                    {
                                                        userRecords = (Int32)command1.ExecuteScalar();
                                                    }
                                                    
                                                }

                                                using (SqlCommand command = new SqlCommand
                                                        ("INSERT INTO Templates VALUES ('"+ this.gestureId+ "','" + this.userId 
                                                        + "','" + this.handLengthRUserAcc / userRecords + "','" + this.upperArmLengthRUserAcc / userRecords + "','" +this.foreArmLengthRUserAcc/ userRecords + "','" + this.shoulderLengthRUserAcc / userRecords
                                                        + "','" + this.handLengthLUserAcc / userRecords + "','" + this.upperArmLengthLUserAcc / userRecords + "','" + this.foreArmLengthLUserAcc / userRecords + "','" + this.shoulderLengthLUserAcc / userRecords
                                                        + "','" + this.neckLengthUserAcc / userRecords + "','" + this.backboneLengthUserAcc / userRecords + "','" + this.lowerBackLengthUserAcc / userRecords + "','" + this.hipLengthRUserAcc / userRecords
                                                        + "','" + this.upperLegLengthRUserAcc / userRecords + "','" + this.shinLengthRUserAcc / userRecords + "','" + this.footLengthRUserAcc / userRecords + "','" + this. hipLengthLUserAcc / userRecords + "','" + this.upperLegLengthLUserAcc / userRecords + "','" + this.shinLengthLUserAcc / userRecords
                                                        + "','" + this.footLengthLUserAcc / userRecords + "','" + this.minHWEAngleRUserAcc / userRecords + "','" + this.meanHWEAngleRUserAcc / userRecords + "','" + this.maxHWEAngleRUserAcc / userRecords + "','" + this.minWEShAngleRUserAcc / userRecords + "','" + this.meanWEShAngleRUserAcc / userRecords
                                                        + "','" + this.maxWEShAngleRUserAcc / userRecords
                                                        +"','" + this.minEShSAngleRUserAcc / userRecords + "','" + this.meanEShSAngleRUserAcc / userRecords
                                                        + "','" + this.maxEShSAngleRUserAcc / userRecords + 
                                                        "','" + this.minHWEAngleLUserAcc / userRecords + "','" + this.meanHWEAngleLUserAcc / userRecords + "','" + this.maxHWEAngleLUserAcc / userRecords
                                                        + "','" + this.minWEShAngleLUserAcc / userRecords + "','" + this.meanWEShAngleLUserAcc / userRecords + "','" + this.maxWEShAngleLUserAcc / userRecords
                                                        + "','" + this.minEShSAngleLUserAcc / userRecords + "','" + this.meanEShSAngleLUserAcc / userRecords
                                                        + "','" + this.maxEShSAngleLUserAcc / userRecords 
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
                                                        + "','" + this.wristVelocityUserAccR / userRecords
                                                        + "','" + this.handVelocityUserAccR / userRecords
                                                        + "','" + this.wristVelocityUserAccL / userRecords
                                                        + "','" + this.handVelocityUserAccL / userRecords
                                                        + "','" + this.wristAccelerationUserAccR / userRecords
                                                        + "','" + this.wristAccelerationUserAccL / userRecords
                                                        + "')", conn))
                                                {
                                                    SqlDataReader reader1 = command.ExecuteReader();
                                                    reader1.Close();
                                                }

                                                using (SqlCommand command2 = new SqlCommand
                                                ("SELECT * FROM Extracted_Kinect_Data WHERE User_Id=" + this.userId, conn))
                                                {
                                                    using (SqlDataReader reader2 = command2.ExecuteReader())
                                                    {
                                                        do
                                                        {
                                                            System.IO.File.AppendAllText("C:\\Users\\Nadeen\\Documents\\Visual Studio 2015\\Projects\\DiscreteGestureBasics-WPF\\KinectDataset.csv", HelperMethods.EKDtblReaderToCSV(reader2, false, ","));
                                                        }
                                                        while (!reader2.IsClosed && reader2.Read());
                                                        reader2.Close();
                                                    }
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
                                            gestureComboBox.Visibility = Visibility.Visible;
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

                                double EShSAngleL = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.ElbowLeft].Position,
                                    joints[JointType.ShoulderLeft].Position,
                                    joints[JointType.SpineShoulder].Position);

                                double HWEAngleR = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.ElbowRight].Position,
                                    joints[JointType.WristRight].Position,
                                    joints[JointType.ThumbRight].Position);

                                double WEShAngleR = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.WristRight].Position,
                                    joints[JointType.ElbowRight].Position,
                                    joints[JointType.ShoulderRight].Position);

                                double EShSAngleR = HelperMethods.getAngleAtMiddleJoint
                                    (joints[JointType.ElbowRight].Position,
                                    joints[JointType.ShoulderRight].Position,
                                    joints[JointType.SpineShoulder].Position);

                                this.meanHWEAngleLAcc += HWEAngleL;
                                this.meanWEShAngleLAcc += WEShAngleL;
                                this.meanEShSAngleLAcc += EShSAngleL;

                                this.meanHWEAngleRAcc += HWEAngleR;
                                this.meanWEShAngleRAcc += WEShAngleR;
                                this.meanEShSAngleRAcc += EShSAngleR;


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

                                if (this.minWEShAngleL > WEShAngleL)
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

                                if (this.minEShSAngleL > EShSAngleL)
                                {
                                    this.minEShSAngleL = EShSAngleL;
                                }
                                if (this.maxEShSAngleL < EShSAngleL)
                                {
                                    this.maxEShSAngleL = EShSAngleL;
                                }

                                if (this.minEShSAngleR > EShSAngleR)
                                {
                                    this.minEShSAngleR = EShSAngleR;
                                }
                                if (this.maxEShSAngleR < EShSAngleR)
                                {
                                    this.maxEShSAngleR = EShSAngleR;
                                }

                                if (frameCounter == 1)
                                {
                                    this.wristVelocityAccR = 0;
                                    this.handVelocityAccR = 0;
                                    this.oldWristVelocityR = 0;

                                    this.wristVelocityAccL = 0;
                                    this.handVelocityAccL = 0;
                                    this.oldWristVelocityL = 0;
                                }
                                else
                                {
                                    this.newWristVelocityR = HelperMethods.getVelocity(this.oldWristPosR, joints[JointType.WristRight].Position, DateTime.Now - oldFrameTime);
                                    this.handVelocityAccR += HelperMethods.getVelocity(this.oldHandPosR, joints[JointType.HandRight].Position, DateTime.Now - oldFrameTime);
                                    this.wristVelocityAccR += this.newWristVelocityR;
                                    this.wristAccelerationAccR += HelperMethods.getAcceleration(this.oldWristVelocityR, this.newWristVelocityR, DateTime.Now - oldFrameTime);

                                    this.oldWristVelocityR = this.newWristVelocityR;

                                    this.newWristVelocityL = HelperMethods.getVelocity(this.oldWristPosL, joints[JointType.WristLeft].Position, DateTime.Now - oldFrameTime);
                                    this.handVelocityAccL += HelperMethods.getVelocity(this.oldHandPosL, joints[JointType.HandLeft].Position, DateTime.Now - oldFrameTime);
                                    this.wristVelocityAccL += this.newWristVelocityL;
                                    this.wristAccelerationAccL += HelperMethods.getAcceleration(this.oldWristVelocityL, this.newWristVelocityL, DateTime.Now - oldFrameTime);

                                    this.oldWristVelocityL = this.newWristVelocityL;
                                }

                                this.oldFrameTime = DateTime.Now;
                                this.oldWristPosR = joints[JointType.WristRight].Position;
                                this.oldHandPosR = joints[JointType.HandRight].Position;

                                this.oldWristPosL = joints[JointType.WristLeft].Position;
                                this.oldHandPosL = joints[JointType.HandLeft].Position;

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
                                    this.meanEShSAngleL = meanEShSAngleLAcc / frameCounter;
                                    this.meanEShSAngleR = meanEShSAngleRAcc / frameCounter;

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

                                    this.wristVelocityR = wristVelocityAccR / (frameCounter - 1);
                                    this.handVelocityR = handVelocityAccR / (frameCounter - 1);

                                    this.wristVelocityL = wristVelocityAccL / (frameCounter - 1);
                                    this.handVelocityL = handVelocityAccL / (frameCounter - 1);

                                    this.wristAccelerationR = wristAccelerationAccR / (frameCounter - 1);
                                    this.wristAccelerationL = wristAccelerationAccL / (frameCounter - 1);

                                    resetAccumilators();

                                    string connectionString = null;
                                    connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=KinectDatabase;Integrated Security=True;Pooling=False";
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
                                                    currentError = Math.Sqrt(
                                                        Math.Pow(Convert.ToDouble(reader["Hand_Length_R"].ToString()) - this.handLengthR, 2) +
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

                                                        Math.Pow(Convert.ToDouble(reader["Min_WESh_Ang_R"].ToString()) - this.minWEShAngleR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Mean_WESh_Ang_R"].ToString()) - this.meanWEShAngleR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Max_WESh_Ang_R"].ToString()) - this.maxWEShAngleR, 2) +

                                                        Math.Pow(Convert.ToDouble(reader["Min_EShS_Ang_R"].ToString()) - this.minEShSAngleR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Mean_EShS_Ang_R"].ToString()) - this.meanEShSAngleR, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Max_EShS_Ang_R"].ToString()) - this.maxEShSAngleR, 2)+

                                                        Math.Pow(Convert.ToDouble(reader["Min_HWE_Ang_L"].ToString()) - this.minHWEAngleL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Mean_HWE_Ang_L"].ToString()) - this.meanHWEAngleL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Max_HWE_Ang_L"].ToString()) - this.maxHWEAngleL, 2) +
                                                       
                                                        Math.Pow(Convert.ToDouble(reader["Min_WESh_Ang_L"].ToString()) - this.minWEShAngleL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Mean_WESh_Ang_L"].ToString()) - this.meanWEShAngleL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Max_WESh_Ang_L"].ToString()) - this.maxWEShAngleL, 2) +

                                                        Math.Pow(Convert.ToDouble(reader["Min_EShS_Ang_L"].ToString()) - this.minEShSAngleL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Mean_EShS_Ang_L"].ToString()) - this.meanEShSAngleL, 2) +
                                                        Math.Pow(Convert.ToDouble(reader["Max_EShS_Ang_L"].ToString()) - this.maxEShSAngleL, 2) +

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
                                                        
                                                        Math.Pow((Convert.ToDouble(reader["Wrist_Velocity_R"].ToString())) - this.wristVelocityR, 2) +
                                                        Math.Pow((Convert.ToDouble(reader["Hand_Velocity_R"].ToString())) - this.handVelocityR, 2)+
                                                        Math.Pow((Convert.ToDouble(reader["Wrist_Acceleration_R"].ToString())) - this.wristAccelerationR, 2)+


                                                        Math.Pow((Convert.ToDouble(reader["Wrist_Velocity_L"].ToString())) - this.wristVelocityL, 2) +
                                                        Math.Pow((Convert.ToDouble(reader["Hand_Velocity_L"].ToString())) - this.handVelocityL, 2) +
                                                        Math.Pow((Convert.ToDouble(reader["Wrist_Acceleration_L"].ToString())) - this.wristAccelerationL, 2)
                                                        );
                                                    if (currentError < minError)
                                                    {
                                                        minError = currentError;
                                                        currentId = Convert.ToInt32(reader["User_Id"].ToString());
                                                    }

                                                }
                                                reader.Close();
                                            }

                                            if (minError < 200)
                                            {
                                                using (SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Id=" + currentId, conn))
                                                {
                                                    reader1 = command.ExecuteReader();
                                                    if(reader1.Read())
                                                    {
                                                        MessageBox.Show(reader1["User_Name"].ToString()
                                                            +"\n Wrist Velocity R:"
                                                            +this.wristVelocityR 
                                                            + "\n Wrist Acceleration R:"
                                                            + this.wristAccelerationR 
                                                            + "\n Wrist Velocity L:"
                                                            + this.wristVelocityL
                                                            + "\n Wrist Acceleration L:"
                                                            + this.wristAccelerationL
                                                            + "\n Shoulder Angle R:"
                                                            + this.meanEShSAngleR
                                                            + "\n Shoulder Angle L:"
                                                            + this.meanEShSAngleL);
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

        private void train_btn_Click(object sender, RoutedEventArgs e)
        {
            string message = "";
            string connectionString = "Data Source=NADEENS-PC\\SQLEXPRESS;Initial Catalog=KinectDatabase;Integrated Security=True;Pooling=False";
            this.frameCounter = 0;
            this.startClickedCounter = 5;
            train = true;
            register = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    try
                    {
                        this.username = username_txtbx.Text;
                        this.gestureName = gestureComboBox.SelectedValue.ToString();
                        // System.Diagnostics.Debug.WriteLine(this.username);
                        using (SqlCommand command =
                            new SqlCommand("SELECT * FROM Users WHERE Users.User_Name =\'" + this.username + "\'", conn))
                        {
                            uniqueUsername = false;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.Read() && !(gestureName.Equals("") || gestureName == null))
                                {
                                    uniqueUsername = true;
                                    register_btn.Visibility = Visibility.Hidden;
                                    username_txtbx.Visibility = Visibility.Hidden;
                                    register_label.Visibility = Visibility.Hidden;
                                    gestureComboBox.Visibility = Visibility.Hidden;
                                    signIn_btn.Visibility = Visibility.Hidden;
                                    enter_label.Visibility = Visibility.Visible;
                                    start_btn.Visibility = Visibility.Visible;

                                }
                            }
                        }

                        using (SqlCommand command =
                            new SqlCommand("SELECT Id FROM Recognizable_Gestures WHERE Recognizable_Gestures.Gesture_Name =\'" + this.gestureName + "\'", conn))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    this.gestureId = Convert.ToInt32(reader["Id"].ToString());
                                }
                            }
                        }
                    }
                    catch (Exception ex1)
                    {
                        message += "Could not validate username! \n" + ex1.ToString();
                    }


                    if (!uniqueUsername)
                    {
                        message += "Valid Username \n";

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
                                new SqlCommand("INSERT INTO Sessions VALUES ('" + this.userId + "')", conn))
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
                        message += "User does not exist!";
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
        private void gestureComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}