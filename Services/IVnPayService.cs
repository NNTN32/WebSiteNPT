using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using WebShopNPT.Models;

namespace WebShopNPT.Services
{
    public interface IVnPayService
    {
        //Build duong dan thanh toan
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentRequestModel PaymentExecute(IQueryCollection collections);

    }
}
