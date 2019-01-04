using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Wox.Plugin.Kitty
{
    public class Main : IPlugin
    {
        private string kittyFolderPath;
        private string kittyPath;

        public void Init(PluginInitContext context)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string confFileName = Path.Combine(assemblyFolder, "path.conf");
            kittyFolderPath = File.ReadAllText(confFileName).Replace(System.Environment.NewLine, "") + "\\";
            kittyPath = FindKittyExe();
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            if (kittyFolderPath == "" || kittyPath == "")
                return results;
            foreach (string session in KittySessions())
            {
                if (session == "Default%20Settings")
                    continue;
                if (query.Search != "" && !session.ToUpper().StartsWith(query.Search.ToUpper()))
                    continue;
                results.Add(new Result()
                {
                    Title = session,
                    IcoPath = "kitty.png",  //relative path to your plugin directory
                    Action = e =>
                    {
                        Process.Start(kittyPath, "-load " + session);
                        // after user select the item
                        return true;
                    }
                });
            }
            return results;
        }

        private string[] KittySessions()
        {
            return (new List<string>(Directory.GetFiles(kittyFolderPath + "Sessions"))).Select(x => x.Split('\\').Last()).ToArray();
        }

        private string FindKittyExe()
        {
            string exe = kittyFolderPath + "kitty.exe";
            if (File.Exists(exe))
                return exe;
            exe = kittyFolderPath + "kitty_portable.exe";
            if (File.Exists(exe))
                return exe;
            return "";
        }
    }
}
