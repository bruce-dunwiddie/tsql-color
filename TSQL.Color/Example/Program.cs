using System;
using System.Diagnostics;
using System.IO;

using TSQL.Color;

namespace Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
			string sql = null;

			using (StreamReader sqlReader = new StreamReader("./Scripts/AdventureWorks2014.dbo.uspSearchCandidateResumes.sql"))
			{
				sql = sqlReader.ReadToEnd();
			}

			string html = new SQLColorizer().GetHtml(sql);

			using (StreamWriter htmlWriter = new StreamWriter("./sql.html"))
			{
				htmlWriter.WriteLine("<html>");
				htmlWriter.WriteLine("<body>");
				htmlWriter.Write(html);
				htmlWriter.WriteLine("</body>");
				htmlWriter.WriteLine("</html>");
			}
			
			Process.Start("cmd", "/C start " + new FileInfo("./sql.html").FullName);
		}
    }
}
