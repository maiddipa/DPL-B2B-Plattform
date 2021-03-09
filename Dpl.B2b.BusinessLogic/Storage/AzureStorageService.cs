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

    public class AzureStorageService : IStorageService
    {
        private readonly object UserDelegationLock = new object();
        private const int UserDelegationKeyLifetimeInDays = 7; // 7 days is the maximum
        private const int SasTokenLifetimeInMinutes = 15;

        private readonly string _accountName;
        private readonly string _containerName;
        private readonly BlobServiceClient _client;

        private UserDelegationKey __userDelegationKey { get; set; }

        public AzureStorageService(
            IConfiguration configuration)
        {
            var storageSection = configuration.GetSection("AzureStorage");
            var azureAdInfo = new
            {
                TenantId = storageSection["TenantId"],
                ClientId = storageSection["ClientId"],
                ClientSecret = storageSection["ClientSecret"],
            };

            var credential = new ClientSecretCredential(azureAdInfo.TenantId, azureAdInfo.ClientId, azureAdInfo.ClientSecret);
            var storageInfo = new
            {
                AccountName = storageSection["AccountName"],
                ContainerName = storageSection["ContainerName"]
            };

            _accountName = storageInfo.AccountName;
            _containerName = storageInfo.ContainerName;
            _client = new BlobServiceClient(new Uri($"https://{_accountName}.blob.core.windows.net/"), credential);

            // make sure the container exists
            if (!_client.GetBlobContainerClient(_containerName).Exists().Value)
            {
                // the below code prevents handles when 2 instances start simulatenously
                // and one has already created the container
                try
                {
                    _client.CreateBlobContainer(_containerName, PublicAccessType.None);
                }
                catch (Azure.RequestFailedException ex)
                {
                    // rethrow if anything OTHER than ContainerAlreadyExists
                    if (ex.ErrorCode != BlobErrorCode.ContainerAlreadyExists)
                    {
                        throw ex;
                    }

                    // if error was ContainerAlreadyExists, ignore as we can now be sure that the container exists
                }
            }
        }

        public bool ContainerExists
        {
            get
            {
                return true;
            }
        }

        public async Task<string> GetDownloadLink(string fileName)
        {
            var builder = new BlobSasBuilder()
            {
                BlobContainerName = _containerName,
                BlobName = fileName,
                Resource = "b", // specifies that resource is a blob
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-15), // this is recommended as clocks might not be synced
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(SasTokenLifetimeInMinutes),
            };

            builder.SetPermissions(BlobSasPermissions.Read);

            var delegationKey = await GetUserDelegationKey(builder.ExpiresOn);
            string sasToken = builder.ToSasQueryParameters(delegationKey, _accountName).ToString();

            var container = _client.GetBlobContainerClient(_containerName);
            var blob = container.GetBlobClient(fileName);

            var uriWithSasToken = new UriBuilder(blob.Uri)
            {
                Query = sasToken
            };

            return uriWithSasToken.ToString();
        }

        public async Task<Stream> Read(string fileName)
        {
            var container = _client.GetBlobContainerClient(_containerName);
            var blob = container.GetBlobClient(fileName);
            var download = await blob.DownloadAsync();
            return download.Value.Content;
        }

        private async Task<UserDelegationKey> GetUserDelegationKey(DateTimeOffset? expiration = null)
        {
            // refresh user-delegation key as necessary, valid for up to a maximum of 7 days
            if (__userDelegationKey == null || (expiration != null && __userDelegationKey.SignedExpiresOn <= expiration))
            {
                lock (UserDelegationLock)
                {
                    // additional check is neccessary to ensure that token hasnt been refreshed while thread was waiting for lock to be released
                    if (__userDelegationKey == null || (expiration != null && __userDelegationKey.SignedExpiresOn <= expiration))
                    {
                        var response = _client.GetUserDelegationKey(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(UserDelegationKeyLifetimeInDays));
                        if (response == null)
                        {
                            throw new ArgumentNullException($"User account delegation key generation failed for storage account: {_client.AccountName}");
                        }

                        __userDelegationKey = response.Value;
                    }
                }
            }

            return __userDelegationKey;
        }

        public async Task<string> Write(string fileName, MemoryStream stream)
        {
            var container = _client.GetBlobContainerClient(_containerName);
            // Get a reference to a blob
            BlobClient blobClient = container.GetBlobClient(fileName);
            stream.Position = 0;
            await blobClient.UploadAsync(stream);
            //stream.Close();

            return await GetDownloadLink(fileName);
        }
    }
}
