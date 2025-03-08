﻿using Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace CS2AAranezAllen_MVCProject.BusLogic.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        IDbConnection _connection;

        readonly string connectionString = "Server=DESKTOP-NATU95R\\SQLEXPRESS; Database=SampleMVC_db; Trusted_Connection=True; MultipleActiveResultSets=true";
        public GenericRepository()
        {

            _connection = new SqlConnection(connectionString);
        }

        public T AddParentAttribute(T entity)
        {

            int rowsEffected = 0;
            try
            {
                string tableName = GetTableName();
                string columns = GetColumns(excludeKey: true);
                string properties = GetPropertyNames(excludeKey: true);
                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";

                rowsEffected = _connection.Execute(query, entity);

                if (rowsEffected > 0)
                {
                    IEnumerable<T> result = null;
                    string keyColumn = GetKeyColumnName();

                    query = $"SELECT * FROM {tableName} Order by {keyColumn} desc";

                    result = _connection.Query<T>(query);
                    return result.FirstOrDefault();
                }
            }
            catch (Exception ex) { }
            return null;

        }

        public bool Add(T entity)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = GetTableName();
                string columns = GetColumns(excludeKey: true);
                string properties = GetPropertyNames(excludeKey: true);
                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";

                rowsEffected = _connection.Execute(query, entity);
            }
            catch (Exception ex) { }

            return rowsEffected > 0 ? true : false;
        }
        public IEnumerable<T> GetAll()
        {
            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();
                string query = $"SELECT * FROM {tableName}";

                result = _connection.Query<T>(query);
            }
            catch (Exception ex) { }

            return result;
        }
        public T GetById(int id)
        {

            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string query = $"SELECT * FROM {tableName} WHERE {keyColumn} = '{id}'";

                result = _connection.Query<T>(query);
            }
            catch (Exception ex) { }

            return result.FirstOrDefault();
        }

        public T Delete(int id)
        {
            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();

                string query = $"DELETE FROM {tableName} WHERE {keyColumn} = '{id}'";
                result = _connection.Query<T>(query);
            }
            catch (Exception ex)
            {
                // Handle exception or log error
            }
            return result.FirstOrDefault();
        }
        public bool Update(T entity)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();

                StringBuilder query = new StringBuilder();
                query.Append($"UPDATE {tableName} SET ");

                foreach (var property in GetProperties(true))
                {
                    var columnAttr = property.GetCustomAttribute<ColumnAttribute>();

                    string propertyName = property.Name;
                    string columnName = property.Name;

                    query.Append($"{columnName} = ISNULL(@{propertyName},{columnName}),");
                }

                query.Remove(query.Length - 1, 1);

                query.Append($" WHERE {keyColumn} = @{keyProperty}");

                rowsEffected = _connection.Execute(query.ToString(), entity);
            }
            catch (Exception ex) { }

            return rowsEffected > 0 ? true : false;
        }

        private string GetTableName()
        {
            string tableName = "";
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return type.Name;
        }

        public static string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }

        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(T);
            var columns = string.Join(", ", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }));

            return columns;
        }

        protected string GetPropertyNames(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;
        }
        protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

        protected string GetKeyPropertyName()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }
    }
}
