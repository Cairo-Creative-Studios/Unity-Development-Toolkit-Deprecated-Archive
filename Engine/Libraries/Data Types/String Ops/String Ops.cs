using System;
public static class String_Ops
{
    /// <summary>
    /// Gets the Token at the specified Index in a string
    /// </summary>
    /// <returns>The <see cref="T:System.String"/>.</returns>
    /// <param name="checkedString">The string we're searching for a Token.</param>
    /// <param name="index">The Index to find the token at.</param>
    /// <param name="seperater">What character Seperated Tokens in the String?.</param>
    public static string TokenAt(this string checkedString, int index, char seperater)
    {
        char[] characters = checkedString.ToCharArray();

        int token = 0;
        string returnValue = "";
        foreach(char character in characters)
        {
            if(token == index)
            {
                if (character == seperater)
                    break;
                else
                    returnValue += character;
            }
            else
            {
                if (character == seperater)
                    token++;
            }
        }

        return returnValue;
    }
}
