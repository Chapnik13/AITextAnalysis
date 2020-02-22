using System.Collections.Generic;

namespace Crawler
{
	public interface IDataFileLoader
	{
		List<string> Load(string filename);
	}
}