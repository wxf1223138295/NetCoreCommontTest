using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using DecorateTest2.MyScheduler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DecorateTest2.Controllers
{

    public class test
    {
        public ThreadLocal<int> testint = new ThreadLocal<int>();
    }
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        public static ThreadLocal<int> testint = new ThreadLocal<int>();

        private static object lockobj = new object();

        public static int LengthOfLongestSubstring(string s)
        {
            if (s.Length <= 0)
            {
                return 0;
            }
            List<char> listsstr = new List<char>();
            int maxvalu = 0;
            //左指针
            int point = -1;
            //i 有指针
            for (int i = 0; i < s.Length; i++)
            {
                if (i != 0)
                {
                    //窗口 右移动  删除 窗口外 
                    listsstr.Remove(s.ElementAt(i - 1));
                }
                while (point + 1 < s.Length && !listsstr.Contains(s.ElementAt(point)))
                {
                    listsstr.Add(s.ElementAt(point));
                    ++point;
                }
                maxvalu = Math.Max(maxvalu, point - i + 1);
            }

            return maxvalu;
        }

        [HttpGet("get3")]
        public async Task<string> Get7()
        {
            var count= ThreadPool.ThreadCount;

           Console.WriteLine($"线程池线程数：{count}");

           ThreadLocal<int> tt=new ThreadLocal<int>();
           tt.Value = 2;

               int maxworkthread = 0;
           int maxcomthread = 0;
           int minwordthread = 0;
           int mincomthread = 0;
           ThreadPool.GetMaxThreads(out maxworkthread,out maxcomthread);
           Console.WriteLine($"MaxWork:{maxworkthread},MaxIO:{maxcomthread}");

           ThreadPool.GetMinThreads(out minwordthread,out mincomthread);
           Console.WriteLine($"MinWork:{minwordthread},MinIO:{mincomthread}");

            int avaworkthread = 0;
           int avaconthread = 0;
           ThreadPool.GetAvailableThreads(out avaworkthread,out avaconthread);
           Console.WriteLine($"AvaWork:{minwordthread},AvaIO:{mincomthread}");

           string s = "";
           

           

            return ";";
        }

        [HttpGet("get4")]
        public async Task<string> Get6()
        {


            int a = 0;
            Stopwatch watch = new Stopwatch();

            watch.Start();


            Parallel.For(1, 101, (i) =>
            {
                int coms = 0;
                try
                {
                    while (Interlocked.CompareExchange(ref coms, 1, 0) == 1)
                    {
                        Console.WriteLine($"线程Id：{Thread.CurrentThread.ManagedThreadId}，开始：a= {a}");
                        a = a + i;

                        Interlocked.Exchange(ref coms, 0);
                    }

                   


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                Console.WriteLine($"线程Id：{Thread.CurrentThread.ManagedThreadId}，结束：a= {a}");
            });
            Console.WriteLine($"sum:{a}");

            watch.Stop();
            TimeSpan ts = watch.Elapsed;

            Console.WriteLine($"耗时：{ts.Milliseconds}");
            return "2";
        }

        [HttpGet("get6")]
        public async Task<string> Get5()
        {
            SpinWait spinner = new SpinWait();


            int a = 0;
            Stopwatch watch = new Stopwatch();

            watch.Start();


            Parallel.For(1, 101, (i) =>
            {
                try
                {
                    lock (lockobj)
                    {
                        Console.WriteLine($"线程Id：{Thread.CurrentThread.ManagedThreadId}，开始：a= {a}");
                        a = a + i;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                Console.WriteLine($"线程Id：{Thread.CurrentThread.ManagedThreadId}，结束：a= {a}");
            });
            Console.WriteLine($"sum:{a}");

            watch.Stop();
            TimeSpan ts = watch.Elapsed;

            Console.WriteLine($"耗时：{ts.Milliseconds}");
            return "2";
        }
        [HttpGet("get5")]
        public async Task<string> Get4()
        {
            SpinWait spinner = new SpinWait();


            int a = 0;
            Stopwatch watch = new Stopwatch();

            watch.Start();

            var sl = new SpinLock(true);
            Parallel.For(1, 101, (i) =>
            {
                var lockTaken = false;
                try
                {
                    sl.Enter(ref lockTaken);
                    Console.WriteLine($"线程Id：{Thread.CurrentThread.ManagedThreadId}，开始：a= {a}");
                    a = a + i;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    if (lockTaken) sl.Exit();
                }
                Console.WriteLine($"线程Id：{Thread.CurrentThread.ManagedThreadId}，结束：a= {a}");
            });
            Console.WriteLine($"sum:{a}");

            watch.Stop();
            TimeSpan ts = watch.Elapsed;

            Console.WriteLine($"耗时：{ts.Milliseconds}");
            return "2";
        }



        [HttpGet("get4")]
        public async Task<string> Get3()
        {
            int a = 0;
            Stopwatch watch = new Stopwatch();

            watch.Start();

            Parallel.For(1, 101, (i) =>
            {
                var lockTaken = false;
                try
                {
                    Console.WriteLine($"线程Id：{Thread.CurrentThread.ManagedThreadId}，开始：a= {a}");

                    while (!lockTaken)
                    {
                        Monitor.TryEnter(lockobj, ref lockTaken);
                    }

                    if (lockTaken)
                    {


                        Thread.MemoryBarrier();

                        a = a + i;


                        Thread.MemoryBarrier();
                    }



                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(lockobj);
                    }

                }

                Console.WriteLine($"线程Id：{Thread.CurrentThread.ManagedThreadId}，结束：a= {a}");
            });

            Console.WriteLine($"sum:{a}");

            watch.Stop();
            TimeSpan ts = watch.Elapsed;

            Console.WriteLine($"耗时：{ts.Milliseconds}");


            return "2";
        }



        [HttpGet("get4")]
        public async Task<string> Get4(CancellationToken token)
        {
            //Volatile.Read()

            int a = 0;

            int isModifying = 0;

            testint.Value = 3333;
            Console.WriteLine($"threadlocal:{testint.Value},线程Id：{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("__________________________________________________________");
            List<int> list = new List<int> { 1, 2, 3, 4, 5, 6 };

            var tasks = list.Select(async (p) =>
              {
                  await Task.Delay(1000);

                  if (1 == Interlocked.CompareExchange(ref isModifying, 1, 0))
                  {
                      a = a + 1;
                      Interlocked.Exchange(ref isModifying, 0);
                  }
                  Console.WriteLine($"线程Id：{Thread.CurrentThread.ManagedThreadId}");

                  //testint.Value = p;
                  //var t = TaskScheduler.Current;
                  //var ttt = TaskScheduler.Default;
                  //var tt = SynchronizationContext.Current;
                  //if (tt != null)
                  //{
                  //    Console.WriteLine($"线程Id：{Thread.CurrentThread.ManagedThreadId}的同步上下文不为空");
                  //}

                  //Console.WriteLine($"threadlocal:{testint.Value},当前运行到 ：{p.ToString()},线程Id：{Thread.CurrentThread.ManagedThreadId},currentschedulerid:{t.Id},currentmaxcon{t.MaximumConcurrencyLevel},defaultscheduler:{ttt.Id},defaultconmax:{ttt.MaximumConcurrencyLevel}");


              });


            await Task.WhenAll(tasks);
            Console.WriteLine("__________________________________________________________");

            Console.WriteLine($"a={a}");

            var t = TaskScheduler.Current;
            var ttt = TaskScheduler.Default;
            var tt = SynchronizationContext.Current;

            Console.WriteLine($"threadlocal:{testint.Value},线程Id：{Thread.CurrentThread.ManagedThreadId},currentschedulerid:{t.Id},currentmaxcon{t.MaximumConcurrencyLevel},defaultscheduler:{ttt.Id},defaultconmax:{ttt.MaximumConcurrencyLevel}");

            return 0.ToString();
        }
        [HttpGet("get2")]
        public async Task<string> Get2(CancellationToken token)
        {
            //var t1=Task.Run(async () =>
            // {
            //     await Task.Delay(2000);
            //     Console.WriteLine($"Task1的线程id:{ Thread.CurrentThread.ManagedThreadId}");
            // });

            //var t= Task.Run(async () =>
            // {
            //     await Task.Delay(3000);
            //     Console.WriteLine($"Task2的线程id:{ Thread.CurrentThread.ManagedThreadId}");
            // });

            // var t3=Task.Run(async () =>
            // {
            //     await Task.Delay(4000);
            //     Console.WriteLine($"Task3的线程id:{ Thread.CurrentThread.ManagedThreadId}");
            // });








            try
            {
                Console.WriteLine("await");
                await Task.Run(() =>
                {

                    while (true)
                    {
                        if (token.IsCancellationRequested)
                        {
                            token.ThrowIfCancellationRequested();
                        }
                        Thread.Sleep(5000);
                        Console.WriteLine("222");

                    }



                }, token);

            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return "22";
        }
        [HttpGet("get")]
        public async Task<string> Get()
        {
            var canccs = new CancellationTokenSource();
            var token = canccs.Token;

            List<int> list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };


            LimitedConcurrencyLevelTaskScheduler scheduler = new LimitedConcurrencyLevelTaskScheduler(1);





            var task = Task.Run(() =>
              {
                  token.ThrowIfCancellationRequested();
                  while (true)
                  {
                      if (token.IsCancellationRequested)
                      {
                          // Clean up here, then...
                          token.ThrowIfCancellationRequested();
                      }
                  }
              }, token);

            canccs.Cancel();

            try
            {
                await task;
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return "sdsdsds";
        }
    }



}
