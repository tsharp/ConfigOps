using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigOps.Core.Persistence
{
    internal interface IKeyValueStore
    {
        public Task SetAsync(string key, byte[] value, CancellationToken cancellationToken = default);

        public Task<byte[]> GetAsync(string key, CancellationToken cancellationToken = default);

        public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default);

        public Task<IEnumerable<string>> ScanAsync(string prefix, CancellationToken cancellationToken = default);
    }
}
