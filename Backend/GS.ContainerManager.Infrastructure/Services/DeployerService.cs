using System;
using System.Collections.Generic;
using System.Linq;
using GS.ContainerManager.Infrastructure.Utils;
using Renci.SshNet;

namespace GS.ContainerManager.Infrastructure.Services
{
    public class DeployerService : IDisposable
    {
        public void DeployNewerImage(string imageName)
        {
            Console.WriteLine("Connecting to ssh");
            var connectionInfo = GetConnectionInfo();

            using var sshClient = new SshClient(connectionInfo);
            sshClient.Connect();
    
            var containersUsingImage = GetAllContainersThatUseAnImage(sshClient, imageName);
            
            Console.WriteLine($"Got {containersUsingImage.Count} using image");
            
            var runCommands = new List<string>();
            
            foreach (var containerId in containersUsingImage)
            {
                Console.WriteLine($"Stopping {containerId}");
                
                StopContainer(sshClient, containerId);
                runCommands.Add(ExtractRunCommandFromContainer(sshClient, containerId));
            }
            
            foreach (var containerId in containersUsingImage)
            {
                Console.WriteLine($"Removing {containerId}");
                RemoveContainer(sshClient, containerId);
            }
            
            Console.WriteLine($"Pulling newest image {imageName}");
            PullImage(sshClient, imageName);
            
            foreach (var command in runCommands)
            {
                Console.WriteLine("Redeploying container");
                sshClient.RunCommand(command);
            }
            
            Console.WriteLine("Removing old image");
            RemoveOldImage(sshClient, imageName);
            
            Console.WriteLine("All done");
            sshClient.Disconnect();    
        }

        private void RemoveOldImage(SshClient sshClient, string imageName)
        {
            if (imageName.Contains(":"))
            {
                imageName = imageName.Split(':').First();
            }

            sshClient.RunCommand($@"docker rmi $(docker images |grep '{imageName}')");
        }

        private ConnectionInfo GetConnectionInfo()
        {
            return new ConnectionInfo(
                EnvironmentConfiguration.Instance.DockerServerUrl, 
                EnvironmentConfiguration.Instance.SshPort,
                EnvironmentConfiguration.Instance.SshUsername, 
                new PasswordAuthenticationMethod(
                    EnvironmentConfiguration.Instance.SshUsername, EnvironmentConfiguration.Instance.SshPassword));
        }

        private void PullImage(SshClient sshClient, string imageName)
        {
            sshClient.RunCommand($"docker pull {imageName}");
        }

        private void RemoveContainer(SshClient sshClient, string containerId)
        {
            sshClient.RunShellCommand($"docker rm {containerId}");
        }

        private string ExtractRunCommandFromContainer(SshClient sshClient, string containerIdOrName)
        {
            var runTplExists = CheckIfFileExists(sshClient, "run.tpl");
            if (!runTplExists)
            {
                CreateRunTpl(sshClient);
            }

            var result = sshClient.RunShellCommand($@"docker inspect --format ""$(<run.tpl)"" {containerIdOrName}");
            return result;
        }

        private void CreateRunTpl(SshClient sshClient)
        {
            var shellBridge = new ShellBridge(sshClient);
            
            shellBridge.RunCommand("cat > run.tpl");
            shellBridge.RunCommand(RUN_TPL_TEXT);
            shellBridge.RunCommand("'\u0004'"); // --> ASCII for Ctrl + D which writes out from cat
        }

        private bool CheckIfFileExists(SshClient sshClient, string fileName)
        {
            var result = sshClient.RunShellCommand($@"if [ -e {fileName} ]; then echo ""ok""; else echo ""nok""; fi");

            return result.Trim() == "ok";
        }

        private void StopContainer(SshClient sshClient, string containerId, int timeoutInSeconds = 500)
        {
            sshClient.RunShellCommand(@$"docker stop --time {timeoutInSeconds} {containerId}");
        }

        private List<string> GetAllContainersThatUseAnImage(SshClient sshClient, string imageName)
        {
            var containersAndImages = GetContainersAndImages(sshClient);
            return containersAndImages
                .Where(x => x.Value.Contains(imageName))
                .Select(x => x.Key)
                .ToList();
        }

        private Dictionary<string, string> GetContainersAndImages(SshClient sshClient)
        {
            var result = sshClient.RunShellCommand(@"docker ps -a --format=""{{.ID}}|{{.Image}}""");
            var lines = result.Split('\n');
            
            var containersAndImages = lines.Select(x =>
            {
                var splittedVar = x.Split('|');
                return new KeyValuePair<string, string>(splittedVar[0], splittedVar[1]);
            }).ToDictionary(x => x.Key, x => x.Value);

            return containersAndImages;
        }
        
