using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo($"cfgctl{ConfigOps.Sdk.Constants.PUBLIC_KEY}")]
[assembly: InternalsVisibleTo($"ConfigOps.Sdk.UnitTests{ConfigOps.Sdk.Constants.PUBLIC_KEY}")]
[assembly: InternalsVisibleTo($"ConfigOps.Core{ConfigOps.Sdk.Constants.PUBLIC_KEY}")]