using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Wox.Plugin.Kitty
{
    public class Main : IPlugin
    {
        private string kittyFolderPath = "D:\\Apps\\kitty\\";
        private string kittyPath;

        public void Init(PluginInitContext context)
        {
            kittyPath = kittyFolderPath + "kitty.exe";
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            foreach (string session in KittySessions())
            {
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
    }
}
