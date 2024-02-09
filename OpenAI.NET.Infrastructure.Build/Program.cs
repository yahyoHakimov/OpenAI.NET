// See https://aka.ms/new-console-template for more information



using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

Console.WriteLine("Hello, World!");
var adotNetClient = new ADotNetClient();

var githubPipeline = new GithubPipeline
{
    Name = "OpenAI.NET Build",

    OnEvents = new Events
    {
        Push = new PushEvent
        {
            Branches = new string[] { "main" }
        },

        PullRequest = new PullRequestEvent
        {
            Branches = new string[] { "main" }
        }
    },

    Jobs = new Jobs
    {
        Build = new BuildJob
        {
            RunsOn = BuildMachines.Windows2019,

            Steps = new List<GithubTask>
            {
                new CheckoutTaskV2
                {
                    Name = "Pulling Code",

                },
                new SetupDotNetTaskV1
                {
                    Name = "Installing .NET",

                    TargetDotNetVersion = new TargetDotNetVersion
                    {
                        DotNetVersion = "7.0.201",
                        IncludePrerelease = true
                    }
                },

                new RestoreTask
                {
                    Name = "Restoring .NET Packages"
                },

                new DotNetBuildTask
                {
                    Name = "Building Solution"
                },

                new TestTask
                {
                    Name = "Running Tests"
                }
            }
        }
    }
};


//Gihutb pipelines

string filePath = Path.GetFullPath("../../../../.github/workflows/dotnet.yml");

// Ensure that the directory exists, if not create it
string directoryPath = Path.GetDirectoryName(filePath);
if (!Directory.Exists(directoryPath))
{
    Directory.CreateDirectory(directoryPath);
}

adotNetClient.SerializeAndWriteToFile(
    githubPipeline,
    filePath
    );

