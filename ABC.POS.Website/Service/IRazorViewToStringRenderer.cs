using System.Threading.Tasks;

namespace ABC.POS.Website.Service
{
    public interface IRazorViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync(string viewName,object model);
    }
}