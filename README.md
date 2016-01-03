# ScriptJunkie
**What is ScriptJunkie?**
It allows you to run many scripts/executables through one program and check expected exit code results.

## Example XML
```csharp
// Will generate a template xml for you to use to generate your own.
ScriptJunkie.exe /XmlTemplatePath="C:/Temp/Template.xml"
```
### Template Xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<Setup xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Downloads>
    <Download Name="Nothing Powershell Script" Description="This script does nothing">
      <DownloadUrl>www.blank.com/nothing.ps1</DownloadUrl>
      <DestinationPath>C:/Temp/Downloads/nothing.ps1</DestinationPath>
    </Download>
  </Downloads>
  <Scripts>
    <Script Name="Script 1" Description="Does nothing">
      <Executable Path="C:/Temp/nothing.ps1" />
      <Arguments>
        <Argument Key="-i" Value="C:/Temp/something.bin" />
        <Argument Key="-x" Value="" />
      </Arguments>
      <ExitCodes>
        <ExitCode Value="0" Message="Files deleted" IsSuccess="true" />
        <ExitCode Value="1" Message="Files failed to delete" IsSuccess="false" />
        <ExitCode Value="2" Message="Couldn't find any files" IsSuccess="false" />
      </ExitCodes>
    </Script>
  </Scripts>
</Setup>
```

### Exit Code Info
```xml
<ExitCodes>
  <ExitCode Value="0" Message="Files deleted" IsSuccess="true" />
  <ExitCode Value="1" Message="Files failed to delete" IsSuccess="false" />
  <ExitCode Value="2" Message="Couldn't find any files" IsSuccess="false" />
</ExitCodes>
```

The above xml allows the program to determine what kind of exit codes are expected. It also determines which ones count as a pass of the program being ran. So in the above program doesn't get an exit code where the `<ExitCode ... IsSuccess="true" / > it will cause ScriptJunkie to exit with exit code 1. If all programs exit with a successfull exit code ScriptJunkie will exit with a 0.

1. The "Value" attribute is the exit code.
2. The "Message" is displayed in the execution process of ScriptJunkie.

## Tested File Types
1. Powershell (.ps1) - Works as expected.
2. Executable (.exe) - Next to test.

## Known Issues
1. As this is still being worked on I haven't tested it with everything so some things might not work properly. If you find a problem please open an issue.
2. Currently all downloads will time out in 60 seconds and will give feedback every 10 seconds. Tried getting an xml attribute for the DownloadCollection however it wouldn't appear in the xml. How I wanted it to look `<Downloads Timeout="60" RecheckInterval="10">` but couldn't get it to work. So for now it is hard coded.
