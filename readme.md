# <img src="/src/icon.png" height="30px"> Verify.Mockly

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://img.shields.io/appveyor/build/SimonCropp/Verify-Mockly)](https://ci.appveyor.com/project/SimonCropp/Verify-Mockly)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Mockly.svg)](https://www.nuget.org/packages/Verify.Mockly/)

Adds [Verify](https://github.com/VerifyTests/Verify) support for verifying [Mockly](https://mockly.org/) types.<!-- singleLineInclude: intro. path: /docs/intro.include.md -->

**See [Milestones](../../milestones?state=closed) for release notes.**


## Sponsors


### Entity Framework Extensions<!-- include: zzz. path: /docs/zzz.include.md -->

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.Mockly) is a major sponsor and is proud to contribute to the development this project.

[![Entity Framework Extensions](https://raw.githubusercontent.com/VerifyTests/Verify.Mockly/refs/heads/main/docs/zzz.png)](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.Mockly)<!-- endInclude -->


## NuGet

 * https://nuget.org/packages/Verify.Mockly


## Usage

<!-- snippet: Enable -->
<a id='snippet-Enable'></a>
```cs
[ModuleInitializer]
public static void Init() =>
    VerifyMockly.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-Enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

An HttpMock and its captured requests can then be verified:

<!-- snippet: VerifyGetRequest -->
<a id='snippet-VerifyGetRequest'></a>
```cs
[Test]
public async Task VerifyGetRequest()
{
    var mock = new HttpMock();

    mock.ForGet()
        .ForHttps()
        .ForHost("api.example.com")
        .WithPath("/api/users/123")
        .RespondsWithJsonContent(new { id = 123, name = "John" });

    var client = mock.GetClient();
    await client.GetAsync("https://api.example.com/api/users/123");
    await Verify(mock);
}
```
<sup><a href='/src/Tests/Tests.cs#L4-L22' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyGetRequest' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Results in:

<!-- snippet: Tests.VerifyGetRequest.verified.txt -->
<a id='snippet-Tests.VerifyGetRequest.verified.txt'></a>
```txt
[
  {
    Method: GET,
    Scheme: https,
    Host: api.example.com,
    Path: /api/users/123,
    WasExpected: true,
    StatusCode: OK
  }
]
```
<sup><a href='/src/Tests/Tests.VerifyGetRequest.verified.txt#L1-L10' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.VerifyGetRequest.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Verifying POST Requests with Body

<!-- snippet: VerifyPostRequest -->
<a id='snippet-VerifyPostRequest'></a>
```cs
[Test]
public async Task VerifyPostRequest()
{
    var mock = new HttpMock();

    mock.ForPost()
        .ForHttps()
        .ForHost("api.example.com")
        .WithPath("/api/users")
        .RespondsWithStatus(System.Net.HttpStatusCode.Created);

    var client = mock.GetClient();
    await client.PostAsync(
        "https://api.example.com/api/users",
        new StringContent(
            """{"name":"Jane"}""",
            Encoding.UTF8,
            "application/json"));
    await Verify(mock);
}
```
<sup><a href='/src/Tests/Tests.cs#L24-L47' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyPostRequest' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Results in:

<!-- snippet: Tests.VerifyPostRequest.verified.txt -->
<a id='snippet-Tests.VerifyPostRequest.verified.txt'></a>
```txt
[
  {
    Method: POST,
    Scheme: https,
    Host: api.example.com,
    Path: /api/users,
    Body: {"name":"Jane"},
    WasExpected: true,
    StatusCode: Created
  }
]
```
<sup><a href='/src/Tests/Tests.VerifyPostRequest.verified.txt#L1-L11' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.VerifyPostRequest.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Verifying a RequestCollection

<!-- snippet: VerifyRequestCollection -->
<a id='snippet-VerifyRequestCollection'></a>
```cs
[Test]
public async Task VerifyRequestCollection()
{
    var mock = new HttpMock();
    var requests = new RequestCollection();

    mock.ForGet()
        .ForHttps()
        .ForHost("api.example.com")
        .WithPath("/api/users/*")
        .CollectingRequestsIn(requests)
        .RespondsWithJsonContent(new { id = 1, name = "John" });

    var client = mock.GetClient();
    await client.GetAsync("https://api.example.com/api/users/1");
    await client.GetAsync("https://api.example.com/api/users/2");
    await Verify(requests);
}
```
<sup><a href='/src/Tests/Tests.cs#L49-L70' title='Snippet source file'>snippet source</a> | <a href='#snippet-VerifyRequestCollection' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Results in:

<!-- snippet: Tests.VerifyRequestCollection.verified.txt -->
<a id='snippet-Tests.VerifyRequestCollection.verified.txt'></a>
```txt
[
  {
    Method: GET,
    Scheme: https,
    Host: api.example.com,
    Path: /api/users/1,
    WasExpected: true,
    StatusCode: OK
  },
  {
    Method: GET,
    Scheme: https,
    Host: api.example.com,
    Path: /api/users/2,
    WasExpected: true,
    StatusCode: OK
  }
]
```
<sup><a href='/src/Tests/Tests.VerifyRequestCollection.verified.txt#L1-L18' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.VerifyRequestCollection.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Scrubbing Members

Members can be scrubbed by name:

<!-- snippet: ScrubBody -->
<a id='snippet-ScrubBody'></a>
```cs
[Test]
public async Task ScrubBody()
{
    var mock = new HttpMock();

    mock.ForPost()
        .ForHttps()
        .ForHost("api.example.com")
        .WithPath("/api/users")
        .RespondsWithStatus(System.Net.HttpStatusCode.Created);

    var client = mock.GetClient();
    await client.PostAsync(
        "https://api.example.com/api/users",
        new StringContent(
            """{"name":"Jane"}""",
            Encoding.UTF8,
            "application/json"));
    await Verify(mock)
        .ScrubMember("Body");
}
```
<sup><a href='/src/Tests/Tests.cs#L72-L96' title='Snippet source file'>snippet source</a> | <a href='#snippet-ScrubBody' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Results in:

<!-- snippet: Tests.ScrubBody.verified.txt -->
<a id='snippet-Tests.ScrubBody.verified.txt'></a>
```txt
[
  {
    Method: POST,
    Scheme: https,
    Host: api.example.com,
    Path: /api/users,
    Body: Scrubbed,
    WasExpected: true,
    StatusCode: Created
  }
]
```
<sup><a href='/src/Tests/Tests.ScrubBody.verified.txt#L1-L11' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.ScrubBody.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->
