namespace GitHubApp.Model
{
    public class PathDirContainer
    {


        public PathDirContainer(string fullPath, string name)
        {
            FullPath = fullPath;
            Name = name;
        }

        public string FullPath { get; set; }
        public string Name { get; set; }

    }
}
