﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;




//使用方式：  

//// 获得查询结果  

//DataTable dt = DbHelper.ExecuteDataTable(...);

//// 把DataTable转换为IList<UserInfo>  

//IList<UserInfo> users = ModelConvertHelper<UserInfo>.ConvertToModel(dt);
public class D2t
{
    public static DataTable ToDataTable<T>(IEnumerable<T> collection)

    {

        var props = typeof(T).GetProperties();

        var dt = new DataTable();

        dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());

        if (collection.Count() > 0)

        {

            for (int i = 0; i < collection.Count(); i++)

            {

                ArrayList tempList = new ArrayList();

                foreach (PropertyInfo pi in props)

                {

                    object obj = pi.GetValue(collection.ElementAt(i), null);

                    tempList.Add(obj);

                }

                object[] array = tempList.ToArray();

                dt.LoadDataRow(array, true);

            }

        }

        return dt;

    }

    /// <summary>    

    /// 实体转换辅助类    

    /// </summary>    

    public class ModelConvertHelper<T> where T : new()

    {

        public static IList<T> ConvertToModel(DataTable dt)

        {

            // 定义集合    

            IList<T> ts = new List<T>();



            // 获得此模型的类型   

            Type type = typeof(T);

            string tempName = "";



            foreach (DataRow dr in dt.Rows)

            {

                T t = new T();

                // 获得此模型的公共属性      

                PropertyInfo[] propertys = t.GetType().GetProperties();

                foreach (PropertyInfo pi in propertys)

                {

                    tempName = pi.Name;  // 检查DataTable是否包含此列    



                    if (dt.Columns.Contains(tempName))

                    {

                        // 判断此属性是否有Setter      

                        if (!pi.CanWrite) continue;



                        object value = dr[tempName];

                        if (value != DBNull.Value)

                            pi.SetValue(t, value, null);

                    }

                }

                ts.Add(t);

            }

            return ts;

        }

    }

}


