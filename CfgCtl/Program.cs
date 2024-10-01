using ConfigOps.Core;
using ConfigOps.Core.Persistence;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace CfgCtl
{
    public enum ConfigScope
    {
        Global,
        Cloud,
        Regional,
        Service
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            IKeyValueStore fileSystemKeyValueStore = FileSystemKeyValueStore.Create("C:\\testing\\sample-store");
            IConfigStore configStore = ConfigStore.Create(fileSystemKeyValueStore);

            // cfgctl --scope Global --instance None --namespace default apply --file C:\testing\sample-store\sample.json

            var rootCommand = new RootCommand("ConfigOps Config Control - cfgctl")
            {
                TreatUnmatchedTokensAsErrors = true
            };

            // Scope: Global, Cloud, Regional, Service
            rootCommand.AddOption(new Option<ConfigScope>(new string[] { "--scope", "-s" }, () => ConfigScope.Global, "The scope that this operation is applied.")
            {
                IsRequired = false
            });

            // Global Instance: None
            // Cloud Instance: Azure, AWS, GCP, etc
            // Regional Instance: US, EU, etc
            // Service Instance: Instance name
            rootCommand.AddOption(new Option<string>(new string[] { "--instance", "-i" }, "The instance that this operation is applied.")
            {
                IsRequired = false
            });

            // Namespaces can further subdivide any instances (Global, Cloud, Regional, Service, etc)
            rootCommand.AddOption(new Option<string>(new string[] { "--namespace", "-n" }, () => "default", "The namespace that this operation is applied.")
            {
                IsRequired = false 
            });

            var apply_command = new Command("apply", "Apply a file and the resources it creates.");
            apply_command.AddOption(new Option<string>(new string[] { "--file", "-f" }, "Filename, directory, or URL to file to use to apply the resource."));

            apply_command.SetHandler(async (InvocationContext context) =>
            {
                var file = context.ParseResult.GetValueForOption(apply_command.Options[0]) as string;

                if (File.Exists(file))
                {
                    var fileContents = File.ReadAllText(file);
                    var fileExtension = Path.GetExtension(file).ToLowerInvariant();

                    switch (fileExtension)
                    {
                        case ".json":
                            await configStore.Apply(ConfigFormat.Json, fileContents);
                            break;
                        case ".yml":
                        case ".yaml":
                            await configStore.Apply(ConfigFormat.Yaml, fileContents);
                            break;
                        default:
                            Console.WriteLine($"Unsupported file type: {file}");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"File not found: {file}");
                }
            });

            var describe_command = new Command("describe", "Describe one or many resources.");
            var delete_command = new Command("delete", "Delete resources by keys, stdin, resources and names, or by resources and label selector.");
            var get_command = new Command("get", "Display one or many resources.");
            var export_command = new Command("export", "Export resources to a file.");
            var import_command = new Command("import", "Import resources from a file.");

            // var edit_command = new Command("edit", "Edit a resource on the server.");
            // var label_command = new Command("label", "Update the labels on a resource.");
            // var annotate_command = new Command("annotate", "Update the annotations on a resource.");
            // var patch_command = new Command("patch", "Update field(s) of a resource using strategic merge patch.");
            // var replace_command = new Command("replace", "Replace a resource by filename or stdin.");
            // var scale_command = new Command("scale", "Set a new size for a Deployment, ReplicaSet, Replication Controller, or Job.");
            // var set_command = new Command("set", "Configure specific resources.");
            // var rollout_command = new Command("rollout", "Manage the rollout of a resource.");
            // var run_command = new Command("run", "Run a particular image on the cluster.");
            // var expose_command = new Command("expose", "Take a replication controller, service, deployment or pod and expose it as a new Kubernetes Service.");
            // var autoscale_command = new Command("autoscale", "Auto-scale a Deployment, ReplicaSet, or ReplicationController.");
            // var attach_command = new Command("attach", "Attach to a running container.");
            // var exec_command = new Command("exec", "Execute a command in a container.");
            // var port_forward_command = new Command("port-forward", "Forward one or more local ports to a pod.");
            // var proxy_command = new Command("proxy", "Run a proxy to the Kubernetes API server.");
            // var cp_command = new Command("cp", "Copy files and directories to and from containers.");
            // var auth_command = new Command("auth", "Inspect authorization.");
            // var logs_command = new Command("logs", "Print the logs for a container in a pod.");

            rootCommand.AddCommand(apply_command);

            await rootCommand.InvokeAsync(args);
        }
    }
}
