using CommandSystem;
using LurkBoisModded.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SubclassList : ICommand
    {
        public string Command => "subclasslist";

        public string[] Aliases => new string[] { "sublist", "listsub" };

        public string Description => "List of currently loaded subclasses";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Join(", ", SubclassManager.GetLoadedSubclasses());
            return true;
        }
    }
}
