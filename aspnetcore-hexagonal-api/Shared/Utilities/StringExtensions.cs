using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace aspnetcore_hexagonal_api.Shared.Utilities;

public static class StringExtensions
{
    public static string ToTitleCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(input.ToLower());
    }

    public static string RemoveAccents(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var normalizedString = input.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string ToSlug(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var slug = input.RemoveAccents().ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", " ").Trim();
        slug = Regex.Replace(slug, @"\s", "-");

        return slug;
    }

    public static string Truncate(this string input, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length <= maxLength)
            return input ?? string.Empty;

        return input.Substring(0, maxLength - suffix.Length) + suffix;
    }

    public static bool IsNullOrWhiteSpace(this string? input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    public static string OnlyNumbers(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return Regex.Replace(input, @"[^\d]", "");
    }

    public static string FormatCpf(this string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return string.Empty;

        cpf = cpf.OnlyNumbers();

        if (cpf.Length != 11)
            return cpf;

        return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
    }

    public static string FormatCnpj(this string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return string.Empty;

        cnpj = cnpj.OnlyNumbers();

        if (cnpj.Length != 14)
            return cnpj;

        return $"{cnpj.Substring(0, 2)}.{cnpj.Substring(2, 3)}.{cnpj.Substring(5, 3)}/{cnpj.Substring(8, 4)}-{cnpj.Substring(12, 2)}";
    }

    public static string FormatPhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        phoneNumber = phoneNumber.OnlyNumbers();

        return phoneNumber.Length switch
        {
            10 => $"({phoneNumber.Substring(0, 2)}) {phoneNumber.Substring(2, 4)}-{phoneNumber.Substring(6, 4)}",
            11 => $"({phoneNumber.Substring(0, 2)}) {phoneNumber.Substring(2, 5)}-{phoneNumber.Substring(7, 4)}",
            _ => phoneNumber
        };
    }
}