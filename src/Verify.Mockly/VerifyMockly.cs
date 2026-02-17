namespace VerifyTests;

public static class VerifyMockly
{
    public static bool Initialized { get; private set; }

    static HttpMockConverter httpMockConverter = new();
    static CapturedRequestConverter capturedRequestConverter = new();
    static RequestCollectionConverter collectionConverter = new();

    public static void Initialize()
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();
        VerifierSettings
            .AddExtraSettings(settings =>
            {
                var converters = settings.Converters;
                converters.Add(httpMockConverter);
                converters.Add(capturedRequestConverter);
                converters.Add(collectionConverter);
            });
    }
}
