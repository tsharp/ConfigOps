namespace ConfigOps.Sdk.Dto
{
    public abstract class ConfigObject<TConfigData, TConfigMetadata> : SystemObjectBase<TConfigMetadata>
        where TConfigData : ConfigObjectData, new()
        where TConfigMetadata : ConfigObjectMetadata, new()
    {
        public TConfigData Data { get; set; } = new TConfigData();
    }
}
