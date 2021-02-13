using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Renci.SshNet;

namespace GS.ContainerManager.Infrastructure.Utils
{
    public class ShellBridge
    {
        public ShellStream ShellStream { get; set; }
        
        public SshClient SshClient { get; set; }
        
        private Encoding Encoding { get; set; }
        
        public ShellBridge(SshClient sshClient)
        {
            SshClient = sshClient;
            ShellStream = sshClient.CreateShellStream(
                "terminal", 80, 24, 800, 600, 1024);
            //var res = RunKeyCombination('\u0003');
        }

        public string RunKeyCombination(char asc2Char)
        {
            var reader = new StreamReader(ShellStream);
            var writer = new StreamWriter(ShellStream) { AutoFlush = true };
            
            WriteStream(asc2Char.ToString(), writer, ShellStream);

            var result = ReadStream(reader).ToString().Trim();
            Console.Write(result);

            return string.Join('\n', result);
        }

        public void LoginToRoot(string sudoPassword)
        {
            RunCommand("sudo -s");
            RunCommand(sudoPassword);
            RunCommand("echo done");
        }
        
        public string RunCommand(string command, bool waitForCompletion = false)
        {
            var reader = new StreamReader(ShellStream);
            var writer = new StreamWriter(ShellStream) { AutoFlush = true };

            string result = null;
            if (waitForCompletion)
            {
                writer.WriteLine(command);
                Thread.Sleep(500);
                reader.ReadToEnd();
                while(!ShellStream.DataAvailable)
                {
                    Thread.Sleep(500);
                }

                result = reader.ReadToEnd();
            }
            else
            {
                WriteStream(command, writer, ShellStream);
                Thread.Sleep(500);
                result = ReadStream(reader).ToString().Trim();
            }

            if (!string.IsNullOrEmpty(result))
            {
                Console.Write(result);
                var list = result.Split('\n').ToList();
                
                if (list.Count > 1)
                {
                    list.Remove(list.First());
                    list.Remove(list.Last());
                }
            
                var newResult = string.Join('\n', list);
            
                return newResult;
            }

            return null;
        }
        
        private void WriteStream(string cmd, StreamWriter writer, ShellStream stream)
        {
            writer.WriteLine(cmd);
            while (stream.Length == 0)
            {
                Thread.Sleep(500);
            }
        }

        private StringBuilder ReadStream(StreamReader reader)
        {
            StringBuilder result = new StringBuilder();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                result.AppendLine(line);
            }
            return result;
        }
    }
}