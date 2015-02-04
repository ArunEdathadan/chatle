using System;
using System.IO;
using System.Threading;
using Owin;
using System.Threading.Tasks;
using Gate.Utils;

namespace Gate.Middleware.StaticFiles
{
    internal class FileBody
    {
        private Stream fileStream;
        private readonly Tuple<long, long> range;
        private readonly string path;

        public FileBody(string path, Tuple<long, long> range)
        {
            this.path = path;
            this.range = range;
        }

        public Task Start(Stream stream)
        {
            this.OpenFileStream();
            return this.fileStream.CopyToAsync(stream, (int)(range.Item2 - range.Item1 + 1));
        }

        private void OpenFileStream()
        {
            if (this.fileStream == null || !this.fileStream.CanRead)
            {
                this.fileStream = File.OpenRead(path);
                this.fileStream.Seek(range.Item1, SeekOrigin.Begin);
            }
        }
    }
}