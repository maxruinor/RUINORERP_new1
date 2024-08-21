using RUINORERP.IServices.BASE;
using RUINORERP.Model;
using RUINORERP.Model.Models;
using System.Threading.Tasks;

namespace RUINORERP.IServices
{
    public partial interface IGuestbookServices : IBaseServices<Guestbook>
    {
        Task<MessageModel<string>> TestTranInRepository();
        Task<bool> TestTranInRepositoryAOP();

        Task<bool> TestTranPropagation();

        Task<bool> TestTranPropagationNoTran();

        Task<bool> TestTranPropagationTran();
    }
}