using System.Text.RegularExpressions;

namespace ConfigOps.Core.Persistence
{
    internal static class KeyValidator
    {
        // A key is comprised of multiple key segments separated by a forward slash.
        // Each key segment can contain only letters, digits, underscores, and periods.
        // A key cannot start or end with a forward slash.
        // A key cannot start or end with a period.
        // A key cannot start or end with a underscore.
        // A key cannot contain consecutive forward slashes.
        // A key cannot contain consecutive periods.
        // A key cannot contain consecutive underscores.
        private readonly static Regex keySegmentValidator = new Regex(@"^[a-zA-Z0-9]{1,}[a-zA-Z0-9_\.-]{0,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsValid(string key)
        {
            var cleanKey = key.Replace(" ", "");

            var isBasicValidationsInvalid = string.IsNullOrWhiteSpace(cleanKey) ||
                cleanKey.StartsWith('/') ||
                cleanKey.EndsWith('/') ||
                cleanKey.Contains("//") ||
                cleanKey.Contains("__") ||
                cleanKey.Contains("--") ||
                cleanKey.Contains("..");

            if (isBasicValidationsInvalid)
            {
                return false;
            }

            var keySegments = cleanKey.Split('/');

            foreach (var keySegment in keySegments)
            {
                if (keySegment.StartsWith('.') ||
                    keySegment.EndsWith('.') ||
                    keySegment.StartsWith('_') ||
                    keySegment.EndsWith('_') ||
                    keySegment.StartsWith('-') ||
                    keySegment.EndsWith('-') ||
                    !keySegmentValidator.IsMatch(keySegment))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
