# SwitchLoggers
A .NET 8.0 compatible class library that streamlines the selection and utilization of diverse loggers in software projects. Initially, it offers integration with NLog, Serilog and a custom Trace Log, while also providing the flexibility to expand and integrate additional logging frameworks.

## Detailed Description
The library streamlines the process of logger selection and integration into your projects. Specify your desired logger and add it without the need for extensive reconfiguration. This functionality not only saves time and effort but also ensures swift and smooth logging implementation.

With the dependency injection and a clear separation of concerns, SwitchLoggers offers scalability and enabling you to customize it according to your project's requirements. A notable feature includes a Trace system, which working as a logger, providing insights into the library's capabilities.

## Uses and Applications
The project's architecture, coupled with the use of interfaces, offers flexibility for contributions and enhancements, allowing for the seamless integration of new loggers and functionalities. This versatile tool serves the following purposes:
+ Catering to the diverse needs of end-users.
+ Adapting to the different stages of development.
+ Streamlining workflow for both FrontEnd and Backend development teams.

Consider a scenario where a logger loses support or improvements are desired for the library. Without disrupting the operation of our application and the library itself, we can:
+ Incorporate new loggers seamlessly.
+ Identify and address any issues promptly.
+ Adapt the library or our application as needed.

## Compatibility
The class library has been developed in .NET 8.0.
It workd on Windows, Linux, and MacOS.
You will need to install .NET on your OS if you do not already have it.
Remember to publish your application with the target architecture: Linux-64, osx-64, win-x64...
___
# Files System Structure
This is the file system structure of the SwitchLoggers library:
```
SwitchLoggers/
│
├── Enums/
│   └── LoggerType.cs
│
├── Loggers/
│   ├── Interfaces/
│   │   ├── IFactory_Loggers.cs
│   │   └── ILoggers.cs
│   │
│   ├── NLogger/
│   │   └── MyNLog.cs
│   │
│   ├── Serilog/
│   │   └── MySerilog.cs
│   │
│   ├── TraceLogger/
│   │   └── MyTraceLogger.cs
│   │
│   └── Factory_Loggers.cs
│
├── Settings/
│   └── SwitchLoggersSettings.cs
│
└── Utilities/
    └── Utilities_Loggers.cs
```

This is the ``ILoggers`` interface:
```
{
   void LogDebug(string message);
   void LogError(string message);
   void LogFatal(string message);
   void LogInformation(string message);
   void LogVerbose(string message);
   void LogWarning(string message);
}
```
## Basic Settings/Properties
It has the ``SwitchLoggersSettings.cs`` class which contains various pre-established configuration properties:
+ Automatically list the set loggers without any margin of error. In case a logger that doesn't exist is specified, the ``MyTraceLogger.cs`` class is set by default.
+ Set minimum levels that must be logged/shown for NLog and Serilog.
+ Establish the possibility of saving logs to a file, with the ability to customize the file path and name.
+ Additional properties can be added as deemed necessary.

Each logger has the ``private static Logger Build{MyLogger}(bool enableFileLogging, string filePath, string fileName)`` class where it can be improved and adapted for more configurations.

## NuGet Packages used
+ NLog
+ NLog.Schema
+ Serilog.Sinks.Console
+ Serilog.Sinks.File
___
# Implementation
Import the SwitchLoggers.csproj project into your main project. Make sure it is compatible with .NET 8.0.
In your main project, create a method to:
+ Create a logger factory instance.
+ Create and set the ILoggers instance.
+ Assign values to properties.

Example of a simple method:
```
public static void CreateLogger(string loggerName, bool enableFileLogging, string filePath, string fileName)
{
    // Create a logger factory instance.
    IFactory_Loggers factory_Loggers = new Factory_Loggers();

    // Create and set the logger instance.
    ILoggers logger = factory_Loggers.CreateLogger(loggerName, enableFileLogging, filePath, fileName);

    // Update configuration settings.
    SwitchLoggersSettings.EnableFileLogging = enableFileLogging;
    SwitchLoggersSettings.FilePath = filePath;
    SwitchLoggersSettings.FileName = fileName;
    SwitchLoggersSettings.Logger = logger;
}
```

To avoid errors, you can call the registered loggers using the LoggerType enum.
```
string loggerName = nameof(LoggerType.Nlog)
string loggerName = nameof(LoggerType.Serilog)
string loggerName = nameof(LoggerType.TraceLog)
```
Remember that you can also obtain them from the ``SwitchLoggersSettings.ListOfLoggers`` property.
To persist values throughout the application, assign them directly the desired value:
```
SwitchLoggersSettings.EnableFileLogging = enableFileLogging;
SwitchLoggersSettings.FilePath = filePath;
SwitchLoggersSettings.FileName = fileName;
SwitchLoggersSettings.Logger = logger;
```
Use the logger throughout your application using private, public, or scoped variables:
``private static ILoggers _logger = SwitchLoggersSettings.Logger``
or
``ILoggers logger = SwitchLoggersSettings.Logger``

