using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AliPayServer
{
    class AliPay
    {
        static AliPay instance;
        public static AliPay Instance {
            get {
                if (instance==null)
                {
                    instance = new AliPay();
                }
                return instance;
            }
        }

        #region APP_Private_Key and AliPay_Public_Key
        /// <summary>
        /// 应用密钥
        /// </summary>
        string APP_Private_Key = @"MIIEpAIBAAKCAQEAqx0Ebk9Io0ROSf6uYWSn2fJttz7PQP+HfrkDxaZX+MqlIRsnkWlBwmujw/U6XLJ1KtCs2o7f9bBwNMDJQZC+tktTDsE0e0NbcjqdL2xWld8hyoYuOIUZ8fSM5ama04MLf7B5F56Vu/95k7nYSwtGXWjrASyQu247+ga4KrEe1U5JsY9oS1QffqNLq2riBg6Cs0XdiO3g6geU3b//2/X/pZz7qzPKvflsEBdRbs1PqPare8l6uoRB+xKbgG1dUApmzwi2FDSe+PwuuMtqdDmGgyfofNcU691ButSbPHCPL8QVJZi+CimI4M7wBGQ2wE5Jp1NcO7KD35laDxDnQSvBHwIDAQABAoIBAEGRj/YZKXNupDVUg0vMv0kTzZkPV2nHwQr9KIXfdQxf0qD5/9KHq+wtRQa8/I0y0RUD+4iQgR9racO9MCGQrpO6D2yy+kJVkEAYV80pTZCGfTNW8XU1A7kkha0nra1pJMncPLqhSS1N+y9xYoF3I5J9trevdRJtbkwjsQSi9Ha1tZxFfmgBvsR+W2rD9ORpfqtrZVL/+DJFAiD8ZozRRXYh0+oAU1WXYNYy8LQDDN+nM+2VmDBdMuRUGiVyaSqnmcN4S8QCwYVWVmVO39dNSVWY11q7LJcKCYTZmLgQa01oY3oUbZ7n154k8oDcbPVemdqqGVyH7BgZX2LeRqUiNQECgYEA2gO8eRYV9WAAJcWqP0f+k/4i3m71FGPobsoIuNQr0iz5D2rQ7UlhgZ2Y72LqN2/Srx2g+KTnPigBnck3MHxh6wiBvzwTbF3pzD6SLHoNGRjNmPE+HNXlOaSxXWM6Qvf/KN2J6vua6P0DwkZehOi58TTR1ZRQi/51xWWYp7F2uYECgYEAyO1Phx93kZ273oSz2Igxj7YVhdH/8V9cvx5SiSAnsZ8Eeq7IsFFOE6z/ttxA3qQIhHgTeF66+4LUftjBD6F3M6m39C42SJKcv+C15u31vZ9WyLwqi5RbYDlq62ITBeRIveT0G/Xc24t3vrjinobWgHSq5IX0NDT1d/lNQVnvip8CgYEA10zTRz1RaCZrXuILFD10IxDZvJMVQxK7SxYIcQdPU1uIhvo04/EQ8yEBFH+50A+Fn9yByKuJlm+J0RoSf7aGOMcI4yNgByfjqQmt73CFGODOwZiUf4OYwUlsw04oDlS9Ts0h08awICEmIii+VUFDx/ois2qp9ObRxaRkkk8GcYECgYBoQFlHLtiHQWQ87HW0H9Y3Tq6UJIW741LoBv+kDn8J9gwI669NbKIqK1TyuA0gd9PDh9nyVpSF8zf2KNjjF1AWCjVcCK45sXiLRjibfVRH8ujAdoFMsslGgAQt5VEheXUUsjrGVyck8pRK7PsIbcXWGLKip64xeFj0yvF+uv9C2QKBgQCfjoNp+wygYJlQTo6YzhsnIvdzsqv3Nyq+kldBw4QXFWoK/P69Sh0aUZ+pwSdQ3HwzynSd3EotLlfZNRgNdjK1w9td1GosMoQYJu+LKoRSb0rmCdy1cKuyHCEIkrtOiVvumGE+nUSWrHRPopdRTQo7k3nvzWwQRKB8Pq9iK0aHZA==";

        /// <summary>
        /// 支付宝公钥
        /// </summary>
        string AliPay_Public_Key = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA37vnkTXPnA83r39o1TowQh8WChOgkbrR3ylQqWGm8eeFGn+wi1bhmNyO4MobIwgejgthVWTs/hXKr5YXUgTH3kEEZQpc+CP2YFroMyYKu5tl8my8Ki5LuPtnOnvEHNtMfXmm1TmqJzv5a+igjN3XY4x+CxKrK9hzzRplmpdoeJOtbQ1twYKk0wKRDitVY4ikbvKKy6mDaBh69pYfCpjAL9fDbewU7QjWKBg1YIFRlwOveUNSBdbflpiAr4DIS75xYrg8X/4i6u7p7M5Ma/tAV/Y0No3P++3roaiLaRzBkvmjQcL4akOlQ6klOCiKkcxYC/hEwNs1C+Gh8KuEKTFXXwIDAQAB";
        #endregion

        string appid = "2017051607249364";

        Dictionary<string, AliPayOrderInfo> orderDic = new Dictionary<string, AliPayOrderInfo>();
        //向支付宝服务器获取支付所需的参数
        //然后发送回给客户端 
        public void Pay(Agent agent) {
            //价格(元) 名称 标题 订单号...
            AliPayOrderInfo model = new AliPayOrderInfo()
            {
                Body="活动时装(测试)",
                Subject = "超酷的英雄外表,仅限活动期间可以进行购买",//商品的标题/交易标题/订单标题/订单关键字等。
                TotalAmount = "0.10",//价格 单位为元
                ProductCode = "QUICK_MSECURITY_PAY",//官方提供的固定值
                OutTradeNo = ConvertDateTimeInt(DateTime.Now).ToString(),//唯一订单号
                TimeoutExpress = "30m",//付款超时时间
                clientAgent = agent,
            };
            //缓存 为了验签使用
            orderDic[model.OutTradeNo] = model;

            string str= GetAliPayParameter(model);
            Console.WriteLine($"调起支付接口的参数:{str}");
            agent.Send(str);
        }

        //阿里 服务端SDK提供的对象 用于请求给客户端的支付参数
        IAopClient client;
        AlipayTradeAppPayRequest request;
        //http post url :订单状态发生变化(支付结果)通知的地址
        string aliPayResultListenerUrl = @"http://193.112.44.199:9090/";
        /// <summary>
        /// 请求支付参数:https://docs.open.alipay.com/204/105465/
        /// </summary>
        /// <returns>客户端向安卓层（支付宝客户端SDK）请求的字符串</returns>
        public string GetAliPayParameter(AlipayTradeAppPayModel alipaymode)
        {
            if (client == null)
            {
                client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", appid, APP_Private_Key,
                    "JSON", "1.0", "RSA2", AliPay_Public_Key, "UTF-8", false);
            }

            //参数对照: https://docs.open.alipay.com/204/105465/

            //用于请求的对象
            request = new AlipayTradeAppPayRequest();

            request.SetBizModel(alipaymode);//请求的数据模型
            request.SetNotifyUrl(aliPayResultListenerUrl);//设置支付结果通知的地址

            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);

            //(不用理这句注释)HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
            //通过Body获取到返回的参数
            return response.Body;
        }

        /// <summary> 获取时间戳 </summary>
        public int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime =
                TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            Console.WriteLine("计算后的时间戳是：" + (int)(time - startTime).TotalSeconds);
            return (int)(time - startTime).TotalSeconds;
        }

        //------------------以上是支付所用到的代码------------------------//
        //------------------以下是监听支付结果用到的代码------------------------//
        string httpListenerUrl = @"http://172.16.0.5:9090/";
        HttpListener httpListener;

        public AliPay() {
            ListenerAliPayResult();
        }

        /// <summary>监听阿里支付结果 https://docs.open.alipay.com/204/105301/ </summary>
        public void ListenerAliPayResult()
        {
            //http监听器
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(httpListenerUrl);
            httpListener.Start();
            //异步的方式处理请求
            httpListener.BeginGetContext(new AsyncCallback(CheckAliPayResult), null);
        }

        /// <summary>解析结果以及校验</summary>
        public void CheckAliPayResult(IAsyncResult ar)
        {
            try
            {
                HttpListenerContext context = httpListener.EndGetContext(ar);
                HttpListenerRequest request = context.Request;//请求对象
                if (context != null)
                {
                    StreamReader body = new StreamReader(request.InputStream, Encoding.UTF8);//读取流，用来获取支付宝请求的数据
                    string pay_notice = HttpUtility.UrlDecode(body.ReadToEnd(), Encoding.UTF8);//HttpUtility.UrlDecode：解码 url编码，将字符串格式为%的形式，解码就是将%转化为字符串信息

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("支付结果来了：" + pay_notice);


                    Dictionary<string, string> aliPayResultDic = StringToDictionary(pay_notice);
                    // 验签 API 
                    bool result = AlipaySignature.RSACheckV1(aliPayResultDic, AliPay_Public_Key,
                        "UTF-8", "RSA2", false);

                    if (result)
                    {
                        //缓存的订单
                        AliPayOrderInfo souceDic = orderDic[aliPayResultDic["out_trade_no"]];
                        //订单的判定
                        if (souceDic.OutTradeNo.Equals(aliPayResultDic["out_trade_no"]))
                        {
                            Console.WriteLine("存在相同的订单");
                          
                            if (souceDic.TotalAmount.Equals(aliPayResultDic["total_amount"]))
                            {
                                Console.WriteLine("金额也是一致的:" +
                                    aliPayResultDic["total_amount"] + "元");
                                souceDic.clientAgent.Send("支付成功");
                            }
                            else
                            {
                                Console.WriteLine("金额不一致");
                                //souceDic.clientAgent.Send("支付失败");
                            }
                        }
                        else
                        {
                            Console.WriteLine("未存在的订单记录:" +
                                aliPayResultDic["out_trade_no"]);
                        }

                        //验签（支付）成功 告诉客户端 加钻石，给数据库加记录，等。。。  
                        //另外官方建议：最好将返回数据中的几个重要信息，与支付时候对应的参数去对比。 
                        //返回的所有参数都在这里：StringToDictionary(pay_notice)          
                        //请求的参数 在：Get_equest_Str()    

                        //成功了就需要给支付宝回消息“success”
                        //https://docs.open.alipay.com/204/105301/
                        HttpListenerResponse response = context.Response;


                        string responseString = "success";
                        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;
                        Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);//响应支付宝服务器本次的通知
                        output.Close();
                        response.Close();

                        //给客户端发送道具

                        //  OutTradeNo(订单号),道具ID,道具数量
                        //string toClientMsg = "sendProps" + "," + souceDic.OutTradeNo + "," + souceDic.objID + "," + souceDic.objCount;
                        //byte[] sendByte = Encoding.UTF8.GetBytes(toClientMsg);
                        //souceDic.clientAgent.SendBytes(sendByte);
                    }
                    else
                    {
                        Console.WriteLine("验签失败");
                    }

                    Console.WriteLine("验签结果：" + (result == true ? "支付成功" : "支付失败"));

                    if (aliPayResultDic.ContainsKey("trade_status"))
                    {
                        switch (aliPayResultDic["trade_status"])
                        {
                            case "WAIT_BUYER_PAY":
                                Console.WriteLine("交易状态:" + "交易创建，等待买家付款");
                                break;
                            case "TRADE_CLOSED":
                                Console.WriteLine("交易状态:" + "未付款交易超时关闭，或支付完成后全额退款");
                                break;
                            case "TRADE_SUCCESS":
                                Console.WriteLine("交易状态:" + "交易支付成功");
                                break;
                            case "TRADE_FINISHED":
                                Console.WriteLine("交易结束，不可退款");
                                break;

                            default:
                                break;
                        }

                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            if (httpListener.IsListening)
            {
                try
                {
                    httpListener.BeginGetContext(new AsyncCallback(CheckAliPayResult), null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        /// <summary>
        /// 支付结果返回来的是字符串格式,而验证结果的API需要一个字典结构 so..提供这样的一个API
        /// </summary>
        public Dictionary<string, string> StringToDictionary(string value)
        {
            if (value.Length < 1)
            {
                return null;
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();

            //每个字段之间用"&"拼接
            string[] dicStrs = value.Split('&');
            foreach (string str in dicStrs)
            {
                //    Console.Write("183value--" + str); 
                //每个字段的结构是通过"="拼接键值
                //key=value$key=value$key=value$key=value$key=value$
                string[] strs = str.Split(new char[] { '=' }, 2);
                dic.Add(strs[0], strs[1]);
            }
            return dic;
        }

    }

    class AliPayOrderInfo: AlipayTradeAppPayModel
    {
        public Agent clientAgent;

    }
}
