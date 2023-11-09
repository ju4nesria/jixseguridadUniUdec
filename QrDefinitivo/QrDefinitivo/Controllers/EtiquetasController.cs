/*using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using QRCoder;
using QrDefinitivo.Models;
using QrDefinitivo.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection.Metadata;


namespace QrDefinitivo.Controllers
{
    [Route("api/etiquetas")]
    public class EtiquetasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EtiquetasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> GenerarYAsignarEtiqueta([FromBody] Etiqueta etiqueta)
        {
            if (ModelState.IsValid)
            {
                // Genera un código QR único basado en GUID y la fecha actual
                string qrCodeData = GenerateUniqueQRCode();
                byte[] qrCodeImage = GenerateQRCode(qrCodeData);

                // Convierte el código QR a Base64
                string qrCodeBase64 = Convert.ToBase64String(qrCodeImage);

                // Crea una nueva etiqueta
                etiqueta.QRCodeBase64 = qrCodeBase64;
                etiqueta.Estado = false; // La etiqueta se inicializa como no ocupada

                // Guarda la etiqueta en la base de datos
                _context.Etiquetas.Add(etiqueta);
                await _context.SaveChangesAsync();

                return Ok(new { QRCode = qrCodeBase64 });
            }

            return BadRequest(ModelState);
        }
        


        
        [HttpDelete("eliminar-qr/{id}")]
        public async Task<IActionResult> EliminarEtiqueta(int id)
        {
            var etiqueta = await _context.Etiquetas.FindAsync(id);

            if (etiqueta == null)
            {
                return NotFound();
            }

            // Elimina la etiqueta de la base de datos
            _context.Etiquetas.Remove(etiqueta);
            await _context.SaveChangesAsync();

            return Ok(new { Mensaje = "Etiqueta eliminada exitosamente" });
        }



        /// <summary>
        /// ///////////
        /// </summary>
        /// <returns></returns>
        /// 
        
        private string GenerateUniqueQRCode()
        {
            // Genera un código QR único basado en GUID y la fecha actual
            return Guid.NewGuid().ToString("N") + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        private byte[] GenerateQRCode(string data)
        {
            using QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);

            using BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qrCode.GetGraphic(20);

            return qrCodeBytes;
        }
    }
}
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using QRCoder;
using QrDefinitivo.Data;
using QrDefinitivo.Models;
using System;
using System.Drawing;

namespace QrDefinitivo.Controllers
{
    [Route("api/etiquetas")]
    public class EtiquetasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EtiquetasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("generar/{cantidad}")]
        public IActionResult GenerarEtiquetas(int cantidad)
        {
            if (cantidad <= 0)
            {
                return BadRequest("La cantidad debe ser mayor que cero.");
            }

            try
            {
                for (int i = 0; i < cantidad; i++)
                {
                    // Genera un código QR único basado en GUID y la fecha actual
                    string qrCodeData = GenerateUniqueQRCode();
                    byte[] qrCodeImage = GenerateQRCode(qrCodeData);

                    // Convierte el código QR a Base64
                    string qrCodeBase64 = Convert.ToBase64String(qrCodeImage);

                    // Crea una nueva etiqueta
                    var etiqueta = new Etiqueta
                    {
                        QRCodeBase64 = qrCodeBase64,
                        Estado = false, // La etiqueta se inicializa como no ocupada
                        FechaPegado = DateTime.UtcNow // Establece la fecha actual en formato UTC
                    };

                    // Guarda la etiqueta en la base de datos
                    _context.Etiquetas.Add(etiqueta);
                }

                _context.SaveChanges();

                return Ok($"Se generaron y guardaron {cantidad} etiquetas exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al generar las etiquetas: {ex.Message}");
            }
        }
        [HttpDelete("eliminar-etiqueta/{id}")]
        public IActionResult EliminarEtiqueta(int id)
        {
            var etiqueta = _context.Etiquetas.Find(id);

            if (etiqueta == null)
            {
                return NotFound("Etiqueta no encontrada");
            }

            // Elimina la etiqueta de la base de datos
            _context.Etiquetas.Remove(etiqueta);
            _context.SaveChanges();

            return Ok($"Etiqueta con ID {id} eliminada exitosamente");
        }


        // Otras acciones...

        private string GenerateUniqueQRCode()
        {
            // Genera un código QR único basado en GUID y la fecha actual
            return Guid.NewGuid().ToString("N") + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        private byte[] GenerateQRCode(string data)
        {
            using QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);

            using BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }
    }
}


