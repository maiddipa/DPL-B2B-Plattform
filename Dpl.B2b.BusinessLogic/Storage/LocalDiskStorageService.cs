using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Dpl.B2b.Contracts;
using Microsoft.Extensions.Configuration;

namespace Dpl.B2b.BusinessLogic.Storage
{

    public class LocalDiskStorageService : IStorageService
    {
        private readonly string _basePath;

        public LocalDiskStorageService(
            IConfiguration configuration)
        {
            _basePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        }

        public async Task<string> GetDownloadLink(string fileName)
        {
            return $"https://localhost:5001/downloads/{fileName}";
        }

        public async Task<Stream> Read(string fileName)
        {
            var filePath = Path.Combine(_basePath, Constants.Documents.ContainerName, fileName);
            return File.OpenRead(filePath);
        }

        public Task<string> Write(string fileName, MemoryStream stream)
        {
            var filePath = Path.Combine(_basePath, Constants.Documents.ContainerName, fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllBytes(filePath, stream.ToArray());
            return GetDownloadLink(fileName);
        }
    }
}
