using Microsoft.Data.SqlClient;
using System.Data;

namespace DATN_Helpers.Database
{
    public class DBHelper
    {
        private string _cnnString = string.Empty;

        public string cnnString
        {
            get
            {
                return _cnnString;
            }
            set
            {
                _cnnString = value;
            }
        }

        /// <summary>
        /// Chuẩn hoá connection string
        /// </summary>
        public string FixCNN(string connStr, bool Pooling)
        {
            var aconnStr = connStr.Split(';');
            var sTemp = string.Empty;

            for (var i = 0; i < aconnStr.Length; i++)
            {
                if (aconnStr[i].ToLower().StartsWith("pooling=") ||
                    aconnStr[i].ToLower().StartsWith("min pool size=") ||
                    aconnStr[i].ToLower().StartsWith("max pool size=") ||
                    aconnStr[i].ToLower().StartsWith("connect timeout="))
                {
                    continue;
                }
                if (!aconnStr[i].Equals(string.Empty))
                {
                    sTemp += string.Format("{0};", aconnStr[i]);
                }
            }

            if (Pooling)
            {
                sTemp += "Pooling=true;Min Pool Size=5;Max Pool Size=25;Connect Timeout=5;";
            }
            else
            {
                sTemp += "Pooling=false;Connect Timeout=10;";
            }
            return sTemp;
        }

        public DBHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connStr"></param>
        public DBHelper(string connStr)
        {
            _cnnString = connStr;
        }

        /// <summary>
        /// 
        /// </summary>
        private SqlConnection _ConnectionToDB;

