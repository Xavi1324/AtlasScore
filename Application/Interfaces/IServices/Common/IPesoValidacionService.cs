namespace Application.Interfaces.IServices.Common
{
    public interface IPesoValidacionService
    {
        Task<decimal> ObtenerSumaDePesosAsync();
        Task<bool> PuedeAgregarConPesoAsync(decimal nuevoPeso);
        Task<bool> PuedeActualizarPesoAsync(int id, decimal nuevoPeso);

       
    }
}
