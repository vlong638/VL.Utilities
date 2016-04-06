using System;
using System.Collections.Generic;

namespace VL.Common.Testing.Objects
{
    public class ConsoloMenuItem
    {
        public string Key { set; get; }
        public string Name { set; get; }
        public Action Action { set; get; }

        public ConsoloMenuItem(string key, string name, Action action)
        {
            this.Key = key;
            this.Name = name;
            this.Action = action;
        }
        public ConsoloMenuItem(string key, string name, Action actionStart, Action actionEnd)
        {
            this.Key = key;
            this.Name = name;
            this.Action = () =>
            {
                actionStart();
                Console.WriteLine("runner started");
                Console.WriteLine("input 'q' to quit");
                while (Console.ReadLine() != "q")
                {
                    Console.WriteLine("input 'q' to quit");
                }
                actionEnd();
                Console.WriteLine("runner stopped");
                Console.ReadLine();
            };
        }
        public ConsoloMenuItem(string key, string name, Func<bool> function)
        {
            this.Key = key;
            this.Name = name;
            this.Action = () =>
            {
                Console.WriteLine(function() ? "succeed" : "failed");
            };
        }
    }
    public static class ConsoloMenuItemEx
    {
        private static void DisplayMenu(this List<ConsoloMenuItem> menuItems)
        {
            Console.WriteLine("-------------------Menu Start--------------------");
            foreach (var menuItem in menuItems)
            {
                Console.WriteLine(string.Format("input {0} for {1}", menuItem.Key, menuItem.Name));
            }
            Console.WriteLine("-------------------Menu End--------------------");
        }
        private static void OperateItem(this List<ConsoloMenuItem> menuItems, string key)
        {
            foreach (var menuItem in menuItems)
            {
                if (menuItem.Key.ToLower() == key.ToLower())
                {
                    menuItem.Action();
                }
            }
        }
        private static bool ContaisKey(this List<ConsoloMenuItem> menuItems, string key)
        {
            foreach (var menuItem in menuItems)
            {
                if (menuItem.Key.ToLower() == key.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        public static void WaitForOperation(this List<ConsoloMenuItem> menuItems)
        {
            menuItems.DisplayMenu();
            string s;
            while (!string.IsNullOrEmpty(s = Console.ReadLine()) && menuItems.ContaisKey(s))
            {
                menuItems.OperateItem(s);
                menuItems.DisplayMenu();
            }
        }
    }
}
