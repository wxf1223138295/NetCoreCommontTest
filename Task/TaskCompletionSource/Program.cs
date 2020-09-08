using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskCompletionSource
{

    class Test
    {
        public volatile int a;
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            

            //TaskCompletionSource<string> tcs=new TaskCompletionSource<string>();

            //var api=new EventClass();
            //api.Done += (arg) => {tcs.SetResult(arg); };

            //api.Do();

            Console.WriteLine("OK");
           Console.ReadKey();
        }
    }

    public class EventClass
    {
        public Action<string> Done = (args) => { };

        public void Do()
        {
            Done("Done");
        }
    }
}
