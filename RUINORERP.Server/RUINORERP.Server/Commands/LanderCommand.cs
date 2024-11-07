using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Server.ServerSession;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.Server.Abstractions.Session;

namespace RUINORERP.Server.Commands
{
    /// <summary>
    /// 0xFF 0xFE
    /// </summary>
    [Command(Key = unchecked((short) (0xFF * 256 + 0xFE)))]
    public class LanderCommand : IAsyncCommand<BizPackageInfo>
    {
        public async ValueTask ExecuteAsync(IAppSession session, BizPackageInfo package, CancellationToken cancellationToken)
        {
            await Task.Delay(0);
            HandleData(session, package.Body);
        }

        private void HandleData(IAppSession session, byte[] body)
        {
            var buffer = new ReadOnlySequence<byte>(body);
            var reader = new SequenceReader<byte>(buffer);
            reader.TryReadBigEndian(out short temperature); //以大端方式读取short类型数据
            reader.TryReadBigEndian(out short humidity); //以大端方式读取short类型数据
            humidity = (short) (humidity / 10);
            (session as SessionforLander )?.ReceiveData(temperature / 10f, humidity);
        }
    }
}
