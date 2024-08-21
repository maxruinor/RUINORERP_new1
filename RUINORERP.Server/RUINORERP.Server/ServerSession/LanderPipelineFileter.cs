using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Server.ServerSession
{
    public class LanderCommandLinePipelineFilter : TerminatorPipelineFilter<LanderPackageInfo>
    {
        /// <summary>
        /// 解包  登陆 器 暂时 约定   数据\n  结尾一个请求。
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override LanderPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            var reader = new SequenceReader<byte>(buffer);

            //var body = buffer.Slice(0).ToArray();
            var mydata = reader.ReadString();
            String[] ss = mydata.Split("|");
            string mykey = ss[0].Trim();
            return new LanderPackageInfo
            {
                Key = mykey,
                Body = mydata.Trim() // new byte[] { 1 }
            };
        }

        /// <summary>
        /// \n结尾巴  为一个分割数据包
        /// </summary>
        public LanderCommandLinePipelineFilter()
               : base(new[] { (byte)'\n' })
        {

        }
    }


}
