using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using rp_back.Data;
using rp_back.DTOs;
using rp_back.Interfaces;
using rp_back.Models;

namespace rp_back.Services
{
    public class FotoReporteService : IFotoReporteService
    {
        private readonly ReportaSabanaDbContext _context;
        private readonly IConfiguration _configuration;

        public FotoReporteService(ReportaSabanaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> FotoReporteAsync(CrearFotoReporteDTO dTo)
        {
            var reporteExiste = await _context.Reportes.FindAsync(dTo.ReporteId);

            if (reporteExiste == null)
            {
                return false;
            }

            var fotoReporte = new Foto
            {
                ReporteId = dTo.ReporteId,
                Url = dTo.Url
            };

            _context.Fotos.Add(fotoReporte);
            await _context.SaveChangesAsync();

            return true;
        }

        public Task<(string uploadUrl, string publicUrl)> GenerarUrlFirmadaAsync(string fileName)
        {
            var accessKey = _configuration.GetValue<string>("CloudflareR2:AccessKey");
            var secretKey = _configuration.GetValue<string>("CloudflareR2:SecretKey");
            var serviceUrl = _configuration.GetValue<string>("CloudflareR2:ServiceUrl");
            var bucketName = _configuration.GetValue<string>("CloudflareR2:BucketName");
            var publicDomain = _configuration.GetValue<string>("CloudflareR2:PublicDomain");

            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("Las credenciales de Cloudflare R2 no están configuradas correctamente.");
            }

            var nombreUnico = $"reporte_fotos/{Guid.NewGuid()}_{fileName}";

            var s3Client = new Amazon.S3.AmazonS3Client(
                accessKey,
                secretKey,
                new Amazon.S3.AmazonS3Config { ServiceURL = serviceUrl }
            );

            var solicitud = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = nombreUnico,
                Expires = DateTime.UtcNow.AddMinutes(15),
                Verb = HttpVerb.PUT
            };

            var uploadUrl = s3Client.GetPreSignedURL(solicitud);
            var publicUrl = $"{publicDomain}/{nombreUnico}";

            return Task.FromResult((uploadUrl, publicUrl));
        }

        public async Task<IEnumerable<Foto>> ObtenerFotosPorReporteIdAsync(int reporteId)
        {
            var fotos = await _context.Fotos
                                      .Where(f => f.ReporteId == reporteId)
                                      .ToListAsync();
            return fotos;
        }
    }
}