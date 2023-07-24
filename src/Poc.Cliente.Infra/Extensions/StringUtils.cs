using System;

namespace Poc.Cliente.Infra.Extensions;

public static class StringUtils
{
    public static string PrimeiraLetraMinuscula(this string palavra)
    {
        if (string.IsNullOrWhiteSpace(palavra))
            return string.Empty;

        var result =
            palavra.Length == 1
                ? palavra.ToLower()
                : char.ToLower(palavra[0]) + palavra[1..];

        return result;
    }

    public static string TranformarEmSnakeCase(this string palavra)
    {
        var palavraParaConverter = palavra.PrimeiraLetraMinuscula();
        if (string.IsNullOrEmpty(palavraParaConverter))
            return string.Empty;

        var result = string.Empty;
        foreach (char letra in palavraParaConverter)
            result += char.IsUpper(letra) ? $"_{char.ToLower(letra)}" : letra;

        return result;
    }

    public static string ObterNoMaximo(this string str, int numeroDeCaracteres)
    => numeroDeCaracteres > str.Length
        ? str
        : str.Substring(0, numeroDeCaracteres);


    public static Guid ToGuid(string strGuid)
    {
        return Guid.Parse(strGuid);
    }
}
