using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigOps.Core.Persistence
{
    public class FileSystemKeyValueStore : IKeyValueStore
    {
        private readonly string filePath;

        public static IKeyValueStore Create(string filePath)
        {
            return new FileSystemKeyValueStore(filePath);
        }

        private FileSystemKeyValueStore(string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            this.filePath = filePath;
        }

        public Task<byte[]> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            var path = ValidateAndGetFileSystemKey(key);

            if (File.Exists(path))
            {
                return File.ReadAllBytesAsync(path, cancellationToken);
            }

            throw new KeyNotFoundException($"Key not found: {key}");
        }

        public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            var path = ValidateAndGetFileSystemKey(key);
            var directory = Path.GetDirectoryName(path);

            if (File.Exists(path))
            {
                File.Delete(path);
                return Task.FromResult(true);
            }
            else if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task SetAsync(string key, byte[] value, CancellationToken cancellationToken = default)
        {
            var path = ValidateAndGetFileSystemKey(key, true);

            var directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            return File.WriteAllBytesAsync(path, value, cancellationToken);
        }

        private string ValidateAndGetFileSystemKey(string key, bool forUpdate = false)
        {
            if (!KeyValidator.IsValid(key))
            {
                throw new InvalidDataException($"Invalid key: {key}");
            }

            // If part of a key already exists as a file, it cannot be used as a key because the directory structure required would conflict with the file.
            // Enumerate all parts of the key and check if they exist as files.
            var parts = key.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var cleanPath = Path.Combine(filePath, key.Replace('/', Path.DirectorySeparatorChar));

            if (!forUpdate)
            {
                return cleanPath;
            }

            for (int i = 0; i < parts.Length; i++)
            {
                var keyPath = Path.Combine(parts.Take(i + 1).ToArray());
                // Replace environment path separator with /
                var keyPathId = keyPath.Replace(Path.DirectorySeparatorChar, '/');
                var path = Path.Combine(filePath, keyPath);

                if (File.Exists(path) && !key.Equals(keyPathId))
                {
                    throw new InvalidDataException($"Invalid key: {key}");
                }
            }

            return cleanPath;
        }

        private string ConvertPathToKey(string path)
            => path
                .Replace(filePath, string.Empty)
                .Replace('\\', '/')
                .TrimStart('/')
                .TrimEnd('/');

        public async Task<IEnumerable<string>> ScanAsync(string prefix, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            string filePathPrefix = string.IsNullOrWhiteSpace(prefix) ? filePath : ValidateAndGetFileSystemKey(prefix);
            var files = Directory.EnumerateFiles(filePathPrefix, "*", SearchOption.AllDirectories);
            return files.Select(ConvertPathToKey).ToArray();
        }
    }
}
