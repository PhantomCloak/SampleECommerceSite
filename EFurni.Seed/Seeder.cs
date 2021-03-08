using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bogus;

// ReSharper disable All

namespace EFurni.Seed
{
    public delegate Faker<T> SeedFunc<T>() where T : class;

    public class Seeder
    {
        // public static MySqlConnection? DbConnection { get; set; }

        public static void WipeTable(Type modelType)
        {
            string tableName = GetModelTableName<string>(modelType.Name);
            // MySqlCommand cmd = new MySqlCommand($"DELETE FROM {tableName}", DbConnection);
            // cmd.ExecuteNonQuery();
        }

        private static string GetModelTableName<T>(T model)
        {
            string modelName = model.GetType().Name.ToString();

            return GetModelTableName<string>(modelName);
        }

        private static string GetModelTableName<T>(string modelName)
        {
            string[] word_fragments = Regex.Split(modelName, @"(?<!^)(?=[A-Z])");

            if (word_fragments.Length == 1)
            {
                return modelName.ToLower();
            }
            else
            {
                string tableName = string.Empty;
                foreach (var word in word_fragments)
                {
                    tableName += "_" + word.ToLower();
                }

                tableName = tableName.Substring(1);

                return tableName;
            }
        }

        private static object[] GetModelFieldValues<T>(T model)
        {
            var modelProperties = model.GetType()
                .GetProperties();

            var propertyValues = new List<object>();

            foreach (var property in modelProperties)
            {
                bool isVirtual = property.GetAccessors().FirstOrDefault(x => x.IsVirtual) != null;

                if (isVirtual)
                    continue;

                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    DateTime? dateObj = null;
                    try
                    {
                        dateObj = (DateTime)property.GetValue(model);

                    }
                    catch (Exception e)
                    {
                    }
                    
                    
                    if (dateObj == null)
                    {
                        propertyValues.Add(SqlDateTime.Null);
                    }
                    else
                    {
                        propertyValues.Add(((DateTime)dateObj).ToString("yyyy-MM-dd"));
                    }
                }
                else if (property.PropertyType == typeof(bool))
                {
                    var dateObj = (bool)property.GetValue(model) == true ? 1 : 0;
                    propertyValues.Add(dateObj);
                }
                else
                {
                    propertyValues.Add(property.GetValue(model));
                }
            }

            return propertyValues.ToArray();
        }

        public static void Seed<T>(SeedFunc<T> seedFunc, int seedCount = 1) where T : class
        {
            var models = seedFunc().Generate(seedCount);


            foreach (var model in models)
            {

                string tableName = GetModelTableName(model);
                object[] tableValues = GetModelFieldValues(model);

                string queryString = string.Empty;
                var paramStr = new StringBuilder();

                //add parameter symbols
                for (int i = 0; i < tableValues.Length; i++)
                {
                    string statement = $"@{i},";

                    if (i == tableValues.Length - 1)
                        statement = statement.Substring(0, statement.Length - 1);

                    paramStr.Append(statement);
                }


                // var cmd = new MySqlCommand($"INSERT INTO {tableName} VALUES({paramStr.ToString()});", DbConnection);
                //
                // for (int i = 0; i < tableValues.Length; i++)
                // {
                //     cmd.Parameters.AddWithValue(i.ToString(), tableValues[i].ToString());
                // }
                //
                // cmd.ExecuteNonQuery();
            }
        }
    }
}