using System;

namespace QrDefinitivo.Models
{
    public class Etiqueta
    {
        public int Id { get; set; }
        public string? QRCodeBase64 { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaPegado { get; set; }
    }
}