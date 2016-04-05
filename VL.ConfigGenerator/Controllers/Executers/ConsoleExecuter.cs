using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VL.Common.Testing.Controllers.Executers
{
    class ConsoleExecuter
    {
        public void ConsoleExecute(string title, Action function)
        {
            function();
            Console.WriteLine(string.Format("执行时间:{0}-------{1}------执行结束", DateTime.Now, title));
            System.Threading.Thread.Sleep(100);
        }
        public void ConsoleExecute(string title, Func<int> function)
        {
            int result = function();
            Console.WriteLine(string.Format("执行时间:{0}-------{1}------执行结束", DateTime.Now, title));
            Console.WriteLine(string.Format("影响条数:{0}", result));
            System.Threading.Thread.Sleep(100);
        }
        public void ConsoleExecute(string title, Func<bool> function)
        {
            bool result = function();
            Console.WriteLine(string.Format("执行时间:{0}-------{1}------执行结束", DateTime.Now, title));
            Console.WriteLine(string.Format("影响结果:{0}", result ? "成功" : "失败"));
            System.Threading.Thread.Sleep(100);
        }
        public void ConsoleWait(string title, int times)
        {
            Console.WriteLine(string.Format("{0},需等待{1}秒", title, times));
            for (int i = 0; i < times; i++)
            {
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("等待中");
            }
        }
    }
}
