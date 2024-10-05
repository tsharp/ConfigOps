using ConfigOps.Core;
using ConfigOps.Core.Persistence;
using ConfigOps.Sdk.Dto;
using ConfigOps.Sdk.Serialization;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Reflection;

namespace CfgCtl
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Console.WriteLine(Convert.ToHexString(Assembly.GetExecutingAssembly().GetName().GetPublicKey()));

            IKeyValueStore fileSystemKeyValueStore = FileSystemKeyValueStore.Create("C:\\testing\\sample-store");
            IConfigStore configStore = ConfigStore.Create(fileSystemKeyValueStore);

            // cfgctl --scope Global --instance None --namespace default apply --file C:\testing\sample-store\sample.json

            var rootCommand = new RootCommand("ConfigOps Config Control - cfgctl")
            {
                TreatUnmatchedTokensAsErrors = true
            };

            var namespaceOption = new Option<string>(["--namespace", "-n"], () => "default", "The namespace that this operation is applied.")
            {
                IsRequired = false
            };

            var outputFormatOption = new Option<OutputFormat>(["--output", "-o"], () => OutputFormat.Yaml, "The output format used") 
            { 
                IsRequired = false 
            };

            var fileOption = new Option<string>(["--file", "-f"], "Filename, directory, or URL to file to use to apply the resource.");

            // --> Apply / Post
            var apply_command = new Command("apply", "Apply a file and the resources it creates. If the resource exists, it is overwritten.");
            apply_command.SetHandler(async (InvocationContext context) =>
            {
                var file = context.ParseResult.GetValueForOption(fileOption) as string;

                if (File.Exists(file))
                {
                    var fileContents = File.ReadAllText(file);
                    var fileExtension = Path.GetExtension(file).ToLowerInvariant();
                    var @namespace = context.ParseResult.GetValueForOption(namespaceOption) as string;

                    switch (fileExtension)
                    {
                        case ".json":
                            await configStore.Apply(ConfigFormat.Json, fileContents, @namespace);
                            break;
                        case ".yml":
                        case ".yaml":
                            Console.WriteLine($"Processing File: {file} ...");
                            await configStore.Apply(ConfigFormat.Yaml, fileContents, @namespace);
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

            // --> Get
            var get_command = new Command("get", "Display one or many resources.");
            get_command.AddArgument(new Argument<string>("resource", "The resource type to get.")
            {
                Arity = ArgumentArity.ExactlyOne
            });

            get_command.AddArgument(new Argument<string>("name", "The name of the resource to get.")
            {
                Arity = ArgumentArity.ZeroOrOne
            });

            get_command.SetHandler(async (InvocationContext context) =>
            {
                var @namespace = context.ParseResult.GetValueForOption(namespaceOption) as string;
                var resource = context.ParseResult.GetValueForArgument(get_command.Arguments[0]) as string;
                var name = context.ParseResult.GetValueForArgument(get_command.Arguments[1]) as string;

                var parts = new string[] { @namespace, resource, name };
                var key = string.Join('/', parts.Where(p => !string.IsNullOrWhiteSpace(p)));

                Console.WriteLine($"Getting Resource: {key}");
                
                try
                {
                    var @object = await configStore.Get<SystemObject>(key);
                    Console.WriteLine(@object.ObjectToYaml());
                }
                catch(KeyNotFoundException)
                {
                    Console.WriteLine($"Resource not found: {key}");
                }
            });

            // --> Delete
            var delete_command = new Command("delete", "Delete resources by keys, stdin, resources and names, or by resources and label selector.");
            delete_command.AddArgument(new Argument<string>("resource", "The resource type to delete.")
            {
                Arity = ArgumentArity.ExactlyOne
            });

            delete_command.AddArgument(new Argument<string>("name", "The name of the resource to delete.")
            {
                Arity = ArgumentArity.ExactlyOne
            });

            delete_command.SetHandler(async (InvocationContext context) =>
            {
                var @namespace = context.ParseResult.GetValueForOption(namespaceOption) as string;
                var resource = context.ParseResult.GetValueForArgument(delete_command.Arguments[0]) as string;
                var name = context.ParseResult.GetValueForArgument(delete_command.Arguments[1]) as string;

                var parts = new string[] { @namespace, resource, name };
                var key = string.Join('/', parts.Where(p => !string.IsNullOrWhiteSpace(p)));

                Console.WriteLine($"Deleting Resource: {key}");

                if (await configStore.Delete(key))
                {
                    Console.WriteLine($"Resource deleted: {key}");
                }
                else
                {
                    Console.WriteLine($"Resource not found: {key}");
                }
            });

            // --> Patch
            var patch_command = new Command("patch", "Update field(s) of a resource using strategic merge patch.");
            patch_command.SetHandler(async (InvocationContext context) =>
            {
                var @namespace = context.ParseResult.GetValueForOption(namespaceOption) as string;
                var file = context.ParseResult.GetValueForOption(fileOption) as string;

                if (File.Exists(file))
                {
                    var fileContents = File.ReadAllText(file);
                    var fileExtension = Path.GetExtension(file).ToLowerInvariant();

                    switch (fileExtension)
                    {
                        case ".json":
                            await configStore.Patch(ConfigFormat.Json, fileContents, @namespace);
                            break;
                        case ".yml":
                        case ".yaml":
                            Console.WriteLine($"Processing File: {file} ...");
                            await configStore.Patch(ConfigFormat.Yaml, fileContents, @namespace);
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

            // var export_command = new Command("export", "Export resources to a file.");
            // var import_command = new Command("import", "Import resources from a file.");

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

            // Namespaces can further subdivide any instances (Global, Cloud, Regional, Service, etc)
            apply_command.AddOption(namespaceOption);
            apply_command.AddOption(fileOption);

            patch_command.AddOption(namespaceOption);
            patch_command.AddOption(fileOption);

            get_command.AddOption(namespaceOption);
            get_command.AddOption(outputFormatOption);

            delete_command.AddOption(namespaceOption);

            rootCommand.AddCommand(apply_command);
            rootCommand.AddCommand(get_command);
            rootCommand.AddCommand(delete_command);
            rootCommand.AddCommand(patch_command);

            await rootCommand.InvokeAsync(args);
        }
    }
}
