namespace ConfigOps.Sdk.Dto
{
    public abstract class ConfigObject<TConfigSpec> : SystemObject
        where TConfigSpec : ConfigObjectSpec, new()
    {
        public TConfigSpec Spec {  get; set; } = new TConfigSpec();
    }
}
