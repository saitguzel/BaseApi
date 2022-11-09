using System.Data;
using System.Reflection;

namespace Base.Service.Helpers
{
    public class ConvertHelper
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T[] ConvertDataTableToArray<T>(DataTable dt)
        {
            T[] data = new T[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T item = GetItem<T>(dt.Rows[i]);
                data[i] = item;
            }
            return data;
        }

        public static T ConvertDataTableOneRow<T>(DataTable dt)
        {
            // Get first row result retuned from query
            DataRow dataRow = dt.Rows[0];

            T item = GetItem<T>(dataRow);

            return item;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        pro.SetValue(obj, (dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName]), null);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        // When the object element has a DateTime, convert it to Mssql format (yyyy/MM/dd HH:mm:ss)
        public static Dictionary<string, object> ConvertFilter(object obj)
        {
            Dictionary<string, object> ret = new();

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                string propName = prop.Name;
                var val = obj.GetType().GetProperty(propName).GetValue(obj, null);

                if (val != null)
                {
                    if (val is DateTime)
                    {
                        DateTime dt = (DateTime)val;
                        var newDate = dt.ToString("yyyy/MM/dd HH:mm:ss");

                        ret.Add(propName, newDate);
                    }
                    else
                    {
                        if (val.GetType() == typeof(string[]))
                        {
                            var strArray = (string[])val;
                            string newVal = "";
                            for (int i = 0; i < strArray.Length; i++)
                            {
                                newVal += strArray[i] + ",";
                            }
                            newVal = newVal.Length > 0 ? newVal.Remove(newVal.Length - 1, 1) : "";
                            ret.Add(propName, newVal);
                        }
                        else
                        {
                            ret.Add(propName, val);
                        }
                    }
                }
                else
                {
                    ret.Add(propName, DBNull.Value);
                }
            }

            return ret;
        }
    }
}
