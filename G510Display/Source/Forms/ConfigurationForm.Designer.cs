﻿namespace G510Display.Source.Forms
{
  partial class ConfigurationForm
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
      this.button1 = new System.Windows.Forms.Button();
      this.CalendarItems = new System.Windows.Forms.ListView();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(620, 334);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 0;
      this.button1.Text = "Read";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // CalendarItems
      // 
      this.CalendarItems.GridLines = true;
      this.CalendarItems.Location = new System.Drawing.Point(41, 95);
      this.CalendarItems.Name = "CalendarItems";
      this.CalendarItems.Size = new System.Drawing.Size(333, 104);
      this.CalendarItems.TabIndex = 1;
      this.CalendarItems.UseCompatibleStateImageBehavior = false;
      this.CalendarItems.View = System.Windows.Forms.View.Details;
      // 
      // ConfigurationForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.CalendarItems);
      this.Controls.Add(this.button1);
      this.Name = "ConfigurationForm";
      this.Text = "ConfigurationForm";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.ListView CalendarItems;
  }
}