using System;
using System.Drawing;
using System.Linq;
using System.Web;

using TSQL.Tokens;

namespace TSQL.Color
{
	public class SQLColorizer
	{
		public string GetHtml(string sqlText)
		{
			string html = "";

			foreach (TSQLToken token in new TSQLTokenizer(sqlText)
			{
				IncludeWhitespace = true
			})
			{
				string escapedText = GetHtmlEscapedText(token.Text);

				string formattedText = GetFormattedText(escapedText);

				if (token.Type != TSQLTokenType.Whitespace)
				{
					System.Drawing.Color color = GetColorForToken(token);

					html += GetHtmlWithColor(
						formattedText,
						color);
				}
				else
				{
					html += formattedText;
                }
			}

			return
				"<span style=\"font-family:consolas;\">\r\n" +
                html +
				"</span>\r\n";
		}

		public System.Drawing.Color GetColorForToken(
			TSQLToken token)
		{
			if (token.Type == TSQLTokenType.Keyword)
			{
				if (token.AsKeyword.Keyword.In(
					TSQLKeywords.NULL,
					TSQLKeywords.AND,
					TSQLKeywords.OR,
					TSQLKeywords.NOT,
					TSQLKeywords.LEFT,
					TSQLKeywords.RIGHT,
					TSQLKeywords.INNER,
					TSQLKeywords.OUTER,
					TSQLKeywords.JOIN,
					TSQLKeywords.ALL,
					TSQLKeywords.ANY,
					TSQLKeywords.SOME,
					TSQLKeywords.BETWEEN,
					TSQLKeywords.CROSS,
					TSQLKeywords.EXISTS,
					TSQLKeywords.IN,
					TSQLKeywords.IS,
					TSQLKeywords.LIKE,
					TSQLKeywords.PIVOT,
					TSQLKeywords.UNPIVOT
					))
				{
					return System.Drawing.Color.Gray;
				}

				if (token.AsKeyword.Keyword.In(
					TSQLKeywords.UPDATE,
					TSQLKeywords.COLLATE
					))
				{
					return System.Drawing.Color.Fuchsia;
				}

				return System.Drawing.Color.Blue;
			}

			if (
				token.Type == TSQLTokenType.Character ||
				token.Type == TSQLTokenType.Operator)
			{
				return System.Drawing.Color.Gray;
			}

			if (
				token.Type == TSQLTokenType.SingleLineComment ||
				token.Type == TSQLTokenType.MultilineComment)
			{
				return System.Drawing.Color.Green;
			}

			if (token.Type == TSQLTokenType.StringLiteral)
			{
				return System.Drawing.Color.Red;
			}

			if (token.Type == TSQLTokenType.SystemIdentifier)
			{
				return System.Drawing.Color.Fuchsia;
			}

			if (token.Type == TSQLTokenType.SystemVariable)
			{
				return System.Drawing.Color.Fuchsia;
			}

			// https://docs.microsoft.com/en-us/sql/t-sql/functions/functions
			if (token.Type == TSQLTokenType.Identifier &&
				new string[]
				{
					"serverproperty"
				}.Contains(
					token.AsIdentifier.Name,
					StringComparer.CurrentCultureIgnoreCase))
			{
				return System.Drawing.Color.Fuchsia;
			}

			// https://docs.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql
			if (token.Type == TSQLTokenType.Identifier &&
				!token.Text.StartsWith("[") &&
				new string[]
				{
					"bigint",
					"numeric",
					"bit",
					"smallint",
					"decimal",
					"smallmoney",
					"int",
					"tinyint",
					"money",
					"float",
					"real",
					"date",
					"datetimeoffset",
					"datetime2",
					"smalldatetime",
					"datetime",
					"time",
					"char",
					"varchar",
					"text",
					"nchar",
					"nvarchar",
					"ntext",
					"binary",
					"varbinary",
					"image",
					"cursor",
					"timestamp",
					"hierarchyid",
					"uniqueidentifier",
					"sql_variant",
					"xml",
					"table",
					"geography",
					"geometry"
				}.Contains(
					token.AsIdentifier.Name,
					StringComparer.CurrentCultureIgnoreCase))
			{
				return System.Drawing.Color.Blue;
			}

			if (token.Type == TSQLTokenType.Identifier &&
				new string[]
				{
					"ANSI_NULLS",
					"QUOTED_IDENTIFIER",
					"CALLER",
					"NOCOUNT"
				}.Contains(
					token.AsIdentifier.Name,
					StringComparer.CurrentCultureIgnoreCase))
			{
				return System.Drawing.Color.Blue;
			}

			return System.Drawing.Color.Black;
		}

		private static string GetHtmlEscapedText(
			string tokenText)
		{
			return HttpUtility.HtmlEncode(tokenText);
		}

		private static string GetFormattedText(
			string htmlText)
		{
			return htmlText
				.Replace("\r\n", "<br/>")
				.Replace("\r", "<br/>")
				.Replace("\n", "<br/>")
				.Replace("<br/>", "<br/>\r\n")
				.Replace("\t", "&emsp;")
				.Replace(" ", "&nbsp;");
		}

		private static string GetHtmlWithColor(
			string html,
			System.Drawing.Color color)
		{
			
			return 
				"<span style=\"color: " +
				color.Name +
				";\">" + 
				html +
				"</span>";
		}
	}
}
