using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TableParser;

[TestFixture]
public class FieldParserTaskTests
{
	public static void Test(string input, string[] expectedResult)
	{
		var actualResult = FieldsParserTask.ParseLine(input);
		Assert.AreEqual(expectedResult.Length, actualResult.Count);
		for (int i = 0; i < expectedResult.Length; ++i)
		{
			Assert.AreEqual(expectedResult[i], actualResult[i].Value);
		}
	}
    [TestCase("a\"b c d e\"f", new[] {"a","b c d e","f"})]
    [TestCase("text", new[] { "text" })]
    [TestCase("hello world", new[] { "hello", "world" })]
    [TestCase("", new string[] { })]
    [TestCase("0xDEADBEEF", new[] { "0xDEADBEEF" })]
    [TestCase("''", new[] { "" })]
    [TestCase("i like writing tests", new[] { "i", "like", "writing", "tests" })]
    [TestCase("separate us plz", new[] { "separate", "us", "plz" })]
    [TestCase("what  about    this?", new[] { "what", "about", "this?" })]
    [TestCase("cut 'it'", new[] { "cut", "it" })]
    [TestCase("\"cut\" it", new[] { "cut", "it" })]
    [TestCase("' '", new[] { " " })]
    [TestCase("\"'smile plz :)'\"", new[] { "'smile plz :)'" })]
    [TestCase("'\"another  one :)\"'", new[] { "\"another  one :)\"" })]
    [TestCase("\"jeeeeez ", new[] { "jeeeeez " })]
    [TestCase(" <-ignore damn spaces->   ", new[] { "<-ignore", "damn", "spaces->" })]
    [TestCase("i'm'too'lazy'to'put'spaces'sorry'",
        new[] { "i", "m", "too", "lazy", "to", "put", "spaces", "sorry" })]
    [TestCase("\"\\\\\"", new[] { "\\" })]
    [TestCase("\'\\\'MAGIC\\\'\'", new[] { "\'MAGIC\'" })]
    [TestCase("\"\\\"MAGIC\\\"\"", new[] { "\"MAGIC\"" })]
    // Вставляйте сюда свои тесты
    public static void RunTests(string input, string[] expectedOutput)
    {
        Test(input, expectedOutput);
    }
    // Скопируйте сюда метод с тестами из предыдущей задачи.
}

public class FieldsParserTask
{
	// При решении этой задаче постарайтесь избежать создания методов, длиннее 10 строк.
	// Подумайте как можно использовать ReadQuotedField и Token в этой задаче.
	public static List<Token> ParseLine(string line)
	{
        List<Token> tokens = new List<Token>();
        int i = 0;
        while(i < line.Length)
        {
            if (line[i] == ' ')
            {
                i++;
                continue;
            }
            if (line[i] == '\'' || line[i] == '\"')
            {
                tokens.Add(ReadQuotedField(line, i));
                i = i + tokens.Last().Length;
            }
            else
            {
                tokens.Add(ReadField(line, i));
                i=i+tokens.Last().Length;
            }

        }
        return tokens; // сокращенный синтаксис для инициализации коллекции.
	}
        
	private static Token ReadField(string line, int startIndex)
	{
        int i = startIndex;
        StringBuilder word=new StringBuilder();
        while ((i < line.Length)&&(line[i] != '\'')&& (line[i] != '\"') && (line[i]!=' '))
        {
            word.Append(line[i]);
            i++;
        }
		return new Token(word.ToString(), startIndex, word.Length);
	}
    public static Token ReadQuotedField(string line, int startIndex)
	{
        return QuotedFieldTask.ReadQuotedField(line, startIndex); 
	}
}