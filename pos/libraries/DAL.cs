using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

/// <summary>
/// Summary description for DAL
/// </summary>
public class DAL
{
    private string connString;
    private string modulename;
    private Exception err;
    public DAL()
    {
        //
        // TODO: Add constructor logic here
        //
        //connString = ConfigurationManager.ConnectionStrings["localdb"].ToString();
        //"Data Source=.\SQLEXPRESS;Initial Catalog=PMSDB;User ID=sa;Password=031988";
        string datasource = pos.libraries.settings.GetValue("servername");
        string initialcatalog = pos.libraries.settings.GetValue("databasename");
        string userid = pos.libraries.settings.GetValue("username");
        string password = pos.libraries.settings.GetValue("password");
        connString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}",datasource,initialcatalog,userid,password);
    }

    public enum ExecuteType
    {
        ExecuteReader,
        ExecuteNonQuery,
        ExecuteScalar,
        ExecuteDatatable
    };

    public enum CmdType
    {
        Text,
        StoredProcedure
    };

    public string ModuleName
    {
        set { modulename = value; }
        get { return modulename; }
    }

    public Exception sqlErr
    {
        get { return err; }
    }

    public DAL(string ConnectionString)
    {
        connString = ConnectionString;
    }

    public DataTable GetDataTable(string queryString)
    {
        DataTable dt = new DataTable();
        using (SqlConnection myConn = new SqlConnection(connString))
        {
            myConn.Open();
            SqlCommand cmd = new SqlCommand(queryString, myConn);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            dt.Load(dr);
            myConn.Close();
            dr.Dispose();
            myConn.Dispose();

        }
        return dt;
    }

    public DataTable GetDataTable(string queryString, CmdType type, params SqlParameter[] arrParam)
    {
        DataTable dt = new DataTable();
        using (SqlConnection myConn = new SqlConnection(connString))
        {
                myConn.Open();
                SqlCommand cmd = new SqlCommand();
                SqlParameter firstOutputParameter = null;
                // Handle the parameters 
                if (arrParam != null)
                {
                    foreach (SqlParameter param in arrParam)
                    {
                        cmd.Parameters.Add(param);
                        if (firstOutputParameter == null && param.Direction == ParameterDirection.Output && param.SqlDbType == SqlDbType.Int)
                            firstOutputParameter = param;
                    }
                }
                

                switch (type)
                {
                    case CmdType.Text:
                        cmd.CommandType = CommandType.Text;
                        
                        break;
                    case CmdType.StoredProcedure:
                        cmd.CommandType = CommandType.StoredProcedure;
                        break;
                }
                cmd.CommandText = queryString;
                cmd.Connection = myConn;

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(dr);
                myConn.Close();
                dr.Dispose();
                myConn.Dispose();

        }
        return dt;
    }

    public int ExecuteNonQuery(string commandText, params SqlParameter[] arrParam)
    {
        int retVal = 0;
        SqlParameter firstOutputParameter = null;

        // Open the connection
        using (SqlConnection cnn = new SqlConnection(connString))
        {
            cnn.Open();
            // Define the command 
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandText;

                // Handle the parameters 
                if (arrParam != null)
                {
                    foreach (SqlParameter param in arrParam)
                    {
                        cmd.Parameters.Add(param);
                        if (firstOutputParameter == null && param.Direction == ParameterDirection.Output && param.SqlDbType == SqlDbType.Int)
                            firstOutputParameter = param;
                    }
                }
                try
                {
                    // Execute the stored procedure 
                    cmd.ExecuteNonQuery();
                    // Return the first output parameter value 
                    if (firstOutputParameter != null)
                        retVal = (int)firstOutputParameter.Value;
                }
                catch(Exception e)
                {
                    retVal = 1;
                    err =e;
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            cnn.Dispose();
        }
        return retVal;
    }

    public int ExecuteScalar(string commandText, params SqlParameter[] arrParam)
    {
        int retVal = 0;
        SqlParameter firstOutputParameter = null;

        // Open the connection
        using (SqlConnection cnn = new SqlConnection(connString))
        {
            cnn.Open();
            // Define the command 
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandText;

                // Handle the parameters 
                if (arrParam != null)
                {
                    foreach (SqlParameter param in arrParam)
                    {
                        cmd.Parameters.Add(param);
                        if (firstOutputParameter == null && param.Direction == ParameterDirection.Output && param.SqlDbType == SqlDbType.Int)
                            firstOutputParameter = param;
                    }
                }
                // Execute the stored procedure 
                retVal = (Int32)cmd.ExecuteScalar();
                
                cmd.Dispose();
            }
            cnn.Dispose();
        }
        return retVal;
    }

    public object ExecuteProcedure(string procedureName, ExecuteType executeType,  params SqlParameter[] arrParam)
    {
        object returnObject = null;
        //SqlParameter firstOutputParameter = null;

        // Open the connection
        using (SqlConnection cnn = new SqlConnection(connString))
        {
            cnn.Open();
            // Define the command 
            using (SqlCommand cmd = new SqlCommand())
            {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procedureName;

                    // Handle the parameters 
                    if (arrParam != null)
                    {
                        foreach (SqlParameter param in arrParam)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }
                    // Execute the stored procedure 

                    switch (executeType)
                    {
                        case ExecuteType.ExecuteReader:
                            returnObject = cmd.ExecuteReader();
                            break;
                        case ExecuteType.ExecuteNonQuery:
                            SqlParameter returnValue = new SqlParameter("@returnValue", SqlDbType.Int);
                            returnValue.Direction = ParameterDirection.ReturnValue;
                            cmd.Parameters.Add(returnValue);
                            cmd.ExecuteNonQuery();
                            returnObject = (int)cmd.Parameters["@returnValue"].Value;
                            break;
                        case ExecuteType.ExecuteScalar:
                            returnObject = cmd.ExecuteScalar();
                            break;
                        case ExecuteType.ExecuteDatatable:
                            SqlDataReader dr = cmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            return dt;

                        default:
                            break;
                    }
                    cmd.Dispose();
            }
            cnn.Dispose();
        }
        return returnObject;
    }

}
