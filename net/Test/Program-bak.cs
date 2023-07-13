using Iop.Api;
using System;
using Iop.Api.Util;

namespace Iop.Test
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    IopClient client = new IopClient("https://api.taobao.tw/rest", "${appKey}", "${appSecret}");
        //    client.SetSignMethod(Constants.SIGN_METHOD_SHA256);

        //    try
        //    {
        //        IopRequest request = new IopRequest("/xiaoxuan/mockfileupload");
        //        request.AddApiParameter("file_name", "IopTest.exe");
        //        request.AddFileParameter("file_bytes", new FileItem("./IopTest.exe"));

        //        IopResponse response = client.Execute(request);
        //        Console.WriteLine(response.IsError());
        //        Console.WriteLine(response.Body);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.Write(e.StackTrace);
        //    }

        //    Console.Read();
        //}

        public static void Main(string[] args)
        {
            IopClient client = new IopClient("https://api.taobao.tw/rest", "100663", "qejmLtk36Fkww0z7s8KkpnJvkzAy4wsa");
            client.SetSignMethod(Constants.SIGN_METHOD_SHA256);

            try
            {
                IopRequest request = new IopRequest("/orders/get");
                request.SetHttpMethod("GET");
                request.AddApiParameter("offset", "0");
                request.AddApiParameter("limit", "100");

                IopResponse response = client.Execute(request, "50000801b05rOupee1eb7da61hhR5KrUlhjuK4iRTnj49PxcKO0SnPWvCBvPl");
                Console.WriteLine(response.IsError());
                Console.WriteLine(response.Body);
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }

            Console.Read();
        }

    }
}