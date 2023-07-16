namespace AliexpressOpenPlatformAPI
{
    public static class AliExpressDefaults
    {
        public static string AppKey { get; } = Environment.GetEnvironmentVariable("APP_KEY");
        public static string AppSecret { get; } = Environment.GetEnvironmentVariable("APP_SECRET");
        public static string AliApiURL { get; } = Environment.GetEnvironmentVariable("ALI_API_URL");
    }
}
