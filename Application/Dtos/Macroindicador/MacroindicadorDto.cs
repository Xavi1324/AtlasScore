namespace Application.Dtos.Macroindicador
{
    public class MacroindicadorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Peso { get; set; }
        public bool EsMejorMasAlto { get; set; }
    }
}
