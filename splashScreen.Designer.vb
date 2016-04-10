<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class splashScreen
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(splashScreen))
        Me.rtbDebug = New System.Windows.Forms.RichTextBox()
        Me.txtHTML = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.DLS = New mock2Launcher.My.MyWebClient()
        Me.SuspendLayout()
        '
        'rtbDebug
        '
        Me.rtbDebug.Location = New System.Drawing.Point(0, 0)
        Me.rtbDebug.Name = "rtbDebug"
        Me.rtbDebug.Size = New System.Drawing.Size(100, 96)
        Me.rtbDebug.TabIndex = 0
        Me.rtbDebug.Text = ""
        Me.rtbDebug.Visible = False
        '
        'txtHTML
        '
        Me.txtHTML.Location = New System.Drawing.Point(0, 102)
        Me.txtHTML.Name = "txtHTML"
        Me.txtHTML.Size = New System.Drawing.Size(100, 20)
        Me.txtHTML.TabIndex = 1
        Me.txtHTML.Visible = False
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'DLS
        '

        Me.DLS.BaseAddress = ""
        Me.DLS.CachePolicy = Nothing
        Me.DLS.Credentials = Nothing
        Me.DLS.Encoding = CType(resources.GetObject("DLS.Encoding"), System.Text.Encoding)
        Me.DLS.Headers = CType(resources.GetObject("DLS.Headers"), System.Net.WebHeaderCollection)
        Me.DLS.QueryString = CType(resources.GetObject("DLS.QueryString"), System.Collections.Specialized.NameValueCollection)
        Me.DLS.Timeout = 60000
        Me.DLS.UseDefaultCredentials = False
        '
        'splashScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.mock2Launcher.My.Resources.Resources._215
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(250, 349)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtHTML)
        Me.Controls.Add(Me.rtbDebug)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "splashScreen"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "splashScreen"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DLS As My.MyWebClient
    Friend WithEvents rtbDebug As RichTextBox
    Friend WithEvents txtHTML As TextBox
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
End Class
