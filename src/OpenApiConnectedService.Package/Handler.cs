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
    [ConnectedServiceHandlerExport(Constants.ProviderId, AppliesTo = "CSharp+Web")]
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

            var path = await GenerateNswagFileAsync(context, instance);

            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, $"Generated {path}");
        }

        private async Task<string> GenerateNswagFileAsync(ConnectedServiceHandlerContext context, Instance instance)
        {
            var nameSpace = context.ProjectHierarchy.GetProject().GetNameSpace();
            var serviceFolder = instance.Name;
            var serviceUrl = instance.ServiceUri;

            var doc = NSwagDocument.Create();
            doc.CodeGenerators.SwaggerToCSharpClientCommand = new SwaggerToCSharpClientCommand
            {
                OutputFilePath = $"{serviceFolder}Client.Generated.cs",
                ClassName = $"{serviceFolder}Client",
                Namespace = $"{nameSpace}.{serviceFolder}"
            };
            doc.SelectedSwaggerGenerator = new FromSwaggerCommand
            {
                OutputFilePath = $"{serviceFolder}.nswag.json",
                Url = serviceUrl
            };
            var json = doc.ToJson();

            var tempFileName = Path.GetTempFileName();
            File.WriteAllText(tempFileName, json);

            var targetPath = Path.Combine("Connected Services", serviceFolder, $"{serviceFolder}.nswag");
            return await context.HandlerHelper.AddFileAsync(tempFileName, targetPath);
        }
    }
}