        public SqlConnection ConnectionToDB
        {
            get
            {
                return _ConnectionToDB;
            }
            set
            {
                _ConnectionToDB = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Open()
        {
            if (_cnnString == string.Empty)
            {
                throw new Exception("Connection String can not null");
            }
            _ConnectionToDB = OpenConnection();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            CloseConnection(_ConnectionToDB);
        }

        /// <summary>
        /// return an Open SqlConnection
        /// </summary>

        public SqlConnection OpenConnection(string connectionString)
        {
            try
            {
                _cnnString = connectionString;
                return OpenConnection();
            }
            catch (SqlException myException)
            {
                throw myException;// (new Exception(myException.Message));
            }
        }

        /// <summary>
        /// return an Open SqlConnection
        /// </summary>
        public SqlConnection OpenConnection()
        {
            if (_cnnString == string.Empty)
            {
                throw new Exception("Connection String can not null");
            }

            SqlConnection mySqlConnection;

            try
            {
                mySqlConnection = new SqlConnection(FixCNN(_cnnString, true));
                mySqlConnection.Open();
                return mySqlConnection;
            }
            catch (Exception)
            {
                // De phong truong hop bi max pool thi se fix lai connection string pooling=false
                mySqlConnection = new SqlConnection(FixCNN(_cnnString, false));
                mySqlConnection.Open();
                return mySqlConnection;
                // throw (new Exception(myException.Message));
            }
        }

        /// <summary>
        /// close an SqlConnection
        /// </summary>
        public void CloseConnection(SqlConnection mySqlConnection)
        {
            try
            {
                if (mySqlConnection != null)
                {
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                }
            }
            catch (SqlException myException)
            {
                throw myException;// (new Exception(myException.Message));
            }
        }

        #region ExcuteDataReader

        /// <summary>
        /// Trả về datareader với trường hợp CommandType=CommandType.StoreProcedure
        /// </summary>
        /// <param name="connection">connection</param>
        /// <param name="commandText"></param>
        /// <param name="sqlparam">sqlparametrer</param>
        public IDataReader ExecuteReader(string commandText, SqlParameter[] sqlparam)
        {
            try
            {
                SqlConnection con = _ConnectionToDB ?? OpenConnection();
                var com = new SqlCommand
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = commandText
                };
                if (sqlparam != null)
                {
                    com.Parameters.AddRange(sqlparam);
                }
                return com.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (DataException)
            {
                return null;
            }
        }

        public IDataReader ExecuteReader(string commandText, SqlParameter[] sqlparam, out SqlCommand comx)
        {
            try
            {
                SqlConnection con = _ConnectionToDB ?? OpenConnection();
                var com = new SqlCommand
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = commandText
                };
                if (sqlparam != null)
                {
                    com.Parameters.AddRange(sqlparam);
                }
                comx = com;
                return com.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (DataException)
            {
                comx = null;
                return null;
            }
        }

        /// <summary>
        /// Trả về datareader với trường hợp CommandType=CommandType.Textr
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commndText"></param>
        public IDataReader ExecuteReader(string commndText)
        {
            try
            {
                SqlConnection con = _ConnectionToDB ?? OpenConnection();
                var com = new SqlCommand
                {
                    CommandText = commndText,
                    CommandType = CommandType.Text,
                    Connection = con
                };
                return com.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (DataException)
            {
                return null;
            }
        }

        #endregion

        # region GetDataTable

        public DataTable GetDataTable(SqlCommand sqlCommand)
        {
            SqlConnection conn = null;
            try
            {
                if (_ConnectionToDB == null)
                {
                    conn = OpenConnection();
                    sqlCommand.Connection = conn;
                }
                else
                {
                    sqlCommand.Connection = _ConnectionToDB;
                }
                var myDataAdapter = new SqlDataAdapter(sqlCommand);
                var dt = new DataTable();
                myDataAdapter.Fill(dt);
                myDataAdapter.Dispose();
                return dt;
            }
            catch (Microsoft.Data.SqlClient.SqlException myException)
            {
                throw myException; //(new Exception(myException.Message));
            }
            finally
            {
                if (conn != null)
                {
                    CloseConnection(conn);
                }
            }
        }

        public DataTable GetDataTable(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return GetDataTable(sqlCommand);
        }

        public DataTable GetDataTable(string strSQL)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return GetDataTable(sqlCommand);
        }

        public DataTable GetDataTable(string strSQL, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return GetDataTable(sqlCommand, Parameters);
        }

        #endregion

        # region ExecuteScalar

        public object ExecuteScalar(SqlCommand sqlCommand)
        {
            SqlConnection conn = null;
            try
            {
                if (_ConnectionToDB == null)
                {
                    conn = OpenConnection();
                    sqlCommand.Connection = conn;
                }
                else
                {
                    sqlCommand.Connection = _ConnectionToDB;
                }
                return sqlCommand.ExecuteScalar();
            }
            catch (SqlException myException)
            {
                throw myException;// (new Exception(myException.Message));
            }
            finally
            {
                if (conn != null)
                {
                    CloseConnection(conn);
                }
            }
        }

        public object ExecuteScalar(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return ExecuteScalar(sqlCommand);
        }

        public object ExecuteScalar(string strSQL)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return ExecuteScalar(sqlCommand);
        }

        public object ExecuteScalar(string strSQL, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return ExecuteScalar(sqlCommand, Parameters);
        }

        #endregion

        # region ExecuteNonQuery

        public int ExecuteNonQuery(SqlCommand sqlCommand, int timeout)
        {
            SqlConnection conn = null;
            try
            {
                if (_ConnectionToDB == null)
                {
                    conn = OpenConnection();

                    sqlCommand.Connection = conn;
                }
                else
                {
                    sqlCommand.Connection = _ConnectionToDB;
                }

                sqlCommand.CommandTimeout = timeout;
                return sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException myException)
            {
                throw myException;
            }
            finally
            {
                if (conn != null)
                {
                    CloseConnection(conn);
                }
            }
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand)
        {
            SqlConnection conn = null;
            try
            {
                if (_ConnectionToDB == null)
                {
                    conn = OpenConnection();
                    sqlCommand.Connection = conn;
                }
                else
                {
                    sqlCommand.Connection = _ConnectionToDB;
                }
                return sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException myException)
            {
                throw myException; //(new Exception(myException.Message));
            }
            finally
            {
                if (conn != null)
                {
                    CloseConnection(conn);
                }
            }
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand, int timeout, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return ExecuteNonQuery(sqlCommand, timeout);
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return ExecuteNonQuery(sqlCommand);
        }

        public int ExecuteNonQuery(string strSQL)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return ExecuteNonQuery(sqlCommand);
        }

