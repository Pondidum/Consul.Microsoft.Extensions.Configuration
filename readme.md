# Consul.Microsoft.Extensions.Configuration

## Installation

```powershell
PM> Install-Package Consul.Microsoft.Extensions.Configuration
```

```bash
dotnet add package Consul.Microsoft.Extensions.Configuration
```

## Usage

```csharp
var config = new ConfigurationBuilder()
    .AddConsul()    // add this line
    .Build()
    .Get<ApplicationConfig>();
```

You can also specify a key prefix (which is removed from the key names):

```csharp
var config = new ConfigurationBuilder()
    .AddConsul(prefix: "appsettings/myapp/")
    .Build()
    .Get<ApplicationConfig>();
```

And you can override the `QueryOptions` used for talking to Consul too (so you can control `Consistency`, `Datacenter` etc.)

```csharp
var config = new ConfigurationBuilder()
    .AddConsul(consul => {
        consul.Prefix = "appsettings/myapp/";
        consul.Options = new QueryOptions
        {
            Consistency = ConsistencyMode.Consistent,
            Datacenter = "ue-west-1",
        };
    })
    .Build()
    .Get<ApplicationConfig>();
```