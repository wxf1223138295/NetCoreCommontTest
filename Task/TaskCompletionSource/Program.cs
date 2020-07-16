﻿using System;
using System.Threading.Tasks;

namespace TaskCompletionSource
{
    class Program
    {
        static void Main(string[] args)
        {
           TaskCompletionSource<string> tcs=new TaskCompletionSource<string>();

           var api=new EventClass();
           api.Done += (arg) => {tcs.SetResult(arg); };

           api.Do();

           Console.WriteLine();
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