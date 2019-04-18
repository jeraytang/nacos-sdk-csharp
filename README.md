# nacos-sdk-csharp

非官方版的 nacos sdk

## 安装Nuget包

```bash
dotnet add package nacos-sdk-csharp-unofficial
```

## 用法

### 依赖注入

```cs
public class Startup
{
    //...
    
    public void ConfigureServices(IServiceCollection services)
    {
        // 配置
        services.AddNacos(configure =>
        {
            configure.DefaultTimeOut = 8;
            configure.EndPoint = "http://192.168.12.209:8848";
            configure.Namespace = "";
        });   

        // 或着从配置文件中读取
        services.AddNacos(Configuration);
    }    
}
```

配置文件示例

```JSON
{
    "nacos": {
        "EndPoint": "http://localhost:8848",
        "DefaultTimeOut": 15,
        "Namespace": ""
    }
}
```

### INacosClient

所有的操作都通过`INacosClient`来进行。

### 配置管理

```cs

// 获取Nacos上的配置
var getConfigResult = await _client.GetConfigAsync(new GetConfigRequest
{
    DataId = "dataId",
    Group = "DEFAULT_GROUP",
    //Tenant = "tenant"
});

// 发布 Nacos 上的配置
var publishConfigRessult = await _client.PublishConfigAsync(new PublishConfigRequest
{
    DataId = "dataId",
    Group = "DEFAULT_GROUP",
    //Tenant = "tenant",
    Content = "test"
});

// 删除 Nacos 上的配置
var removeConfigResult = await _client.RemoveConfigAsync(new RemoveConfigRequest
{
    DataId = "dataId",
    Group = "DEFAULT_GROUP",
    //Tenant = "tenant"
});

```

### 服务发现

```cs
// 注册实例
var registerInstance = await _client.RegisterInstanceAsync(new RegisterInstanceRequest
{
    ServiceName = "testservice",
    Ip = "192.168.0.74",
    Port = 9999
});

// 注销实例
var removeInstance = await _client.RemoveInstanceAsync(new RemoveInstanceRequest
{
    ServiceName = "testservice",
    Ip = "192.168.0.74",
    Port = 9999
});

// 修改实例
var modifyInstance = await _client.ModifyInstanceAsync(new ModifyInstanceRequest
{
    ServiceName = "testservice",
    Ip = "192.168.0.74",
    Port = 5000
});

// 查询实例列表
var listInstances = await _client.ListInstancesAsync(new ListInstancesRequest
{
    ServiceName = "testservice",
});
   
// 查询实例详情
var getInstance = await _client.GetInstanceAsync(new GetInstanceRequest
{
    ServiceName = "testservice",
    Ip = "192.168.0.74",
    Port = 9999,                 
});

// 发送实例心跳
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
    
// 创建服务
var createService = await _client.CreateServiceAsync(new CreateServiceRequest
{
    ServiceName = "testservice"
});

// 删除服务
var removeService = await _client.RemoveServiceAsync(new RemoveServiceRequest
{
    ServiceName = "testservice"
});

// 修改服务
var modifyService = await _client.ModifyServiceAsync(new ModifyServiceRequest
{
    ServiceName = "testservice",
    ProtectThreshold = 0.5,
});

// 查询服务
var getService = await _client.GetServiceAsync(new GetServiceRequest
{
    ServiceName = "testservice",
});

// 查询服务列表
var listServices = await _client.ListServicesAsync(new ListServicesRequest
{
    PageNo = 1,
    PageSize = 2,
});

// 查询系统开关
var getSwitches = await _client.GetSwitchesAsync();

// 修改系统开关
var modifySwitches = await _client.ModifySwitchesAsync(new ModifySwitchesRequest
{
        Debug = true,
        Entry = "test",
        Value = "test"
});

// 查看系统当前数据指标
var getMetricsres = await _client.GetMetricsAsync();

// 查看当前集群Server列表
var listClusterServers = await _client.ListClusterServersAsync(new ListClusterServersRequest
{
        
});

// 查看当前集群leader
var getCurrentClusterLeader = await _client.GetCurrentClusterLeaderAsync();

// 更新实例的健康状态
var modifyInstanceHealthStatus = await _client.ModifyInstanceHealthStatusAsync(new ModifyInstanceHealthStatusRequest
{
    Ip = "192.168.0.74",
    Port = 9999,
    ServiceName = "testservice",
    Healthy = false,
});
```