        public int ExecuteNonQuery(string strSQL, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return ExecuteNonQuery(sqlCommand, Parameters);
        }

        #endregion

        #region ExecuteScalarSP

        public object ExecuteScalarSP(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return ExecuteScalar(sqlCommand);
        }

        public object ExecuteScalarSP(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return ExecuteScalar(sqlCommand, Parameters);
        }

        #endregion

        #region ExecuteNonQuerySP

        public int ExecuteNonQuerySP(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return ExecuteNonQuery(sqlCommand);
        }

        public int ExecuteNonQuerySP(string SPName, int timeout, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return ExecuteNonQuery(sqlCommand, timeout, Parameters);
        }

        public int ExecuteNonQuerySP(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };

            return ExecuteNonQuery(sqlCommand, Parameters);
        }

        public int ExecuteNonQuery(string commandText, SqlParameter[] sqlparam, out SqlCommand comx)
        {
            try
            {
                SqlConnection con = _ConnectionToDB ?? OpenConnection();
                using (con)
                {
                    var com = new SqlCommand
                    {
                        CommandText = commandText,
                        CommandType = CommandType.StoredProcedure,
                        Connection = con
                    };
                    if (sqlparam != null)
                    {
                        com.Parameters.AddRange(sqlparam);
                    }
                    comx = com;
                    return com.ExecuteNonQuery();
                }
            }
            catch (DataException)
            {
                comx = null;
                return -1;
            }
        }

        #endregion

        #region GetDataTableSP

        public DataTable GetDataTableSP(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetDataTable(sqlCommand);
        }

        public DataTable GetDataTableSP(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetDataTable(sqlCommand, Parameters);
        }

        #endregion

        #region[ListGenerate]

        public List<T> GetList<T>(SqlCommand sqlCommand)
        {
            try
            {
                sqlCommand.Connection = _ConnectionToDB ?? OpenConnection();

                var dr = sqlCommand.ExecuteReader();
                if (dr == null || dr.FieldCount == 0)
                {
                    return null;
                }
                var fCount = dr.FieldCount;
                var m_Type = typeof(T);
                var l_Property = m_Type.GetProperties();
                object obj;
                var m_List = new List<T>();
                string pName;
                while (dr.Read())
                {
                    obj = Activator.CreateInstance(m_Type);
                    for (var i = 0; i < fCount; i++)
                    {
                        pName = dr.GetName(i);
                        if (l_Property.Where(a => a.Name == pName).Select(a => a.Name).Count() <= 0)
                        {
                            continue;
                        }
                        if (dr[i] != DBNull.Value)
                        {
                            m_Type.GetProperty(pName).SetValue(obj, dr[i], null);
                        }
                        else
                        {
                            m_Type.GetProperty(pName).SetValue(obj, null, null);
                        }
                    }
                    m_List.Add((T)obj);
                }
                dr.Close();
                return m_List;
            }
            catch (SqlException myException)
            {
                throw myException; //(new Exception(myException.Message));
            }
            finally
            {
                CloseConnection(sqlCommand.Connection);
            }
        }

        public List<T> GetList<T>(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return GetList<T>(sqlCommand);
        }

