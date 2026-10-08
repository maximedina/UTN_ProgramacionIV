using IntroEF.Excepciones;
using System.Net;

namespace IntroEF.Middlewares
{
    public class ManejadorErroresMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<ManejadorErroresMiddleware> logger;

        public ManejadorErroresMiddleware(RequestDelegate siguiente, ILogger<ManejadorErroresMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext contexto)
        {
            try
            {
                await siguiente(contexto);
            }
            catch (ReglaDeNegocioException ex)
            {
                logger.LogInformation(ex, "Regla de negocio incumplida");
                contexto.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await contexto.Response.WriteAsJsonAsync(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error no controlado");
                contexto.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await contexto.Response.WriteAsJsonAsync(new { mensaje = "Ocurrió un error inesperado" });
            }
        }
    }
}
