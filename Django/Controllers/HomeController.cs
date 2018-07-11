using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Django.Controllers
{
    public class ZhiHangInfo
    {
        public string id { get; set; }
        public string text { get; set; }
    }
    public class HomeController : Controller
    {
        [HttpGet]
        public JsonResult GetBank(string q, string page)
        {
            List<ZhiHangInfo> list = new List<ZhiHangInfo>();
            list.Add(new ZhiHangInfo {  id="1234", text="中故宫1" });
            list.Add(new ZhiHangInfo { id = "1234",text = "中故宫2" });
            list.Add(new ZhiHangInfo { id = "1234",text = "中故宫3" });
            list.Add(new ZhiHangInfo { id = "1234",text = "中故宫4" });
            list.Add(new ZhiHangInfo { id = "1234",text = "中故宫5" });
            list.Add(new ZhiHangInfo { id = "1234",text = "中故宫6" });
           var items= JsonConvert.SerializeObject(list);
            return Json(new {  items }, JsonRequestBehavior.AllowGet);
         
        }
        public ActionResult Index()
        {
            if (Session["userName"] == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.userName = Session["userName"].ToString();
            return View();
        }



        public ActionResult Login()
        {
            string userName = Request["userName"];
            Session["userName"] = userName;
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public string GetUserInfo()
        {
            ApiResult result = new ApiResult();
            int userId = Convert.ToInt32(Request["userId"]);
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
            return JsonConvert.SerializeObject(result);
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