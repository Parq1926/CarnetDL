namespace SRV4_Areas.DTOs
{
    public class CreateAreaRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public int InstitucionID { get; set; }
        public string InstitucionNombre { get; set; } = string.Empty;
    }

    public class UpdateAreaRequest
    {
        public int ID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int InstitucionID { get; set; }
        public string InstitucionNombre { get; set; } = string.Empty;
    }
}