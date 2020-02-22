using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crawler
{
	public class DataFileLoader : IDataFileLoader
	{
		public List<string> Load(string filename)
		{
			var filelines = File.ReadAllLines(filename);

			return filelines.Select(line => line.ToLower()).ToList();
		}
	}
}
