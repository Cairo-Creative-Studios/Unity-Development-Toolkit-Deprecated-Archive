//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;

public static class String_Ops
{
    /// <summary>
    /// Gets the Token at the specified Index in a string
    /// </summary>
    /// <returns>The <see cref="T:System.String"/>.</returns>
    /// <param name="checkedString">The string we're searching for a Token.</param>
    /// <param name="index">The Index to find the token at.</param>
    /// <param name="seperater">What character Seperated Tokens in the String?.</param>
    public static string TokenAt(this string checkedString, int index, char itemDelimiter = ',')
    {
        char[] characters = checkedString.ToCharArray();

        int token = 0;
        string returnValue = "";

        foreach(char character in characters)
        {
            if(token == index)
            {
                if (character == itemDelimiter)
                    break;
                else
                    returnValue += character;
            }
            else
            {
                if (character == itemDelimiter)
                    token++;
            }
        }

        return returnValue;
    }

    /// <summary>
    /// Counts all tokens seperated by the Delimiter
    /// </summary>
    /// <returns>The count.</returns>
    /// <param name="checkedString">Checked string.</param>
    /// <param name="seperater">Seperater.</param>
    public static int TokenCount(this string checkedString, char itemDelimiter = ',')
    {
        char[] characters = checkedString.ToCharArray();

        int token = 0;

        foreach (char character in characters)
        {
            if (character == itemDelimiter)
                token++;
        }

        return token;
    }

    /// <summary>
    /// Gets an Array of Tokens from the given String
    /// </summary>
    /// <returns>The array.</returns>
    /// <param name="checkedString">Checked string.</param>
    /// <param name="seperater">Seperater.</param>
    public static string[] TokenArray(this string listString, char itemDelimiter = ',')
    {
        List<string> tokens = new List<string>();

        for(int i = 0; i < TokenCount(listString, itemDelimiter); i++)
        {
            tokens.Add(listString.TokenAt(i, itemDelimiter));
        }

        return tokens.ToArray();
    }

    /// <summary>
    /// Replaces the Token at the Index with the passed Value
    /// </summary>
    /// <param name="listString">List string.</param>
    /// <param name="itemDelimiter">Item delimiter.</param>
    public static string SetToken(this string listString, int index, string value, char itemDelimiter = ',')
    {
        string[] ogTokenArray = listString.TokenArray();
        string returnedString = "";

        //Create a new List by replacing the old Token with the new Value
        int i = 0;
        foreach(string token in ogTokenArray)
        {
            if (i == index)
                returnedString = returnedString + value;
            else
                returnedString = listString.TokenAt(index, itemDelimiter);

            if (i != ogTokenArray.Length - 1)
                returnedString = returnedString + ",";

            i++;
        }

        return returnedString;
    }

    /// <summary>
    /// Parses a string as Dictionary Items
    /// </summary>
    /// <returns>The to dictionary.</returns>
    /// <param name="parsedString">Parsed string.</param>
    /// <param name="itemDelimiter">Item delimiter.</param>
    /// <param name="pairDelimiter">Pair delimiter.</param>
    public static SDictionary<string, string> ParseToDictionary(this string parsedString, char itemDelimiter = ',', char pairDelimiter = ':')
    {
        SDictionary<string, string> result = new SDictionary<string, string>();

        for (int i = 0; i < parsedString.TokenCount(itemDelimiter); i++)
        {
            result.Add(parsedString.TokenAt(i, itemDelimiter).TokenAt(0, pairDelimiter), parsedString.TokenAt(i, itemDelimiter).TokenAt(1, pairDelimiter));
        }

        return result;
    }

    /// <summary>
    /// Parses a string as a list of strings
    /// </summary>
    /// <returns>The to list.</returns>
    /// <param name="parsedString">Parsed string.</param>
    /// <param name="itemDelimiter">Item delimiter.</param>
    public static List<string> ParseToList(this string parsedString, char itemDelimiter = ',')
    {
        List<string> result = new List<string>();

        for (int i = 0; i < parsedString.TokenCount(itemDelimiter); i++)
        {
            result.Add(parsedString.TokenAt(i, itemDelimiter));
        }

        return result;
    }
}
