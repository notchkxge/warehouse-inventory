public static class StringHelper
{
    public static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        return string.Join(" ",
            name.Trim()
                .ToLowerInvariant()  // Better for internationalization
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) // Explicit array
        );
    }
}