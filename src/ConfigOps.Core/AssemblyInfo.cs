using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo($"cfgctl{ConfigOps.Sdk.Constants.PUBLIC_KEY}")]
[assembly: InternalsVisibleTo($"ConfigOps.Sdk.UnitTests{ConfigOps.Sdk.Constants.PUBLIC_KEY}")]
[assembly: InternalsVisibleTo($"ConfigOps.Sdk{ConfigOps.Sdk.Constants.PUBLIC_KEY}")]