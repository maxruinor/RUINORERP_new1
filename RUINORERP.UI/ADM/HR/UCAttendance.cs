using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Global;
using RUINORERP.UI.Common;
namespace RUINORERP.UI.ADM.HR
{
    [MenuAttrAssemblyInfo("人事考勤",  ModuleMenuDefine.模块定义.行政管理,ModuleMenuDefine.行政管理.人事管理)]
    public partial class UCAttendance : UserControl
    {
        public UCAttendance()
        {
            InitializeComponent();
        }

        tb_AttendanceController<tb_Attendance> mc = Startup.GetFromFac<tb_AttendanceController<tb_Attendance>>();
        private async void kryptonButton1_Click(object sender, EventArgs e)
        {
            List<tb_Attendance> attendances = new List<Model.tb_Attendance>();
            attendances = mc.Query();
            foreach (tb_Attendance item in attendances)
            {
                if (item.ID == 183)
                {

                }
                //5个时间 如果第一个和第二个的间距在5分钟内，则认为是早上的卡
                if (item.t1 != null)
                {
                    continue;
                }
                string[] sts = item.stime.Trim().Split(' ');
                if (sts.Length == 5)
                {
                    if (sts[0] == sts[1])
                    {
                        DateTime cday = DateTime.Parse(item.sDate);
                        item.t1 = cday.AddHours(DateTime.Parse(sts[0]).Hour).AddMinutes(DateTime.Parse(sts[0]).Minute);
                        item.t2 = cday.AddHours(DateTime.Parse(sts[2]).Hour).AddMinutes(DateTime.Parse(sts[2]).Minute);
                        item.t3 = cday.AddHours(DateTime.Parse(sts[3]).Hour).AddMinutes(DateTime.Parse(sts[3]).Minute);
                        item.t4 = cday.AddHours(DateTime.Parse(sts[4]).Hour).AddMinutes(DateTime.Parse(sts[4]).Minute);
                        await mc.UpdateAsync(item);
                    }
                }
                if (sts.Length == 4)
                {
                    DateTime cday = DateTime.Parse(item.sDate);
                    item.t1 = cday.AddHours(DateTime.Parse(sts[0]).Hour).AddMinutes(DateTime.Parse(sts[0]).Minute);
                    item.t2 = cday.AddHours(DateTime.Parse(sts[1]).Hour).AddMinutes(DateTime.Parse(sts[1]).Minute);
                    item.t3 = cday.AddHours(DateTime.Parse(sts[2]).Hour).AddMinutes(DateTime.Parse(sts[2]).Minute);
                    item.t4 = cday.AddHours(DateTime.Parse(sts[3]).Hour).AddMinutes(DateTime.Parse(sts[3]).Minute);
                    await mc.UpdateAsync(item);
                }
                if (sts.Length == 3)
                {
                    DateTime cday = DateTime.Parse(item.sDate);
                    item.t1 = cday.AddHours(DateTime.Parse(sts[0]).Hour).AddMinutes(DateTime.Parse(sts[0]).Minute);
                    item.t2 = cday.AddHours(DateTime.Parse(sts[1]).Hour).AddMinutes(DateTime.Parse(sts[1]).Minute);
                    item.t3 = cday.AddHours(DateTime.Parse(sts[1]).Hour).AddMinutes(DateTime.Parse(sts[1]).Minute);
                    item.t4 = cday.AddHours(DateTime.Parse(sts[2]).Hour).AddMinutes(DateTime.Parse(sts[2]).Minute);
                    await mc.UpdateAsync(item);
                }
                if (sts.Length == 2)
                {
                    DateTime cday = DateTime.Parse(item.sDate);
                    DateTime cday1 = cday.AddHours(DateTime.Parse(sts[0]).Hour).AddMinutes(DateTime.Parse(sts[0]).Minute);
                    DateTime cday2 = cday.AddHours(DateTime.Parse(sts[1]).Hour).AddMinutes(DateTime.Parse(sts[1]).Minute);
                    if (cday1.Hour >= DateTime.Parse("11:30").Hour)//算下午
                    {
                        item.t3 = cday1;
                    }
                    else
                    {
                        //算上午
                        item.t1 = cday1;
                    }

                    if (cday2.Hour >= DateTime.Parse("14:00").Hour)//算下午
                    {

                        item.t4 = cday2;
                    }
                    else
                    {
                        //算上午
                        item.t4 = cday2;
                    }
                    await mc.UpdateAsync(item);
                }
            }
        }
    }
}
