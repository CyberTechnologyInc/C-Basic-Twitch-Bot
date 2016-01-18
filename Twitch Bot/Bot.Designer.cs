namespace TwitchBot
{
    partial class Bot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.TabControl1 = new System.Windows.Forms.TabControl();
			this.tbPageMain = new System.Windows.Forms.TabPage();
			this.txtCommand = new System.Windows.Forms.TextBox();
			this.cmdSendCommand = new System.Windows.Forms.Button();
			this.logs = new System.Windows.Forms.ListBox();
			this.chkboxAutoStart = new System.Windows.Forms.CheckBox();
			this.cmdStartBot = new System.Windows.Forms.Button();
			this.tbPageSettings = new System.Windows.Forms.TabPage();
			this.txtBotPassword = new System.Windows.Forms.TextBox();
			this.lblBotPassword = new System.Windows.Forms.Label();
			this.lblBotUsername = new System.Windows.Forms.Label();
			this.txtBotUsername = new System.Windows.Forms.TextBox();
			this.chkboxPurgeNonSubsLinks = new System.Windows.Forms.CheckBox();
			this.chkboxLogChatMessages = new System.Windows.Forms.CheckBox();
			this.txtLeavingMessage = new System.Windows.Forms.TextBox();
			this.lblLeavingMessage = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.txtCooldownTime = new System.Windows.Forms.TextBox();
			this.txtWelcomeMessage = new System.Windows.Forms.TextBox();
			this.lblWelcomeMessage = new System.Windows.Forms.Label();
			this.btnSaveSettings = new System.Windows.Forms.Button();
			this.chkboxShowAPICode = new System.Windows.Forms.CheckBox();
			this.lblApiCode = new System.Windows.Forms.Label();
			this.txtAPICode = new System.Windows.Forms.TextBox();
			this.lblChannel = new System.Windows.Forms.Label();
			this.txtChannel = new System.Windows.Forms.TextBox();
			this.tbPageGiveaway = new System.Windows.Forms.TabPage();
			this.gpboxGiveaways = new System.Windows.Forms.GroupBox();
			this.lblGiveawayLengthProgress = new System.Windows.Forms.Label();
			this.gpbxGiveawaySettings = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtGiveawayPointsEarnedPerX = new System.Windows.Forms.TextBox();
			this.gpboxCurrentGiveawaySettings = new System.Windows.Forms.GroupBox();
			this.lblGiveawayAmountOfGiveawayItems = new System.Windows.Forms.Label();
			this.txtGiveawayPointsRequired = new System.Windows.Forms.TextBox();
			this.txtGiveawayAmountOfGiveawayItems = new System.Windows.Forms.TextBox();
			this.lblGiveawayPointsRequired = new System.Windows.Forms.Label();
			this.lblGiveawayLength = new System.Windows.Forms.Label();
			this.txtGiveawayLength = new System.Windows.Forms.TextBox();
			this.lblGiveawayProgress = new System.Windows.Forms.Label();
			this.cmdGiveawayStart = new System.Windows.Forms.Button();
			this.giveawayEnteredUsers = new System.Windows.Forms.ListBox();
			this.tbPageCommands = new System.Windows.Forms.TabPage();
			this.lstViewCommands = new System.Windows.Forms.ListView();
			this.chCommand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chPrivileges = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.TabControl1.SuspendLayout();
			this.tbPageMain.SuspendLayout();
			this.tbPageSettings.SuspendLayout();
			this.tbPageGiveaway.SuspendLayout();
			this.gpboxGiveaways.SuspendLayout();
			this.gpbxGiveawaySettings.SuspendLayout();
			this.gpboxCurrentGiveawaySettings.SuspendLayout();
			this.tbPageCommands.SuspendLayout();
			this.SuspendLayout();
			// 
			// TabControl1
			// 
			this.TabControl1.Controls.Add(this.tbPageMain);
			this.TabControl1.Controls.Add(this.tbPageSettings);
			this.TabControl1.Controls.Add(this.tbPageGiveaway);
			this.TabControl1.Controls.Add(this.tbPageCommands);
			this.TabControl1.Location = new System.Drawing.Point(0, 0);
			this.TabControl1.Multiline = true;
			this.TabControl1.Name = "TabControl1";
			this.TabControl1.SelectedIndex = 0;
			this.TabControl1.Size = new System.Drawing.Size(615, 326);
			this.TabControl1.TabIndex = 4;
			// 
			// tbPageMain
			// 
			this.tbPageMain.Controls.Add(this.txtCommand);
			this.tbPageMain.Controls.Add(this.cmdSendCommand);
			this.tbPageMain.Controls.Add(this.logs);
			this.tbPageMain.Controls.Add(this.chkboxAutoStart);
			this.tbPageMain.Controls.Add(this.cmdStartBot);
			this.tbPageMain.Location = new System.Drawing.Point(4, 22);
			this.tbPageMain.Name = "tbPageMain";
			this.tbPageMain.Padding = new System.Windows.Forms.Padding(3);
			this.tbPageMain.Size = new System.Drawing.Size(607, 300);
			this.tbPageMain.TabIndex = 0;
			this.tbPageMain.Text = "Main";
			this.tbPageMain.UseVisualStyleBackColor = true;
			// 
			// txtCommand
			// 
			this.txtCommand.Location = new System.Drawing.Point(87, 275);
			this.txtCommand.Name = "txtCommand";
			this.txtCommand.Size = new System.Drawing.Size(428, 20);
			this.txtCommand.TabIndex = 4;
			this.txtCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCommand_KeyDown);
			// 
			// cmdSendCommand
			// 
			this.cmdSendCommand.Location = new System.Drawing.Point(6, 273);
			this.cmdSendCommand.Name = "cmdSendCommand";
			this.cmdSendCommand.Size = new System.Drawing.Size(75, 23);
			this.cmdSendCommand.TabIndex = 3;
			this.cmdSendCommand.Text = "Send";
			this.cmdSendCommand.UseVisualStyleBackColor = true;
			this.cmdSendCommand.Click += new System.EventHandler(this.cmdSendCommand_Click);
			// 
			// logs
			// 
			this.logs.FormattingEnabled = true;
			this.logs.HorizontalScrollbar = true;
			this.logs.Location = new System.Drawing.Point(6, 6);
			this.logs.Name = "logs";
			this.logs.Size = new System.Drawing.Size(509, 264);
			this.logs.TabIndex = 0;
			// 
			// chkboxAutoStart
			// 
			this.chkboxAutoStart.AutoSize = true;
			this.chkboxAutoStart.Location = new System.Drawing.Point(521, 35);
			this.chkboxAutoStart.Name = "chkboxAutoStart";
			this.chkboxAutoStart.Size = new System.Drawing.Size(73, 30);
			this.chkboxAutoStart.TabIndex = 2;
			this.chkboxAutoStart.Text = "Start bot\r\non startup";
			this.chkboxAutoStart.UseVisualStyleBackColor = true;
			this.chkboxAutoStart.CheckedChanged += new System.EventHandler(this.chkboxAutoStart_CheckedChanged);
			// 
			// cmdStartBot
			// 
			this.cmdStartBot.Location = new System.Drawing.Point(521, 6);
			this.cmdStartBot.Name = "cmdStartBot";
			this.cmdStartBot.Size = new System.Drawing.Size(77, 23);
			this.cmdStartBot.TabIndex = 1;
			this.cmdStartBot.Text = "Start Bot";
			this.cmdStartBot.UseVisualStyleBackColor = true;
			this.cmdStartBot.Click += new System.EventHandler(this.cmdStartBot_Click);
			// 
			// tbPageSettings
			// 
			this.tbPageSettings.Controls.Add(this.txtBotPassword);
			this.tbPageSettings.Controls.Add(this.lblBotPassword);
			this.tbPageSettings.Controls.Add(this.lblBotUsername);
			this.tbPageSettings.Controls.Add(this.txtBotUsername);
			this.tbPageSettings.Controls.Add(this.chkboxPurgeNonSubsLinks);
			this.tbPageSettings.Controls.Add(this.chkboxLogChatMessages);
			this.tbPageSettings.Controls.Add(this.txtLeavingMessage);
			this.tbPageSettings.Controls.Add(this.lblLeavingMessage);
			this.tbPageSettings.Controls.Add(this.Label1);
			this.tbPageSettings.Controls.Add(this.txtCooldownTime);
			this.tbPageSettings.Controls.Add(this.txtWelcomeMessage);
			this.tbPageSettings.Controls.Add(this.lblWelcomeMessage);
			this.tbPageSettings.Controls.Add(this.btnSaveSettings);
			this.tbPageSettings.Controls.Add(this.chkboxShowAPICode);
			this.tbPageSettings.Controls.Add(this.lblApiCode);
			this.tbPageSettings.Controls.Add(this.txtAPICode);
			this.tbPageSettings.Controls.Add(this.lblChannel);
			this.tbPageSettings.Controls.Add(this.txtChannel);
			this.tbPageSettings.Location = new System.Drawing.Point(4, 22);
			this.tbPageSettings.Name = "tbPageSettings";
			this.tbPageSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tbPageSettings.Size = new System.Drawing.Size(607, 300);
			this.tbPageSettings.TabIndex = 1;
			this.tbPageSettings.Text = "Settings";
			this.tbPageSettings.UseVisualStyleBackColor = true;
			// 
			// txtBotPassword
			// 
			this.txtBotPassword.Location = new System.Drawing.Point(91, 95);
			this.txtBotPassword.Name = "txtBotPassword";
			this.txtBotPassword.Size = new System.Drawing.Size(182, 20);
			this.txtBotPassword.TabIndex = 17;
			this.txtBotPassword.UseSystemPasswordChar = true;
			// 
			// lblBotPassword
			// 
			this.lblBotPassword.AutoSize = true;
			this.lblBotPassword.Location = new System.Drawing.Point(13, 98);
			this.lblBotPassword.Name = "lblBotPassword";
			this.lblBotPassword.Size = new System.Drawing.Size(72, 13);
			this.lblBotPassword.TabIndex = 16;
			this.lblBotPassword.Text = "Bot Password";
			// 
			// lblBotUsername
			// 
			this.lblBotUsername.AutoSize = true;
			this.lblBotUsername.Location = new System.Drawing.Point(11, 75);
			this.lblBotUsername.Name = "lblBotUsername";
			this.lblBotUsername.Size = new System.Drawing.Size(74, 13);
			this.lblBotUsername.TabIndex = 15;
			this.lblBotUsername.Text = "Bot Username";
			// 
			// txtBotUsername
			// 
			this.txtBotUsername.Location = new System.Drawing.Point(91, 72);
			this.txtBotUsername.Name = "txtBotUsername";
			this.txtBotUsername.Size = new System.Drawing.Size(182, 20);
			this.txtBotUsername.TabIndex = 14;
			// 
			// chkboxPurgeNonSubsLinks
			// 
			this.chkboxPurgeNonSubsLinks.AutoSize = true;
			this.chkboxPurgeNonSubsLinks.Checked = true;
			this.chkboxPurgeNonSubsLinks.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkboxPurgeNonSubsLinks.Location = new System.Drawing.Point(6, 223);
			this.chkboxPurgeNonSubsLinks.Name = "chkboxPurgeNonSubsLinks";
			this.chkboxPurgeNonSubsLinks.Size = new System.Drawing.Size(146, 17);
			this.chkboxPurgeNonSubsLinks.TabIndex = 13;
			this.chkboxPurgeNonSubsLinks.Text = "Purge any non-sub\'s links";
			this.chkboxPurgeNonSubsLinks.UseVisualStyleBackColor = true;
			// 
			// chkboxLogChatMessages
			// 
			this.chkboxLogChatMessages.AutoSize = true;
			this.chkboxLogChatMessages.Checked = true;
			this.chkboxLogChatMessages.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkboxLogChatMessages.Location = new System.Drawing.Point(6, 246);
			this.chkboxLogChatMessages.Name = "chkboxLogChatMessages";
			this.chkboxLogChatMessages.Size = new System.Drawing.Size(118, 17);
			this.chkboxLogChatMessages.TabIndex = 12;
			this.chkboxLogChatMessages.Text = "Log chat messages";
			this.chkboxLogChatMessages.UseVisualStyleBackColor = true;
			this.chkboxLogChatMessages.CheckedChanged += new System.EventHandler(this.chkboxLogChatMessages_CheckedChanged);
			// 
			// txtLeavingMessage
			// 
			this.txtLeavingMessage.Location = new System.Drawing.Point(432, 72);
			this.txtLeavingMessage.Multiline = true;
			this.txtLeavingMessage.Name = "txtLeavingMessage";
			this.txtLeavingMessage.Size = new System.Drawing.Size(146, 60);
			this.txtLeavingMessage.TabIndex = 11;
			this.txtLeavingMessage.Text = "Bai guis :c";
			// 
			// lblLeavingMessage
			// 
			this.lblLeavingMessage.AutoSize = true;
			this.lblLeavingMessage.Location = new System.Drawing.Point(328, 72);
			this.lblLeavingMessage.Name = "lblLeavingMessage";
			this.lblLeavingMessage.Size = new System.Drawing.Size(91, 13);
			this.lblLeavingMessage.TabIndex = 10;
			this.lblLeavingMessage.Text = "Leaving Message";
			// 
			// Label1
			// 
			this.Label1.AutoSize = true;
			this.Label1.Location = new System.Drawing.Point(4, 35);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(81, 13);
			this.Label1.TabIndex = 9;
			this.Label1.Text = "CMD Cooldown";
			// 
			// txtCooldownTime
			// 
			this.txtCooldownTime.Location = new System.Drawing.Point(91, 32);
			this.txtCooldownTime.Name = "txtCooldownTime";
			this.txtCooldownTime.Size = new System.Drawing.Size(182, 20);
			this.txtCooldownTime.TabIndex = 8;
			this.txtCooldownTime.Text = "0";
			// 
			// txtWelcomeMessage
			// 
			this.txtWelcomeMessage.Location = new System.Drawing.Point(433, 6);
			this.txtWelcomeMessage.Multiline = true;
			this.txtWelcomeMessage.Name = "txtWelcomeMessage";
			this.txtWelcomeMessage.Size = new System.Drawing.Size(146, 60);
			this.txtWelcomeMessage.TabIndex = 7;
			this.txtWelcomeMessage.Text = "Guess who\'s here :3";
			// 
			// lblWelcomeMessage
			// 
			this.lblWelcomeMessage.AutoSize = true;
			this.lblWelcomeMessage.Location = new System.Drawing.Point(329, 6);
			this.lblWelcomeMessage.Name = "lblWelcomeMessage";
			this.lblWelcomeMessage.Size = new System.Drawing.Size(98, 13);
			this.lblWelcomeMessage.TabIndex = 6;
			this.lblWelcomeMessage.Text = "Welcome Message";
			// 
			// btnSaveSettings
			// 
			this.btnSaveSettings.Location = new System.Drawing.Point(6, 270);
			this.btnSaveSettings.Name = "btnSaveSettings";
			this.btnSaveSettings.Size = new System.Drawing.Size(595, 23);
			this.btnSaveSettings.TabIndex = 5;
			this.btnSaveSettings.Text = "Save Settings";
			this.btnSaveSettings.UseVisualStyleBackColor = true;
			this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
			// 
			// chkboxShowAPICode
			// 
			this.chkboxShowAPICode.AutoSize = true;
			this.chkboxShowAPICode.Location = new System.Drawing.Point(583, 250);
			this.chkboxShowAPICode.Name = "chkboxShowAPICode";
			this.chkboxShowAPICode.Size = new System.Drawing.Size(15, 14);
			this.chkboxShowAPICode.TabIndex = 4;
			this.chkboxShowAPICode.UseVisualStyleBackColor = true;
			this.chkboxShowAPICode.Visible = false;
			this.chkboxShowAPICode.CheckedChanged += new System.EventHandler(this.chkboxShowAPICode_CheckedChanged);
			// 
			// lblApiCode
			// 
			this.lblApiCode.AutoSize = true;
			this.lblApiCode.Location = new System.Drawing.Point(358, 250);
			this.lblApiCode.Name = "lblApiCode";
			this.lblApiCode.Size = new System.Drawing.Size(52, 13);
			this.lblApiCode.TabIndex = 3;
			this.lblApiCode.Text = "API Code";
			this.lblApiCode.Visible = false;
			// 
			// txtAPICode
			// 
			this.txtAPICode.Location = new System.Drawing.Point(416, 247);
			this.txtAPICode.Name = "txtAPICode";
			this.txtAPICode.ReadOnly = true;
			this.txtAPICode.Size = new System.Drawing.Size(161, 20);
			this.txtAPICode.TabIndex = 2;
			this.txtAPICode.UseSystemPasswordChar = true;
			this.txtAPICode.Visible = false;
			// 
			// lblChannel
			// 
			this.lblChannel.AutoSize = true;
			this.lblChannel.Location = new System.Drawing.Point(39, 9);
			this.lblChannel.Name = "lblChannel";
			this.lblChannel.Size = new System.Drawing.Size(46, 13);
			this.lblChannel.TabIndex = 1;
			this.lblChannel.Text = "Channel";
			// 
			// txtChannel
			// 
			this.txtChannel.Location = new System.Drawing.Point(91, 6);
			this.txtChannel.Name = "txtChannel";
			this.txtChannel.Size = new System.Drawing.Size(182, 20);
			this.txtChannel.TabIndex = 0;
			this.txtChannel.Tag = "";
			this.txtChannel.Text = "manseld";
			// 
			// tbPageGiveaway
			// 
			this.tbPageGiveaway.Controls.Add(this.gpboxGiveaways);
			this.tbPageGiveaway.Location = new System.Drawing.Point(4, 22);
			this.tbPageGiveaway.Name = "tbPageGiveaway";
			this.tbPageGiveaway.Padding = new System.Windows.Forms.Padding(3);
			this.tbPageGiveaway.Size = new System.Drawing.Size(607, 300);
			this.tbPageGiveaway.TabIndex = 2;
			this.tbPageGiveaway.Text = "Custom Commands";
			this.tbPageGiveaway.UseVisualStyleBackColor = true;
			// 
			// gpboxGiveaways
			// 
			this.gpboxGiveaways.Controls.Add(this.lblGiveawayLengthProgress);
			this.gpboxGiveaways.Controls.Add(this.gpbxGiveawaySettings);
			this.gpboxGiveaways.Controls.Add(this.gpboxCurrentGiveawaySettings);
			this.gpboxGiveaways.Controls.Add(this.lblGiveawayProgress);
			this.gpboxGiveaways.Controls.Add(this.cmdGiveawayStart);
			this.gpboxGiveaways.Controls.Add(this.giveawayEnteredUsers);
			this.gpboxGiveaways.Location = new System.Drawing.Point(6, 6);
			this.gpboxGiveaways.Name = "gpboxGiveaways";
			this.gpboxGiveaways.Size = new System.Drawing.Size(597, 285);
			this.gpboxGiveaways.TabIndex = 0;
			this.gpboxGiveaways.TabStop = false;
			this.gpboxGiveaways.Text = "Giveaway";
			// 
			// lblGiveawayLengthProgress
			// 
			this.lblGiveawayLengthProgress.AutoSize = true;
			this.lblGiveawayLengthProgress.Location = new System.Drawing.Point(191, 227);
			this.lblGiveawayLengthProgress.Name = "lblGiveawayLengthProgress";
			this.lblGiveawayLengthProgress.Size = new System.Drawing.Size(145, 13);
			this.lblGiveawayLengthProgress.TabIndex = 21;
			this.lblGiveawayLengthProgress.Text = "60 seconds left on giveaway.";
			this.lblGiveawayLengthProgress.Visible = false;
			// 
			// gpbxGiveawaySettings
			// 
			this.gpbxGiveawaySettings.Controls.Add(this.label3);
			this.gpbxGiveawaySettings.Controls.Add(this.txtGiveawayPointsEarnedPerX);
			this.gpbxGiveawaySettings.Location = new System.Drawing.Point(417, 14);
			this.gpbxGiveawaySettings.Name = "gpbxGiveawaySettings";
			this.gpbxGiveawaySettings.Size = new System.Drawing.Size(174, 46);
			this.gpbxGiveawaySettings.TabIndex = 20;
			this.gpbxGiveawaySettings.TabStop = false;
			this.gpbxGiveawaySettings.Text = "General Giveaway Settings";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 13);
			this.label3.TabIndex = 24;
			this.label3.Text = "Points per minute";
			// 
			// txtGiveawayPointsEarnedPerX
			// 
			this.txtGiveawayPointsEarnedPerX.Location = new System.Drawing.Point(100, 17);
			this.txtGiveawayPointsEarnedPerX.Name = "txtGiveawayPointsEarnedPerX";
			this.txtGiveawayPointsEarnedPerX.Size = new System.Drawing.Size(68, 20);
			this.txtGiveawayPointsEarnedPerX.TabIndex = 23;
			this.txtGiveawayPointsEarnedPerX.Text = "5";
			// 
			// gpboxCurrentGiveawaySettings
			// 
			this.gpboxCurrentGiveawaySettings.Controls.Add(this.lblGiveawayAmountOfGiveawayItems);
			this.gpboxCurrentGiveawaySettings.Controls.Add(this.txtGiveawayPointsRequired);
			this.gpboxCurrentGiveawaySettings.Controls.Add(this.txtGiveawayAmountOfGiveawayItems);
			this.gpboxCurrentGiveawaySettings.Controls.Add(this.lblGiveawayPointsRequired);
			this.gpboxCurrentGiveawaySettings.Controls.Add(this.lblGiveawayLength);
			this.gpboxCurrentGiveawaySettings.Controls.Add(this.txtGiveawayLength);
			this.gpboxCurrentGiveawaySettings.Location = new System.Drawing.Point(194, 14);
			this.gpboxCurrentGiveawaySettings.Name = "gpboxCurrentGiveawaySettings";
			this.gpboxCurrentGiveawaySettings.Size = new System.Drawing.Size(217, 100);
			this.gpboxCurrentGiveawaySettings.TabIndex = 11;
			this.gpboxCurrentGiveawaySettings.TabStop = false;
			this.gpboxCurrentGiveawaySettings.Text = "Current Giveaway Settings";
			// 
			// lblGiveawayAmountOfGiveawayItems
			// 
			this.lblGiveawayAmountOfGiveawayItems.AutoSize = true;
			this.lblGiveawayAmountOfGiveawayItems.Location = new System.Drawing.Point(13, 20);
			this.lblGiveawayAmountOfGiveawayItems.Name = "lblGiveawayAmountOfGiveawayItems";
			this.lblGiveawayAmountOfGiveawayItems.Size = new System.Drawing.Size(144, 13);
			this.lblGiveawayAmountOfGiveawayItems.TabIndex = 10;
			this.lblGiveawayAmountOfGiveawayItems.Text = "How many items to giveaway";
			// 
			// txtGiveawayPointsRequired
			// 
			this.txtGiveawayPointsRequired.Location = new System.Drawing.Point(163, 69);
			this.txtGiveawayPointsRequired.Name = "txtGiveawayPointsRequired";
			this.txtGiveawayPointsRequired.Size = new System.Drawing.Size(48, 20);
			this.txtGiveawayPointsRequired.TabIndex = 1;
			this.txtGiveawayPointsRequired.Text = "150";
			// 
			// txtGiveawayAmountOfGiveawayItems
			// 
			this.txtGiveawayAmountOfGiveawayItems.Location = new System.Drawing.Point(163, 17);
			this.txtGiveawayAmountOfGiveawayItems.Name = "txtGiveawayAmountOfGiveawayItems";
			this.txtGiveawayAmountOfGiveawayItems.Size = new System.Drawing.Size(48, 20);
			this.txtGiveawayAmountOfGiveawayItems.TabIndex = 9;
			this.txtGiveawayAmountOfGiveawayItems.Text = "1";
			// 
			// lblGiveawayPointsRequired
			// 
			this.lblGiveawayPointsRequired.AutoSize = true;
			this.lblGiveawayPointsRequired.Location = new System.Drawing.Point(41, 72);
			this.lblGiveawayPointsRequired.Name = "lblGiveawayPointsRequired";
			this.lblGiveawayPointsRequired.Size = new System.Drawing.Size(116, 13);
			this.lblGiveawayPointsRequired.TabIndex = 2;
			this.lblGiveawayPointsRequired.Text = "Points required to enter";
			// 
			// lblGiveawayLength
			// 
			this.lblGiveawayLength.AutoSize = true;
			this.lblGiveawayLength.Location = new System.Drawing.Point(25, 46);
			this.lblGiveawayLength.Name = "lblGiveawayLength";
			this.lblGiveawayLength.Size = new System.Drawing.Size(132, 13);
			this.lblGiveawayLength.TabIndex = 6;
			this.lblGiveawayLength.Text = "Giveaway length (Minutes)";
			// 
			// txtGiveawayLength
			// 
			this.txtGiveawayLength.Location = new System.Drawing.Point(163, 43);
			this.txtGiveawayLength.Name = "txtGiveawayLength";
			this.txtGiveawayLength.Size = new System.Drawing.Size(48, 20);
			this.txtGiveawayLength.TabIndex = 7;
			this.txtGiveawayLength.Text = "3";
			// 
			// lblGiveawayProgress
			// 
			this.lblGiveawayProgress.AutoSize = true;
			this.lblGiveawayProgress.Location = new System.Drawing.Point(191, 240);
			this.lblGiveawayProgress.Name = "lblGiveawayProgress";
			this.lblGiveawayProgress.Size = new System.Drawing.Size(158, 13);
			this.lblGiveawayProgress.TabIndex = 8;
			this.lblGiveawayProgress.Text = "A giveaway hasn\'t been started.";
			// 
			// cmdGiveawayStart
			// 
			this.cmdGiveawayStart.Location = new System.Drawing.Point(191, 256);
			this.cmdGiveawayStart.Name = "cmdGiveawayStart";
			this.cmdGiveawayStart.Size = new System.Drawing.Size(400, 23);
			this.cmdGiveawayStart.TabIndex = 3;
			this.cmdGiveawayStart.Text = "Start Giveaway";
			this.cmdGiveawayStart.UseVisualStyleBackColor = true;
			this.cmdGiveawayStart.Click += new System.EventHandler(this.cmdGiveawayStart_Click);
			// 
			// giveawayEnteredUsers
			// 
			this.giveawayEnteredUsers.FormattingEnabled = true;
			this.giveawayEnteredUsers.HorizontalScrollbar = true;
			this.giveawayEnteredUsers.Location = new System.Drawing.Point(6, 14);
			this.giveawayEnteredUsers.Name = "giveawayEnteredUsers";
			this.giveawayEnteredUsers.Size = new System.Drawing.Size(179, 264);
			this.giveawayEnteredUsers.TabIndex = 0;
			// 
			// tbPageCommands
			// 
			this.tbPageCommands.Controls.Add(this.lstViewCommands);
			this.tbPageCommands.Location = new System.Drawing.Point(4, 22);
			this.tbPageCommands.Name = "tbPageCommands";
			this.tbPageCommands.Padding = new System.Windows.Forms.Padding(3);
			this.tbPageCommands.Size = new System.Drawing.Size(607, 300);
			this.tbPageCommands.TabIndex = 3;
			this.tbPageCommands.Text = "Commands";
			this.tbPageCommands.UseVisualStyleBackColor = true;
			// 
			// lstViewCommands
			// 
			this.lstViewCommands.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chCommand,
            this.chDescription,
            this.chPrivileges});
			this.lstViewCommands.FullRowSelect = true;
			this.lstViewCommands.GridLines = true;
			this.lstViewCommands.Location = new System.Drawing.Point(0, 0);
			this.lstViewCommands.Name = "lstViewCommands";
			this.lstViewCommands.Size = new System.Drawing.Size(607, 300);
			this.lstViewCommands.TabIndex = 0;
			this.lstViewCommands.UseCompatibleStateImageBehavior = false;
			this.lstViewCommands.View = System.Windows.Forms.View.Details;
			// 
			// chCommand
			// 
			this.chCommand.Text = "Command";
			this.chCommand.Width = 226;
			// 
			// chDescription
			// 
			this.chDescription.Text = "Description";
			this.chDescription.Width = 288;
			// 
			// chPrivileges
			// 
			this.chPrivileges.Text = "Privileges";
			this.chPrivileges.Width = 77;
			// 
			// Bot
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(614, 325);
			this.Controls.Add(this.TabControl1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Bot";
			this.ShowIcon = false;
			this.Text = "Twitch Bot";
			this.Load += new System.EventHandler(this.Bot_Load);
			this.TabControl1.ResumeLayout(false);
			this.tbPageMain.ResumeLayout(false);
			this.tbPageMain.PerformLayout();
			this.tbPageSettings.ResumeLayout(false);
			this.tbPageSettings.PerformLayout();
			this.tbPageGiveaway.ResumeLayout(false);
			this.gpboxGiveaways.ResumeLayout(false);
			this.gpboxGiveaways.PerformLayout();
			this.gpbxGiveawaySettings.ResumeLayout(false);
			this.gpbxGiveawaySettings.PerformLayout();
			this.gpboxCurrentGiveawaySettings.ResumeLayout(false);
			this.gpboxCurrentGiveawaySettings.PerformLayout();
			this.tbPageCommands.ResumeLayout(false);
			this.ResumeLayout(false);

        }

		#endregion

		internal System.Windows.Forms.TabControl TabControl1;
		internal System.Windows.Forms.TabPage tbPageMain;
		internal System.Windows.Forms.TextBox txtCommand;
		internal System.Windows.Forms.Button cmdSendCommand;
		internal System.Windows.Forms.ListBox logs;
		internal System.Windows.Forms.CheckBox chkboxAutoStart;
		internal System.Windows.Forms.Button cmdStartBot;
		internal System.Windows.Forms.TabPage tbPageSettings;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.TextBox txtCooldownTime;
		internal System.Windows.Forms.TextBox txtWelcomeMessage;
		internal System.Windows.Forms.Label lblWelcomeMessage;
		internal System.Windows.Forms.Button btnSaveSettings;
		internal System.Windows.Forms.CheckBox chkboxShowAPICode;
		internal System.Windows.Forms.Label lblApiCode;
		internal System.Windows.Forms.TextBox txtAPICode;
		internal System.Windows.Forms.Label lblChannel;
		internal System.Windows.Forms.TextBox txtChannel;
		private System.Windows.Forms.TabPage tbPageGiveaway;
		internal System.Windows.Forms.TextBox txtLeavingMessage;
		internal System.Windows.Forms.Label lblLeavingMessage;
		private System.Windows.Forms.GroupBox gpboxGiveaways;
		private System.Windows.Forms.ListBox giveawayEnteredUsers;
		private System.Windows.Forms.TextBox txtGiveawayPointsRequired;
		private System.Windows.Forms.Label lblGiveawayPointsRequired;
		private System.Windows.Forms.Button cmdGiveawayStart;
		private System.Windows.Forms.TextBox txtGiveawayLength;
		private System.Windows.Forms.Label lblGiveawayLength;
		private System.Windows.Forms.Label lblGiveawayProgress;
		private System.Windows.Forms.Label lblGiveawayAmountOfGiveawayItems;
		private System.Windows.Forms.TextBox txtGiveawayAmountOfGiveawayItems;
		private System.Windows.Forms.GroupBox gpboxCurrentGiveawaySettings;
		private System.Windows.Forms.GroupBox gpbxGiveawaySettings;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtGiveawayPointsEarnedPerX;
		private System.Windows.Forms.Label lblGiveawayLengthProgress;
		private System.Windows.Forms.CheckBox chkboxLogChatMessages;
		private System.Windows.Forms.CheckBox chkboxPurgeNonSubsLinks;
		private System.Windows.Forms.TextBox txtBotPassword;
		private System.Windows.Forms.Label lblBotPassword;
		private System.Windows.Forms.Label lblBotUsername;
		private System.Windows.Forms.TextBox txtBotUsername;
		private System.Windows.Forms.TabPage tbPageCommands;
		private System.Windows.Forms.ListView lstViewCommands;
		private System.Windows.Forms.ColumnHeader chCommand;
		private System.Windows.Forms.ColumnHeader chDescription;
		private System.Windows.Forms.ColumnHeader chPrivileges;
	}
}