        private Dictionary<string, string> GetContainerEnvinronmentVariables(ShellBridge shellBridge, string containerNameOrId)
        {
            var result = shellBridge.RunCommand($"docker inspect -f '{{range $index, $value := .Config.Env}}{{println $value}}{{end}}' {containerNameOrId}");
            var splitted = result.Split('\n');
            var variables = splitted.Select(x =>
            {
                var splittedVar = x.Split('=');
                return new KeyValuePair<string, string>(splittedVar[0], splittedVar[1]);
            }).ToDictionary(x => x.Key, x => x.Value);

            return variables;
        }
        
        private void CheckVersions()
        {
            
        }

        private const string RUN_TPL_TEXT =
           @"docker run \
              --name {{printf ""%q"" .Name}} \
                {{- with .HostConfig}}
                    {{- if .Privileged}}
              --privileged \
                    {{- end}}
                    {{- if .AutoRemove}}
              --rm \
                    {{- end}}
                    {{- if .Runtime}}
              --runtime {{printf ""%q"" .Runtime}} \
                    {{- end}}
                    {{- range $b := .Binds}}
              --volume {{printf ""%q"" $b}} \
                    {{- end}}
                    {{- range $v := .VolumesFrom}}
              --volumes-from {{printf ""%q"" $v}} \
                    {{- end}}
                    {{- range $l := .Links}}
              --link {{printf ""%q"" $l}} \
                    {{- end}}
                    {{- if .PublishAllPorts}}
              --publish-all \
                    {{- end}}
                    {{- if .UTSMode}}
              --uts {{printf ""%q"" .UTSMode}} \
                    {{- end}}
                    {{- with .LogConfig}}
              --log-driver {{printf ""%q"" .Type}} \
                        {{- range $o, $v := .Config}}
              --log-opt {{$o}}={{printf ""%q"" $v}} \
                        {{- end}}
                    {{- end}}
                    {{- with .RestartPolicy}}
              --restart ""{{.Name -}}
                        {{- if eq .Name ""on-failure""}}:{{.MaximumRetryCount}}
                        {{- end}}"" \
                    {{- end}}
                    {{- range $e := .ExtraHosts}}
              --add-host {{printf ""%q"" $e}} \
                    {{- end}}
                    {{- range $v := .CapAdd}}
              --cap-add {{printf ""%q"" $v}} \
                    {{- end}}
                    {{- range $v := .CapDrop}}
              --cap-drop {{printf ""%q"" $v}} \
                    {{- end}}
                    {{- range $d := .Devices}}
              --device {{printf ""%q"" (index $d).PathOnHost}}:{{printf ""%q"" (index $d).PathInContainer}}:{{(index $d).CgroupPermissions}} \
                    {{- end}}
                {{- end}}
                {{- with .NetworkSettings -}}
                    {{- range $p, $conf := .Ports}}
                        {{- with $conf}}
              --publish ""
                            {{- if $h := (index $conf 0).HostIp}}{{$h}}:
                            {{- end}}
                            {{- (index $conf 0).HostPort}}:{{$p}}"" \
                        {{- end}}
                    {{- end}}
                    {{- range $n, $conf := .Networks}}
                        {{- with $conf}}
              --network {{printf ""%q"" $n}} \
                            {{- range $a := $conf.Aliases}}
              --network-alias {{printf ""%q"" $a}} \
                            {{- end}}
                        {{- end}}
                    {{- end}}
                {{- end}}
                {{- with .Config}}
                    {{- if .Hostname}}
              --hostname {{printf ""%q"" .Hostname}} \
                    {{- end}}
                    {{- if .Domainname}}
              --domainname {{printf ""%q"" .Domainname}} \
                    {{- end}}
                    {{- range $p, $conf := .ExposedPorts}}
              --expose {{printf ""%q"" $p}} \
                    {{- end}}
                    {{- range $e := .Env}}
              --env {{printf ""%q"" $e}} \
                    {{- end}}
                    {{- range $l, $v := .Labels}}
              --label {{printf ""%q"" $l}}={{printf ""%q"" $v}} \
                    {{- end}}
                {{- if not (or .AttachStdin  (or .AttachStdout .AttachStderr))}}
              --detach \
                {{- end}}
                {{- if .AttachStdin}}
              --attach stdin \
                {{- end}}
                {{- if .AttachStdout}}
              --attach stdout \
                {{- end}}
                {{- if .AttachStderr}}
              --attach stderr \
                {{- end}}
                {{- if .Tty}}
              --tty \
                {{- end}}
                {{- if .OpenStdin}}
              --interactive \
                {{- end}}
                {{- if .Entrypoint}}
            {{- /* Since the entry point cannot be overridden from the command line with an array of size over 1,
                   we are fine assuming the default value in such a case. */ -}}
                    {{- if eq (len .Entrypoint) 1 }}
              --entrypoint ""
                        {{- range $i, $v := .Entrypoint}}
                            {{- if $i}} {{end}}
                            {{- $v}}
                        {{- end}}"" \
                    {{- end}}
                {{- end}}
              {{printf ""%q"" .Image}} \
              {{range .Cmd}}{{printf ""%q "" .}}{{- end}}
            {{- end}}";
        
        public void Dispose()
        {
        }
    }
}