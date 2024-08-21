using System.Text;
using System.Threading.Tasks;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;

namespace RUINORERP.Server.Commands
{
    public abstract class BaseCommand : IAsyncCommand<StringPackageInfo>
    {
        public async ValueTask ExecuteAsync(IAppSession session, StringPackageInfo package)
        {
            await Task.Delay(0);

            var result = GetResult(package);

            //发送消息给客户端
            await session.SendAsync(Encoding.UTF8.GetBytes(result + "\r\n"));
        }

        protected abstract string GetResult(StringPackageInfo package);
    }
}
