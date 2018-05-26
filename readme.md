# Microsoft.Extensions.Configuration.Consul

## Installation

```powershell
PM> Install-Package Microsoft.Extensions.Configuration.Consul
```

```bash
dotnet add package Microsoft.Extensions.Configuration.Consul
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