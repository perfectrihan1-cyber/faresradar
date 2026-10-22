using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Viman.Models
{
    public class SqlHelper
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
        public static SqlConnection GetConnectionFlightoffice()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringFlightoffice"].ConnectionString);
        }
        public static DataSet ExecuteDataset(string commandText, params SqlParameter[] commandParameters)
        {
            SqlConnection connection = GetConnection();
            if (connection == null) throw new ArgumentNullException("connection");

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;

            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            cmd.Connection = connection;
            cmd.CommandTimeout = 120;
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.Text;

            if (commandParameters != null)
            {
                foreach (SqlParameter param in commandParameters)
                {
                    if (param != null)
                    {
                        if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && (param.Value == null))
                        {
                            param.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(param);
                    }
                }
            }

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                da.Fill(ds);
                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    connection.Close();

                return ds;
            }
        }

        public static DataSet ExecuteDatasetFlightoffice(string commandText, params SqlParameter[] commandParameters)
        {
            SqlConnection connection = GetConnectionFlightoffice();
            if (connection == null) throw new ArgumentNullException("connection");

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;

            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            cmd.Connection = connection;
            cmd.CommandTimeout = 120;
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.Text;

            if (commandParameters != null)
            {
                foreach (SqlParameter param in commandParameters)
                {
                    if (param != null)
                    {
                        if ((param.Direction == ParameterDirection.InputOutput || param.Direction == ParameterDirection.Input) && (param.Value == null))
                        {
                            param.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(param);
                    }
                }
            }

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                da.Fill(ds);
                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    connection.Close();

                return ds;
            }
        }
    }
}