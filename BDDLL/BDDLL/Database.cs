using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDLL
{
    public class Database
    {
        private Database() { } // Запрет на создание ещё одного экземпляра класса.
        private static Database Instance; // Единственный экземпляр Database для работы с БД.
        private string sqlConnectionString;
        private delegate object Handler(SqlCommand cmd); // Делагат для определения функции, которая принимает SqlCommand
        private void ExecuteCommand(Handler handler, string query, out object result, Parameter[] parameters = null)
        {
            result = null;
            try
            {
                // Подключиться в БД.
                SqlConnection connection = new SqlConnection(sqlConnectionString);
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                // Если список параметров не пустой, переписать их в параметры `SqlCommand cmd`
                if (parameters != null)
                {
                    foreach (Parameter param in parameters)
                    {
                        /*
                        * cmd.Parameters.AddWithValue() это то же самое, что Add().
                        * Подсказки рекомендуют использовать AddWithValue(), так как Add() устарел.
                        */
                        cmd.Parameters.AddWithValue(param.Name, param.Value);
                    }
                }
                // Если обработчик есть, вызвать его и получить вернувшееся значение.
                if (handler != null) result = handler.Invoke(cmd);
                connection.Close();
            }
            catch (Exception ex) { /* ignore */ }
        }

        /// <summary>
        /// Получить единственный экземпляр Database.
        /// </summary>
        /// <param name="sqlconnectionString">строка подключения к БД</param>
        /// <returns></returns>
        public static Database GetInstance(string sqlconnectionString = null)
        {
            // Если экзепляр Instance не существует, инициализировать новый.
            if (Instance == null)
            {
                Instance = new Database();
            }
            if (sqlconnectionString != null) Instance.sqlConnectionString = sqlconnectionString;
            return Instance;
        }

        /// <summary>
        /// Выполнить запрос.
        /// </summary>
        /// <param name="query">SQL запрос</param>
        /// <param name="parameters">Список параметров</param>
        /// <returns>Количество затронутых строк</returns>
        public int Execute(string query, Parameter[] parameters = null)
        {
            // Выполнить команду cmd.ExecuteNonQuery() в качестве функции handler
            Handler handler = (SqlCommand cmd) => cmd.ExecuteNonQuery();
            ExecuteCommand(handler, query, out object result, parameters);
            return result != null ? (int)result : 0;
        }

        /// <summary>
        /// Получить скалярное значение.
        /// </summary>
        /// <param name="query">SQL запрос</param>
        /// <param name="parameters">Список параметров</param>
        /// <returns>Первый столбец первой строки</returns>
        public object GetScalar(string query, Parameter[] parameters = null)
        {
            // Выполнить команду cmd.ExecuteScalar() в качестве функции handler
            Handler handler = (SqlCommand cmd) => cmd.ExecuteScalar();
            ExecuteCommand(handler, query, out object result, parameters);
            return result;
        }

        /// <summary>
        /// Получить таблицу значений.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns>Массив из строк, в каждой из которых - значения столбцов</returns>
        public object[][] GetRowsData(string query, Parameter[] parameters = null)
        {
            // Выполнить пакет команд в качестве функции handler
            Handler handler = (SqlCommand cmd) =>
            {
                SqlDataReader dr = cmd.ExecuteReader();
                List<object[]> list = new List<object[]>();
                // Перезаписать значения из `SqlDataReader dr` в list
                while (dr.Read())
                {
                    object[] values = new object[dr.FieldCount];
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        values[i] = dr.GetValue(i);
                    }
                    list.Add(values);
                }
                // вернуть массив объектов вместо листа.
                return list.ToArray();
            };
            ExecuteCommand(handler, query, out object result, parameters);
            return (object[][])result;
        }
    }
}