        public List<T> GetList<T>(string strSQL)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return GetList<T>(sqlCommand);
        }

        public List<T> GetList<T>(string strSQL, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            sqlCommand.Parameters.AddRange(Parameters);
            return GetList<T>(sqlCommand);
        }

        #endregion

        #region[ListGenerateSP]

        public List<T> GetListSP<T>(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetList<T>(sqlCommand);
        }

        public List<T> GetListSP<T>(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };

            return GetList<T>(sqlCommand, Parameters);
        }

        #endregion

        #region[GetInstance]

        public T GetInstance<T>(SqlCommand sqlCommand)
        {
            try
            {
                T temp = default(T);

                sqlCommand.Connection = _ConnectionToDB ?? OpenConnection();
                var dr = sqlCommand.ExecuteReader();
                if (dr.Read())
                {
                    var fCount = dr.FieldCount;
                    var m_Type = typeof(T);
                    var l_Property = m_Type.GetProperties();
                    object obj;
                    var m_List = new List<T>();
                    string pName;

                    obj = Activator.CreateInstance(m_Type);
                    for (var i = 0; i < fCount; i++)
                    {
                        pName = dr.GetName(i);
                        if (l_Property.Where(a => a.Name == pName).Select(a => a.Name).Count() <= 0)
                        {
                            continue;
                        }
                        if (dr[i] != DBNull.Value)
                        {
                            m_Type.GetProperty(pName).SetValue(obj, dr[i], null);
                        }
                        else
                        {
                            m_Type.GetProperty(pName).SetValue(obj, null, null);
                        }
                    }
                    dr.Close();
                    return (T)obj;
                }
                else
                {
                    return temp;
                }
            }
            catch (SqlException myException)
            {
                throw myException; //(new Exception(myException.Message));
            }
            finally
            {
                CloseConnection(sqlCommand.Connection);
            }
        }

        public T GetInstance<T>(SqlCommand sqlCommand, params SqlParameter[] Parameters)
        {
            sqlCommand.Parameters.AddRange(Parameters);
            return GetInstance<T>(sqlCommand);
        }

        public T GetInstance<T>(string strSQL)
        {
            var sqlCommand = new SqlCommand(strSQL);
            return GetInstance<T>(sqlCommand);
        }

        public T GetInstance<T>(string strSQL, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(strSQL);
            sqlCommand.Parameters.AddRange(Parameters);
            return GetInstance<T>(sqlCommand);
        }

        #endregion

        #region[GetInstanceSP]

        public T GetInstanceSP<T>(string SPName)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetInstance<T>(sqlCommand);
        }

        public T GetInstanceSP<T>(string SPName, params SqlParameter[] Parameters)
        {
            var sqlCommand = new SqlCommand(SPName) { CommandType = CommandType.StoredProcedure };
            return GetInstance<T>(sqlCommand, Parameters);
        }

        #endregion



        #region Lấy dữ liệu có mối quan hệ phức tạp (test)
        /// <summary>
        /// Chuyển đổi dữ liệu từ SqlDataReader thành một List các đối tượng kiểu T
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của đối tượng cần chuyển đổi</typeparam>
        /// <param name="reader">SqlDataReader chứa dữ liệu cần chuyển đổi</param>
        /// <returns>List các đối tượng kiểu T được map từ dữ liệu của SqlDataReader</returns>
        /// <remarks>
        /// - Tự động map các trường dữ liệu từ reader vào properties của đối tượng <br/>
        /// - Xử lý các kiểu nullable <br/>
        /// - Bỏ qua các trường không tồn tại trong đối tượng đích <br/>
        /// - Set null cho các giá trị không thể chuyển đổi <br/>
        /// </remarks>
        private List<T> MapToList<T>(SqlDataReader reader)
        {
            var results = new List<T>();
            var type = typeof(T);
            var properties = type.GetProperties();
            var fieldNames = Enumerable.Range(0, reader.FieldCount)
                                      .Select(i => reader.GetName(i))
                                      .ToList();

            while (reader.Read())
            {
                var item = Activator.CreateInstance<T>();
                foreach (var property in properties)
                {
                    // Check if the property exists in the reader
                    if (fieldNames.Contains(property.Name))
                    {
                        var value = reader[property.Name];
                        if (value != DBNull.Value)
                        {
                            try
                            {
                                // Handle nullable types
                                if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                                {
                                    property.SetValue(item, Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType)));
                                }
                                else
                                {
                                    property.SetValue(item, Convert.ChangeType(value, property.PropertyType));
                                }
                            }
                            catch (Exception)
                            {
                                // If conversion fails, set default value
                                property.SetValue(item, null);
                            }
                        }
                    }
                }
                results.Add(item);
            }

            return results;
        }


        /// <summary>
        /// Thực thi stored procedure và trả về 2 tập kết quả dạng List <br/>
        /// Cái này giống với GetListSP nhưng khác ở chỗ lấy được mối quan hệ
        /// </summary>
        /// <typeparam name="T1">Kiểu dữ liệu của tập kết quả thứ nhất</typeparam>
        /// <typeparam name="T2">Kiểu dữ liệu của tập kết quả thứ hai</typeparam>
        /// <param name="storedProcedure">Tên stored procedure</param>
        /// <param name="parameters">Mảng tham số truyền vào stored procedure</param>
        /// <returns>Tuple chứa 2 List kết quả (List{T1}, List{T2})</returns>
        /// <remarks>
        /// Sử dụng để lấy 2 tập dữ liệu có quan hệ với nhau từ cùng một stored procedure <br/>
        /// Ví dụ: Ví dụ 1 movie có thể có nhiều actor <br/>
        /// </remarks>
        public (List<T1>, List<T2>) GetMultipleSP<T1, T2>(string storedProcedure, SqlParameter[] parameters)
        {
            var result1 = new List<T1>();
            var result2 = new List<T2>();

            SqlConnection conn = null;
            try
            {
                if (_ConnectionToDB == null)
                {
                    conn = OpenConnection();
                }

                using (var command = new SqlCommand(storedProcedure, conn ?? _ConnectionToDB))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        result1 = MapToList<T1>(reader);

                        if (reader.NextResult())
                        {
                            result2 = MapToList<T2>(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    CloseConnection(conn);
                }
            }

            return (result1, result2);
        }

        /// <summary>
        /// Thực thi stored procedure và trả về 1 đối tượng + cùng với 1 tập kết quả dạng List (quan hệ mình muốn lấy) <br/>
        /// Cái này giống với GetInstanceSP nhưng khác ở chỗ lấy được mối quan hệ
        /// </summary>
        /// <typeparam name="T1">Kiểu dữ liệu của đối tượng đơn lẻ</typeparam>
        /// <typeparam name="T2">Kiểu dữ liệu của tập kết quả danh sách</typeparam>
        /// <param name="storedProcedure">Tên stored procedure</param>
        /// <param name="parameters">Mảng tham số truyền vào stored procedure</param>
        /// <returns>Tuple chứa (đối tượng T1, List{T2})</returns>
        /// <remarks>
        /// Sử dụng khi cần lấy 1 đối tượng chính và danh sách các đối tượng liên quan <br/>
        /// Ví dụ: Lấy thông tin 1 movie + danh sách actor trong movie đó
        /// </remarks>
        public (T1, List<T2>) GetSingleSP<T1, T2>(string storedProcedure, SqlParameter[] parameters) where T1 : class, new()
        {
            T1 result1 = null;
            var result2 = new List<T2>();
            SqlConnection conn = null;

            try
            {
                if (_ConnectionToDB == null)
                {
                    conn = OpenConnection();
                }

                using (var command = new SqlCommand(storedProcedure, conn ?? _ConnectionToDB))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        // Lấy đối tượng đầu tiên
                        var items = MapToList<T1>(reader);
                        result1 = items.FirstOrDefault();

                        // Lấy danh sách đối tượng thứ hai
                        if (reader.NextResult())
                        {
                            result2 = MapToList<T2>(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    CloseConnection(conn);
                }
            }

            return (result1, result2);
        }
        #endregion
    }
}