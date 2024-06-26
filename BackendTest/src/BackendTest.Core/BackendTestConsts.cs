using BackendTest.Debugging;

namespace BackendTest
{
    public class BackendTestConsts
    {
        public const string LocalizationSourceName = "BackendTest";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "bc4d8f8d5510479fb36d133363d8308f";
    }
}
