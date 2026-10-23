using System;
using Timur_Tooded;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Timur_Tooded;

partial class Form2
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

    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.Name = "Form2";
        this.ResumeLayout(false);
    }
}