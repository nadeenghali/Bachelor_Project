using System;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;

namespace Microsoft.Samples.Kinect.DiscreteGestureBasics
{
    public class HelperMethods
    {
        public static double getAcceleration(double u, double v, TimeSpan t)
        {
            return ((v - u) / t.TotalSeconds);
        }

        public static double getVelocity(CameraSpacePoint previous, CameraSpacePoint current, TimeSpan time)
        {
            double distance = getDistanceBetweenJoints(previous, current);
            double velocity = distance / time.TotalSeconds;

            return velocity;
        }

        public static double getDistanceBetweenJoints(CameraSpacePoint a, CameraSpacePoint b)
        {
            double result = 0;
            double xInter = Math.Pow(b.X - a.X, 2);
            double yInter = Math.Pow(b.Y - a.Y, 2);
            double zInter = Math.Pow(b.Z - a.Z, 2);

            result = Math.Sqrt(xInter + yInter + zInter);
            return result;
        }

        public static double getAngleBetweenTwoVectors(double vx, double vy, double vz, double ux, double uy, double uz)
        {
            double result = 0;
            Vector3D v = new Vector3D(vx, vy, vz);
            Vector3D u = new Vector3D(ux, uy, uz);

            result = Vector3D.AngleBetween(v, u);
            return result;
        }

        public static double getAngleAtMiddleJoint(CameraSpacePoint a, CameraSpacePoint b, CameraSpacePoint c)
        {
            double result = 0;
            Vector3D v = new Vector3D(b.X - a.X, b.Y - a.Y, b.Z - a.Z);
            Vector3D u = new Vector3D(b.X - c.X, b.Y - c.Y, b.Z - c.Z);

            result = Vector3D.AngleBetween(v, u);
            return result;
        }

    }
}
