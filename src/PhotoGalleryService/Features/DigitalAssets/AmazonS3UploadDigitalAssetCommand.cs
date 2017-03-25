using Amazon.S3;
using Amazon.S3.Model;
using PhotoGalleryService.Features.DigitalAssets.UploadHandlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using static Amazon.RegionEndpoint;
using static Amazon.S3.S3CannedACL;

namespace PhotoGalleryService.Features.DigitalAssets
{
    public class AmazonS3UploadDigitalAssetCommand
    {
        public class AmazonS3UploadDigitalAssetRequest : IRequest<AmazonS3UploadDigitalAssetResponse>
        {
            public InMemoryMultipartFormDataStreamProvider Provider { get; set; }
            public string Folder { get; set; } = "amazon-s3-upload";
        }

        public class AmazonS3UploadDigitalAssetResponse {
            public ICollection<DigitalAssetApiModel> DigitalAssets { get; set; } = new HashSet<DigitalAssetApiModel>();
        }

        public class AmazonS3UploadDigitalAssetHandler : IAsyncRequestHandler<AmazonS3UploadDigitalAssetRequest, AmazonS3UploadDigitalAssetResponse>
        {
            public AmazonS3UploadDigitalAssetHandler(Lazy<IAmazonS3Configuration> amazonS3Configuration)
            {                             
                _amazonS3Configuration = amazonS3Configuration.Value;
                _client = new AmazonS3Client(_amazonS3Configuration.AccessKey, _amazonS3Configuration.SecretKey, USEast1);
            }

            public async Task<AmazonS3UploadDigitalAssetResponse> Handle(AmazonS3UploadDigitalAssetRequest request)
            {
                var response = new AmazonS3UploadDigitalAssetResponse();
                foreach (var file in request.Provider.Files)
                {                        
                    var filename = new FileInfo(file.Headers.ContentDisposition.FileName.Trim(new char[] { '"' })
                        .Replace("&", "and")).Name;
                    var stream = await file.ReadAsStreamAsync();

                    var putObjectRequest = new PutObjectRequest()
                    {
                        BucketName = _amazonS3Configuration.BucketName,
                        Key = $"{request.Folder}/{filename}",
                        InputStream = stream,
                        CannedACL = PublicRead
                    };
                    
                    var putObjectResponse = await _client.PutObjectAsync(putObjectRequest);
                    
                    response.DigitalAssets.Add(new DigitalAssetApiModel() {
                        Url = $"{_amazonS3Configuration.BaseUrl}${_amazonS3Configuration.BucketName}\\{request.Folder}\\{filename}"
                    });
                }

                return response;
            }
            private readonly IAmazonS3Configuration _amazonS3Configuration;
            protected readonly IAmazonS3 _client;
        }
    }
}
