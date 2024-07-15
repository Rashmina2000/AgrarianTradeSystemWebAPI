using AgrarianTradeSystemWebAPI.Models;
using System.IO;

namespace AgrarianTradeSystemWebAPI.Services.ProductServices
{
	public interface IFileServices
	{
		Task<string> Upload(IFormFile file, string containerName);
		Task<Stream> Get(string name, string containerName);

		Task Delete(String name, String containerName);
	}
}
