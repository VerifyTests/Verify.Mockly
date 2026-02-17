class RequestCollectionConverter :
    WriteOnlyJsonConverter<RequestCollection>
{
    public override void Write(VerifyJsonWriter writer, RequestCollection collection)
    {
        writer.WriteStartArray();
        foreach (var request in collection)
        {
            writer.Serialize(request);
        }

        writer.WriteEndArray();
    }
}
