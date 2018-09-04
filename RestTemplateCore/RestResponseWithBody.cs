using System;
using System.Collections.Generic;
using System.Text;

namespace RestTemplateCore
{
   public class RestResponseWithBody<T>:RestResponse
    {
        public T Body { get; set; }
    }
}
