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
        double thumbArmAngle = 0.0;
        double elbowAngle = 0.0;

        //counters and flags
        Boolean uniqueUsername = false;
        int frameCounter = 0;

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

        public double getDistanceBetweenJoints(double x, double y, double z, double x1, double y1, double z1)
        {
            double result = 0;
            double xInter = Math.Pow(x1 - x, 2);
            double yInter = Math.Pow(y1 - y, 2);
            double zInter = Math.Pow(z1 - z, 2);

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
            frameCounter = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    try {
                        this.username = username_txtbx.Text;
                        // System.Diagnostics.Debug.WriteLine(this.username);
                        using (SqlCommand command =
                            new SqlCommand("SELECT * FROM Users WHERE Users.User_Name =\'" + username + "\'", conn))
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
                                new SqlCommand("SELECT * FROM Users WHERE Users.User_Name =\'" + username + "\'", conn))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    this.userId = Convert.ToInt32(reader["Id"].ToString());
                                }
                            }

                            using (SqlCommand command2 = 
                                new SqlCommand ("INSERT INTO Sessions VALUES ('" + this.userId + "')", conn))
                            {
                                SqlDataReader reader2 = command2.ExecuteReader();
                                reader2.Close();
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
            //this.bodyFrameReader.FrameArrived -= this.Reader_BodyFrameArrived;
            this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;
            //enter_label.Content = this.kinectSensor.BodyFrameSource.BodyCount;
        }

        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
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
                // we may have lost/acquired bodies, so update the corresponding gesture detectors
                if (this.bodies != null)
                {
                    // loop through all bodies
                    int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;

                    for (int i = 0; i < maxBodies; ++i)
                    {
                        Body body = this.bodies[i];
                        
                        if (body.IsTracked)//rougly 2 seconds
                        {
                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
                            // convert the joint points to depth (display) space
                            Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();
                           
                            frameCounter += 1;

                            this.handLengthRAcc += getDistanceBetweenJoints(joints[JointType.WristRight].Position.X,
                                joints[JointType.WristRight].Position.Y, joints[JointType.WristRight].Position.Z,
                                joints[JointType.HandTipRight].Position.X, joints[JointType.HandTipRight].Position.Y,
                                joints[JointType.HandTipRight].Position.Z);
                            this.foreArmLengthRAcc += getDistanceBetweenJoints(joints[JointType.ElbowRight].Position.X,
                                joints[JointType.ElbowRight].Position.Y, joints[JointType.ElbowRight].Position.Z,
                                joints[JointType.WristRight].Position.X, joints[JointType.WristRight].Position.Y,
                                joints[JointType.WristRight].Position.Z);
                            this.upperArmLengthRAcc += getDistanceBetweenJoints(joints[JointType.ShoulderRight].Position.X,
                                joints[JointType.ShoulderRight].Position.Y, joints[JointType.ShoulderRight].Position.Z,
                                joints[JointType.ElbowRight].Position.X, joints[JointType.ElbowRight].Position.Y,
                                joints[JointType.ElbowRight].Position.Z);
                            this.shoulderLengthRAcc += getDistanceBetweenJoints(joints[JointType.ShoulderRight].Position.X,
                                joints[JointType.ShoulderRight].Position.Y, joints[JointType.ShoulderRight].Position.Z,
                                joints[JointType.SpineShoulder].Position.X, joints[JointType.SpineShoulder].Position.Y,
                                joints[JointType.SpineShoulder].Position.Z);
                            this.hipLengthRAcc += getDistanceBetweenJoints(joints[JointType.HipRight].Position.X,
                                joints[JointType.HipRight].Position.Y, joints[JointType.HipRight].Position.Z,
                                joints[JointType.SpineBase].Position.X, joints[JointType.SpineBase].Position.Y,
                                joints[JointType.SpineBase].Position.Z);
                            this.upperLegLengthRAcc += getDistanceBetweenJoints(joints[JointType.HipRight].Position.X,
                                joints[JointType.HipRight].Position.Y, joints[JointType.HipRight].Position.Z,
                                joints[JointType.KneeRight].Position.X, joints[JointType.KneeRight].Position.Y,
                                joints[JointType.KneeRight].Position.Z);
                            this.shinLengthRAcc += getDistanceBetweenJoints(joints[JointType.AnkleRight].Position.X,
                                joints[JointType.AnkleRight].Position.Y, joints[JointType.AnkleRight].Position.Z,
                                joints[JointType.KneeRight].Position.X, joints[JointType.KneeRight].Position.Y,
                                joints[JointType.KneeRight].Position.Z);
                            this.footLengthRAcc += getDistanceBetweenJoints(joints[JointType.AnkleRight].Position.X,
                                joints[JointType.AnkleRight].Position.Y, joints[JointType.AnkleRight].Position.Z,
                                joints[JointType.FootRight].Position.X, joints[JointType.FootRight].Position.Y,
                                joints[JointType.FootRight].Position.Z);


                            this.handLengthLAcc += getDistanceBetweenJoints(joints[JointType.WristLeft].Position.X,
                                joints[JointType.WristLeft].Position.Y, joints[JointType.WristLeft].Position.Z,
                                joints[JointType.HandTipLeft].Position.X, joints[JointType.HandTipLeft].Position.Y,
                                joints[JointType.HandTipLeft].Position.Z);
                            this.foreArmLengthLAcc += getDistanceBetweenJoints(joints[JointType.ElbowLeft].Position.X,
                                joints[JointType.ElbowLeft].Position.Y, joints[JointType.ElbowLeft].Position.Z,
                                joints[JointType.WristLeft].Position.X, joints[JointType.WristLeft].Position.Y,
                                joints[JointType.WristLeft].Position.Z);
                            this.upperArmLengthLAcc += getDistanceBetweenJoints(joints[JointType.ShoulderLeft].Position.X,
                                joints[JointType.ShoulderLeft].Position.Y, joints[JointType.ShoulderLeft].Position.Z,
                                joints[JointType.ElbowLeft].Position.X, joints[JointType.ElbowLeft].Position.Y,
                                joints[JointType.ElbowLeft].Position.Z);
                            this.shoulderLengthLAcc += getDistanceBetweenJoints(joints[JointType.ShoulderLeft].Position.X,
                                joints[JointType.ShoulderLeft].Position.Y, joints[JointType.ShoulderLeft].Position.Z,
                                joints[JointType.SpineShoulder].Position.X, joints[JointType.SpineShoulder].Position.Y,
                                joints[JointType.SpineShoulder].Position.Z);
                            this.hipLengthLAcc += getDistanceBetweenJoints(joints[JointType.HipLeft].Position.X,
                                joints[JointType.HipLeft].Position.Y, joints[JointType.HipLeft].Position.Z,
                                joints[JointType.SpineBase].Position.X, joints[JointType.SpineBase].Position.Y,
                                joints[JointType.SpineBase].Position.Z);
                            this.upperLegLengthLAcc += getDistanceBetweenJoints(joints[JointType.HipLeft].Position.X,
                                joints[JointType.HipLeft].Position.Y, joints[JointType.HipLeft].Position.Z,
                                joints[JointType.KneeLeft].Position.X, joints[JointType.KneeLeft].Position.Y,
                                joints[JointType.KneeLeft].Position.Z);
                            this.shinLengthLAcc += getDistanceBetweenJoints(joints[JointType.AnkleLeft].Position.X,
                                joints[JointType.AnkleLeft].Position.Y, joints[JointType.AnkleLeft].Position.Z,
                                joints[JointType.KneeLeft].Position.X, joints[JointType.KneeLeft].Position.Y,
                                joints[JointType.KneeLeft].Position.Z);
                            this.footLengthLAcc += getDistanceBetweenJoints(joints[JointType.AnkleLeft].Position.X,
                                joints[JointType.AnkleLeft].Position.Y, joints[JointType.AnkleLeft].Position.Z,
                                joints[JointType.FootLeft].Position.X, joints[JointType.FootLeft].Position.Y,
                                joints[JointType.FootLeft].Position.Z);

                            this.neckLengthAcc += getDistanceBetweenJoints(joints[JointType.Head].Position.X,
                                joints[JointType.Head].Position.Y, joints[JointType.Head].Position.Z,
                                joints[JointType.SpineShoulder].Position.X, joints[JointType.SpineShoulder].Position.Y,
                                joints[JointType.SpineShoulder].Position.Z);
                            this.backboneLengthAcc += getDistanceBetweenJoints(joints[JointType.SpineMid].Position.X,
                                joints[JointType.SpineMid].Position.Y, joints[JointType.SpineMid].Position.Z,
                                joints[JointType.SpineShoulder].Position.X, joints[JointType.SpineShoulder].Position.Y,
                                joints[JointType.SpineShoulder].Position.Z);
                            this.lowerBackLengthAcc += getDistanceBetweenJoints(joints[JointType.SpineMid].Position.X,
                                joints[JointType.SpineMid].Position.Y, joints[JointType.SpineMid].Position.Z,
                                joints[JointType.SpineBase].Position.X, joints[JointType.SpineBase].Position.Y,
                                joints[JointType.SpineBase].Position.Z);

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

                                handLengthRAcc = 0.0;
                                upperArmLengthRAcc = 0.0;
                                foreArmLengthRAcc = 0.0;
                                shoulderLengthRAcc = 0.0;
                                hipLengthRAcc = 0.0;
                                upperLegLengthRAcc = 0.0;
                                shinLengthRAcc = 0.0;
                                footLengthRAcc = 0.0;

                                handLengthLAcc = 0.0;
                                upperArmLengthLAcc = 0.0;
                                foreArmLengthLAcc = 0.0;
                                shoulderLengthLAcc = 0.0;
                                hipLengthLAcc = 0.0;
                                upperLegLengthLAcc = 0.0;
                                shinLengthLAcc = 0.0;
                                footLengthLAcc = 0.0;

                                neckLengthAcc = 0.0;
                                backboneLengthAcc = 0.0;
                                lowerBackLengthAcc = 0.0;

                                string message = "";
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
                                                    + this.userId + "', 100 , 1, '" + 
                                                    this.handLengthR * 100 +"','"+ this.upperArmLengthR * 100 +
                                                    "','" + this.foreArmLengthR * 100 + "','" + this.shoulderLengthR * 100 +
                                                    "','" + this.handLengthL * 100 + "','" + this.upperArmLengthL * 100 + 
                                                    "','" + this.foreArmLengthL * 100 + "','" + this.shoulderLengthL * 100 + 
                                                    "','" + this.neckLength * 100 + "','" + this.backboneLength * 100 +
                                                    "','" + this.lowerBackLength * 100 + "','" + this.hipLengthR * 100 + 
                                                    "','" + this.upperLegLengthR * 100 + "','" + this.shinLengthR * 100 + 
                                                    "','" + this.footLengthR * 100 + "','" + this.hipLengthL * 100 +
                                                    "','" + this.upperLegLengthL * 100 + "','" + this.shinLengthL * 100 +
                                                    "','" + this.footLengthL * 100 + "')", conn))
                                            {
                                                SqlDataReader reader = command.ExecuteReader();
                                                reader.Close();
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

                                        }
                                        else if (!uniqueUsername && this.username.Equals("")) //sign in
                                        {
                                            using (SqlCommand command = new SqlCommand
                                                    ("SELECT Extracted_Kinect_Data.User_Id FROM Extracted_Kinect_Data WHERE (Extracted_Kinect_Data.Fore_Arm_Length_R BETWEEN "
                                                    + ((this.foreArmLengthR * 100) + 4.0) + " AND " + ((this.foreArmLengthR * 100) - 4.0) +
                                                    ") AND (Extracted_Kinect_Data.Hand_Length_R BETWEEN" + ((this.handLengthR * 100) + 4.0) + 
                                                    " AND " + ((this.handLengthR * 100) - 4.0) + ")"
                                                    , conn))
                                            {
                                                using (SqlDataReader reader = command.ExecuteReader())
                                                {
                                                    int queryRes = Convert.ToInt32(reader["User_Id"].ToString());
                                                    if (!reader.Read())
                                                    {
                                                        MessageBox.Show("No match found!");
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show(""+queryRes);
                                                    }
                                                    reader.Close();
                                                    return;
                                                }
                                            }
                                        }
                                        conn.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Oops an error has occured!\n" + ex.ToString());
                                    return;
                                }
                                MessageBox.Show(message);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}