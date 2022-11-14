# C# JSON test demo

Simple C# integration test demo using NUnit framework

### Tested setup

```
macOS Monterey
12.6.2
```
```
dotnet --info
.NET SDK:
 Version:   7.0.100
 Commit:    e12b7af219
```

### Run NUnit tests

```
$ dotnet test
```
Example:
```
Janis-MBP:dotnet-json-test janijegoroff$ dotnet test
  Determining projects to restore...
  All projects are up-to-date for restore.
  dotnet-json-test -> /Users/janijegoroff/projects/dotnet-json-test/bin/Debug/net7.0/dotnet-json-test.dll
Test run for /Users/janijegoroff/projects/dotnet-json-test/bin/Debug/net7.0/dotnet-json-test.dll (.NETCoreApp,Version=v7.0)
Microsoft (R) Test Execution Command Line Tool Version 17.4.0 (x64)
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:   121, Skipped:     0, Total:   121, Duration: 264 ms - dotnet-json-test.dll (net7.0)
```

### Future improvement ideas

- Static code analysis
- JSON schema validation
- Is `Newtonsoft.Json` the best library for handling JSON's?
- Test results logging to XML file (.runsettings)
- NUnit -> xUnit

### Schemas

Albums:
```json
{
  "$schema": "https://json-schema.org/draft/2019-09/schema",
  "type": "array",
  "default": [],
  "items": {
    "type": "object",
    "required": [
      "userId",
      "id",
      "title"
    ],
    "properties": {
      "userId": {
        "type": "integer"
      },
      "id": {
        "type": "integer"
      },
      "title": {
        "type": "string"
      }
    }
  }
}
```

Photos:
```json
{
  "$schema": "https://json-schema.org/draft/2019-09/schema",
  "type": "array",
  "default": [],
  "items": {
    "type": "object",
    "required": [
      "albumId",
      "id",
      "title",
      "url",
      "thumbnailUrl"
    ],
    "properties": {
      "albumId": {
        "type": "integer"
      },
      "id": {
        "type": "integer"
      },
      "title": {
        "type": "string"
      },
      "url": {
        "type": "string"
      },
      "thumbnailUrl": {
        "type": "string"
      }
    }
  }
}
```
