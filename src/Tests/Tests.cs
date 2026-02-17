[TestFixture]
public class Tests
{
    #region VerifyGetRequest

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

    #endregion

    #region VerifyPostRequest

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

    #endregion

    #region VerifyRequestCollection

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

    #endregion

    #region ScrubBody

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

    #endregion
}
