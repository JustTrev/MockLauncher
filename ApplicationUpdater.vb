Imports System.Net
Imports System.IO

Public Class ApplicationUpdater


    Dim web As New WebClient
    Dim FilePath As String = CurDir()

    Dim LatestAppVersion As String = web.DownloadString("http://liveoriginal.servegame.com/pso2/version.txt") 'Checks downloads to the latest version of the launcher
    Dim currentversion As String = Application.ProductVersion

    Dim client As WebClient = New WebClient
    Dim client2 As WebClient = New WebClient

    'Dim FileName As String = "ACO Launcher.temp"
    Dim NewFileName As String = "mock2launcher.exe"

    Private Sub cancel_btn_Click(sender As Object, e As EventArgs) Handles website_btn.Click
        Process.Start("http://liveoriginal.org")
    End Sub

    Private Sub quit_btn_Click(sender As Object, e As EventArgs) Handles quit_btn.Click


        client.CancelAsync()
        client.Dispose()
        mocklauncher.Dispose()


    End Sub

    Private Sub cancel_btn_MouseDown(sender As Object, e As MouseEventArgs) Handles website_btn.MouseDown
        website_btn.BackgroundImage = Resources._143EN
    End Sub

    Private Sub website_btn_MouseEnter(sender As Object, e As EventArgs) Handles website_btn.MouseEnter
        website_btn.BackgroundImage = Resources._144EN
    End Sub

    Private Sub website_btn_MouseLeave(sender As Object, e As EventArgs) Handles website_btn.MouseLeave
        website_btn.BackgroundImage = Resources._145EN
    End Sub

    Private Sub ApplicationUpdater_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If File.Exists(FilePath & "/mock2updater.exe") = True Then
            File.Delete(FilePath & "/mock2updater.exe")
        Else
            '
        End If


        Call UpdateNow()
    End Sub
    Sub UpdateNow()
        Try


            If LatestAppVersion <> currentversion Then
                'Label1.Text = "Downloading update.  Please wait.."
                'MsgBox("New Update Available!")
                'Dim client As WebClient = New WebClient
                AddHandler client.DownloadProgressChanged, AddressOf client_ProgressChanged
                AddHandler client.DownloadFileCompleted, AddressOf client_DownloadCompleted

                'MsgBox("There is a new update available and will begin to download.")
                messageForm.message_txt.Text = "There is a new update available and will begin to download."
                messageForm.ShowDialog()

                Try
                    client.DownloadFileAsync(New Uri("http://162.206.115.127:64080/pso2/mock2updater.exe"), FilePath & "/mock2updater.exe")
                    ' client.DownloadFileAsync(New Uri("http://99.88.137.250:64080/CryEngineLauncher/UpdateVersion/CE3Updater.exe"), FilePath & "/CE3Updater.exe")
                Catch ex As Exception
                    MsgBox(ex.ToString)

                End Try


            Else
                mocklauncher.Show()
                Me.Close()
            End If
        Catch ex As Exception
            'MsgBox("Could not connect to the update server at this time.  Click 'OK' to continue. ")
            messageForm.message_txt.Text = "Could not connect to the update server at this time."
            messageForm.ShowDialog()
            mocklauncher.Show()
            Me.Close()
        End Try



    End Sub
    Private Sub client_ProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)

        Dim bytesIn As Double = Double.Parse(e.BytesReceived.ToString())
        Dim totalBytes As Double = Double.Parse(e.TotalBytesToReceive.ToString())
        Dim percentage As Double = bytesIn / totalBytes * 100

        ProgressBar1.Value = e.ProgressPercentage  'Int32.Parse(Math.Truncate(percentage).ToString())
        bytsIn_lbl.Text = bytesIn
        totalbyts_lbl.Text = totalBytes
    End Sub

    Private Sub client_DownloadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)


        Dim launcher

        launcher = FilePath & "/mock2updater.exe"
        messageForm.message_txt.Text = "Download completed. Initializing update."
        messageForm.ShowDialog()
        Try
                Process.Start(launcher)
                'Res = Shell(Updater)
                Application.Exit()
            Catch ex As Exception
            'there was a problem
            messageForm.message_txt.Text = ex.Message & " " & ex.StackTrace
            messageForm.ShowDialog()
            Me.Close()
            End Try



    End Sub

    Sub Finish()

        Dim Res
        Dim Updater

        Updater = FilePath & "/mock2updater.exe"
        messageForm.message_txt.Text = "Download complete. Initializing update."
        messageForm.ShowDialog()
        'MsgBox("Download complete. Applying update.")
        System.Threading.Thread.Sleep(1000)
        Res = Shell(Updater)
        Application.Exit()

    End Sub



End Class