﻿using MediatR;
using PhotoGalleryService.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PhotoGalleryService.Features.DigitalAssets.UploadHandlers;
using System.Collections.Specialized;
using System.Net.Http;
using System.IO;
using PhotoGalleryService.Data.Model;
using PhotoGalleryService.Features.Core;
using static PhotoGalleryService.Features.DigitalAssets.Constants;

namespace PhotoGalleryService.Features.DigitalAssets
{
    public class UploadDigitalAssetCommand
    {
        public class UploadDigitalAssetRequest : IRequest<UploadDigitalAssetResponse>
        {
            public InMemoryMultipartFormDataStreamProvider Provider { get; set; }
        }

        public class UploadDigitalAssetResponse
        {
            public ICollection<DigitalAssetApiModel> DigitalAssets { get; set; }
        }

        public class UploadDigitalAssetHandler : IAsyncRequestHandler<UploadDigitalAssetRequest, UploadDigitalAssetResponse>
        {
            public UploadDigitalAssetHandler(IPhotoGalleryServiceContext context, ICache cache)
            {
                _context = context;
                _cache = cache;
            }

            public async Task<UploadDigitalAssetResponse> Handle(UploadDigitalAssetRequest request)
            {
                NameValueCollection formData = request.Provider.FormData;
                IList<HttpContent> files = request.Provider.Files;
                List<DigitalAsset> digitalAssets = new List<DigitalAsset>();
                foreach (var file in files)
                {
                    var filename = new FileInfo(file.Headers.ContentDisposition.FileName.Trim(new char[] { '"' })
                        .Replace("&", "and")).Name;
                    Stream stream = await file.ReadAsStreamAsync();
                    var bytes = StreamHelper.ReadToEnd(stream);
                    var digitalAsset = new DigitalAsset();
                    digitalAsset.FileName = filename;
                    digitalAsset.Bytes = bytes;
                    digitalAsset.ContentType = System.Convert.ToString(file.Headers.ContentType);
                    _context.DigitalAssets.Add(digitalAsset);
                    digitalAssets.Add(digitalAsset);
                }

                await _context.SaveChangesAsync();

                _cache.Add(null, DigitalAssetCacheKeys.DigitalAssets);

                return new UploadDigitalAssetResponse()
                {
                    DigitalAssets = digitalAssets.Select(x => DigitalAssetApiModel.FromDigitalAsset(x)).ToList()
                };
            }

            private readonly IPhotoGalleryServiceContext _context;
            private readonly ICache _cache;
            
        }

    }

}
