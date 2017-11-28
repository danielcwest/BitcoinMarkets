using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Core.DbService
{
    public static class DbServiceHelper
    {

        public static void BulkCopySqlTable(DataTable sourceTable, string destinationTableName, string connectionString)
        {
            if (sourceTable == null || sourceTable.Rows.Count == 0)
                return;

            using (var destConnection = new SqlConnection(connectionString))
            using (SqlBulkCopy s = new SqlBulkCopy(destConnection))
            {
                s.DestinationTableName = destinationTableName;
                destConnection.Open();
                s.WriteToServer(sourceTable);
            }
        }

        public static void ExecuteNonQuery(string sqlConnectionString, string sprocName, int commandTimeoutInSeconds, params object[] sqlParams)
        {
            using (var cn = new SqlConnection(sqlConnectionString))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sprocName;
                cmd.CommandTimeout = commandTimeoutInSeconds;

                if (sqlParams != null)
                    foreach (var p in sqlParams)
                        cmd.Parameters.Add(p);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Caller needs to close the reader!!
        /// </summary>
        /// <param name="sqlConnectionString"></param>
        /// <param name="sprocName"></param>
        /// <param name="commandTimeoutInSeconds"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public static IDataReader ExecuteQuery(string sqlConnectionString, string sprocName, int commandTimeoutInSeconds, params object[] sqlParams)
        {
            var cn = new SqlConnection(sqlConnectionString);
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sprocName;
                cmd.CommandTimeout = commandTimeoutInSeconds;

                if (sqlParams != null)
                    foreach (var p in sqlParams)
                        cmd.Parameters.Add(p);
                cn.Open();

                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public static object ExecuteScalar(string sqlConnectionString, string sprocName, int commandTimeOutInSeconds, params object[] sqlParams)
        {
            using (var cn = new SqlConnection(sqlConnectionString))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sprocName;
                cmd.CommandTimeout = commandTimeOutInSeconds;

                if (sqlParams != null)
                    foreach (var p in sqlParams)
                        cmd.Parameters.Add(p);

                cn.Open();

                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Convert IEnumerable to DataTable. Callers are responsbile to dispose the DataTable object before it goes out of scope.
        /// </summary>
        /// <typeparam name="T">Generic type of objects to be converted</typeparam>
        /// <param name="data">objects to be converted</param>
        /// <returns>DataTable object</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            //if input is null, we return a empty DataTable with columns added
            if (data == null)
            {
                data = new List<T>();
            }

            List<T> list = data.Cast<T>().ToList();

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.CurrentCulture;

            //Special handling if we receive a collection of string objects, we want to map to our specific TVP & contract, which is a single column called "StringValue"
            if (typeof(T) == typeof(string))
            {
                table.Columns.Add("StringValue", typeof(string));
                foreach (var item in list)
                {
                    table.Rows.Add(item);
                }
                return table;
            }

            //Special handling if we receive a collection of integer objects, we want to map to our specific TVP & contract, which is a single column called "IntValue"
            if (typeof(T) == typeof(int))
            {
                table.Columns.Add("IntValue", typeof(int));
                foreach (var item in list)
                {
                    table.Rows.Add(item);
                }
                return table;
            }

            if (typeof(T) == typeof(long))
            {
                table.Columns.Add("LongValue", typeof(long));
                foreach (var item in list)
                {
                    table.Rows.Add(item);
                }
                return table;
            }

            //Get the fields for these objects
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

            // Populate DataTable columns based on object's field names
            foreach (var fieldInfo in fields)
            {
                table.Columns.Add(fieldInfo.Name, Nullable.GetUnderlyingType(fieldInfo.FieldType) ?? fieldInfo.FieldType);
            }

            // Populate DataTable rows
            if (list != null || list.Any() && fields != null)
            {
                object[] values = new object[fields.Length];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = fields[i].GetValue(item) ?? DBNull.Value;
                    }
                    table.Rows.Add(values);
                }
            }
            return table;
        }

        /// <summary>
        /// Extension method for IDataReader object that constructs a list of objects of generic type T by reading from the datareader.
        /// Only the public properties of an instance object will be set.
        /// Example usage:
        ///     var customerList = sqlDataReader.ToList<Customer>();
        /// </summary>
        /// <typeparam name="T">Returned object type, must be an instance</typeparam>
        /// <param name="dataReader">DataReader object to be read from</param>
        /// <returns>A list of objects of type T</returns>
        public static List<T> ToList<T>(this IDataReader dataReader)
        {
            // Instantiate return list
            List<T> list = new List<T>();
            T obj = default(T);
            object boxedObj = null;

            while (dataReader.Read())
            {
                obj = Activator.CreateInstance<T>();

                // The value of T is being boxed for struct, more details at http://stackoverflow.com/questions/6280506/is-there-a-way-to-set-properties-on-struct-instances-using-reflection
                boxedObj = obj;

                foreach (var fieldInfo in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance))     // Only setting public property of the instance object
                {
                    if (!Equals(dataReader[fieldInfo.Name], DBNull.Value))
                    {
                        //fieldInfo.SetValueDirect(obj, Convert.ChangeType(dataReader[fieldInfo.Name], fieldInfo.FieldType));
                        fieldInfo.SetValue(boxedObj, Convert.ChangeType(dataReader[fieldInfo.Name], fieldInfo.FieldType));
                    }
                }

                list.Add((T)boxedObj);
            }

            return list;
        }


    }

}