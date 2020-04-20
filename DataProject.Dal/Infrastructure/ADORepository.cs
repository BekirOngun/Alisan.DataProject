using IKYS.Dal.Infrastructure;
using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace DataProject.Dal
{

    #region ABSTRACT

    //.. ConBase
    [Serializable]
    public abstract class ConBase
    {

        #region VARIABLES

        public DbConnection CON;

        #endregion

        #region PROPERTIES

        public string IDB_provider { get { return "System.Data.SqlClient"; } }
        public string IDB_conStrDef { get; set; }
        public string IDB_dbName { get { return "DERBIS"; } }
        public string IDB_owner { get { return "dbo"; } }
        public string IDB_conStr { get; set; }
        public string IDB_connectionError { get; set; }
        public string IDB_commandError { get; set; }
        public string IDB_prefix { get { return "["; } }
        public string IDB_sufix { get { return "]"; } }
        public string IDB_schemaSeparator { get { return "."; } }
        public string IDB_parameterMarker { get { return "@"; } }
        public bool IDB_isConnected { get; set; }
        public int IDB_rows_affected { get; set; }
        public long IDB_last_identity { get; set; }

        #endregion

        #region CONSTRUCTOR

        public ConBase()
        {

            IDB_conStr = Cryptographer.GetConnectionString();

        }

        #endregion

        #region METHODS

        //.. IDBgetConnection
        public DbConnection IDBgetConnection()
        {
            DbProviderFactory fac = null;
            if (string.IsNullOrEmpty(IDB_conStr)) IDB_conStr = IDB_conStrDef;
            fac = DbProviderFactories.GetFactory(IDB_provider);
            try
            {
                DbConnection con = fac.CreateConnection();
                con.ConnectionString = IDB_conStr;
                IDB_connectionError = "ok";
                IDB_isConnected = true;
                DbCommandBuilder cb = fac.CreateCommandBuilder();
                return con;
            }
            catch (Exception ee)
            {
                IDB_connectionError = ee.Message.ToString();
                IDB_conStr = string.Empty;
                IDB_isConnected = false;
            }
            return null;
        }

        #endregion

    }

    #endregion

    #region OBJECTS

    //.. Table
    [Serializable]
    public class Table
    {
        public string id { get; set; }
        public string tablecatalog { get; set; }
        public string tableschema { get; set; }
        public string tablename { get; set; }
        public string columncount { get; set; }

        public Table()
        { }

        public override string ToString()
        {
            return string.Format("[{0}].[{1}]", tableschema, tablename);
        }
    }

    //.. Column
    [Serializable]
    public class Column
    {
        public string id { get; set; }
        public string tablecatalog { get; set; }
        public string tableschema { get; set; }
        public string tablename { get; set; }
        public string columnname { get; set; }
        public string ordinalposition { get; set; }
        public string columndefault { get; set; }
        public string nullable { get; set; }
        public string datatype { get; set; }
        public string charactermaximumlength { get; set; }
        public string sequencename { get; set; }
        public string isprimary { get; set; }
        public string isidentity { get; set; }
        public string collationname { get; set; }

        public Column()
        { }

        public override string ToString()
        {
            return string.Format("{0}", columnname);
        }
    }

    //.. DbParam
    [Serializable]
    public class DbParam
    {
        public object value { get; set; }
        public DbType dbType { get; set; }
        public ParameterDirection direction { get; set; }

        public DbParam(object pvalue)
        {
            this.value = pvalue;
            this.dbType = DbType.String;
            this.direction = ParameterDirection.Input;
        }

        public DbParam(object pvalue, DbType ptype)
        {
            this.value = pvalue;
            this.dbType = ptype;
            this.direction = ParameterDirection.Input;
        }

        public DbParam(object pvalue, DbType ptype, ParameterDirection pdirection)
        {
            this.value = pvalue;
            this.dbType = ptype;
            this.direction = pdirection;
        }
    }

    #endregion

    #region MSSQL

    //.. MSSQL
    [Serializable]
    public class MSSQL : ConBase
    {
        #region CONSTRUCTOR

        public MSSQL()
        {
            //.. create connection
            CON = IDBgetConnection();
        }

        #endregion

        #region BASE METHODS

        //.. ConOpen
        public void ConOpen()
        {
            switch (CON.State)
            {
                //case ConnectionState.Connecting:
                //case ConnectionState.Executing:
                //case ConnectionState.Fetching:
                case ConnectionState.Broken:
                case ConnectionState.Closed:
                    {
                        CON.Open();
                        break;
                    }
            }

        }

        //.. ConClose
        public void ConClose()
        {
            switch (CON.State)
            {
                //case ConnectionState.Connecting:
                //case ConnectionState.Executing:
                //case ConnectionState.Fetching:
                case ConnectionState.Open:
                    {
                        CON.Close();
                        break;
                    }
                default:
                    break;
            }
        }

        //.. Select
        public DataTable Select(string sql)
        {
            if (CON != null)
            {
                DataTable dt = new DataTable();
                try
                {
                    ConOpen();
                    DbDataReader dr;
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    dr = cmd.ExecuteReader();
                    dt.BeginLoadData();
                    dt.Load(dr);
                    dt.EndLoadData();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
                return dt;
            }
            else
                return null;
        }
        public DataTable Select(string sql, CommandType commandType)
        {
            if (CON != null)
            {
                DataTable dt = new DataTable();
                try
                {
                    ConOpen();
                    DbDataReader dr;
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = commandType;
                    dr = cmd.ExecuteReader();
                    dt.BeginLoadData();
                    dt.Load(dr);
                    dt.EndLoadData();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
                return dt;
            }
            else
                return null;
        }
        public DataTable Select(string sql, params DbParam[] values)
        {
            if (CON != null)
            {
                DataTable dt = new DataTable();
                try
                {
                    ConOpen();
                    DbDataReader dr;
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        //cmd.Parameters.AddWithValue(mc[i].Value, values[i]);
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i].value == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i].value;
                        p.DbType = values[i].dbType;
                        p.Direction = values[i].direction;
                        cmd.Parameters.Add(p);
                    }
                    dr = cmd.ExecuteReader();
                    dt.BeginLoadData();
                    dt.Load(dr);
                    dt.EndLoadData();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
                return dt;
            }
            else
                return null;
        }
        public DataTable Select(string sql, CommandType commandType, params DbParam[] values)
        {
            if (CON != null)
            {
                DataTable dt = new DataTable();
                try
                {
                    ConOpen();
                    DbDataReader dr;
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = commandType;

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        //cmd.Parameters.AddWithValue(mc[i].Value, values[i]);
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i].value == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i].value;
                        p.DbType = values[i].dbType;
                        p.Direction = values[i].direction;
                        cmd.Parameters.Add(p);
                    }
                    dr = cmd.ExecuteReader();
                    dt.BeginLoadData();
                    dt.Load(dr);
                    dt.EndLoadData();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
                return dt;
            }
            else
                return null;
        }
        public DataTable Select(string sql, params object[] values)
        {
            if (CON != null)
            {
                DataTable dt = new DataTable();
                try
                {
                    ConOpen();
                    DbDataReader dr;
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i] == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i];
                        cmd.Parameters.Add(p);
                    }

                    dr = cmd.ExecuteReader();
                    dt.BeginLoadData();
                    dt.Load(dr);
                    dt.EndLoadData();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
                return dt;
            }
            else
                return null;
        }
        public DataTable Select(string sql, CommandType commandType, params object[] values)
        {
            if (CON != null)
            {
                DataTable dt = new DataTable();
                try
                {
                    ConOpen();
                    DbDataReader dr;
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = commandType;

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i] == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i];
                        cmd.Parameters.Add(p);
                    }
                    dr = cmd.ExecuteReader();
                    dt.BeginLoadData();
                    dt.Load(dr);
                    dt.EndLoadData();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
                return dt;
            }
            else
                return null;
        }

        //.. Transact
        public int Transact(string sql)
        {
            int res = -1;
            if (CON != null)
            {
                DbTransaction trans = null;
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    trans = CON.BeginTransaction();
                    cmd.Transaction = trans;
                    res = cmd.ExecuteNonQuery();
                    IDB_commandError = "ok";
                    trans.Commit();
                    IDB_rows_affected = res;
                }
                catch (Exception ee)
                {
                    IDB_rows_affected = 0;
                    IDB_commandError = ee.Message.ToString();
                    trans.Rollback();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }
        public int Transact(string sql, CommandType commandType)
        {
            int res = -1;
            if (CON != null)
            {
                DbTransaction trans = null;
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = commandType;
                    trans = CON.BeginTransaction();
                    cmd.Transaction = trans;
                    res = cmd.ExecuteNonQuery();
                    IDB_commandError = "ok";
                    trans.Commit();
                    IDB_rows_affected = res;
                }
                catch (Exception ee)
                {
                    IDB_rows_affected = 0;
                    IDB_commandError = ee.Message.ToString();
                    trans.Rollback();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }
        public int Transact(string sql, params DbParam[] values)
        {
            int res = -1;
            if (CON != null)
            {
                DbTransaction trans = null;
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    trans = CON.BeginTransaction();

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i].value == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i].value;
                        p.DbType = values[i].dbType;
                        p.Direction = values[i].direction;
                        cmd.Parameters.Add(p);
                    }
                    cmd.Transaction = trans;
                    res = cmd.ExecuteNonQuery();
                    IDB_rows_affected = res;
                    IDB_commandError = "ok";
                    trans.Commit();
                }
                catch (Exception ee)
                {
                    IDB_rows_affected = 0;
                    IDB_commandError = ee.Message.ToString();
                    trans.Rollback();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }
        public int Transact(string sql, CommandType commandType, params DbParam[] values)
        {
            int res = -1;
            if (CON != null)
            {
                DbTransaction trans = null;
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = commandType;
                    trans = CON.BeginTransaction();

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i].value == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i].value;
                        p.DbType = values[i].dbType;
                        p.Direction = values[i].direction;
                        cmd.Parameters.Add(p);
                    }
                    cmd.Transaction = trans;
                    res = cmd.ExecuteNonQuery();
                    IDB_commandError = "ok";
                    trans.Commit();
                    IDB_rows_affected = res;
                }
                catch (Exception ee)
                {
                    IDB_rows_affected = 0;
                    IDB_commandError = ee.Message.ToString();
                    trans.Rollback();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }
        public int Transact(string sql, params object[] values)
        {
            int res = -1;
            if (CON != null)
            {
                DbTransaction trans = null;
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    trans = CON.BeginTransaction();

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i] == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i];
                        cmd.Parameters.Add(p);
                    }
                    cmd.Transaction = trans;
                    res = cmd.ExecuteNonQuery();
                    IDB_commandError = "ok";
                    trans.Commit();
                    IDB_rows_affected = res;
                }
                catch (Exception ee)
                {
                    IDB_rows_affected = 0;
                    IDB_commandError = ee.Message.ToString();
                    trans.Rollback();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }
        public int Transact(string sql, CommandType commandType, params object[] values)
        {
            int res = -1;
            if (CON != null)
            {
                DbTransaction trans = null;
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = commandType;
                    trans = CON.BeginTransaction();

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i] == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i];
                        cmd.Parameters.Add(p);
                    }
                    cmd.Transaction = trans;
                    res = cmd.ExecuteNonQuery();
                    IDB_commandError = "ok";
                    trans.Commit();
                    IDB_rows_affected = res;
                }
                catch (Exception ee)
                {
                    IDB_rows_affected = 0;
                    IDB_commandError = ee.Message.ToString();
                    trans.Commit();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }

        //.. Scalar
        public object Scalar(string sql)
        {
            object res = new object();
            res = null;
            if (CON != null)
            {
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    res = cmd.ExecuteScalar();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }
        public object Scalar(string sql, CommandType commandType)
        {
            object res = new object();
            res = null;

            if (CON != null)
            {
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = commandType;
                    res = cmd.ExecuteScalar();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }
        public object Scalar(string sql, params DbParam[] values)
        {
            object res = new object();
            res = null;

            if (CON != null)
            {
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i].value == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i].value;
                        p.DbType = values[i].dbType;
                        p.Direction = values[i].direction;
                        cmd.Parameters.Add(p);
                    }

                    res = cmd.ExecuteScalar();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }
        public object Scalar(string sql, CommandType commandType, params DbParam[] values)
        {
            object res = new object();
            res = null;

            if (CON != null)
            {
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = commandType;

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i].value == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i].value;
                        p.DbType = values[i].dbType;
                        p.Direction = values[i].direction;
                        cmd.Parameters.Add(p);
                    }

                    res = cmd.ExecuteScalar();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }
        public object Scalar(string sql, params object[] values)
        {
            object res = new object();
            res = null;

            if (CON != null)
            {
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i] == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i];
                        cmd.Parameters.Add(p);
                    }

                    res = cmd.ExecuteScalar();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }
        public object Scalar(string sql, CommandType commandType, params object[] values)
        {
            object res = new object();
            res = null;

            if (CON != null)
            {
                try
                {
                    ConOpen();
                    DbCommand cmd = CON.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.CommandType = commandType;

                    MatchCollection mc = Regex.Matches(sql, string.Format("[{0}]\\w+", IDB_parameterMarker));
                    for (int i = 0; i < mc.Count; i++)
                    {
                        DbParameter p = cmd.CreateParameter();
                        p.ParameterName = mc[i].Value;
                        if (values[i] == null)
                            p.Value = DBNull.Value;
                        else
                            p.Value = values[i];
                        cmd.Parameters.Add(p);
                    }
                    res = cmd.ExecuteScalar();
                    IDB_commandError = "ok";
                }
                catch (Exception ee)
                {
                    IDB_commandError = ee.Message.ToString();
                }
                finally
                {
                    ConClose();
                }
            }
            return res;
        }

        #endregion
    }

    #endregion


}