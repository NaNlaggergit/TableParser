using NUnit.Framework;
using System;
using System.Text;

namespace TableParser;

[TestFixture]
public class QuotedFieldTaskTests
{
	[TestCase("''", 0, "", 2)]
	[TestCase("'a'", 0, "a", 3)]
    [TestCase("a\"b c d e\"f",1,"b c d e",9)]
	public void Test(string line, int startIndex, string expectedValue, int expectedLength)
	{
		var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
		Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
	}
    [TestCase("'Hello\\\\ \\'' 'ads'",new int[] {0,13},new string[] {"Hello\\ \'","ads"})]
    public void Test(string line, int[] startIndex, string[] expectedValue)
    {
        Console.WriteLine(line);
        for(int i = 0; i < startIndex.Length; i++)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex[i]);
            Assert.AreEqual(expectedValue[i],actualToken.Value);
        }
    }
    

	// Добавьте свои тесты
}

class QuotedFieldTask
{
    public static Token ReadQuotedField(string line, int startIndex)
    {
        StringBuilder value = new StringBuilder();
        int i = startIndex + 1;
        char startChar = line[startIndex];
        int length = 1;
        while (i < line.Length &&line[i] != startChar)
        {
            if (line[i] == '\\')
            {
                i++;
                length++;
            }
            value.Append(line[i]);
            i++;
            length++;
        }
        if (i < line.Length)
        {
            length++;
        }
        return new Token(value.ToString(), startIndex, length);
    }
}