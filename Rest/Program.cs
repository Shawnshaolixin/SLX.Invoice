using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;//这个相当于 python 里面 json模块
namespace Rest
{
    class Program
    {
        static void Main(string[] args)
        {
            var apiResult = GetUserInfo(2);//这里返回的是一个对象，我们手动给他转成json就行了。


            //比如说，你这个提供给别人了。别人想用 就这样用就行了
            Console.WriteLine(JsonConvert.SerializeObject(apiResult));
            Console.ReadKey();
        }
        static ApiResult GetUserInfo(int userId)
        {
            ApiResult result = new ApiResult();

            if (userId == 1)//我这里假设如果传过来的是1 就能获取到
            {
                UserInfo userInfo = new UserInfo();
                userInfo.Age = 12;
                userInfo.UserName = "邵立新";
                result.Code = 200;
                result.Message = "success";
                result.Data = JsonConvert.SerializeObject(userInfo);//json  就是字符串嘛，我这里就是把一个对象搞成了一个json 字符串
            }
            else
            {
                result.Code = 500;
                result.Message = "error:没有找到响应的用户";
                result.Data = "";//这里可以给值可以不给 无所谓。
            }
            return result;
        }
    }
    public class UserInfo
    {
        public string UserName { get; set; }
        public int Age { get; set; }
    }
    /// <summary>
    /// 接口统一返回这种格式。
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 编码：这个我们自定义比如：
        /// 200：成功
        /// 500：错误
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 如果是错误的时候，提示内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回的数据都放到这个里
        /// </summary>

        public string Data { get; set; }
    }
}
