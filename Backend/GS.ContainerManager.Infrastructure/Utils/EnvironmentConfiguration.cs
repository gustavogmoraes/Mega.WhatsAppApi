namespace GS.ContainerManager.Infrastructure.Utils
{
    public class EnvironmentConfiguration
    {
        private static EnvironmentConfiguration _instance { get; set; }
        public static EnvironmentConfiguration Instance => _instance ??= new EnvironmentConfiguration();
        
        public string ApiSecret { get; set; }
        
        public string DockerServerUrl { get; set; }
        
        public int SshPort { get; set; }
        
        public string SshUsername { get; set; }
        
        public string SshPassword { get; set; }
        
        public int ApiPort { get; set; }
    }
}