<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ApplicationUpdater
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ApplicationUpdater))
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.WorkingLabel = New System.Windows.Forms.Label()
        Me.website_btn = New System.Windows.Forms.Button()
        Me.quit_btn = New System.Windows.Forms.Button()
        Me.bytsIn_lbl = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.totalbyts_lbl = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(144, 38)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(282, 17)
        Me.ProgressBar1.Step = 5
        Me.ProgressBar1.TabIndex = 0
        '
        'WorkingLabel
        '
        Me.WorkingLabel.AutoSize = True
        Me.WorkingLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(32, Byte), Integer))
        Me.WorkingLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.WorkingLabel.Font = New System.Drawing.Font("Yu Gothic Medium", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WorkingLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(159, Byte), Integer), CType(CType(221, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.WorkingLabel.Location = New System.Drawing.Point(26, 38)
        Me.WorkingLabel.Name = "WorkingLabel"
        Me.WorkingLabel.Size = New System.Drawing.Size(112, 17)
        Me.WorkingLabel.TabIndex = 1
        Me.WorkingLabel.Text = "Download Status"
        '
        'website_btn
        '
        Me.website_btn.BackgroundImage = Global.mock2Launcher.My.Resources.Resources._145EN
        Me.website_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.website_btn.FlatAppearance.BorderSize = 0
        Me.website_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.website_btn.Location = New System.Drawing.Point(437, 33)
        Me.website_btn.Name = "website_btn"
        Me.website_btn.Size = New System.Drawing.Size(151, 26)
        Me.website_btn.TabIndex = 4
        Me.website_btn.UseVisualStyleBackColor = True
        '
        'quit_btn
        '
        Me.quit_btn.BackgroundImage = CType(resources.GetObject("quit_btn.BackgroundImage"), System.Drawing.Image)
        Me.quit_btn.FlatAppearance.BorderSize = 0
        Me.quit_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.quit_btn.Location = New System.Drawing.Point(564, 7)
        Me.quit_btn.MaximumSize = New System.Drawing.Size(29, 17)
        Me.quit_btn.MinimumSize = New System.Drawing.Size(29, 17)
        Me.quit_btn.Name = "quit_btn"
        Me.quit_btn.Size = New System.Drawing.Size(29, 17)
        Me.quit_btn.TabIndex = 8
        Me.quit_btn.UseVisualStyleBackColor = True
        '
        'bytsIn_lbl
        '
        Me.bytsIn_lbl.AutoSize = True
        Me.bytsIn_lbl.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(32, Byte), Integer))
        Me.bytsIn_lbl.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bytsIn_lbl.Font = New System.Drawing.Font("Yu Gothic Medium", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bytsIn_lbl.ForeColor = System.Drawing.Color.FromArgb(CType(CType(159, Byte), Integer), CType(CType(221, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.bytsIn_lbl.Location = New System.Drawing.Point(141, 72)
        Me.bytsIn_lbl.Name = "bytsIn_lbl"
        Me.bytsIn_lbl.Size = New System.Drawing.Size(57, 17)
        Me.bytsIn_lbl.TabIndex = 9
        Me.bytsIn_lbl.Text = "2000000"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(32, Byte), Integer))
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Label1.Font = New System.Drawing.Font("Yu Gothic Medium", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(159, Byte), Integer), CType(CType(221, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(204, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(14, 17)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "/"
        '
        'totalbyts_lbl
        '
        Me.totalbyts_lbl.AutoSize = True
        Me.totalbyts_lbl.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(32, Byte), Integer))
        Me.totalbyts_lbl.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.totalbyts_lbl.Font = New System.Drawing.Font("Yu Gothic Medium", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.totalbyts_lbl.ForeColor = System.Drawing.Color.FromArgb(CType(CType(159, Byte), Integer), CType(CType(221, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.totalbyts_lbl.Location = New System.Drawing.Point(224, 72)
        Me.totalbyts_lbl.Name = "totalbyts_lbl"
        Me.totalbyts_lbl.Size = New System.Drawing.Size(57, 17)
        Me.totalbyts_lbl.TabIndex = 11
        Me.totalbyts_lbl.Text = "2000000"
        '
        'ApplicationUpdater
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.mock2Launcher.My.Resources.Resources._197__Japanese__Japan__
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(600, 110)
        Me.Controls.Add(Me.totalbyts_lbl)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.bytsIn_lbl)
        Me.Controls.Add(Me.quit_btn)
        Me.Controls.Add(Me.website_btn)
        Me.Controls.Add(Me.WorkingLabel)
        Me.Controls.Add(Me.ProgressBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "ApplicationUpdater"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ApplicationUpdater"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents WorkingLabel As Label
    Private WithEvents website_btn As Button
    Private WithEvents quit_btn As Button
    Friend WithEvents bytsIn_lbl As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents totalbyts_lbl As Label
End Class
