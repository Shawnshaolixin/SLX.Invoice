using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifyCode
{
    class Program
    {
        static void Main(string[] args)
        {
     

            string sqlText = "SELECT top 10 Mobile,Code,CreateTime FROM yixiaotong.dbo.MobileCode ORDER BY CreateTime DESC;";
            Console.WriteLine("输入 2查开发环境，直接回车测试环境");
            var type = Console.ReadLine();
            string connStr = string.Empty;
            if (type == "2")
            {
                connStr = ConfigurationManager.ConnectionStrings["YiXiaoTongDev"].ConnectionString;
            }
            else
            {
                connStr = ConfigurationManager.ConnectionStrings["YiXiaoTongTest"].ConnectionString;

            }
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlText;

                    cmd.CommandType = CommandType.Text;
                    var dr = cmd.ExecuteReader();
                    dt.Load(dr);
                }
                conn.Close();
            }
            if (dt.Rows.Count > 0)
            {
                Console.WriteLine("获取最近10条验证码为:");
                foreach (DataRow item in dt.Rows)
                {
                    Console.WriteLine(item["Mobile"]+"---"+ item["Code"]+"--"+ item["CreateTime"]);
                   
                }
            }
            else
            {
                Console.WriteLine("没有查到验证码");
            }

      
            Console.ReadKey();
        }
    }
}
