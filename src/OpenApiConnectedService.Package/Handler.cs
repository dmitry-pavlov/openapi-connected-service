using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ConnectedServices;
using NSwag.Commands;
using NSwag.Commands.CodeGeneration;
using NSwag.Commands.SwaggerGeneration;
using OpenApiConnectedService.Package.Utilities;

namespace OpenApiConnectedService.Package
{
    [ConnectedServiceHandlerExport(Constants.ProviderId, AppliesTo = "CSharp")]
    internal class Handler : ConnectedServiceHandler
    {
        public override async Task<AddServiceInstanceResult> AddServiceInstanceAsync(ConnectedServiceHandlerContext context,
            CancellationToken ct)
        {
            await GenerateAsync(context);

            var folderName = context.ServiceInstance.Name;
            var gettingStartedUrl = new Uri(Constants.Website);
            var result = new AddServiceInstanceResult(folderName, gettingStartedUrl);

            return result;
        }

        private async Task GenerateAsync(ConnectedServiceHandlerContext context)
        {
            var instance = (Instance) context.ServiceInstance;

            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, $"Generating {instance.Name} from {instance.ServiceUri}");
            var nswagFilePath = await GenerateNswagFileAsync(context, instance);
            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, $"Generated {nswagFilePath}");

            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, $"Generating {instance.Name}Client.Generated.cs from {Path.GetFileName(nswagFilePath)}");
            var csharpFilepath = await GenerateCSharpFileAsync(context, instance);
            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, $"Generated {csharpFilepath}");
        }

        private async Task<string> GenerateCSharpFileAsync(ConnectedServiceHandlerContext context, Instance instance)
        {
            var serviceFolder = instance.Name;
            var folderPath = context.ProjectHierarchy.GetProject().GetServiceFolderPath(serviceFolder);
            var nswagFilePath = Path.Combine(folderPath, $"{serviceFolder}.nswag");
            var document = await NSwagDocument.LoadAsync(nswagFilePath);
            await document.ExecuteAsync();
            var generatedFilePath = document.CodeGenerators.SwaggerToCSharpClientCommand.OutputFilePath;
            var generatedFullPath = Path.Combine(folderPath, generatedFilePath);
            await context.HandlerHelper.AddFileAsync(generatedFullPath, generatedFilePath);
            return generatedFilePath;
        }

        private async Task<string> GenerateNswagFileAsync(ConnectedServiceHandlerContext context, Instance instance)
        {
            var nameSpace = context.ProjectHierarchy.GetProject().GetNameSpace();
            var serviceFolder = instance.Name;
            var serviceUrl = instance.ServiceUri;

            var document = NSwagDocument.Create();
            document.CodeGenerators.SwaggerToCSharpClientCommand = new SwaggerToCSharpClientCommand
            {
                OutputFilePath = $"{serviceFolder}Client.Generated.cs",
                ClassName = $"{serviceFolder}Client",
                Namespace = $"{nameSpace}.{serviceFolder}"
            };
            document.SelectedSwaggerGenerator = new FromSwaggerCommand
            {
                OutputFilePath = $"{serviceFolder}.nswag.json",
                Url = serviceUrl
            };
            var json = document.ToJson();

            var tempFileName = Path.GetTempFileName();
            File.WriteAllText(tempFileName, json);

            var targetPath = Path.Combine("Connected Services", serviceFolder, $"{serviceFolder}.nswag");
            var nswagFilePath = await context.HandlerHelper.AddFileAsync(tempFileName, targetPath);

            return nswagFilePath;
        }
    }
}