## ServiceProvider Usage (optional)
Optionally, you can centralize the configuration in a ServiceProvider.
You will need to install the NuGet package ``Microsoft.Extensions.DependencyInjection``.
Add the class ``SwitchLoggersServiceProvider.cs`` to your main project:
```
SwitchLoggers
│
MyProject/
│
├── Configuration/
│   └── SwitchLoggersServiceProvider.cs
│
└── Program.cs
```

Register the interfaces and the ``Factory_Loggers`` class in ``SwitchLoggersServiceProvider.cs``:
```
public class SwitchLoggersServiceProvider
{
    public static IServiceProvider ConfigureLogger(string loggerName, bool enableFileLogging, string filePath, string fileName)
    {
        try
        {
            // Create the service collection and add singleton services.
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IFactory_Loggers, Factory_Loggers>()
                .AddSingleton<ILoggers>(provider =>
                {
                    // Configure the logger here using IFactory_Loggers.
                    var factoryLoggers = provider.GetService<IFactory_Loggers>();
                    return factoryLoggers?.CreateLogger(loggerName, enableFileLogging, filePath, fileName) ?? throw new InvalidOperationException("Logger cannot be null");
                })
                .BuildServiceProvider();

            // Get the logger instance from the service provider.
            var logger = serviceProvider.GetService<ILoggers>();

            // Set the logger in SwitchLoggersConfiguration if it is not null.
            if (logger != null)
            {
                // Set custom values
                SwitchLoggersSettings.EnableFileLogging = enableFileLogging;
                SwitchLoggersSettings.FilePath = filePath;
                SwitchLoggersSettings.FileName = fileName;
                SwitchLoggersSettings.Logger = logger;
                logger.LogInformation("Logger initialized successfully.");
            }

            return serviceProvider;
        }
        catch (Exception ex)
        {
            // Handle and rethrow the exception with additional information.
            throw new Exception(ex.ToString());
        }
    }
}
```
Now you can initialize your preferred configuration in the ``Main()`` method of ``Program.cs``:
``var serviceProvider = SwitchLoggersServiceProvider.ConfigureLogger(nameof(LoggerType.Serilog), true, string.Empty, string.Empty);``

Once the ``ILoggers`` variable is defined, you can retrieve its value and use it anywhere in your program using a private or scoped variable:
``private static ILoggers _logger = SwitchLoggersSettings.Logger``
or
``ILoggers logger = SwitchLoggersSettings.Logger``

Example of ``Program.cs`` for a Windows Forms application:

### Program.cs example for Windows Forms:
```
internal static class Program
{
    [STAThread]

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main()
    {
        // Open the console.
        AllocConsole();

        // Initialize Windows Forms App.
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Initialize SwitchLogger
        var serviceProvider = SwitchLoggersServiceProvider.ConfigureLogger(string.Empty, false, string.Empty, string.Empty);

        // Run Form1.cs
        Application.Run(new Form1());
    }

    // Declare AllocConsole from API Windows.
    [DllImport("kernel32.dll")]
    private static extern bool AllocConsole();
}
```
### Program.cs example for App Console without ServicePrivider:
```
using ConsoleSwitchLoggers.Menu;

// Start the main menu for logger selection and configuration.
MainMenu.StartMenu(); // Your cool methods.

// Wait for a key press before exiting.
Console.WriteLine("Press any key to exit.");
Console.ReadKey();
```
## Scalability
If you wish to incorporate a new logger, follow these simple steps:
1. Register it in the LoggerType.cs enum.
2. Create the folder structure within the ``Loggers\`` folder:
```
SwitchLoggers/
│
├── Loggers/
│   │
│   ├── NewLogger/
│   │   └── MyNewLogger.cs

```
3. Follow the code pattern for ``MyNewLogger.cs``, which you can see in the other files. Pay attention to the creation of your ``private static void BuildNewLogger(bool enableFileLogging, string filePath, string fileName)`` method, determined by the characteristics of your logger.
4. Register it in ``Factory_Loggers.cs``:
```
private static ILoggers CreateMyNewLog(bool enableFileLogging, string filePath, string fileName) => new MyNewLogger(enableFileLogging, filePath, fileName);
```
5. Annotate it in the main method ``ILoggers CreateLogger(...)``:
```
return logName switch
{
    …
    nameof(LoggerType.MyNewLogger) => CreateMyNewLog(enableFileLogging, filePath, fileName),
    …
};
```