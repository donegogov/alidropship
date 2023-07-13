using Iop.Api;
using System;
using Iop.Api.Util;

namespace Iop.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IopClient client = new IopClient("https://api-pre.aliexpress.com", "33505222", "e1fed6b34feb26aabc391d187732af93");
            client.SetSignMethod(Constants.SIGN_METHOD_SHA256);

            try
            {
                IopRequest request = new IopRequest("/auth/token/create");
                request.AddApiParameter("code", "pickup");
                request.SetHttpMethod(Constants.METHOD_POST);
                request.SetSimplify("true");

                IopResponse response = client.Execute(request, "50000001a27l15rndYBjw6PrtFFHPGZfy09k1Cp1bd8597fsduP0RsNy0jhF6FL", GopProtocolEnum.GOP);
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