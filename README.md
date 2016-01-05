
# ScriptJunkie [![Build status](https://ci.appveyor.com/api/projects/status/ls9qpbdnn9n4svg4/branch/master?svg=true)](https://ci.appveyor.com/project/lzinga/scriptjunkie/branch/master)

**What is ScriptJunkie?**
It allows you to run many scripts/executables through one program and check expected exit code results.

# Usage

```csharp
// Will run ScriptJunkie against the scripts.xml file.
ScriptJunkie.exe /XmlPath="C:/Temp/scripts.xml"

// Will pause ScriptJunkie at the start allowing attachment of debugger.
ScriptJunkie.exe /Debug

// Will genereate a template xml that ScriptJunkie can accept.
ScriptJunkie.exe /XmlTemplatePath="C:/Temp/Template.xml"
```

# Template Xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<Setup xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
 <Downloads TimeOut="60" RefreshRate="10">
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

##### Exit Code Info
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

# Tested File Types
##### Downloads
All files should download correctly as long as the url is a direct link to the file.

##### Execution File Type
1. ***Powershell (.ps1)*** - Unit Test Verification

##### Not Implemented
1. ***Executable (.exe)*** - However it should work fine, just hasn't been tested.
2. ***Archive (.zip/.rar)***


# Known Issues
1. As this is still being worked on I haven't tested it with everything so some things might not work properly. If you find a problem please open an issue.
