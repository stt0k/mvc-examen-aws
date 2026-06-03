using Amazon.S3;
using Amazon.S3.Model;
using mvc_examen_aws.Models;
using mvc_examen_aws.Repositories;

namespace mvc_examen_aws.Services
{
    public class ZapatillaService
    {
        private readonly ZapatillaRepository _repository;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _region;

        public ZapatillaService(ZapatillaRepository repository, IAmazonS3 s3Client, IConfiguration configuration)
        {
            _repository = repository;
            _s3Client = s3Client;
            _bucketName = configuration["AWS:BucketName"]!;
            _region = configuration["AWS:Region"] ?? "us-east-1";
        }

        public async Task<List<Zapatilla>> GetAllAsync()
        {
            var zapatillas = await _repository.GetAllAsync();
            var baseUrl = $"https://{_bucketName}.s3.{_region}.amazonaws.com/zapas/";
            foreach (var z in zapatillas)
            {
                if (!string.IsNullOrEmpty(z.Imagen) && !z.Imagen.StartsWith("http"))
                {
                    z.Imagen = baseUrl + z.Imagen;
                }
            }
            return zapatillas;
        }

        public async Task CreateAsync(Zapatilla zapatilla, IFormFile? archivo)
        {
            if (archivo != null && archivo.Length > 0)
            {
                var fileName = $"zapas/{Guid.NewGuid()}_{archivo.FileName}";
                using var stream = archivo.OpenReadStream();

                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = stream,
                    ContentType = archivo.ContentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                await _s3Client.PutObjectAsync(putRequest);

                zapatilla.Imagen = $"https://{_bucketName}.s3.{_region}.amazonaws.com/{fileName}";
            }

            await _repository.CreateAsync(zapatilla);
        }
    }
}
