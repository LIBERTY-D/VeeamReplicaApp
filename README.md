
# FileReplicaApp

FileReplicaApp is a command-line application that periodically synchronizes files and directories from a source directory to a replica directory. It ensures the replica stays up to date with the source directory by copying or updating files and cleaning up the replica for files no longer present in the source. It utilizes Serilog for logging.
## Features

- Synchronizes files and directories from a source folder to a replica folder.
- Periodically syncs the source and replica based on an interval.
- Logs information about synchronization operations.
- Deletes files and directories from the replica that no longer exist in the source folder.

## Table of Contents

1. [Installation](#installation)
2. [Usage](#usage)
    - [Command-line Arguments](#command-line-arguments)
3. [Dependencies](#dependencies)
4. [Logging](#logging)
5. [Code Structure](#code-structure)


## Installation

1. Clone the repository or download the source code.

    ```bash
    git clone 
    ```

2. Navigate to the FileReplicaApp app by first going into the VeeamReplicaApp folder

    ```bash
    cd  ./VeeamReplicaApp/FileReplicaApp/
    ```
3. Open the project in Visual Studio or any .NET-compatible editor.

3. Restore the NuGet packages to ensure all dependencies are installed.

    ```bash
    dotnet restore
    ```

4. Build the project.

    ```bash
    dotnet build
    ```

## Usage

### Command-line Arguments

The application requires the following arguments to be passed when running:

 ```bash
dotnet run <intervalSeconds> <sourceDir> <replicaDir> <logFile>
```

- `<intervalSeconds>`: The interval (in seconds) between each synchronization cycle.
- `<sourceDir>`: The path to the source directory.
- `<replicaDir>`: The path to the replica directory where files will be synchronized.
- `<logFile>`: The name of the log file where synchronization logs will be stored.

### Example:

```bash
dotnet run 5 source replica "sync_log.txt"
```

# Dependencies

This project uses the following NuGet packages for logging:

- **Serilog**: The core logging library.
- **Serilog.Extensions.Logging**: Provides integration between Serilog and the Microsoft.Extensions.Logging library.
- **Serilog.Sinks.Console**: Writes log entries to the console.
- **Serilog.Sinks.File**: Writes log entries to a file.

## Dependencies in the .csproj File

Here’s how the dependencies are defined in the `.csproj` file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Serilog" Version="4.2.0" />
    <PackageReference Include="Serilog.Extensions.Logging" Version="9.0.1" />
    <PackageReference Include="Serilog.Sinks.Console" Version="6.0.0" />
    <PackageReference Include="Serilog.Sinks.File" Version="6.0.0" />
  </ItemGroup>
</Project>
```
## To Install or Update Dependencies

You can use the following commands to install or update these dependencies:

```bash
dotnet add package Serilog --version 4.2.0
dotnet add package Serilog.Extensions.Logging --version 9.0.1
dotnet add package Serilog.Sinks.Console --version 6.0.0
dotnet add package Serilog.Sinks.File --version 6.0.0
```

## Logging

The application uses **Serilog** for logging synchronization events. Logs are written both to the console and a log file (based on the specified `logFile` argument). The log file is rotated daily.

### Logging Configuration

The logging configuration is set up as follows:

```csharp
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
    .CreateLogger();
```

## Code Structure

### Services:
- **FileSyncService**: Contains the core logic for syncing files, directories, and cleaning up the replica.

### Controllers:
- **FileReplicaController**: Coordinates the application flow, using `FileSyncService` to perform periodic synchronization.

### IFace:
- **IFileSyncService**: Defines methods for file synchronization operations.
- **IFileReplicaController**: Defines the method to start the synchronization process.
### Logs:
- Log file specified in the cmd will be stored here.

### source( May be or not Present):
- if no path specified and maybe your entered a folder which doesnt exist then the folder your entered is stored in the Root App. 

### replica (May be or not Present):
- if no path specified and maybe your entered a folder which doesnt exist then the folder your entered is stored in the Root App.
