using Polly;
using System;
using System.Threading;

namespace PollyTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            #region MyRegion
            //  var policy= Policy<string>.Handle<ArgumentException>().Fallback(() =>
            //    {
            //        Console.WriteLine("执行出错");
            //        return "降级的值";
            //    } );
            //string result=  policy.Execute(() =>
            //  {//在策略中执行业务代码
            //      Console.WriteLine("开始任务");
            //      throw new ArgumentException("hello world");
            //      Console.WriteLine("完成任务");
            //      return "正常的值";

            //  });
            //  Console.WriteLine("返回值："+result); 
            #endregion

            #region 重试
            //Policy policy = Policy
            //    .Handle<Exception>()
            //    .RetryForever();//重试直到成功
            //policy.Execute(()=>
            //{
            //    Console.WriteLine("任务开始执行");
            //    if (DateTime.Now.Second%10!=0)
            //    {
            //        throw new Exception("出错");
            //    }
            //    Console.WriteLine("执行结束");
            //}); 
            #endregion

            #region 熔断 CircuitBreaker
            //Policy policy = Policy.Handle<Exception>()
            //    .CircuitBreaker(6, TimeSpan.FromSeconds(5));//连续出错6次之后熔断5秒(不会再 去尝试执行业务代码）。 
            //while (true)
            //{
            //    Console.WriteLine("开始Execute");
            //    try
            //    {
            //        policy.Execute(() =>
            //        {
            //            Console.WriteLine("开始任务");
            //            throw new Exception("出错");
            //            Console.WriteLine("完成任务");
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Execute出错"+ex);
            //    }
            //    Thread.Sleep(500);
            //} 
            #endregion
            decimal d = 30.52M;
            Console.WriteLine(Convert.ToInt32(d));
            Console.ReadKey();
        }
    }
}
