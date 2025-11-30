namespace RUINORERP.UI.Forms
{
    partial class LockInfoDetailForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageBasicInfo = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelBasicInfo = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblLockKey = new System.Windows.Forms.Label();
            this.lblLockId = new System.Windows.Forms.Label();
            this.lblBillID = new System.Windows.Forms.Label();
            this.lblBillNo = new System.Windows.Forms.Label();
            this.tabPageUserInfo = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelUserInfo = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblLockedUserId = new System.Windows.Forms.Label();
            this.lblLockedUserName = new System.Windows.Forms.Label();
            this.tabPageTimeInfo = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelTimeInfo = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblLockTime = new System.Windows.Forms.Label();
            this.lblExpireTime = new System.Windows.Forms.Label();
            this.lblLastHeartbeat = new System.Windows.Forms.Label();
            this.lblLastUpdateTime = new System.Windows.Forms.Label();
            this.lblDurationText = new System.Windows.Forms.Label();
            this.tabPageStatusInfo = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelStatusInfo = new System.Windows.Forms.TableLayoutPanel();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblIsLocked = new System.Windows.Forms.Label();
            this.lblIsExpired = new System.Windows.Forms.Label();
            this.lblIsOrphaned = new System.Windows.Forms.Label();
            this.lblIsAboutToExpire = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tabPageBusinessInfo = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelBusinessInfo = new System.Windows.Forms.TableLayoutPanel();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblBizType = new System.Windows.Forms.Label();
            this.lblMenuID = new System.Windows.Forms.Label();
            this.lblSessionId = new System.Windows.Forms.Label();
            this.tabPageOtherInfo = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelOtherInfo = new System.Windows.Forms.TableLayoutPanel();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.lblRemark = new System.Windows.Forms.Label();
            this.lblHeartbeatCount = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblExpireTimestamp = new System.Windows.Forms.Label();
            this.lblRemainingLockTimeMs = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageBasicInfo.SuspendLayout();
            this.tableLayoutPanelBasicInfo.SuspendLayout();
            this.tabPageUserInfo.SuspendLayout();
            this.tableLayoutPanelUserInfo.SuspendLayout();
            this.tabPageTimeInfo.SuspendLayout();
            this.tableLayoutPanelTimeInfo.SuspendLayout();
            this.tabPageStatusInfo.SuspendLayout();
            this.tableLayoutPanelStatusInfo.SuspendLayout();
            this.tabPageBusinessInfo.SuspendLayout();
            this.tableLayoutPanelBusinessInfo.SuspendLayout();
            this.tabPageOtherInfo.SuspendLayout();
            this.tableLayoutPanelOtherInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.btnClose, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tabControl, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(571, 418);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.Location = new System.Drawing.Point(236, 378);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 28);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageBasicInfo);
            this.tabControl.Controls.Add(this.tabPageUserInfo);
            this.tabControl.Controls.Add(this.tabPageTimeInfo);
            this.tabControl.Controls.Add(this.tabPageStatusInfo);
            this.tabControl.Controls.Add(this.tabPageBusinessInfo);
            this.tabControl.Controls.Add(this.tabPageOtherInfo);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(3, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(565, 362);
            this.tabControl.TabIndex = 1;
            // 
            // tabPageBasicInfo
            // 
            this.tabPageBasicInfo.Controls.Add(this.tableLayoutPanelBasicInfo);
            this.tabPageBasicInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageBasicInfo.Name = "tabPageBasicInfo";
            this.tabPageBasicInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBasicInfo.Size = new System.Drawing.Size(557, 336);
            this.tabPageBasicInfo.TabIndex = 0;
            this.tabPageBasicInfo.Text = "基本信息";
            this.tabPageBasicInfo.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelBasicInfo
            // 
            this.tableLayoutPanelBasicInfo.ColumnCount = 2;
            this.tableLayoutPanelBasicInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelBasicInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBasicInfo.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelBasicInfo.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanelBasicInfo.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanelBasicInfo.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanelBasicInfo.Controls.Add(this.lblLockKey, 1, 0);
            this.tableLayoutPanelBasicInfo.Controls.Add(this.lblLockId, 1, 1);
            this.tableLayoutPanelBasicInfo.Controls.Add(this.lblBillID, 1, 2);
            this.tableLayoutPanelBasicInfo.Controls.Add(this.lblBillNo, 1, 3);
            this.tableLayoutPanelBasicInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBasicInfo.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelBasicInfo.Name = "tableLayoutPanelBasicInfo";
            this.tableLayoutPanelBasicInfo.RowCount = 4;
            this.tableLayoutPanelBasicInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelBasicInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelBasicInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelBasicInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelBasicInfo.Size = new System.Drawing.Size(551, 330);
            this.tableLayoutPanelBasicInfo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "锁定键:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "锁定ID:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "单据ID:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 30);
            this.label4.TabIndex = 3;
            this.label4.Text = "单据编号:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLockKey
            // 
            this.lblLockKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLockKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLockKey.Location = new System.Drawing.Point(103, 0);
            this.lblLockKey.Name = "lblLockKey";
            this.lblLockKey.Size = new System.Drawing.Size(445, 30);
            this.lblLockKey.TabIndex = 4;
            this.lblLockKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLockId
            // 
            this.lblLockId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLockId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLockId.Location = new System.Drawing.Point(103, 30);
            this.lblLockId.Name = "lblLockId";
            this.lblLockId.Size = new System.Drawing.Size(445, 30);
            this.lblLockId.TabIndex = 5;
            this.lblLockId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBillID
            // 
            this.lblBillID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBillID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillID.Location = new System.Drawing.Point(103, 60);
            this.lblBillID.Name = "lblBillID";
            this.lblBillID.Size = new System.Drawing.Size(445, 30);
            this.lblBillID.TabIndex = 6;
            this.lblBillID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBillNo
            // 
            this.lblBillNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBillNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillNo.Location = new System.Drawing.Point(103, 90);
            this.lblBillNo.Name = "lblBillNo";
            this.lblBillNo.Size = new System.Drawing.Size(445, 30);
            this.lblBillNo.TabIndex = 7;
            this.lblBillNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageUserInfo
            // 
            this.tabPageUserInfo.Controls.Add(this.tableLayoutPanelUserInfo);
            this.tabPageUserInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageUserInfo.Name = "tabPageUserInfo";
            this.tabPageUserInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUserInfo.Size = new System.Drawing.Size(557, 336);
            this.tabPageUserInfo.TabIndex = 1;
            this.tabPageUserInfo.Text = "用户信息";
            this.tabPageUserInfo.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelUserInfo
            // 
            this.tableLayoutPanelUserInfo.ColumnCount = 2;
            this.tableLayoutPanelUserInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelUserInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUserInfo.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanelUserInfo.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanelUserInfo.Controls.Add(this.lblLockedUserId, 1, 0);
            this.tableLayoutPanelUserInfo.Controls.Add(this.lblLockedUserName, 1, 1);
            this.tableLayoutPanelUserInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelUserInfo.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelUserInfo.Name = "tableLayoutPanelUserInfo";
            this.tableLayoutPanelUserInfo.RowCount = 2;
            this.tableLayoutPanelUserInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelUserInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelUserInfo.Size = new System.Drawing.Size(551, 330);
            this.tableLayoutPanelUserInfo.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 30);
            this.label5.TabIndex = 0;
            this.label5.Text = "锁定用户ID:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 30);
            this.label6.TabIndex = 1;
            this.label6.Text = "锁定用户名:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLockedUserId
            // 
            this.lblLockedUserId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLockedUserId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLockedUserId.Location = new System.Drawing.Point(103, 0);
            this.lblLockedUserId.Name = "lblLockedUserId";
            this.lblLockedUserId.Size = new System.Drawing.Size(445, 30);
            this.lblLockedUserId.TabIndex = 2;
            this.lblLockedUserId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLockedUserName
            // 
            this.lblLockedUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLockedUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLockedUserName.Location = new System.Drawing.Point(103, 30);
            this.lblLockedUserName.Name = "lblLockedUserName";
            this.lblLockedUserName.Size = new System.Drawing.Size(445, 30);
            this.lblLockedUserName.TabIndex = 3;
            this.lblLockedUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageTimeInfo
            // 
            this.tabPageTimeInfo.Controls.Add(this.tableLayoutPanelTimeInfo);
            this.tabPageTimeInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageTimeInfo.Name = "tabPageTimeInfo";
            this.tabPageTimeInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTimeInfo.Size = new System.Drawing.Size(557, 336);
            this.tabPageTimeInfo.TabIndex = 2;
            this.tabPageTimeInfo.Text = "时间信息";
            this.tabPageTimeInfo.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTimeInfo
            // 
            this.tableLayoutPanelTimeInfo.ColumnCount = 2;
            this.tableLayoutPanelTimeInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelTimeInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTimeInfo.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanelTimeInfo.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanelTimeInfo.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanelTimeInfo.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanelTimeInfo.Controls.Add(this.label11, 0, 4);
            this.tableLayoutPanelTimeInfo.Controls.Add(this.lblLockTime, 1, 0);
            this.tableLayoutPanelTimeInfo.Controls.Add(this.lblExpireTime, 1, 1);
            this.tableLayoutPanelTimeInfo.Controls.Add(this.lblLastHeartbeat, 1, 2);
            this.tableLayoutPanelTimeInfo.Controls.Add(this.lblLastUpdateTime, 1, 3);
            this.tableLayoutPanelTimeInfo.Controls.Add(this.lblDurationText, 1, 4);
            this.tableLayoutPanelTimeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTimeInfo.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelTimeInfo.Name = "tableLayoutPanelTimeInfo";
            this.tableLayoutPanelTimeInfo.RowCount = 5;
            this.tableLayoutPanelTimeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelTimeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelTimeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelTimeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelTimeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelTimeInfo.Size = new System.Drawing.Size(551, 330);
            this.tableLayoutPanelTimeInfo.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 30);
            this.label7.TabIndex = 0;
            this.label7.Text = "锁定时间:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 30);
            this.label8.TabIndex = 1;
            this.label8.Text = "过期时间:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 30);
            this.label9.TabIndex = 2;
            this.label9.Text = "最后心跳:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 30);
            this.label10.TabIndex = 3;
            this.label10.Text = "最后更新:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(3, 120);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 30);
            this.label11.TabIndex = 4;
            this.label11.Text = "锁定时长:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLockTime
            // 
            this.lblLockTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLockTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLockTime.Location = new System.Drawing.Point(103, 0);
            this.lblLockTime.Name = "lblLockTime";
            this.lblLockTime.Size = new System.Drawing.Size(445, 30);
            this.lblLockTime.TabIndex = 5;
            this.lblLockTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExpireTime
            // 
            this.lblExpireTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExpireTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExpireTime.Location = new System.Drawing.Point(103, 30);
            this.lblExpireTime.Name = "lblExpireTime";
            this.lblExpireTime.Size = new System.Drawing.Size(445, 30);
            this.lblExpireTime.TabIndex = 6;
            this.lblExpireTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLastHeartbeat
            // 
            this.lblLastHeartbeat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLastHeartbeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastHeartbeat.Location = new System.Drawing.Point(103, 60);
            this.lblLastHeartbeat.Name = "lblLastHeartbeat";
            this.lblLastHeartbeat.Size = new System.Drawing.Size(445, 30);
            this.lblLastHeartbeat.TabIndex = 7;
            this.lblLastHeartbeat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLastUpdateTime
            // 
            this.lblLastUpdateTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLastUpdateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastUpdateTime.Location = new System.Drawing.Point(103, 90);
            this.lblLastUpdateTime.Name = "lblLastUpdateTime";
            this.lblLastUpdateTime.Size = new System.Drawing.Size(445, 30);
            this.lblLastUpdateTime.TabIndex = 8;
            this.lblLastUpdateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDurationText
            // 
            this.lblDurationText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDurationText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDurationText.Location = new System.Drawing.Point(103, 120);
            this.lblDurationText.Name = "lblDurationText";
            this.lblDurationText.Size = new System.Drawing.Size(445, 30);
            this.lblDurationText.TabIndex = 9;
            this.lblDurationText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageStatusInfo
            // 
            this.tabPageStatusInfo.Controls.Add(this.tableLayoutPanelStatusInfo);
            this.tabPageStatusInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageStatusInfo.Name = "tabPageStatusInfo";
            this.tabPageStatusInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStatusInfo.Size = new System.Drawing.Size(557, 336);
            this.tabPageStatusInfo.TabIndex = 3;
            this.tabPageStatusInfo.Text = "状态信息";
            this.tabPageStatusInfo.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelStatusInfo
            // 
            this.tableLayoutPanelStatusInfo.ColumnCount = 2;
            this.tableLayoutPanelStatusInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelStatusInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStatusInfo.Controls.Add(this.label12, 0, 0);
            this.tableLayoutPanelStatusInfo.Controls.Add(this.label13, 0, 1);
            this.tableLayoutPanelStatusInfo.Controls.Add(this.label14, 0, 2);
            this.tableLayoutPanelStatusInfo.Controls.Add(this.label15, 0, 3);
            this.tableLayoutPanelStatusInfo.Controls.Add(this.label16, 0, 4);
            this.tableLayoutPanelStatusInfo.Controls.Add(this.label17, 0, 5);
            this.tableLayoutPanelStatusInfo.Controls.Add(this.lblIsLocked, 1, 0);
            this.tableLayoutPanelStatusInfo.Controls.Add(this.lblIsExpired, 1, 1);
            this.tableLayoutPanelStatusInfo.Controls.Add(this.lblIsOrphaned, 1, 2);
            this.tableLayoutPanelStatusInfo.Controls.Add(this.lblIsAboutToExpire, 1, 3);
            this.tableLayoutPanelStatusInfo.Controls.Add(this.lblType, 1, 4);
            this.tableLayoutPanelStatusInfo.Controls.Add(this.lblStatus, 1, 5);
            this.tableLayoutPanelStatusInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelStatusInfo.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelStatusInfo.Name = "tableLayoutPanelStatusInfo";
            this.tableLayoutPanelStatusInfo.RowCount = 6;
            this.tableLayoutPanelStatusInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelStatusInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelStatusInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelStatusInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelStatusInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelStatusInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelStatusInfo.Size = new System.Drawing.Size(551, 330);
            this.tableLayoutPanelStatusInfo.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(3, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 30);
            this.label12.TabIndex = 0;
            this.label12.Text = "已锁定:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(3, 30);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(94, 30);
            this.label13.TabIndex = 1;
            this.label13.Text = "已过期:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Location = new System.Drawing.Point(3, 60);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(94, 30);
            this.label14.TabIndex = 2;
            this.label14.Text = "孤儿锁:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(3, 90);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(94, 30);
            this.label15.TabIndex = 3;
            this.label15.Text = "即将过期:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Location = new System.Drawing.Point(3, 120);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(94, 30);
            this.label16.TabIndex = 4;
            this.label16.Text = "锁定类型:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Location = new System.Drawing.Point(3, 150);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(94, 30);
            this.label17.TabIndex = 5;
            this.label17.Text = "综合状态:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblIsLocked
            // 
            this.lblIsLocked.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIsLocked.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIsLocked.Location = new System.Drawing.Point(103, 0);
            this.lblIsLocked.Name = "lblIsLocked";
            this.lblIsLocked.Size = new System.Drawing.Size(445, 30);
            this.lblIsLocked.TabIndex = 6;
            this.lblIsLocked.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblIsExpired
            // 
            this.lblIsExpired.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIsExpired.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIsExpired.Location = new System.Drawing.Point(103, 30);
            this.lblIsExpired.Name = "lblIsExpired";
            this.lblIsExpired.Size = new System.Drawing.Size(445, 30);
            this.lblIsExpired.TabIndex = 7;
            this.lblIsExpired.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblIsOrphaned
            // 
            this.lblIsOrphaned.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIsOrphaned.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIsOrphaned.Location = new System.Drawing.Point(103, 60);
            this.lblIsOrphaned.Name = "lblIsOrphaned";
            this.lblIsOrphaned.Size = new System.Drawing.Size(445, 30);
            this.lblIsOrphaned.TabIndex = 8;
            this.lblIsOrphaned.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblIsAboutToExpire
            // 
            this.lblIsAboutToExpire.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIsAboutToExpire.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIsAboutToExpire.Location = new System.Drawing.Point(103, 90);
            this.lblIsAboutToExpire.Name = "lblIsAboutToExpire";
            this.lblIsAboutToExpire.Size = new System.Drawing.Size(445, 30);
            this.lblIsAboutToExpire.TabIndex = 9;
            this.lblIsAboutToExpire.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblType
            // 
            this.lblType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Location = new System.Drawing.Point(103, 120);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(445, 30);
            this.lblType.TabIndex = 10;
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(103, 150);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(445, 30);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageBusinessInfo
            // 
            this.tabPageBusinessInfo.Controls.Add(this.tableLayoutPanelBusinessInfo);
            this.tabPageBusinessInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageBusinessInfo.Name = "tabPageBusinessInfo";
            this.tabPageBusinessInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBusinessInfo.Size = new System.Drawing.Size(557, 336);
            this.tabPageBusinessInfo.TabIndex = 4;
            this.tabPageBusinessInfo.Text = "业务信息";
            this.tabPageBusinessInfo.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelBusinessInfo
            // 
            this.tableLayoutPanelBusinessInfo.ColumnCount = 2;
            this.tableLayoutPanelBusinessInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelBusinessInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBusinessInfo.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanelBusinessInfo.Controls.Add(this.label19, 0, 1);
            this.tableLayoutPanelBusinessInfo.Controls.Add(this.label20, 0, 2);
            this.tableLayoutPanelBusinessInfo.Controls.Add(this.lblBizType, 1, 0);
            this.tableLayoutPanelBusinessInfo.Controls.Add(this.lblMenuID, 1, 1);
            this.tableLayoutPanelBusinessInfo.Controls.Add(this.lblSessionId, 1, 2);
            this.tableLayoutPanelBusinessInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelBusinessInfo.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelBusinessInfo.Name = "tableLayoutPanelBusinessInfo";
            this.tableLayoutPanelBusinessInfo.RowCount = 3;
            this.tableLayoutPanelBusinessInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelBusinessInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelBusinessInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelBusinessInfo.Size = new System.Drawing.Size(551, 330);
            this.tableLayoutPanelBusinessInfo.TabIndex = 0;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Location = new System.Drawing.Point(3, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(94, 30);
            this.label18.TabIndex = 0;
            this.label18.Text = "业务类型:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Location = new System.Drawing.Point(3, 30);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(94, 30);
            this.label19.TabIndex = 1;
            this.label19.Text = "菜单ID:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Location = new System.Drawing.Point(3, 60);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(94, 30);
            this.label20.TabIndex = 2;
            this.label20.Text = "会话ID:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBizType
            // 
            this.lblBizType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBizType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBizType.Location = new System.Drawing.Point(103, 0);
            this.lblBizType.Name = "lblBizType";
            this.lblBizType.Size = new System.Drawing.Size(445, 30);
            this.lblBizType.TabIndex = 3;
            this.lblBizType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMenuID
            // 
            this.lblMenuID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMenuID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenuID.Location = new System.Drawing.Point(103, 30);
            this.lblMenuID.Name = "lblMenuID";
            this.lblMenuID.Size = new System.Drawing.Size(445, 30);
            this.lblMenuID.TabIndex = 4;
            this.lblMenuID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSessionId
            // 
            this.lblSessionId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSessionId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSessionId.Location = new System.Drawing.Point(103, 60);
            this.lblSessionId.Name = "lblSessionId";
            this.lblSessionId.Size = new System.Drawing.Size(445, 30);
            this.lblSessionId.TabIndex = 5;
            this.lblSessionId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageOtherInfo
            // 
            this.tabPageOtherInfo.Controls.Add(this.tableLayoutPanelOtherInfo);
            this.tabPageOtherInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageOtherInfo.Name = "tabPageOtherInfo";
            this.tabPageOtherInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOtherInfo.Size = new System.Drawing.Size(557, 336);
            this.tabPageOtherInfo.TabIndex = 5;
            this.tabPageOtherInfo.Text = "其他信息";
            this.tabPageOtherInfo.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelOtherInfo
            // 
            this.tableLayoutPanelOtherInfo.ColumnCount = 2;
            this.tableLayoutPanelOtherInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelOtherInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelOtherInfo.Controls.Add(this.label21, 0, 0);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.label22, 0, 1);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.label23, 0, 2);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.label24, 0, 3);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.label25, 0, 4);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.label26, 0, 5);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.label27, 0, 6);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.label28, 0, 7);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.lblRemark, 1, 0);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.lblHeartbeatCount, 1, 1);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.lblDuration, 1, 2);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.lblExpireTimestamp, 1, 3);
            this.tableLayoutPanelOtherInfo.Controls.Add(this.lblRemainingLockTimeMs, 1, 4);
            this.tableLayoutPanelOtherInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelOtherInfo.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelOtherInfo.Name = "tableLayoutPanelOtherInfo";
            this.tableLayoutPanelOtherInfo.RowCount = 8;
            this.tableLayoutPanelOtherInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelOtherInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelOtherInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelOtherInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelOtherInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelOtherInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelOtherInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelOtherInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelOtherInfo.Size = new System.Drawing.Size(551, 330);
            this.tableLayoutPanelOtherInfo.TabIndex = 0;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label21.Location = new System.Drawing.Point(3, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(94, 30);
            this.label21.TabIndex = 0;
            this.label21.Text = "备注:";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label22.Location = new System.Drawing.Point(3, 30);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(94, 30);
            this.label22.TabIndex = 1;
            this.label22.Text = "心跳次数:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label23.Location = new System.Drawing.Point(3, 60);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(94, 30);
            this.label23.TabIndex = 2;
            this.label23.Text = "时长(秒):";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label24.Location = new System.Drawing.Point(3, 90);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(94, 30);
            this.label24.TabIndex = 3;
            this.label24.Text = "过期时间戳:";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label25.Location = new System.Drawing.Point(3, 120);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(94, 30);
            this.label25.TabIndex = 4;
            this.label25.Text = "剩余锁定时间:";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label26.Location = new System.Drawing.Point(3, 150);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(94, 30);
            this.label26.TabIndex = 5;
            this.label26.Text = "";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label27.Location = new System.Drawing.Point(3, 180);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(94, 30);
            this.label27.TabIndex = 6;
            this.label27.Text = "";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label28.Location = new System.Drawing.Point(3, 210);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(94, 30);
            this.label28.TabIndex = 7;
            this.label28.Text = "";
            // 
            // lblRemark
            // 
            this.lblRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRemark.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemark.Location = new System.Drawing.Point(103, 0);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(445, 30);
            this.lblRemark.TabIndex = 8;
            this.lblRemark.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeartbeatCount
            // 
            this.lblHeartbeatCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeartbeatCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeartbeatCount.Location = new System.Drawing.Point(103, 30);
            this.lblHeartbeatCount.Name = "lblHeartbeatCount";
            this.lblHeartbeatCount.Size = new System.Drawing.Size(445, 30);
            this.lblHeartbeatCount.TabIndex = 9;
            this.lblHeartbeatCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDuration
            // 
            this.lblDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuration.Location = new System.Drawing.Point(103, 60);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(445, 30);
            this.lblDuration.TabIndex = 10;
            this.lblDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExpireTimestamp
            // 
            this.lblExpireTimestamp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExpireTimestamp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExpireTimestamp.Location = new System.Drawing.Point(103, 90);
            this.lblExpireTimestamp.Name = "lblExpireTimestamp";
            this.lblExpireTimestamp.Size = new System.Drawing.Size(445, 30);
            this.lblExpireTimestamp.TabIndex = 11;
            this.lblExpireTimestamp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRemainingLockTimeMs
            // 
            this.lblRemainingLockTimeMs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRemainingLockTimeMs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemainingLockTimeMs.Location = new System.Drawing.Point(103, 120);
            this.lblRemainingLockTimeMs.Name = "lblRemainingLockTimeMs";
            this.lblRemainingLockTimeMs.Size = new System.Drawing.Size(445, 30);
            this.lblRemainingLockTimeMs.TabIndex = 12;
            this.lblRemainingLockTimeMs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LockInfoDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 418);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "LockInfoDetailForm";
            this.Text = "锁定信息详情";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageBasicInfo.ResumeLayout(false);
            this.tableLayoutPanelBasicInfo.ResumeLayout(false);
            this.tableLayoutPanelBasicInfo.PerformLayout();
            this.tabPageUserInfo.ResumeLayout(false);
            this.tableLayoutPanelUserInfo.ResumeLayout(false);
            this.tableLayoutPanelUserInfo.PerformLayout();
            this.tabPageTimeInfo.ResumeLayout(false);
            this.tableLayoutPanelTimeInfo.ResumeLayout(false);
            this.tableLayoutPanelTimeInfo.PerformLayout();
            this.tabPageStatusInfo.ResumeLayout(false);
            this.tableLayoutPanelStatusInfo.ResumeLayout(false);
            this.tableLayoutPanelStatusInfo.PerformLayout();
            this.tabPageBusinessInfo.ResumeLayout(false);
            this.tableLayoutPanelBusinessInfo.ResumeLayout(false);
            this.tableLayoutPanelBusinessInfo.PerformLayout();
            this.tabPageOtherInfo.ResumeLayout(false);
            this.tableLayoutPanelOtherInfo.ResumeLayout(false);
            this.tableLayoutPanelOtherInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageBasicInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBasicInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblLockKey;
        private System.Windows.Forms.Label lblLockId;
        private System.Windows.Forms.Label lblBillID;
        private System.Windows.Forms.Label lblBillNo;
        private System.Windows.Forms.TabPage tabPageUserInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUserInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblLockedUserId;
        private System.Windows.Forms.Label lblLockedUserName;
        private System.Windows.Forms.TabPage tabPageTimeInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTimeInfo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblLockTime;
        private System.Windows.Forms.Label lblExpireTime;
        private System.Windows.Forms.Label lblLastHeartbeat;
        private System.Windows.Forms.Label lblLastUpdateTime;
        private System.Windows.Forms.Label lblDurationText;
        private System.Windows.Forms.TabPage tabPageStatusInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStatusInfo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblIsLocked;
        private System.Windows.Forms.Label lblIsExpired;
        private System.Windows.Forms.Label lblIsOrphaned;
        private System.Windows.Forms.Label lblIsAboutToExpire;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TabPage tabPageBusinessInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBusinessInfo;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblBizType;
        private System.Windows.Forms.Label lblMenuID;
        private System.Windows.Forms.Label lblSessionId;
        private System.Windows.Forms.TabPage tabPageOtherInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOtherInfo;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label lblRemark;
        private System.Windows.Forms.Label lblHeartbeatCount;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblExpireTimestamp;
        private System.Windows.Forms.Label lblRemainingLockTimeMs;
    }
}