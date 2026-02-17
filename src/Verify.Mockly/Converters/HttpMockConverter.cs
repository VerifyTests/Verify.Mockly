class HttpMockConverter :
    WriteOnlyJsonConverter<HttpMock>
{
    public override void Write(VerifyJsonWriter writer, HttpMock mock) =>
        writer.Serialize(mock.Requests);
}
