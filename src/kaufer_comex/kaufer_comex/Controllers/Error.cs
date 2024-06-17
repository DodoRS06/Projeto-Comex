using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;

namespace kaufer_comex.Controllers
{
    public class Error : Controller
    {
       
        public IActionResult BadRequestError()
        {
            return View("Error", new ErrorViewModel { RequestId = "400", ErrorMessage = "Requisição inválida." });
        }

        public IActionResult UnauthorizedError()
        {
            return View("Error", new ErrorViewModel { RequestId = "401", ErrorMessage = "Não autorizado. Faça login para continuar." });
        }

        public IActionResult NotFoundError()
        {
            return View("Error", new ErrorViewModel { RequestId = "404", ErrorMessage = "Recurso não encontrado." });
        }

        public IActionResult MethodNotAllowedError()
        {
            return View("Error", new ErrorViewModel { RequestId = "405", ErrorMessage = "Método não permitido." });
        }

        public IActionResult RequestTimeoutError()
        {
            return View("Error", new ErrorViewModel { RequestId = "408", ErrorMessage = "Tempo de requisição esgotado." });
        }

        public IActionResult ConflictError()
        {
            return View("Error", new ErrorViewModel { RequestId = "409", ErrorMessage = "Conflito no pedido." });
        }

        public IActionResult InternalServerError()
        {
            return View("Error", new ErrorViewModel { RequestId = "500", ErrorMessage = "Erro interno do servidor." });
        }
    }
}
