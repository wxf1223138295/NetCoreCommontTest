using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace APMConsole
{
    class Program
    {
        public delegate string Mydele();

        public static Mydele dele = new Mydele(GetTreadid);
        static void Main(string[] args)
        {
            Console.WriteLine($"主线程1：{Thread.CurrentThread.ManagedThreadId.ToString()}");
            dele.BeginInvoke(CallbackFun, "899");

            Console.WriteLine($"主线程2：{Thread.CurrentThread.ManagedThreadId.ToString()}");

            Console.ReadKey();
        }

        public static string GetTreadid()
        {
            var id = Thread.CurrentThread.ManagedThreadId.ToString();
            Console.WriteLine($"委托内部线程id：{id}");
            //Thread.Sleep(3000);
            return id;
        }

        public static void CallbackFun(IAsyncResult result)
        {
            var sssd = result.AsyncState as string;
            Console.WriteLine(sssd);
            while (true)
            {
                if (!result.IsCompleted)
                {
                    Console.WriteLine($"等待异步结束,当前线程{Thread.CurrentThread.ManagedThreadId.ToString()}");
                }
                else
                {
                    break;
                }
            }
            var re = dele.EndInvoke(result);

            Console.WriteLine($"异步结束,当前线程{Thread.CurrentThread.ManagedThreadId.ToString()}");

            // Thread.Sleep(2000);
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            

        }
    }
}
