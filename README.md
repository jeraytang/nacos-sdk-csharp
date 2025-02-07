# nacos-sdk-csharp 　　　　　   　　   　　　[中文](./README.zh-cn.md)

Unofficial csharp(dotnet core) implementation of [nacos](https://nacos.io/) OpenAPI.

![](https://img.shields.io/nuget/v/nacos-sdk-csharp-unofficial.svg)

![](./media/prj.png)

## CI Build Status

| Platform | Build Server | Master Status  |
|--------- |------------- |---------|
| Travis   | Linux/OSX | [![Build Status](https://travis-ci.com/catcherwong/nacos-sdk-csharp.svg?branch=master)](https://travis-ci.com/catcherwong/nacos-sdk-csharp) |

## Installation

```bash
dotnet add package nacos-sdk-csharp-unofficial
```

## Usages

### Dependency Injection(DI)

```cs
public class Startup
{
    //...
    
    public void ConfigureServices(IServiceCollection services)
    {
        // configuration
        services.AddNacos(configure =>
        {
            // default timeout
            configure.DefaultTimeOut = 8;
            // nacos's server addresses
            configure.ServerAddresses = new List<string> { "localhost:8848", };
            // namespace
            configure.Namespace = "";
            // listen interval
            configure.ListenInterval = 1000;
        });   

        //// or read from configuration file
        //services.AddNacos(Configuration);
    }    
}
```

Sample of configuration file

```JSON
{
    "nacos": {        
        "ServerAddresses": [ "localhost:8848" ],
        "DefaultTimeOut": 15,
        "Namespace": "",
        "ListenInterval": 1000,
    }
}
```

### Configuration Management

```cs
// Get the nacos config client via DI
var _client = IServiceProvider.GetService<INacosConfigClient>();

// Get configurations
var getConfigResult = await _client.GetConfigAsync(new GetConfigRequest
{
    DataId = "dataId",
    Group = "DEFAULT_GROUP",
    //Tenant = "tenant"
});

// Publish configuration
var publishConfigRessult = await _client.PublishConfigAsync(new PublishConfigRequest
{
    DataId = "dataId",
    Group = "DEFAULT_GROUP",
    //Tenant = "tenant",
    Content = "test"
});

// Delete configuration
var removeConfigResult = await _client.RemoveConfigAsync(new RemoveConfigRequest
{
    DataId = "dataId",
    Group = "DEFAULT_GROUP",
    //Tenant = "tenant"
});

// Add listener
await _configClient.AddListenerAsync(new AddListenerRequest
{
    DataId = "dataId",
    //Group = "DEFAULT_GROUP",
    //Tenant = "tenant",
    Callbacks = new List<Action<string>>
    {
        x =>{ Console.WriteLine(x); },
    }
});

// Remove listener
await _configClient.RemoveListenerAsync(new RemoveListenerRequest
{
    DataId = "dataId",
    Callbacks = new List<Action>
    {
        () =>{ Console.WriteLine("removed listener"); },
    }
});
```

### Service Discovery

```cs
// Get the nacos naming client via DI
var _client = IServiceProvider.GetService<INacosNamingClient>();

// Register instance
var registerInstance = await _client.RegisterInstanceAsync(new RegisterInstanceRequest
{
    ServiceName = "testservice",
    Ip = "192.168.0.74",
    Port = 9999
});

// Deregister instance
var removeInstance = await _client.RemoveInstanceAsync(new RemoveInstanceRequest
{
    ServiceName = "testservice",
    Ip = "192.168.0.74",
    Port = 9999
});

// Modify instance
var modifyInstance = await _client.ModifyInstanceAsync(new ModifyInstanceRequest
{
    ServiceName = "testservice",
    Ip = "192.168.0.74",
    Port = 5000
});

// Query instances
var listInstances = await _client.ListInstancesAsync(new ListInstancesRequest
{
    ServiceName = "testservice",
});
   
// Query instance detail
var getInstance = await _client.GetInstanceAsync(new GetInstanceRequest
{
    ServiceName = "testservice",
    Ip = "192.168.0.74",
    Port = 9999,                 
});

// Send instance beat
var sendHeartbeat = await _client.SendHeartbeatAsync(new SendHeartbeatRequest
{
    ServiceName = "testservice",
    BeatInfo = new BeatInfo
    {
        ServiceName = "testservice",
        Ip = "192.168.0.74",
        Port = 9999,                     
    }
});
    
// Create service
var createService = await _client.CreateServiceAsync(new CreateServiceRequest
{
    ServiceName = "testservice"
});

// Delete service
var removeService = await _client.RemoveServiceAsync(new RemoveServiceRequest
{
    ServiceName = "testservice"
});

// Update service
var modifyService = await _client.ModifyServiceAsync(new ModifyServiceRequest
{
    ServiceName = "testservice",
    ProtectThreshold = 0.5,
});

// Query service
var getService = await _client.GetServiceAsync(new GetServiceRequest
{
    ServiceName = "testservice",
});

// Query service list
var listServices = await _client.ListServicesAsync(new ListServicesRequest
{
    PageNo = 1,
    PageSize = 2,
});

// Query system switches
var getSwitches = await _client.GetSwitchesAsync();

// Update system switch
var modifySwitches = await _client.ModifySwitchesAsync(new ModifySwitchesRequest
{
    Debug = true,
    Entry = "test",
    Value = "test"
});

// Query system metrics
var getMetricsres = await _client.GetMetricsAsync();

// Query server list
var listClusterServers = await _client.ListClusterServersAsync(new ListClusterServersRequest
{
        
});

// Query the leader of current cluster
var getCurrentClusterLeader = await _client.GetCurrentClusterLeaderAsync();

// Update instance health status
var modifyInstanceHealthStatus = await _client.ModifyInstanceHealthStatusAsync(new ModifyInstanceHealthStatusRequest
{
    Ip = "192.168.0.74",
    Port = 9999,
    ServiceName = "testservice",
    Healthy = false,
});
```

