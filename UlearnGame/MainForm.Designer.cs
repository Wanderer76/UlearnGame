
namespace UlearnGame
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.upgradePanel = new System.Windows.Forms.TableLayoutPanel();
            this.upgradeDamageButton = new System.Windows.Forms.Button();
            this.upgradeSpeedButton = new System.Windows.Forms.Button();
            this.fillHealthButton = new System.Windows.Forms.Button();
            this.fillArmorButton = new System.Windows.Forms.Button();
            this.upgradePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // upgradePanel
            // 
            this.upgradePanel.AutoSize = true;
            this.upgradePanel.ColumnCount = 2;
            this.upgradePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.upgradePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.upgradePanel.Controls.Add(this.upgradeDamageButton, 0, 0);
            this.upgradePanel.Controls.Add(this.upgradeSpeedButton, 1, 0);
            this.upgradePanel.Controls.Add(this.fillHealthButton, 0, 1);
            this.upgradePanel.Controls.Add(this.fillArmorButton, 1, 1);
            this.upgradePanel.Location = new System.Drawing.Point(69, 108);
            this.upgradePanel.Name = "upgradePanel";
            this.upgradePanel.RowCount = 2;
            this.upgradePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.upgradePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.upgradePanel.Size = new System.Drawing.Size(558, 398);
            this.upgradePanel.TabIndex = 0;
            this.upgradePanel.Visible = false;
            // 
            // upgradeDamageButton
            // 
            this.upgradeDamageButton.AutoSize = true;
            this.upgradeDamageButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upgradeDamageButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.upgradeDamageButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.upgradeDamageButton.Location = new System.Drawing.Point(3, 3);
            this.upgradeDamageButton.Name = "upgradeDamageButton";
            this.upgradeDamageButton.Size = new System.Drawing.Size(273, 193);
            this.upgradeDamageButton.TabIndex = 0;
            this.upgradeDamageButton.Text = "Upgrade Damage";
            this.upgradeDamageButton.UseVisualStyleBackColor = true;
            // 
            // upgradeSpeedButton
            // 
            this.upgradeSpeedButton.AutoSize = true;
            this.upgradeSpeedButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upgradeSpeedButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.upgradeSpeedButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.upgradeSpeedButton.Location = new System.Drawing.Point(282, 3);
            this.upgradeSpeedButton.Name = "upgradeSpeedButton";
            this.upgradeSpeedButton.Size = new System.Drawing.Size(273, 193);
            this.upgradeSpeedButton.TabIndex = 1;
            this.upgradeSpeedButton.Text = "Upgrade Missles Speed";
            this.upgradeSpeedButton.UseVisualStyleBackColor = true;
            // 
            // fillHealthButton
            // 
            this.fillHealthButton.AutoSize = true;
            this.fillHealthButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillHealthButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fillHealthButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fillHealthButton.Location = new System.Drawing.Point(3, 202);
            this.fillHealthButton.Name = "fillHealthButton";
            this.fillHealthButton.Size = new System.Drawing.Size(273, 193);
            this.fillHealthButton.TabIndex = 2;
            this.fillHealthButton.Text = "Fill Health";
            this.fillHealthButton.UseVisualStyleBackColor = true;
            // 
            // fillArmorButton
            // 
            this.fillArmorButton.AutoSize = true;
            this.fillArmorButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillArmorButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fillArmorButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fillArmorButton.Location = new System.Drawing.Point(282, 202);
            this.fillArmorButton.Name = "fillArmorButton";
            this.fillArmorButton.Size = new System.Drawing.Size(273, 193);
            this.fillArmorButton.TabIndex = 3;
            this.fillArmorButton.Text = "Fill Armor";
            this.fillArmorButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.ClientSize = new System.Drawing.Size(704, 681);
            this.Controls.Add(this.upgradePanel);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.upgradePanel.ResumeLayout(false);
            this.upgradePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button upgradeDamageButton;
        private System.Windows.Forms.Button upgradeSpeedButton;
        private System.Windows.Forms.Button fillHealthButton;
        private System.Windows.Forms.Button fillArmorButton;
        public System.Windows.Forms.TableLayoutPanel upgradePanel;
    }
}

