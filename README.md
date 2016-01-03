# ScriptJunkie
**What is ScriptJunkie?**
It allows you to run many scripts/executables through one program and check expected exit code results.

## Example XML
```
// Will generate a template xml for you to use to generate your own.
ScriptJunkie.exe /XmlTemplatePath="<Path>"
```

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


## Known Issues
As this is still being worked on I haven't tested it with everything so some things might not work properly. If you find a problem please open an issue.
