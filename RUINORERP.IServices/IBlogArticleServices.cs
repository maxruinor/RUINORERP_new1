using RUINORERP.IServices.BASE;
using RUINORERP.Model.Models;
using RUINORERP.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.IServices
{
    public interface IBlogArticleServices :IBaseServices<BlogArticle>
    {
        Task<List<BlogArticle>> GetBlogs();
        Task<BlogViewModels> GetBlogDetails(int id);

    }

}
