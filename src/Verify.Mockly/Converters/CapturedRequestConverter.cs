class CapturedRequestConverter :
    WriteOnlyJsonConverter<CapturedRequest>
{
    public override void Write(VerifyJsonWriter writer, CapturedRequest request)
    {
        writer.WriteStartObject();
        writer.WriteMember(request, request.Method.Method, "Method");
        writer.WriteMember(request, request.Scheme, "Scheme");
        writer.WriteMember(request, request.Host, "Host");
        writer.WriteMember(request, request.Path, "Path");

        if (!string.IsNullOrEmpty(request.Query))
        {
            writer.WriteMember(request, request.Query, "Query");
        }

        if (request.Body is not null)
        {
            writer.WriteMember(request, request.Body, "Body");
        }

        writer.WriteMember(request, request.WasExpected, "WasExpected");
        writer.WriteMember(request, request.Response.StatusCode, "StatusCode");
        writer.WriteEndObject();
    }
}
