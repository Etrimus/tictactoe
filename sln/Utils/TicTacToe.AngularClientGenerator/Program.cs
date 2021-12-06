using Microsoft.AspNetCore.Mvc;
using NJsonSchema.CodeGeneration.TypeScript;
using NSwag;
using NSwag.CodeGeneration.TypeScript;
using NSwag.Generation.WebApi;
using TicTacToe.Web;

namespace TicTacToe.AngularClientGenerator;

internal static class Program
{
    private const string OUTPUT_PATH = "../../../../../Server/TicTacToe.Web/wwwroot/tic-tac-toe/src/app/generated";
    private const string DTO_CLASS_NAME = "dto";

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

        await File.WriteAllTextAsync($"{OUTPUT_PATH}/{DTO_CLASS_NAME}.ts", dto);
        await File.WriteAllTextAsync($"{OUTPUT_PATH}/clients.ts", clients);
    }

    private static void RecreateDirectory()
    {
        if (!Directory.Exists(OUTPUT_PATH))
        {
            Directory.CreateDirectory(OUTPUT_PATH);
        }
        else
        {
            var directory = new DirectoryInfo(OUTPUT_PATH);
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }
        }
    }

    private static TypeScriptClientGeneratorSettings CreateTypeScriptSettings()
    {
        return new()
        {
            Template = TypeScriptTemplate.Angular,
            ClassName = "{controller}Client",
            BaseUrlTokenName = "BASE_API_URL",
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

    private static string AddDtoImports(OpenApiDocument document, string code)
    {
        return $"import {{ {string.Join(", ", document.Definitions.Keys)} }} from './{DTO_CLASS_NAME}';{Environment.NewLine}{Environment.NewLine}{code}";
    }
}
