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
using System.Data;
using System.Data.SqlClient;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;
using LibSVMsharp;


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

        public static string ReaderToLibSVMCSV(IDataReader dataReader, bool includeHeaderAsFirstRow = false,
            string separator = ",")
        {
            DataTable dataTable = new DataTable();
            StringBuilder csvRows = new StringBuilder();
            string row = "";
            int columns;
            try
            {
                dataTable.Load(dataReader);
                columns = dataTable.Columns.Count;
                //Create Header
                if (includeHeaderAsFirstRow)
                {
                    for (int index = 0; index < columns; index++)
                    {
                        row += (dataTable.Columns[index]);
                        if (index < columns - 1)
                            row += (separator);
                    }
                    row += (Environment.NewLine);
                }
                csvRows.Append(row);

                //Create Rows
                for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    row = "";
                    //Row
                    for (int index = 0; index < columns - 1; index++)
                    {
                        string value = dataTable.Rows[rowIndex][index].ToString();

                        //If type of field is string
                        if (dataTable.Rows[rowIndex][index] is string)
                        {
                            //If double quotes are used in value, ensure each are replaced by double quotes.
                            if (value.IndexOf("\"") >= 0)
                                value = value.Replace("\"", "\"\"");

                            //If separtor are is in value, ensure it is put in double quotes.
                            if (value.IndexOf(separator) >= 0)
                                value = "\"" + value + "\"";

                            //If string contain new line character
                            while (value.Contains("\r"))
                            {
                                value = value.Replace("\r", "");
                            }
                            while (value.Contains("\n"))
                            {
                                value = value.Replace("\n", "");
                            }
                        }
                        if (index > 2)
                        {
                            row += (index-2) + ":" + value;
                        }
                        else if(index == 2)
                        {
                            row += value;
                        }
                        if (index < columns - 1 && index >= 2)
                            row += separator;
                    }
                    dataTable.Rows[rowIndex][columns - 1].ToString().ToString().Replace(separator, " ");
                    row += Environment.NewLine;
                    csvRows.Append(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return csvRows.ToString();
        }

        public static string EKDtblReaderToCSV(IDataReader dataReader, bool includeHeaderAsFirstRow = false,
            string separator = ",")
        {
            DataTable dataTable = new DataTable();
            StringBuilder csvRows = new StringBuilder();
            string row = "";
            int columns;
            try
            {
                dataTable.Load(dataReader);
                columns = dataTable.Columns.Count;
                //Create Header
                if (includeHeaderAsFirstRow)
                {
                    for (int index = 0; index < columns; index++)
                    {
                        if (index >= 2 && index != 3)
                        {
                            row += (dataTable.Columns[index]);
                            if (index < columns - 1)
                                row += (separator);

                        }
                    }
                    row += (Environment.NewLine);
                }
                csvRows.Append(row);

                //Create Rows
                for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    row = "";
                    //Row
                    for (int index = 0; index < columns - 1; index++)
                    {
                        string value = dataTable.Rows[rowIndex][index].ToString();

                        //If type of field is string
                        if (dataTable.Rows[rowIndex][index] is string)
                        {
                            //If double quotes are used in value, ensure each are replaced by double quotes.
                            if (value.IndexOf("\"") >= 0)
                                value = value.Replace("\"", "\"\"");

                            //If separtor are is in value, ensure it is put in double quotes.
                            if (value.IndexOf(separator) >= 0)
                                value = "\"" + value + "\"";

                            //If string contain new line character
                            while (value.Contains("\r"))
                            {
                                value = value.Replace("\r", "");
                            }
                            while (value.Contains("\n"))
                            {
                                value = value.Replace("\n", "");
                            }
                        }
                        if (index > 3)
                        {
                            row += value;
                        }

                        if (index < columns - 1 && index > 3)
                            row += separator;

                        if(index == columns-2)
                        {
                            row += dataTable.Rows[rowIndex][2].ToString();
                        }
                    }
                    dataTable.Rows[rowIndex][columns - 1].ToString().ToString().Replace(separator, " ");
                    row += Environment.NewLine;
                    csvRows.Append(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return csvRows.ToString();
        }

        public static string ReaderToCSV(IDataReader dataReader, bool includeHeaderAsFirstRow = false,
            string separator = ",")
        {
            DataTable dataTable = new DataTable();
            StringBuilder csvRows = new StringBuilder();
            string row = "";
            int columns;
            try
            {
                dataTable.Load(dataReader);
                columns = dataTable.Columns.Count;
                //Create Header
                if (includeHeaderAsFirstRow)
                {
                    for (int index = 0; index < columns; index++)
                    {
                        row += (dataTable.Columns[index]);
                        if (index < columns - 1)
                            row += (separator);
                    }
                    row += (Environment.NewLine);
                }
                csvRows.Append(row);

                //Create Rows
                for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    row = "";
                    //Row
                    for (int index = 0; index < columns - 1; index++)
                    {
                        string value = dataTable.Rows[rowIndex][index].ToString();

                        //If type of field is string
                        if (dataTable.Rows[rowIndex][index] is string)
                        {
                            //If double quotes are used in value, ensure each are replaced by double quotes.
                            if (value.IndexOf("\"") >= 0)
                                value = value.Replace("\"", "\"\"");

                            //If separtor are is in value, ensure it is put in double quotes.
                            if (value.IndexOf(separator) >= 0)
                                value = "\"" + value + "\"";

                            //If string contain new line character
                            while (value.Contains("\r"))
                            {
                                value = value.Replace("\r", "");
                            }
                            while (value.Contains("\n"))
                            {
                                value = value.Replace("\n", "");
                            }
                        }
                        if (index > 2)
                        {
                            row += value;
                        }
                        else if (index == 2)
                        {
                            row += value;
                        }
                        if (index < columns - 1)
                            row += separator;
                    }
                    dataTable.Rows[rowIndex][columns - 1].ToString().ToString().Replace(separator, " ");
                    row += Environment.NewLine;
                    csvRows.Append(row);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return csvRows.ToString();
        }
    }
}
