<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class messageForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ok_btn = New System.Windows.Forms.Button()
        Me.message_txt = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'ok_btn
        '
        Me.ok_btn.BackgroundImage = Global.mock2Launcher.My.Resources.Resources._136EN
        Me.ok_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ok_btn.FlatAppearance.BorderSize = 0
        Me.ok_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ok_btn.Location = New System.Drawing.Point(185, 94)
        Me.ok_btn.Name = "ok_btn"
        Me.ok_btn.Size = New System.Drawing.Size(150, 26)
        Me.ok_btn.TabIndex = 1
        Me.ok_btn.UseVisualStyleBackColor = True
        '
        'message_txt
        '
        Me.message_txt.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(32, Byte), Integer))
        Me.message_txt.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.message_txt.Cursor = System.Windows.Forms.Cursors.Default
        Me.message_txt.Font = New System.Drawing.Font("Calibri Light", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.message_txt.ForeColor = System.Drawing.Color.Cyan
        Me.message_txt.Location = New System.Drawing.Point(28, 16)
        Me.message_txt.Multiline = True
        Me.message_txt.Name = "message_txt"
        Me.message_txt.ReadOnly = True
        Me.message_txt.ShortcutsEnabled = False
        Me.message_txt.Size = New System.Drawing.Size(462, 70)
        Me.message_txt.TabIndex = 0
        Me.message_txt.Text = "The messages will appear here." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.message_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'messageForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.mock2Launcher.My.Resources.Resources._198__Japanese__Japan__
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(520, 150)
        Me.Controls.Add(Me.ok_btn)
        Me.Controls.Add(Me.message_txt)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "messageForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "messageForm"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents ok_btn As Button
    Friend WithEvents message_txt As TextBox
End Class
