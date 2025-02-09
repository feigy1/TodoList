namespace AuthServer
{
    public class Application
    {
        public string? Name { get; set; }

        public string? Author { get; set; }
        
        public string? GitHubUrl { get; set; }
        
        public string? Version { get; set; }
        
        public string? Technology { get; set; }
    
        public Application(){
            Name="TasksServer";
            Author="Feigy Shmaya";
            GitHubUrl="https://github.com/feigy1";
            Version="0.0.1";
            Technology="NET 8 Web API";
        }
    }
}
