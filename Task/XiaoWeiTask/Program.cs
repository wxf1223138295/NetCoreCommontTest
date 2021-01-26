using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Synyi.Platform.Omiga.Client.ApiService;
using Synyi.Platform.Omiga.Client.Contract;
using XiaoWeiTask.ViewModel;
using Synyi.Platform.Omiga.Client.Contract.Model;
using Synyi.Platform.Omiga.Client.Model;

namespace XiaoWeiTask
{
    public class Response
    {
        public string code { get; set; }
        public string evenId { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }

    public class Packageid
    {
        public Tasks tasks { get; set; }
    }

    public class Tasks
    {
        public string taskId { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            var rooturl = "http://var-center.sy/";

            var UpLoadExcelUrl = "var-center/api/Variable/upload-variable";
            var ToTaskUrl = "var-center/api/Handel/publish-package";

            var projectcode = "var-center/api/Project/get-project";




            var path = Console.ReadLine();

            Console.WriteLine($"路径为：{path}");

            var allfiles = Directory.GetFiles(path, "*.xlsx");



            await mainprocess(allfiles,rooturl,UpLoadExcelUrl,ToTaskUrl);

            //await OnlyGetTask(allfiles, rooturl, projectcode);


            Console.WriteLine("OK");
        }

        public static async Task OnlyGetTask(string[] allfiles, string rooturl, string projectcode)
        {
            foreach (var allfile in allfiles)
            {
                GetProjectcodebyExcel(allfile, rooturl, projectcode).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        public static async Task mainprocess(string[] allfiles, string rooturl, string UpLoadExcelUrl, string ToTaskUrl)
        {
            foreach (var allfile in allfiles)
            {
                ConsumerExecl(allfile, rooturl, UpLoadExcelUrl).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            //var tasks = allfiles.Select(async p =>
            //  {
            //      await ConsumerExecl(p, rooturl, UpLoadExcelUrl);
            //  });

            //await Task.WhenAll(tasks);

            var command = Console.ReadLine();
            Console.WriteLine($"comomd：{command}");

            foreach (var allfile in allfiles)
            {
                PackageTask(allfile, rooturl, ToTaskUrl).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            //var task2s = allfiles.Select(async p =>
            //{
            //    await PackageTask(p, rooturl, ToTaskUrl);
            //});

            //await Task.WhenAll(task2s);

        }

        public static async Task GetProjectcodebyExcel(string excelname, string rooturl, string url)
        {
            var _ = excelname.Split("_");

            var projectid = _[1];

            var urllast = rooturl + url + $"?id={projectid}";

            var res = await HttpGet(urllast);

            if (res.code == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<ResponseResult<ProjectDto>>(res.result);
                var projectcode = result.Data.Code;
                Console.WriteLine($"项目code：{projectcode}");
                var taskid = "api/VariableCenterThirdPart/get-task-list-by-project-code";

                var resu = await HttpGet(rooturl + taskid + $"?ProjectCode={projectcode}");
                if (resu.code == HttpStatusCode.OK)
                {
                    var last = JsonConvert.DeserializeObject<ResponseResult<List<TaskBasicInfoOutput>>>(resu.result);
                    var firsttask=last.Data.FirstOrDefault();
                    Console.WriteLine($"{excelname}执行完毕，执行结果：Taskid={firsttask.TaskId}");
                    var gettaskurl = "http://platform-omiga-omiga-3-0-synyi-platform-omiga-task.sy/api/Thirdparty/get-task";
                    Console.WriteLine($"生成Task json");
                    var respo = await HttpGet(gettaskurl, firsttask.TaskId, 0);
                    if (respo.code == HttpStatusCode.OK)
                    {
                        var rer = JsonConvert.DeserializeObject<ResponseResult<CreateTaskInputClient>>(respo.result);

                        var path = Path.Combine("D:\\SynyiDoc\\单病种质控\\20210120", "TaskJson", result.Data.Remark + "_visit.json");
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false))
                        {
                            file.Write(JsonConvert.SerializeObject(rer.Data));
                        }
                    }
                    else
                    {
                        Console.WriteLine(respo);
                    }
                }
                else
                {
                    Console.WriteLine($"{excelname}执行完毕，执行结果：{resu}");
                }
            }
            else
            {
                Console.WriteLine($"{excelname}执行完毕，执行结果：{res}");
            }

        }
        public static async Task ConsumerExecl(string excelname, string rooturl, string url)
        {
            var _ = excelname.Split("_");

            var projectid = _[1];
            var urllast = url + $"?projectId={projectid}";
            var filebinary = File.ReadAllBytes(excelname);

            var res = await HttpPost(rooturl, urllast, filebinary, excelname);

            if (res.code == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<Response>(res.result);
                Console.WriteLine($"{excelname}执行完毕，执行结果：{result.data}");
            }
            else
            {
                Console.WriteLine($"{excelname}执行完毕，执行结果：{res}");
            }

        }

        public static async Task PackageTask(string excelname, string rooturl, string url)
        {
            var _ = excelname.Split("_");

            var projectid = _[1];
            var urllast = url + $"?projectId={projectid}";
            var filebinary = File.ReadAllBytes(excelname);
            var res = await HttpPost(rooturl, urllast, filebinary, excelname);
            //var result = JsonConvert.DeserializeObject<PublishMode>(res.result);
            var taskid = res.result.Substring(res.result.IndexOf("\"tasksId\":") + 10, 5);
            if (res.code == HttpStatusCode.OK)
            {

                if (!string.IsNullOrEmpty(taskid))
                {
                    var id = Convert.ToInt32(taskid);
                    Console.WriteLine($"{excelname}执行完毕，执行结果：TaskId={id}");
                    var gettaskurl = "http://platform-omiga-omiga-3-0-synyi-platform-omiga-task.sy/api/Thirdparty/get-task";
                    Console.WriteLine($"生成Task json");
                    var respo = await HttpGet(gettaskurl, id, 0);
                    if (respo.code == HttpStatusCode.OK)
                    {
                        var ins = 7;
                        var txt = respo.result.Substring(respo.result.IndexOf("\"data\":") + ins);
                        txt = txt.Substring(0, txt.Length - 1);
                        var path = Path.Combine("D:\\SynyiDoc\\单病种质控\\20210120", "TaskJson", _[2] + _[3] + "_visit.json");
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false))
                        {
                            file.Write(txt);
                        }
                    }
                    else
                    {
                        Console.WriteLine(respo);
                    }
                }

            }
            else
            {
                Console.WriteLine($"TaskId={taskid}的任务生成失败");
                Console.WriteLine($"{excelname}执行完毕，执行结果：{res.result}");
            }

        }
        public static async Task<(HttpStatusCode code, string result)> HttpPost(string rooturl, string url, byte[] data, string filename)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(rooturl);
                client.Timeout = new TimeSpan(0, 8, 0);
                var content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(data), "formFile", filename);

                var res = await client.PostAsync(url, content);
                string result = string.Empty;

                result = await res.Content.ReadAsStringAsync();
                return (res.StatusCode, result);


            }
        }

       
        public static async Task<(HttpStatusCode code, string result)> HttpGet(string rooturl, int? taskid, int version)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(rooturl + $"?taskId={taskid}&version={version}");
                client.Timeout = new TimeSpan(0, 8, 0);

                var res = await client.GetAsync(string.Empty);
                string result = string.Empty;

                result = await res.Content.ReadAsStringAsync();
                return (res.StatusCode, result);

            }
        }

        public static async Task<(HttpStatusCode code, string result)> HttpGet(string rooturl)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(rooturl);
                client.Timeout = new TimeSpan(0, 8, 0);

                var res = await client.GetAsync(string.Empty);
                string result = string.Empty;

                result = await res.Content.ReadAsStringAsync();
                return (res.StatusCode, result);

            }
        }
    }
}
