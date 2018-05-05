using System;
using System.IO;
using Windows.UI.Notifications;
using NDesk.Options;

namespace NotifyMsg
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "NotifyMessage";
            string icon = null;
            bool help = false;

            var extra = new OptionSet() {
                { "icon=",    v => icon = v },
                { "name=",    v => name = v },
                { "h|?|help", v => help = v != null }
            }.Parse(args);

            if (help || extra.Count != 2)
            {
                var command = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                Console.Error.WriteLine(string.Format("Usage of {0}: {{flags}} [Title] [Message]", command));
                Console.Error.WriteLine("    -icon [image file]");
                Console.Error.WriteLine("    -name [notification name]");
                Environment.Exit(1);
            }

            var tmpl = ToastTemplateType.ToastImageAndText02;
            var xml = ToastNotificationManager.GetTemplateContent(tmpl);
            var image = xml.GetElementsByTagName("image")[0];
            var src = image.Attributes.GetNamedItem("src");
            src.InnerText = "file:///" +(icon != null ? Path.GetFullPath(icon) : "");
            var texts = xml.GetElementsByTagName("text");
            texts[0].AppendChild(xml.CreateTextNode(extra[0]));
            texts[1].AppendChild(xml.CreateTextNode(extra[1]));
            var notifier = ToastNotificationManager.CreateToastNotifier(name);
            notifier.Show(new ToastNotification(xml));
        }
    }
}
