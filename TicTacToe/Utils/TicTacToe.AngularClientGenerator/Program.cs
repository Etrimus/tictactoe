using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.TypeScript;
using NSwag.Generation.WebApi;
using TicTacToe.Web.Game;

namespace TicTacToe.AngularClientGenerator
{
    internal static class Program
    {
        private const string OutputPath = "../../../../../Server/TicTacToe.Web/wwwroot/tic-tac-toe/src/app/generated";
        private const string DtoClassName = "dto";

        public static async Task Main()
        {
            RecreateDirectory();

            var webApiSettings = new WebApiOpenApiDocumentGeneratorSettings
            {
                DefaultUrlTemplate = "{controller}/{action}/{id}",
                IsAspNetCore = true
            };

            var controllers = typeof(GameController).Assembly.GetTypes()
                .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract)
                .ToArray();

            var webApigenerator = new WebApiOpenApiDocumentGenerator(webApiSettings);
            var document = await webApigenerator.GenerateForControllersAsync(controllers);

            var clientSettings = CreateTypeScriptSettings();

            clientSettings.GenerateDtoTypes = true;
            var dto = GetCode(document, clientSettings);

            clientSettings.GenerateDtoTypes = false;
            clientSettings.GenerateClientClasses = true;
            var clients = GetCode(document, clientSettings);
            clients = AddDtoImports(document, clients);

            await File.WriteAllTextAsync($"{OutputPath}/{DtoClassName}.ts", dto);
            await File.WriteAllTextAsync($"{OutputPath}/clients.ts", clients);
        }

        private static string AddDtoImports(OpenApiDocument document, string code)
        {
            return $"import {{ {string.Join(", ", document.Definitions.Keys)} }} from './{DtoClassName}';{Environment.NewLine}{Environment.NewLine}{code}";

            //var builder = new StringBuilder();
            //var importAdded = false;
            //foreach (var line in code.Split('\n').Select(l => l.TrimEnd('\r')))
            //{
            //    if (!importAdded && line.StartsWith("export const BASE_URL = "))
            //    {
            //        builder.AppendLine("import {");
            //        foreach (var dtoClass in document.Definitions.Keys)
            //        {
            //            builder.AppendLine($"    {dtoClass},");
            //        }

            //        builder.AppendLine("} from './dto';");
            //        builder.AppendLine();
            //        importAdded = true;
            //    }

            //    builder.AppendLine(line);
            //}

            //return builder.ToString();
        }

        private static TypeScriptClientGeneratorSettings CreateTypeScriptSettings()
        {
            return new()
            {
                Template = TypeScriptTemplate.Angular,
                ClassName = "{controller}Client",
                BaseUrlTokenName = "BASE_URL",
                GenerateClientInterfaces = false,
                GenerateClientClasses = false,
                GenerateDtoTypes = false,
                InjectionTokenType = InjectionTokenType.InjectionToken,
                UseSingletonProvider = true,
                RxJsVersion = (decimal)6.6,
                TypeScriptGeneratorSettings =
                {
                    //DateTimeType = TypeScriptDateTimeType.Date,
                    NullValue = TypeScriptNullValue.Undefined,
                    TypeStyle = TypeScriptTypeStyle.Class,
                    TypeScriptVersion = 4,
                    //MarkOptionalProperties = false
                },
            };
        }

        private static string GetCode(OpenApiDocument document, TypeScriptClientGeneratorSettings clientSettings)
        {
            var clientGenerator = new TypeScriptClientGenerator(document, clientSettings);
            return clientGenerator.GenerateFile();
        }

        private static void RecreateDirectory()
        {
            if (!Directory.Exists(OutputPath))
            {
                Directory.CreateDirectory(OutputPath);
            }
            else
            {
                var directory = new DirectoryInfo(OutputPath);
                foreach (var file in directory.GetFiles())
                {
                    file.Delete();
                }
            }
        }
    }
}