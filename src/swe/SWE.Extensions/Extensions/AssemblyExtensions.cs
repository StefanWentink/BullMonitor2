using System.Reflection;

namespace SWE.Extensions.Extensions
{
    public static class AssemblyExtensions
    {
        public static string? GetInformationalVersion(
            this Assembly? assembly)
        {
            if (assembly == null)
            {
                return null;
            }

            try
            {
                var attribute = assembly
                    .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute))
                    .FirstOrDefault() as AssemblyInformationalVersionAttribute;

                return attribute?.InformationalVersion
                    ?? $"{assembly.FullName} {nameof(AssemblyInformationalVersionAttribute)} not found.";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public static string? GetGitHashFromInformationalVersion(
            this Assembly assembly)
        {
            var version = assembly.GetInformationalVersion();

            if (!string.IsNullOrWhiteSpace(version)
                && version.Length > 6)
            {
                return version[(version.IndexOf('+') + 1)..];
            }

            return null;
        }
    }
}