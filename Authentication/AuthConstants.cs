namespace ApiRestRs.Authentication
{
    public class AuthConstants
    {
        // Esta sección se encuentra en el appsettings.json
        // Ahi estaria la clave con la cual se comparara la clave que se envia en el header
        public const string ApiKeySectionName = "Authentication:ApiKey";
        public const string ApiKeyHeader = "X-Api-Key";
    }
}
