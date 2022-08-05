using System.IO;
using System.Text;
using System.Threading.Tasks;

using IronSphere.Extensions;

namespace Jaxofy.Tools.DtosToTypescript;

public static class AsyncFile
{
    public static async Task WriteTextAsync(string filePath, string text, FileMode fileMode = FileMode.Create)
    {
        byte[] encodedText = text.GetBytes(Encoding.Unicode);

        await using FileStream sourceStream 
            = new (filePath, fileMode, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        await sourceStream.WriteAsync(encodedText);
    }
}