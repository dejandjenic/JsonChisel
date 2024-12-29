using System.Net;
using System.Text.Json;
using FluentAssertions;
using JsonChisel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JsonChiselTests;

public class MiddlewareTests
{
    private IHost host;

    public MiddlewareTests()
    {
        host = new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.ConfigureHttpJsonOptions(o => { o.SerializerOptions.WriteIndented = true; });
                        services.AddRouting();
                    })
                    .Configure(app =>
                    {
                        app.UseRouting();
                        app.UseJsonChisel();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapGet("/users", () =>
                                Results.Ok(
                                    JsonSerializer.Deserialize(
                                        File.OpenRead("users.json"),
                                        typeof(JsonElement))
                                ));
                            endpoints.MapGet("/companies", () =>
                                Results.Ok(
                                    JsonSerializer.Deserialize(
                                        File.OpenRead("companies.json"),
                                        typeof(JsonElement))
                                ));
                        });
                    });
            })
            .Start();
    }

    [InlineData("../../../json/users-full.json","test:some","/users")]
    [InlineData("../../../json/users-case1.json","fields:users.id","/users")]
    [InlineData("../../../json/users-case2.json","fields:users.id,users.firstName,users.lastName,users.address.state","/users")]
    [InlineData("../../../json/users-case3.json","fields:users.id,users.phoneNumbers,users.active","/users")]
    [InlineData("../../../json/users-case4.json","fields:users.id,users.favoriteColors,users.orders.orderId,users.orders.status","/users")]
    [InlineData("../../../json/users-case5.json","fields:users.phoneNumbers","/users")]
    [InlineData("../../../json/companies-full.json","test:some","/companies")]
    [InlineData("../../../json/companies-case1.json","fields:company.name,company.address.city","/companies")]
    [InlineData("../../../json/companies-case2.json","fields:company.name,company.employees.id,company.employees.projects","/companies")]
    [InlineData("../../../json/companies-case3.json","fields:company.name,company.employees.lastName,company.employees.firstName","/companies")]
    [Theory]
    async Task Test(string file,string headers,string route)
    {
        var client = host.GetTestClient();
        var dict = headers.Split("|").ToDictionary(x=>x.Split(":")[0],x=>x.Split(":")[1]);
        var result = await SendWithHeaders(client,route, dict);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await result.Content.ReadAsStringAsync();
        File.ReadAllText(file).Should().Be(content);
    }
    
    async Task<HttpResponseMessage> SendWithHeaders(HttpClient client,string url, Dictionary<string, string> headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }
        return await client.SendAsync(request);
    }
}