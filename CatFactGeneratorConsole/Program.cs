using CatFactGeneratorConsole;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

string filePath = configuration.GetSection("AppSettings")["FilePath"];
string catFactApiUrl = configuration.GetSection("AppSettings")["CatFactApiUrl"];
int maxCatFactLength = int.Parse(configuration.GetSection("AppSettings")["CatFactMaxLength"]);
int intervalInSeconds = int.Parse(configuration.GetSection("AppSettings")["IntervalInSeconds"]);

var serviceProvider = new ServiceCollection()
            .AddHttpClient()
            .AddTransient<CatFactService>()
            .AddTransient<FileService>()
            .BuildServiceProvider();


var httpClient = serviceProvider.GetService<HttpClient>();
var catFactService = new CatFactService(httpClient,catFactApiUrl);
var fileService = new FileService(filePath);



while (true)
{
    string catFact = await catFactService.GetRandomCatFact(maxCatFactLength);
    fileService.AppendToFile(catFact);
    Console.WriteLine("Cat fact written to file: " + catFact);

    await Task.Delay(intervalInSeconds * 1000);
}