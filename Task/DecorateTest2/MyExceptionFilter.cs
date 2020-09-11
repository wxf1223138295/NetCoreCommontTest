using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DecorateTest2
{
    public class MyExceptionFilter:IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {

         
            context.Result=new ObjectResult(context.Exception.Message);
            //防止程序卡死    
            context.Exception = null;
            return Task.CompletedTask;
        }
    }
}
