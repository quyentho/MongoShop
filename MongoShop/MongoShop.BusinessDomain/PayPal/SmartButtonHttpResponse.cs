using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;
using System.Net;

namespace MongoShop.BusinessDomain.PayPal
{
    public class SmartButtonHttpResponse
    {
        readonly PayPalCheckoutSdk.Orders.Order _result;
        public SmartButtonHttpResponse(PayPalHttp.HttpResponse httpResponse)
        {
            Headers = httpResponse.Headers;
        }

        public HttpHeaders Headers { get; }
        public HttpStatusCode StatusCode { get; }

        public PayPalCheckoutSdk.Orders.Order Result()
        {
            return _result;
        }

        public string orderID { get; set; }
    }
}
