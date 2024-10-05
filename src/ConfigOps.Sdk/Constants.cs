namespace ConfigOps.Sdk
{
    public class Constants
    {
        public const string CRD_GROUP = "config.kuipersys.com";
        public const string CRD_VERSION = "v1alpha1";
        public const string CRD_API_VERSION = $"{CRD_GROUP}/{CRD_VERSION}";

        /// <summary>
        /// If you need to set this, use the following format: ", PublicKey=KeyValue"
        /// </summary>
        public const string PUBLIC_KEY = "";
    }
}
