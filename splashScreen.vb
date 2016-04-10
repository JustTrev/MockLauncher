Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.Win32
Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization.Json
Imports System.Security
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Xml
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports mock2Launcher.My

Public Class splashScreen

    Const EnglishPatch = "English Patch"
    Const StoryPatch = "Story Patch"
    Const LargeFiles = "Large Files"

    Dim drag As Boolean
    Dim mousex As Integer
    Dim mousey As Integer

    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const HT_CAPTION As Integer = &H2

    'ReadOnly _pso2OptionsFrm As FrmPso2Options
    'ReadOnly _optionsFrm As FrmOptions
    ReadOnly _myDocuments As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)

    Dim _cancelled As Boolean
    Dim _cancelledFull As Boolean
    Dim _dpiSetting As Single
    Dim _itemDownloadingDone As Boolean
    Dim _mileyCyrus As Integer
    Dim _restartplz As Boolean
    Dim _systemUnlock As Integer
    Dim _vedaUnlocked As Boolean = False
    Public SkipDialogs As Boolean = False
    Dim _totalsize2 As Long

    <DllImportAttribute("user32.dll")>
    Public Shared Function SendMessage(ByVal hWnd As IntPtr,
      ByVal Msg As Integer, ByVal wParam As Integer,
      ByVal lParam As Integer) As Integer
    End Function
    <DllImportAttribute("user32.dll")>
    Public Shared Function ReleaseCapture() As Boolean
    End Function



    Private Sub splashScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try





            ' While Not Program.IsPso2Installed
            '     InstallPso2()
            '     Program.Main()
            ' End While

            ' _cancelledFull = False

            ' CheckIfRun()


            ' If File.Exists((Program.Pso2WinDir & "\" & "a44fbb2aeb8084c5a5fbe80e219a9927")) AndAlso File.Exists((Program.Pso2WinDir & "\" & "a93adc766eb3510f7b5c279551a45585")) Then
            '     If Helper.GetFileSize((Program.Pso2WinDir & "\" & "a44fbb2aeb8084c5a5fbe80e219a9927")) = 167479840 AndAlso Helper.GetFileSize((Program.Pso2WinDir & "\" & "a93adc766eb3510f7b5c279551a45585")) = 151540352 Then
            '         'chkSwapOP.Text = Resources.strSwapPCVitaOpenings & " (" & Resources.strNotSwapped & ")"
            '     ElseIf Helper.GetFileSize((Program.Pso2WinDir & "\" & "a44fbb2aeb8084c5a5fbe80e219a9927")) = 151540352 AndAlso Helper.GetFileSize((Program.Pso2WinDir & "\" & "a93adc766eb3510f7b5c279551a45585")) = 167479840 Then
            '         'chkSwapOP.Text = Resources.strSwapPCVitaOpenings & " (" & Resources.strSwapped & ")"
            '     End If
            ' End If
            '
            '
            '
            '
            ' If Not File.Exists("7za.exe") Then
            '     Helper.WriteDebugInfo("Downloading 7za.exe...")
            '     Application.DoEvents()
            '     DownloadFile(Program.FreedomUrl & "7za.exe", "7za.exe")
            ' End If
            '
            ' For index = 1 To 5
            '     If Helper.GetMd5("7za.exe") <> "42BADC1D2F03A8B1E4875740D3D49336" Then
            '         Helper.WriteDebugInfo("Your corrupt.")
            '         Application.DoEvents()
            '         DownloadFile(Program.FreedomUrl & "7za.exe", "7za.exe")
            '     Else
            '         Exit For
            '     End If
            ' Next
            '
            ' If Not File.Exists("UnRar.exe") Then
            '     Helper.WriteDebugInfo("Downloading UnRar.exe...")
            '     Application.DoEvents()
            '     DownloadFile(Program.FreedomUrl & "UnRAR.exe", "UnRAR.exe")
            ' End If
            '
            ' For index = 1 To 5
            '     If Helper.GetMd5("UnRar.exe") <> "0C83C1293723A682577E3D0B21562B79" Then
            '         Helper.WriteDebugInfo("UnRar.exe is corrupt.")
            '         Application.DoEvents()
            '         DownloadFile(Program.FreedomUrl & "UnRAR.exe", "UnRAR.exe")
            '     Else
            '         Exit For
            '     End If
            ' Next
            '
            ' Helper.DeleteDirectory("TEMPSTORYAIDAFOOL")
            ' Helper.DeleteFile("launcherlist.txt")
            ' Helper.DeleteFile("patchlist.txt")
            ' Helper.DeleteFile("patchlist_old.txt")
            '
            ' 'Added in precede files. Stupid ass SEGA.
            ' Helper.DeleteFile("patchlist0.txt")
            ' Helper.DeleteFile("patchlist1.txt")
            ' Helper.DeleteFile("patchlist2.txt")
            ' Helper.DeleteFile("patchlist3.txt")
            ' Helper.DeleteFile("patchlist4.txt")
            ' Helper.DeleteFile("patchlist5.txt")
            ' Helper.DeleteFile("precede.txt")
            ' Helper.DeleteFile("ServerConfig.txt")
            ' Helper.DeleteFile("precede_apply.txt")
            ' Helper.DeleteFile("version.ver")
            ' Helper.DeleteFile("Story MD5HashList.txt")
            ' Helper.DeleteFile("PluginMD5HashList.txt")
            ' Helper.DeleteFile("working.txt")
            ' Helper.DeleteFile("gnfieldstatus.txt")
            '
            '
            '
            '
            ' If File.Exists("resume.txt") Then
            '     Dim yesNoResume As MsgBoxResult = MsgBox("It seems that the last patching attempt was interrupted. Would you Like to resume patching?", vbYesNo)
            '     If yesNoResume = MsgBoxResult.Yes Then
            '         ResumePatching()
            '     Else
            '         Helper.DeleteFile("resume.txt")
            '     End If
            '
            '
            ' End If
            '



            '   Helper.WriteDebugInfo("Checking for PSO2 updates.")
            '   Application.DoEvents()
            '
            '   CheckForPso2Updates(False)
            '   Helper.WriteDebugInfoSameLine("Finished")
            '   Application.DoEvents()
            '
            '   'Check for PSO2 Updates here, download the version.ver thingie
            '   'Check for PSO2 EN Patch updates here, parse the URL and see if it's different from the saved one
            '   'Check for EN Story Patch
            '   ' Helper.WriteDebugInfo(Resources.strCheckingforUpdatestopatches)
            '
            '   'Check for English Patches (Done! :D)
            '   'CheckForEnPatchUpdates()
            '   Helper.WriteDebugInfo(RegKey.GetValue(Of String)(RegKey.EnPatchVersion))
            '   Application.DoEvents()
            '
            '   'Check for LargeFiles Update (Work-In-Progress!)
            '   'CheckForLargeFilesUpdates()
            '   Helper.WriteDebugInfo(RegKey.GetValue(Of String)(RegKey.LargeFilesVersion))
            '   Application.DoEvents()
            '
            '   'Check for Story Patches (Done! :D)
            '   Application.DoEvents()
            '   'CheckForStoryUpdates()
            '   Helper.WriteDebugInfo(RegKey.GetValue(Of String)(RegKey.StoryPatchVersion))
            '   Application.DoEvents()


            '            Helper.WriteDebugInfo(Resources.strIfAboveVersions)





            'Helper.WriteDebugInfoSameLine(Resources.strDone)
        Catch ex As Exception
            'Helper.Log(ex.Message.ToString & " Stack Trace:  " & ex.StackTrace)
            messageForm.message_txt.Text = ex.Message.ToString & " Stack Trace:  " & ex.StackTrace
            messageForm.ShowDialog()
            'Helper.WriteDebugInfo(Resources.strERROR & ex.Message)
        End Try

        Helper.DeleteFile("Story MD5HashList.txt")
        Helper.DeleteFile("mock2Updater.exe")
        'Helper.WriteDebugInfo(Resources.strAllDoneSystemReady)
        'Close()

    End Sub

    Public Shared Function CheckIfRun(processName As String) As Boolean
        Dim currentProcessId = Process.GetCurrentProcess().Id

        If Process.GetProcessesByName(processName).Length > If(processName = "mock2launcher", 1, 0) Then
            Dim closeItYesNo As MsgBoxResult = MsgBox("It seems that " & processName.Replace(".exe", "") & " is already running. Would you like to close it?", vbYesNo)

            If closeItYesNo = vbYes Then
                For Each proc As Process In Process.GetProcessesByName(processName)
                    If proc.Id <> currentProcessId Then proc.Kill()
                Next
                If processName = "mock2launcher" Then
                    Process.Start("mock2launcher.exe")
                End If

            End If
        Else
            Return False
        End If

        Return True
    End Function

    Private Sub ResumePatching()

        If Not File.Exists("resume.txt") Then
            'Helper.WriteDebugInfo(Resources.strCannotFindResume)
            Return
        End If

        _cancelledFull = False

        Try
            Dim missingfiles As New List(Of String)

            'Helper.WriteDebugInfoAndOk(Resources.strFoundIncompleteJob)
            If _cancelledFull Then Return
            For Each line In Helper.GetLines("resume.txt")
                If _cancelledFull Then Return
                missingfiles.Add(line)
            Next

            Dim totaldownload As Long = missingfiles.Count
            Dim downloaded As Long = 0
            Dim totaldownloaded As Long = 0
            For Each downloadStr As String In missingfiles
                If _cancelledFull Then Return
                'Download the missing files:
                'WHAT THE FUCK IS GOING ON HERE?v3
                downloaded += 1
                totaldownloaded += _totalsize2

                ' lblStatus.Text = Resources.strDownloading & "" & downloaded & "/" & totaldownload & " (" & Helper.SizeSuffix(totaldownloaded) & ")"

                Application.DoEvents()
                _cancelled = False
                DownloadFile(("http://download.pso2.jp/patch_prod/patches/data/win32/" & downloadStr & ".pat"), downloadStr)
                If New FileInfo(downloadStr).Length = 0 Then
                    Helper.Log("File appears to be empty, trying to download from secondary SEGA server")
                    DownloadFile(("http://download.pso2.jp/patch_prod/patches_old/data/win32/" & downloadStr & ".pat"), downloadStr)
                End If
                If _cancelled Then Return
                'Delete the existing file FIRST
                Helper.DeleteFile((Program.Pso2WinDir & "\" & downloadStr))
                File.Move(downloadStr, (Program.Pso2WinDir & "\" & downloadStr))
                If _vedaUnlocked Then Helper.WriteDebugInfo("DEBUG: Downloaded and installed " & downloadStr & ".")
                Dim linesList As New List(Of String)(File.ReadAllLines("resume.txt"))

                'Remove the line to delete, e.g.
                linesList.Remove(downloadStr)
                File.WriteAllLines("resume.txt", linesList.ToArray())
                Application.DoEvents()
            Next
            Helper.DeleteFile("resume.txt")
            If missingfiles.Count = 0 Then Helper.WriteDebugInfo("You appear to be up to date.")
            Dim directoryString As String = (Program.Pso2RootDir & "\")
            Helper.WriteDebugInfo("version file...")
            Application.DoEvents()
            _cancelled = False
            Program.Client.DownloadFile("http://arks-layer.com/vanila/version.txt", "version.ver")
            If _cancelled Then Return
            If File.Exists((_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver")) Then Helper.DeleteFile((_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver"))
            File.Copy("version.ver", (_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver"))
            Helper.WriteDebugInfoAndOk(("version file"))
            Helper.WriteDebugInfo("Downloading pso2launcher.exe...")
            Application.DoEvents()
            For Each proc As Process In Process.GetProcessesByName("pso2launcher")
                If proc.MainWindowTitle = "PHANTASY STAR ONLINE 2" AndAlso proc.MainModule.ToString() = "ProcessModule (pso2launcher.exe)" Then proc.Kill()
            Next
            DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2launcher.exe.pat", "pso2launcher.exe")
            If _cancelled Then Return
            If File.Exists((directoryString & "pso2launcher.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((directoryString & "pso2launcher.exe"))
            File.Move("pso2launcher.exe", (directoryString & "pso2launcher.exe"))
            Helper.WriteDebugInfoAndOk(("pso2launcher.exe"))
            ' Helper.WriteDebugInfo(Resources.strDownloading & "pso2updater.exe...")
            Application.DoEvents()
            For Each proc As Process In Process.GetProcessesByName("pso2updater")
                If proc.MainModule.ToString() = "ProcessModule (pso2updater.exe)" Then proc.Kill()
            Next
            DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2updater.exe.pat", "pso2updater.exe")
            If _cancelled Then Return
            If File.Exists((directoryString & "pso2updater.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((directoryString & "pso2updater.exe"))
            File.Move("pso2updater.exe", (directoryString & "pso2updater.exe"))
            'Helper.WriteDebugInfoAndOk((Resources.strDownloadedandInstalled & "pso2updater.exe"))
            Application.DoEvents()
            ' Helper.WriteDebugInfo(Resources.strDownloading & "pso2.exe...")
            For Each proc As Process In Process.GetProcessesByName("pso2")
                If proc.MainModule.ToString() = "ProcessModule (pso2.exe)" Then proc.Kill()
            Next

            DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2.exe.pat", "pso2.exe")
            If _cancelled Then Return

            If File.Exists((directoryString & "pso2.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((directoryString & "pso2.exe"))
            File.Move("pso2.exe", (directoryString & "pso2.exe"))
            If _cancelledFull Then Return
            'Helper.WriteDebugInfoAndOk((Resources.strDownloadedandInstalled & "pso2.exe"))
            RegKey.SetValue(Of String)(RegKey.StoryPatchVersion, "Not Installed")
            RegKey.SetValue(Of String)(RegKey.EnPatchVersion, "Not Installed")
            RegKey.SetValue(Of String)(RegKey.LargeFilesVersion, "Not Installed")
            DownloadFile("http://download.pso2.jp/patch_prod/patches/patchlist.txt", "patchlist.txt")
            ' Helper.WriteDebugInfoSameLine(Resources.strDone)
            RegKey.SetValue(Of String)(RegKey.Pso2PatchlistMd5, Helper.GetMd5("patchlist.txt"))
            'Helper.WriteDebugInfo(Resources.strGameUpdatedVanilla)
            Helper.DeleteFile("resume.txt")
            RegKey.SetValue(Of String)(RegKey.Pso2RemoteVersion, File.ReadAllLines("version.ver")(0))
            'UnlockGui()

            If Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.RemoveCensor)) Then
                If File.Exists((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c.backup")) Then Computer.FileSystem.DeleteFile((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c.backup"), UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently)
                Computer.FileSystem.RenameFile((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c"), "ffbff2ac5b7a7948961212cefd4d402c.backup")
                'Helper.WriteDebugInfoAndOk(Resources.strRemoving & "Censor...")
            End If

            If Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.EnPatchAfterInstall)) Then
                ' Helper.WriteDebugInfo(Resources.strAutoInstallingENPatch)
                DownloadEnglishPatch()
            End If

            If Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.LargeFilesAfterInstall)) Then
                'Helper.WriteDebugInfo(Resources.strAutoInstallingLF)
                DownloadLargeFiles()
            End If

            If Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.StoryPatchAfterInstall)) Then
                'Helper.WriteDebugInfo(Resources.strAutoInstallingStoryPatch)
                InstallStoryPatchNew()
            End If

            ' Helper.WriteDebugInfoAndOk(Resources.strallDone)
        Catch ex As Exception
            Helper.Log(ex.Message.ToString & " Stack Trace: " & ex.StackTrace)
            If ex.Message <> "Arithmetic operation resulted in an overflow." Then
                'Helper.WriteDebugInfo(Resources.strERROR & ex.Message)
            End If
        End Try
        'Close()
    End Sub

    Private Sub DownloadLargeFiles()

        ' The Using statement will dispose "net" as soon as we're done with it.
        ' This parses the sidebar page for compatibility
        ' First it downloads the page and splits it by line
        'Dim compat As String() = Regex.Split(Program.Client.DownloadString(Program.FreedomUrl & "tweaker2.html"), "\r\n|\r|\n")
        'Dim doDownload As Boolean = True

        ' Then for each string in the split page, it does a regex match to grab the compatibility.
        ' This way we can avoid .replace.replace.replace.replace.replace and just get straight to the point;
        ' is it equal to "Compatible"
        'For Each str As String In compat
        ' If Regex.IsMatch(Str, "> Large Files: <font color=""[^""]+"">([^<]+)</font><br>") Then
        ' If Not Regex.Match(Str, "> Large Files: <font color=""[^""]+"">([^<]+)</font><br>").Groups(1).Value.StartsWith("Compatible") Then
        ' Dim reallyInstall As MsgBoxResult = MsgBox("It looks like the Large Files patch isn't compatible right now. Installing it may break your game, force an endless loading screen, crash the universe and/or destablize space and time. Do you really want to install it?", MsgBoxStyle.YesNo)

        '        doDownload = reallyInstall <> MsgBoxResult.No
        '        End If
        '        End If
        '       Next

        'If doDownload Then
        ' Here we parse the text file before passing it to the DownloadPatch function.
        Dim url As String = Program.Client.DownloadString(Program.FreedomUrl & "patches/largefiles.txt")
        DownloadPatch(url, LargeFiles, "LargeFiles.rar", RegKey.LargeFilesVersion, "Would you like to back up large files?", "Would you like to use a pre-downloaded RAR file containing the patch instead of downloading it now?")
        'Else
        'Helper.WriteDebugInfo("Download was cancelled due to incompatibility.")
        'End If
    End Sub

    Private Sub CheckForTweakerUpdates()
        'Helper.WriteDebugInfo(Resources.strCheckingforupdatesPleasewaitamoment)
        Dim source As String = Program.Client.DownloadString(Program.FreedomUrl & "version.xml")

        If Not String.IsNullOrEmpty(source) AndAlso source.Contains("<VersionHistory>") Then
            Dim xm As New XmlDocument
            xm.LoadXml(source)

            Dim xmlNode = xm.SelectSingleNode("//CurrentVersion")
            Dim currentVersion As String = xmlNode.ChildNodes(0).InnerText.Trim

            Helper.Log("Checking for the latest version of the program...")

            If Application.ProductVersion.ToString = currentVersion Then
                Helper.WriteDebugInfo(("You have the latest version. " & Application.ProductVersion.ToString()))
            Else
                Dim changelogtotal As String = ""

                For index As Integer = 2 To 11
                    Dim innerText = xmlNode.ChildNodes(index).InnerText.Trim
                    If Not String.IsNullOrEmpty(innerText) Then changelogtotal &= vbCrLf & innerText
                Next

                Dim updateyesno As MsgBoxResult = MsgBox("Your version is our of date.  Do you wish to update?" & Application.ProductVersion.ToString & " > " & currentVersion & vbCrLf & changelogtotal, MsgBoxStyle.YesNo)
                If updateyesno = MsgBoxResult.Yes Then
                    'Helper.WriteDebugInfo(Resources.strDownloadingUpdate)
                    DownloadFile(Program.FreedomUrl & "mock2Updater.exe", "mock2Updater.exe")
                    Process.Start(Environment.CurrentDirectory & "\mock2Updater.exe")
                    Threading.Thread.Sleep(30000)
                    messageForm.message_txt.Text = "Failed to check for updates. Please try again later."
                    messageForm.ShowDialog()
                    'Helper.WriteDebugInfoAndFailed("Update seems to have failed... ")
                    Application.DoEvents()
                    Return
                End If
            End If
        Else
            'Helper.WriteDebugInfoAndWarning(Resources.strFailedToGetUpdateInfo)
        End If
    End Sub

    Private Shared Function IsPso2WinDirMissing() As Boolean
        If Program.Pso2RootDir = "lblDirectory" OrElse Not Directory.Exists(Program.Pso2WinDir) Then
            messageForm.locateWin32()
            messageForm.ShowDialog()
            Helper.SelectPso2Directory()
            Return True
        End If
        Return False
    End Function
    Private Sub DownloadPluginFiles(state As Object)
        Try
            If File.Exists(Program.Pso2RootDir & "\plugins\PSO2DamageDump.dll") = False Then
                Helper.WriteDebugInfo("Downloading DPS Parser plugin... (disabled by default)")
                Program.ItemPatchClient.DownloadFile("http://107.170.16.100/Plugins/PSO2DamageDump.dll", (Program.Pso2RootDir & "\Plugins\disabled\PSO2DamageDump.dll"))
                Helper.WriteDebugInfoSameLine("Done!")
            End If
            If File.Exists(Program.Pso2RootDir & "\plugins\PSO2TitleTranslator.dll") = False Then
                Helper.WriteDebugInfo("Downloading Title Translator plugin... (enabled by default)")
                Program.ItemPatchClient.DownloadFile("http://107.170.16.100/Plugins/PSO2TitleTranslator.dll", (Program.Pso2RootDir & "\Plugins\PSO2TitleTranslator.dll"))
                Helper.WriteDebugInfoSameLine("Done!")
            End If
            'For when we use the item patch like this
            'If File.Exists(Program.Pso2RootDir & "\plugins\PSO2DamageDump.dll") = False Then
            'Helper.WriteDebugInfo("Installing DPS Parser plugin...")
            'Program.ItemPatchClient.DownloadFile("http://107.170.16.100/Plugins/PSO2DamageDump.dll", (Program.Pso2RootDir & "\Plugins\PSO2DamageDump.dll"))
            'End If
        Catch ex As Exception
            messageForm.message_txt.Text = "Failed to download plugin files! (" & ex.Message & "). Try rebooting your computer or making sure PSO2 isn't open."
            messageForm.ShowDialog()
            'MsgBox("Failed to download plugin files! (" & ex.Message.ToString & "). Try rebooting your computer or making sure PSO2 isn't open.")
        End Try

        _itemDownloadingDone = True
    End Sub

    Public Sub WriteDebugInfo(ByVal addThisText As String)
        If rtbDebug.InvokeRequired Then
            rtbDebug.Invoke(New Action(Of String)(AddressOf WriteDebugInfo), Text)
        Else
            rtbDebug.Text &= (vbCrLf & addThisText)
        End If
    End Sub

    Public Sub WriteDebugInfoSameLine(ByVal addThisText As String)
        If rtbDebug.InvokeRequired Then
            rtbDebug.Invoke(New Action(Of String)(AddressOf WriteDebugInfoSameLine), Text)
        Else
            rtbDebug.Text &= (" " & addThisText)
        End If
    End Sub

    Public Sub WriteDebugInfoAndOk(ByVal addThisText As String)
        If rtbDebug.InvokeRequired Then
            rtbDebug.Invoke(New Action(Of String)(AddressOf WriteDebugInfoAndOk), Text)
        Else
            rtbDebug.Text &= (vbCrLf & addThisText)
            rtbDebug.Select(rtbDebug.TextLength, 0)
            rtbDebug.SelectionColor = Color.Green
            rtbDebug.AppendText(" [OK!]")
            rtbDebug.SelectionColor = rtbDebug.ForeColor
        End If
    End Sub

    Public Sub WriteDebugInfoAndWarning(ByVal addThisText As String)
        If rtbDebug.InvokeRequired Then
            rtbDebug.Invoke(New Action(Of String)(AddressOf WriteDebugInfoAndWarning), Text)
        Else
            rtbDebug.Text &= (vbCrLf & addThisText)
            rtbDebug.Select(rtbDebug.TextLength, 0)
            rtbDebug.SelectionColor = Color.Red
            rtbDebug.AppendText(" [WARNING!]")
            rtbDebug.SelectionColor = rtbDebug.ForeColor
        End If
    End Sub

    Public Sub WriteDebugInfoAndFailed(ByVal addThisText As String)
        If rtbDebug.InvokeRequired Then
            rtbDebug.Invoke(New Action(Of String)(AddressOf WriteDebugInfoAndFailed), Text)
        Else
            If addThisText = "ERROR - Index was outside the bounds of the array." Then Return
            If addThisText = "ERROR - Object reference not set to an instance of an object." Then Return
            rtbDebug.Text &= (vbCrLf & addThisText)
            rtbDebug.Select(rtbDebug.TextLength, 0)
            rtbDebug.SelectionColor = Color.Red
            rtbDebug.AppendText("FAILED")
            rtbDebug.SelectionColor = rtbDebug.ForeColor
            'UnlockGui()
        End If
    End Sub

    Private Shared Sub rtbDebug_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles rtbDebug.LinkClicked
        Process.Start(e.LinkText)
    End Sub

    ' Private Sub rtbDebug_MouseClick(sender As Object, e As MouseEventArgs) Handles rtbDebug.MouseClick
    '     If e.Button = MouseButtons.Right Then
    '          cmsTextBarOptions.Show(DirectCast(sender, Control), e.Location)
    '     End If
    ' End Sub

    Private Sub rtbDebug_TextChanged(sender As Object, e As EventArgs) Handles rtbDebug.TextChanged
        rtbDebug.SelectionStart = rtbDebug.Text.Length
    End Sub

    Private Sub OnDownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs) Handles DLS.DownloadProgressChanged
        Dim totalSize As Long = e.TotalBytesToReceive
        _totalsize2 = totalSize
        Dim downloadedBytes As Long = e.BytesReceived
        Dim percentage As Integer = e.ProgressPercentage
        'PBMainBar.Value = percentage
        'PBMainBar.Text = (Resources.strDownloaded & Helper.SizeSuffix(downloadedBytes) & " / " & Helper.SizeSuffix(totalSize) & " (" & percentage & "%) - " & Resources.strRightClickforOptions)
        'Put your progress UI here, you can cancel download by uncommenting the line below
    End Sub

    Private Sub OnFileDownloadCompleted(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs) Handles DLS.DownloadFileCompleted
        'PBMainBar.Value = 0
        'PBMainBar.Text = ""
    End Sub

    Public Sub DownloadFile(ByVal address As String, ByVal filename As String)
        DLS.Headers("user-agent") = "AQUA_HTTP"
        DLS.Timeout = 10000

        While DLS.IsBusy
            Application.DoEvents()
            Thread.Sleep(16)
        End While

        DLS.DownloadFileAsync(New Uri(address), filename)

        While DLS.IsBusy
            Application.DoEvents()
            Thread.Sleep(16)

            If _restartplz Then
                DLS.CancelAsync()
                _restartplz = False
                DownloadFile(address, filename)
                Exit While
            End If

            If Not Visible And SkipDialogs = False Then
                DLS.CancelAsync()
                _cancelled = True
                _cancelledFull = True
                Application.Exit()
            End If
        End While
    End Sub

    'This is depreciated.
    'Private Sub LockGui()
    '    Enabled = False
    'End Sub
    '
    'Private Sub UnlockGui()
    '    Enabled = True
    'End Sub

    Private Shared Function MergePatches() As Dictionary(Of String, String)
        Dim patchlist As String() = File.ReadAllLines("patchlist.txt")
        Dim patchlistOld As String() = File.ReadAllLines("patchlist_old.txt")

        Dim result = New Dictionary(Of String, String)

        For index As Integer = 0 To (patchlist.Length - 1)
            Dim currentLine = patchlist(index)
            If String.IsNullOrEmpty(currentLine) Then Continue For

            Dim filename = Regex.Split(currentLine, ".pat")
            If String.IsNullOrEmpty(filename(0)) Then Continue For

            Dim key = filename(0).Replace("data/win32/", "")

            If Not result.ContainsKey(key) Then
                result.Add(key, currentLine)
            End If
        Next

        For index As Integer = 0 To (patchlistOld.Length - 1)
            Dim currentLine = patchlistOld(index)
            If String.IsNullOrEmpty(currentLine) Then Continue For

            Dim filename = Regex.Split(currentLine, ".pat")
            If String.IsNullOrEmpty(filename(0)) Then Continue For

            Dim key = filename(0).Replace("data/win32/", "")

            If Not result.ContainsKey(key) Then
                result.Add(key, currentLine)
            End If
        Next

        Return result
    End Function

    Private Shared Function MergePrePatches() As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)

        For i As Integer = 0 To 5
            Dim patchlist = File.ReadAllLines("patchlist" & i & ".txt")
            'Dim patchlist = DlwuaString("http://download.pso2.jp/patch_prod/patches_precede/patchlist" & i & ".txt").Split(ControlChars.Cr)

            For index As Integer = 0 To (patchlist.Length - 1)
                If String.IsNullOrEmpty(patchlist(index)) Then Continue For

                Dim splitLine = patchlist(index).Split(ControlChars.Tab)
                Dim fileName = Path.GetFileNameWithoutExtension(splitLine(0).Replace("data/win32/", ""))
                Dim hash = splitLine(2)

                If (Not String.IsNullOrEmpty(fileName)) AndAlso (Not result.ContainsKey(hash)) Then
                    result.Add(hash, fileName)
                End If
            Next
        Next

        Return result
    End Function



    Private Sub CheckForStoryUpdates()
        Try
            If RegKey.GetValue(Of String)(RegKey.StoryPatchVersion) = "Not Installed" Then Return

            txtHTML.Text = Regex.Match(Program.Client.DownloadString("http://arks-layer.com/story.php"), "<u>.*?</u>").Value
            Dim strDownloadMe = txtHTML.Text.Replace("<u>", "").Replace("</u>", "")
            If RegKey.GetValue(Of String)(RegKey.LatestStoryBase) <> strDownloadMe Then
                Dim StoryChangeLog As String = Program.Client.DownloadString("http://arks-layer.com/storyupdates.txt")
                Dim mbVisitLink As MsgBoxResult = MsgBox("A new story patch is available! Would you like to download and install it? Here's a list of changes: " & vbCrLf & StoryChangeLog, MsgBoxStyle.YesNo)
                If mbVisitLink = vbYes Then InstallStoryPatchNew()
                Return
            End If

        Catch ex As Exception
            messageForm.message_txt.Text = ex.Message
            messageForm.ShowDialog()
        End Try
    End Sub
    Private Shared Function BuildBackupPath(ByVal patchName As String) As String
        Return Program.Pso2WinDir & "\backup\" & patchName & "\"
    End Function

    Private Sub InstallStoryPatchNew()
        'Don't forget to make GUI changes!
        Try
            If IsPso2WinDirMissing() Then Return

            ' Create a match using regular exp<b></b>ressions
            ' Spit out the value plucked from the code
            txtHTML.Text = Regex.Match(Program.Client.DownloadString("http://arks-layer.com/story.php"), "<u>.*?</u>").Value

            Dim backupdir As String = BuildBackupPath(StoryPatch)
            Dim strStoryPatchLatestBase As String = txtHTML.Text.Replace("<u>", "").Replace("</u>", "").Replace("/", "-")
            Helper.WriteDebugInfoAndOk("Downloading story patch info... ")
            DownloadFile(Program.FreedomUrl & "pso2.stripped.db.7z", "pso2.stripped.db.7z")
            Dim processStartInfo2 As New ProcessStartInfo With
            {
                .FileName = (Program.StartPath & "\7za.exe"),
                .Verb = "runas",
                .Arguments = ("e -y pso2.stripped.db.7z"),
                .WindowStyle = ProcessWindowStyle.Hidden,
            .UseShellExecute = True
            }
            Process.Start(processStartInfo2).WaitForExit()
            Dim DBMD5 As String = Helper.GetMd5("pso2.stripped.db")
            Helper.WriteDebugInfoAndOk("Downloading Trans-Am tool... ")
            DownloadFile(Program.FreedomUrl & "pso2-transam.exe", "pso2-transam.exe")

            'execute pso2-transam stuff with -b flag for backup
            Dim processStartInfo As ProcessStartInfo = New ProcessStartInfo() With {.FileName = "pso2-transam.exe", .Verb = "runas"}
            If Directory.Exists(backupdir) Then
                Dim counter = Computer.FileSystem.GetFiles(backupdir)
                If counter.Count > 0 Then
                    processStartInfo.Arguments = ("-t story-eng-" & strStoryPatchLatestBase & " pso2.stripped.db " & """" & Program.Pso2WinDir & """")
                Else
                    Helper.Log("[TRANSAM] Creating backup directory")
                    Directory.CreateDirectory(backupdir)
                    ' Helper.WriteDebugInfo(Resources.strCreatingBackupDirectory)
                    processStartInfo.Arguments = ("-b " & """" & backupdir & """" & " -t story-eng-" & strStoryPatchLatestBase & " pso2.stripped.db " & """" & Program.Pso2WinDir & """")
                End If
            End If

            'We don't need to make backups anymore
            'Yes we do, shut up past AIDA.
            If Not Directory.Exists(backupdir) Then
                Helper.Log("[TRANSAM] Creating backup directory")
                Directory.CreateDirectory(backupdir)
                'Helper.WriteDebugInfo(Resources.strCreatingBackupDirectory)
                processStartInfo.Arguments = ("-b " & """" & backupdir & """" & " -t story-eng-" & strStoryPatchLatestBase & " pso2.stripped.db " & """" & Program.Pso2WinDir & """")
            End If

            processStartInfo.UseShellExecute = False
            Helper.Log("[TRANSAM] Starting shitstorm")
            processStartInfo.Arguments = processStartInfo.Arguments.Replace("\", "/")
            Helper.Log("TRANSM parameters: " & processStartInfo.Arguments & vbCrLf & "TRANSAM Working Directory: " & processStartInfo.WorkingDirectory)
            Helper.Log("[TRANSAM] Program started")
            If Helper.GetMd5("pso2.stripped.db") <> DBMD5 Then
                MsgBox("ERROR: Files have been modified since download. Aborting...")
                Exit Sub
            End If

            Process.Start(processStartInfo).WaitForExit()
            Helper.DeleteFile("pso2.stripped.db")
            Helper.DeleteFile("pso2-transam.exe")
            External.FlashWindow(Handle, True)
            'Story Patch 3-12-2014.rar
            RegKey.SetValue(Of String)(RegKey.StoryPatchVersion, strStoryPatchLatestBase.Replace("-", "/"))
            RegKey.SetValue(Of String)(RegKey.LatestStoryBase, strStoryPatchLatestBase.Replace("-", "/"))
            ' Helper.WriteDebugInfo(Resources.strStoryPatchInstalled)
            CheckForStoryUpdates()
        Catch ex As Exception
            MsgBox("ERROR - " & ex.Message.ToString)
        End Try
    End Sub

    Private Sub DownloadEnglishPatch()
        ' Here we parse the text file before passing it to the DownloadPatch function.
        ' The Using statement will dispose "net" as soon as we're done with it.
        ' If we decide not to, we can do away with "url" and just pass net.DownloadString in as the parameter.
        ' Furthermore, we could also parse it from within the function.
        Dim url As String = Program.Client.DownloadString(Program.FreedomUrl & "patches/enpatch.txt")
        DownloadPatch(url, EnglishPatch, "ENPatch.rar", RegKey.EnPatchVersion, Resources.strBackupEN, "Please select the pre-downloaded EN Patch RAR file")
    End Sub


    Private Sub CheckForEnPatchUpdates()
        Try
            If RegKey.GetValue(Of String)(RegKey.EnPatchVersion) = "Not Installed" Then Return
            Application.DoEvents()
            Dim lastfilename As String() = Program.Client.DownloadString(Program.FreedomUrl & "patches/enpatch.txt").Split("/"c)
            Dim strVersion As String = lastfilename(lastfilename.Length - 1).Replace(".rar", "")
            RegKey.SetValue(Of String)(RegKey.NewEnVersion, strVersion)

            If RegKey.GetValue(Of String)(RegKey.EnPatchVersion) <> strVersion Then
                If MsgBox("Download EN patch?", vbYesNo) = vbYes Then
                    DownloadEnglishPatch()
                End If
            End If
        Catch ex As Exception
            messageForm.message_txt.Text = ex.Message
            messageForm.ShowDialog()
        End Try
        'Close()
    End Sub

    Private Sub CheckForLargeFilesUpdates()
        Try
            If RegKey.GetValue(Of String)(RegKey.LargeFilesVersion) = "Not Installed" Then Return

            Application.DoEvents()

            Dim strVersion As String = Program.Client.DownloadString(Program.FreedomUrl & "patches/largefilesTRANSAM.txt").Replace("-", "/")

            RegKey.SetValue(Of String)(RegKey.NewLargeFilesVersion, strVersion)

            If RegKey.GetValue(Of String)(RegKey.LargeFilesVersion) <> strVersion Then
                If MsgBox("An update for the Large Files is available. Would you like to install it via TRANSAM?", vbYesNo) = vbYes Then
                    mocklauncher.btnLargeFilesTRANSAM.PerformClick()
                End If
            End If
        Catch ex As Exception
            Helper.Log(ex.Message.ToString & " Stack Trace: " & ex.StackTrace)
            messageForm.message_txt.Text = ex.Message
            messageForm.ShowDialog()
        End Try
    End Sub

    Private Sub CheckForPso2Updates(comingFromPrePatch As Boolean)
        Try
            'Precede file, syntax is Yes/No:<Dateoflastprepatch>
            DownloadFile(Program.FreedomUrl & "precede.txt", "precede.txt")
            Dim precedeSplit As String() = File.ReadAllLines("precede.txt")(0).Split(":"c)

            Dim precedeversionstring As String = precedeSplit(1)
            Helper.DeleteFile("precede.txt")
            If comingFromPrePatch Then
                DownloadPrePatch(precedeversionstring)
            Else
                Dim firstTimechecking As Boolean = String.IsNullOrEmpty(RegKey.GetValue(Of String)(RegKey.Pso2PrecedeVersion))
                If firstTimechecking Then RegKey.SetValue(Of String)(RegKey.Pso2PrecedeVersion, precedeversionstring)

                If precedeSplit(0) = "Yes" AndAlso (RegKey.GetValue(Of String)(RegKey.Pso2PrecedeVersion) <> precedeversionstring OrElse firstTimechecking) Then
                    Dim downloadPrepatchResult As MsgBoxResult = MsgBox("New pre-patch data is available to download - Would you like to download it? This is optional, and will let you download some of a large patch now, as opposed to having a larger download all at once when it is released.", MsgBoxStyle.YesNo)
                    If downloadPrepatchResult = vbYes Then DownloadPrePatch(precedeversionstring)
                End If
            End If

            'Check whether or not to apply pre-patch shit. Ugh.
            If Directory.Exists(Program.Pso2RootDir & "\_precede\") AndAlso Directory.Exists(Program.Pso2RootDir & "\_precede\data\win32\") Then
                DownloadFile(Program.FreedomUrl & "precede_apply.txt", "precede_apply.txt")
                If File.ReadAllLines("precede_apply.txt")(0) = "Yes" Then
                    InstallPrePatch()
                End If
            End If

            If Not comingFromPrePatch Then
                DownloadFile("http://arks-layer.com/vanila/version.txt", "version.ver")
                Dim version = File.ReadAllLines("version.ver")(0)
                If String.IsNullOrEmpty(RegKey.GetValue(Of String)(RegKey.Pso2RemoteVersion)) Then
                    RegKey.SetValue(Of String)(RegKey.Pso2RemoteVersion, version)
                End If

                If RegKey.GetValue(Of String)(RegKey.Pso2RemoteVersion) <> version Then
                    If MsgBox("Do you wish to update PSO2?", vbYesNo) = vbYes Then mocklauncher.btnNewShit.PerformClick()
                End If
            End If
        Catch ex As Exception
            Helper.Log(ex.Message.ToString & " Stack Trace: " & ex.StackTrace)
            messageForm.message_txt.Text = ex.Message
            messageForm.ShowDialog()
        End Try
        'Close()
    End Sub

    Private Sub InstallPrePatch()
        Dim applyPrePatchYesNo As MsgBoxResult = MsgBox("It appears that it's time to install the pre-patch download - Is this okay? If you select no, the pre-patch will not be installed.", vbYesNo)
        If applyPrePatchYesNo = vbYes Then
            'WriteDebugInfo("Restoring backup of vanilla JP files...")
            '[AIDA] Apply the precede   
            If Directory.Exists(BuildBackupPath(EnglishPatch)) Then RestoreBackup(EnglishPatch)
            If Directory.Exists(BuildBackupPath(LargeFiles)) Then RestoreBackup(LargeFiles)
            If Directory.Exists(BuildBackupPath(StoryPatch)) Then RestoreBackup(StoryPatch)
            Helper.WriteDebugInfo("Installing prepatch, please wait...")
            Application.DoEvents()

            'list the names of all files in the specified directory
            Dim files = New DirectoryInfo(Program.Pso2RootDir & "\_precede\data\win32\").GetFiles()
            Dim counter = files.Length
            Dim count As Integer = 0

            For Each dra As FileInfo In files
                Dim downloadStr As String = dra.Name
                Helper.DeleteFile((Program.Pso2WinDir & "\" & downloadStr))
                File.Move(Program.Pso2RootDir & "\_precede\data\win32\" & downloadStr, (Program.Pso2WinDir & "\" & downloadStr))
                count += 1
                'lblStatus.Text = "Moved " & count & " files out of " & counter
                Application.DoEvents()
            Next
            If File.Exists("win32list_DO_NOT_DELETE_ME.txt") Then File.Delete("win32list_DO_NOT_DELETE_ME.txt")
            Helper.WriteDebugInfoSameLine("Done!")
            Helper.DeleteDirectory(Program.Pso2RootDir & "\_precede")
        End If
    End Sub
    Private Sub RestoreBackup(patchName As String)
        Dim backupPath As String = BuildBackupPath(patchName)
        If Directory.Exists(backupPath) = False Then Return

        Dim di As New DirectoryInfo(backupPath)
        Helper.WriteDebugInfoAndOk("Restoring " & patchName & " backup...")
        Application.DoEvents()

        'list the names of all files in the specified directory
        For Each dra As FileInfo In di.GetFiles()
            If File.Exists(Program.Pso2WinDir & "\" & dra.ToString()) Then
                Helper.DeleteFile(Program.Pso2WinDir & "\" & dra.ToString())
            End If
            File.Move(backupPath & "\" & dra.ToString(), Program.Pso2WinDir & "\" & dra.ToString())
        Next

        Helper.DeleteDirectory(backupPath)
        External.FlashWindow(Handle, True)

        Select Case patchName
            Case EnglishPatch
                If Not String.IsNullOrEmpty(RegKey.EnPatchVersion) Then RegKey.SetValue(Of String)(RegKey.EnPatchVersion, "Not Installed")
            Case LargeFiles
                If Not String.IsNullOrEmpty(RegKey.LargeFilesVersion) Then RegKey.SetValue(Of String)(RegKey.LargeFilesVersion, "Not Installed")
            Case StoryPatch
                If Not String.IsNullOrEmpty(RegKey.StoryPatchVersion) Then RegKey.SetValue(Of String)(RegKey.StoryPatchVersion, "Not Installed")
        End Select
        'WriteDebugInfoSameLine(" Done!")
        'UnlockGui()
    End Sub

    Private Sub DownloadPatch(patchUrl As String, patchName As String, patchFile As String, versionStr As String, msgBackup As String, msgSelectArchive As String)
        _cancelledFull = False
        Try
            If IsPso2WinDirMissing() Then Return

            Dim backupyesno As MsgBoxResult
            Dim predownloadedyesno As MsgBoxResult
            Dim rarLocation As String = ""
            Dim strVersion As String = ""

            ' Check the patch download method preference
            Dim patchPreference As String = RegKey.GetValue(Of String)(RegKey.PreDownloadedRar)
            Select Case patchPreference
                Case "Ask"
                    predownloadedyesno = MsgBox("Would you like to use predownload patch?", vbYesNo)
                Case "Always"
                    predownloadedyesno = MsgBoxResult.Yes
                Case "Never"
                    predownloadedyesno = MsgBoxResult.No
                Case Else
                    predownloadedyesno = MsgBox("Would you like to use predownload patch?", vbYesNo)
            End Select

            ' Check the backup preference
            patchPreference = "Never"
            'patchPreference = RegKey.GetValue(Of String)(RegKey.Backup)
            Select Case patchPreference
                Case "Ask"
                    backupyesno = MsgBox(msgBackup, vbYesNo)
                Case "Always"
                    backupyesno = MsgBoxResult.Yes
                Case "Never"
                    backupyesno = MsgBoxResult.No
                Case Else
                    backupyesno = MsgBox(msgBackup, vbYesNo)
            End Select

            If predownloadedyesno = MsgBoxResult.No Then
                ' Helper.WriteDebugInfo(Resources.strDownloading & patchName & "...")
                Application.DoEvents()

                ' Might want to switch to a Uri class.
                ' Get the filename from the downloaded Path
                Dim lastfilename As String() = patchUrl.Split("/"c)
                strVersion = lastfilename(lastfilename.Length - 1)
                strVersion = Path.GetFileNameWithoutExtension(strVersion) ' We're using this so that it's not format-specific.

                _cancelled = False

                If Not Helper.CheckLink(patchUrl) Then
                    Helper.WriteDebugInfoAndFailed("Failed to contact " & patchName & " website - Patch install/update canceled!")
                    Helper.WriteDebugInfo("Please visit http://goo.gl/YzCE7 for more information!")
                    Return
                End If

                DownloadFile(patchUrl, patchFile)
                If _cancelled Then Return
                ' Helper.WriteDebugInfo((Resources.strDownloadCompleteDownloaded & patchUrl & ")"))
            ElseIf predownloadedyesno = MsgBoxResult.Yes Then
                OpenFileDialog1.Title = msgSelectArchive
                OpenFileDialog1.FileName = "PSO2 " & patchName & " RAR file"
                OpenFileDialog1.Filter = "RAR Archives|*.rar|All Files (*.*) |*.*"
                If OpenFileDialog1.ShowDialog() = DialogResult.Cancel Then Return

                rarLocation = OpenFileDialog1.FileName
                strVersion = OpenFileDialog1.SafeFileName
                strVersion = Path.GetFileNameWithoutExtension(strVersion)
            End If

            Application.DoEvents()

            Helper.DeleteDirectory("TEMPPATCHAIDAFOOL")
            Directory.CreateDirectory("TEMPPATCHAIDAFOOL")
            Dim startInfo As New ProcessStartInfo() With {.FileName = (Program.StartPath & "\unrar.exe"), .Verb = "runas", .WindowStyle = ProcessWindowStyle.Normal, .UseShellExecute = True}
            If predownloadedyesno = MsgBoxResult.No Then startInfo.Arguments = ("e " & patchFile & " TEMPPATCHAIDAFOOL")
            If predownloadedyesno = MsgBoxResult.Yes Then startInfo.Arguments = ("e " & """" & rarLocation & """" & " TEMPPATCHAIDAFOOL")

            'Helper.WriteDebugInfo("Would you like to use predownload patch?")
            Process.Start(startInfo).WaitForExit()

            If Not Directory.Exists("TEMPPATCHAIDAFOOL") Then
                Directory.CreateDirectory("TEMPPATCHAIDAFOOL")
                Helper.WriteDebugInfo("Had to manually make temp update folder - Did the patch not extract right?")
            End If
            Dim diar1 As FileInfo() = New DirectoryInfo("TEMPPATCHAIDAFOOL").GetFiles()
            'Helper.WriteDebugInfoAndOk((Resources.strExtractingTo & Program.Pso2WinDir))
            Application.DoEvents()
            If _cancelledFull Then Return

            Dim backupPath As String = BuildBackupPath(patchName)
            If backupyesno = MsgBoxResult.Yes Then
                If Directory.Exists(backupPath) Then
                    Directory.Delete(backupPath, True)
                    Directory.CreateDirectory(backupPath)
                    'Helper.WriteDebugInfo(Resources.strErasingPreviousBackup)
                End If
                If Not Directory.Exists(backupPath) Then
                    Directory.CreateDirectory(backupPath)
                    'Helper.WriteDebugInfo(Resources.strCreatingBackupDirectory)
                End If
            End If

            Helper.Log("Extracted " & diar1.Length & " files from the patch")
            If diar1.Length = 0 Then
                Helper.WriteDebugInfo("Patch failed to extract correctly! Installation failed!")
                Return
            End If

            ' Helper.WriteDebugInfo(Resources.strInstallingPatch)
            messageForm.message_txt.Text = "Installing Patch."
            messageForm.Show()


            For Each dra As FileInfo In diar1
                If _cancelledFull Then Return
                If backupyesno = MsgBoxResult.Yes Then
                    If File.Exists((Program.Pso2WinDir & "\" & dra.ToString())) Then
                        File.Move((Program.Pso2WinDir & "\" & dra.ToString()), (backupPath & "\" & dra.ToString()))
                    End If
                End If
                If backupyesno = MsgBoxResult.No Then
                    If File.Exists((Program.Pso2WinDir & "\" & dra.ToString())) Then
                        Helper.DeleteFile((Program.Pso2WinDir & "\" & dra.ToString()))
                    End If
                End If
                File.Move(("TEMPPATCHAIDAFOOL\" & dra.ToString()), (Program.Pso2WinDir & "\" & dra.ToString()))
            Next

            Helper.DeleteDirectory("TEMPPATCHAIDAFOOL")
            If backupyesno = MsgBoxResult.No Then
                External.FlashWindow(Handle, True)
                'Helper.WriteDebugInfo("English patch " & Resources.strInstalledUpdated)
                messageForm.message_txt.Text = "Installed english patch."
                messageForm.Show()
                If Not String.IsNullOrEmpty(versionStr) Then RegKey.SetValue(Of String)(versionStr, strVersion)
            End If
            If backupyesno = MsgBoxResult.Yes Then
                External.FlashWindow(Handle, True)
                'Helper.WriteDebugInfo(("English patch " & Resources.strInstalledUpdatedBackup & backupPath))
                ' messageForm.message_txt.Text = "Installed updated backup."
                ' messageForm.Show()
                If Not String.IsNullOrEmpty(versionStr) Then RegKey.SetValue(Of String)(versionStr, strVersion)
            End If
            Helper.DeleteFile(patchName)
            'UnlockGui()
        Catch ex As Exception

            messageForm.message_txt.Text = ex.Message.ToString & " Stack Trace: " & ex.StackTrace
            messageForm.ShowDialog()

        End Try
    End Sub

    Private Sub DownloadPrePatch(version As String)
        _cancelledFull = False
        'lblStatus.Text = ""
        Helper.WriteDebugInfo("Downloading pre-patch filelist...")
        DownloadFile("http://download.pso2.jp/patch_prod/patches_precede/patchlist0.txt", "patchlist0.txt")
        DownloadFile("http://download.pso2.jp/patch_prod/patches_precede/patchlist1.txt", "patchlist1.txt")
        DownloadFile("http://download.pso2.jp/patch_prod/patches_precede/patchlist2.txt", "patchlist2.txt")
        DownloadFile("http://download.pso2.jp/patch_prod/patches_precede/patchlist3.txt", "patchlist3.txt")
        DownloadFile("http://download.pso2.jp/patch_prod/patches_precede/patchlist4.txt", "patchlist4.txt")
        DownloadFile("http://download.pso2.jp/patch_prod/patches_precede/patchlist5.txt", "patchlist5.txt")
        Helper.WriteDebugInfoSameLine("Done.")
        If Not Directory.Exists(Program.Pso2RootDir & "\_precede\data\win32") Then Directory.CreateDirectory(Program.Pso2RootDir & "\_precede\data\win32")

        Dim mergedPrePatches = MergePrePatches()
        mergedPrePatches.Remove("GameGuard.des")
        mergedPrePatches.Remove("PSO2JP.ini")
        mergedPrePatches.Remove("script/user_default.pso2")
        mergedPrePatches.Remove("script/user_intel.pso2")
        mergedPrePatches.Remove("")

        Helper.WriteDebugInfo("Checking for already existing precede files...")

        Dim missingfiles As New List(Of String)
        Dim truefilename As String
        Dim numberofChecks As Integer = 0

        For Each sBuffer In mergedPrePatches
            If _cancelledFull Then Return

            truefilename = sBuffer.Value

            If Not File.Exists(((Program.Pso2RootDir & "\_precede\data\win32") & "\" & truefilename)) Then
                If _vedaUnlocked Then Helper.WriteDebugInfo("DEBUG: The file " & truefilename & " is missing.")
                missingfiles.Add(truefilename)
            ElseIf Helper.GetMd5(((Program.Pso2RootDir & "\_precede\data\win32") & "\" & truefilename)) <> sBuffer.Key Then
                If _vedaUnlocked Then Helper.WriteDebugInfo("DEBUG: The file " & truefilename & " must be redownloaded.")
                missingfiles.Add(truefilename)
            End If

            numberofChecks += 1
            'lblStatus.Text = (Resources.strCurrentlyCheckingFile & numberofChecks & "")
            Application.DoEvents()
        Next

        Dim totaldownload As Long = missingfiles.Count
        Dim downloaded As Long = 0
        Dim totaldownloaded As Long = 0

        For Each downloadStr As String In missingfiles
            If _cancelledFull Then Return
            'Download the missing files:
            'WHAT THE FUCK IS GOING ON HERE?
            downloaded += 1
            totaldownloaded += _totalsize2

            'lblStatus.Text = Resources.strDownloading & "" & downloaded & "/" & totaldownload & " (" & Helper.SizeSuffix(totaldownloaded) & ")"

            Application.DoEvents()
            _cancelled = False
            DownloadFile(("http://download.pso2.jp/patch_prod/patches_precede/data/win32/" & downloadStr & ".pat"), downloadStr)
            If New FileInfo(downloadStr).Length = 0 Then
                Helper.Log("File appears to be empty, trying to download from secondary SEGA server")
                DownloadFile(("http://download.pso2.jp/patch_prod/patches_precede/data/win32/" & downloadStr & ".pat"), downloadStr)
            End If

            If _cancelled Then Return

            Helper.DeleteFile(((Program.Pso2RootDir & "\_precede\data\win32") & "\" & downloadStr))
            File.Move(downloadStr, ((Program.Pso2RootDir & "\_precede\data\win32") & "\" & downloadStr))
            If _vedaUnlocked Then Helper.WriteDebugInfo("DEBUG: Downloaded and installed " & downloadStr & ".")
            Application.DoEvents()
        Next

        If missingfiles.Count = 0 Then Helper.WriteDebugInfo("Your precede data is up to date!")
        If missingfiles.Count <> 0 Then
            Helper.WriteDebugInfo("Precede data downloaded/updated!")
        End If
        RegKey.SetValue(Of String)(RegKey.Pso2PrecedeVersion, version)
    End Sub



    Private Sub InstallPso2()
        Dim installFolder As String = ""
        'Const installYesNo As MsgBoxResult = vbYes
        'If installYesNo = vbNo Then
        '    WriteDebugInfo("You can view more information about the installer at:" & vbCrLf & "http://arks-layer.com/setup.php")
        '    Return
        'End If
        'If installYesNo = vbYes Then
        messageForm.message_txt.Text = "This will install Phantasy Star Online EPISODE 2! Please select a folder to install into." & vbCrLf & "A folder called PHANTASYSTARONLINE2 will be created inside the folder you choose." & vbCrLf & "(For example, if you choose the C drive, it will install to C:\PHANTASYSTARONLINE2\)" & vbCrLf & "It is HIGHLY RECOMMENDED that you do NOT install into the Program Files folder, but a normal folder like C:\PHANTASYSTARONLINE\"
        messageForm.ShowDialog()
        ' MsgBox("This will install Phantasy Star Online EPISODE 2! Please select a folder to install into." & vbCrLf & "A folder called PHANTASYSTARONLINE2 will be created inside the folder you choose." & vbCrLf & "(For example, if you choose the C drive, it will install to C:\PHANTASYSTARONLINE2\)" & vbCrLf & "It is HIGHLY RECOMMENDED that you do NOT install into the Program Files folder, but a normal folder like C:\PHANTASYSTARONLINE\")
        Dim installPso2 As Boolean = True
        While installPso2
            Dim myFolderBrowser As New FolderBrowserDialog With {.RootFolder = Environment.SpecialFolder.MyComputer, .Description = "Please select a folder (or drive) to install PSO2 into"}
            Dim dlgResult As DialogResult = myFolderBrowser.ShowDialog()

            If dlgResult = DialogResult.OK Then
                installFolder = myFolderBrowser.SelectedPath
            End If
            If dlgResult = DialogResult.Cancel Then
                Helper.WriteDebugInfo("Installation cancelled by user!")
                Return
            End If
            Dim correctYesNo As MsgBoxResult = MsgBox("You wish to install PSO2 into " & (installFolder & "\PHANTASYSTARONLINE2\. Is this correct?").Replace("\\", "\"), vbYesNoCancel)
            If correctYesNo = vbCancel Then
                Helper.WriteDebugInfo("Installation cancelled by user!")
                Return
            End If
            If correctYesNo = vbNo Then
                Continue While
            End If
            If correctYesNo = vbYes Then
                For Each drive In DriveInfo.GetDrives()
                    If (drive.DriveType = DriveType.Fixed) AndAlso (installFolder(0) = drive.Name(0)) Then
                        If drive.TotalSize < 26992893636 Then
                            messageForm.message_txt.Text = "There is not enough space on the selected disk to install PSO2. Please select a different drive. (Requires 36GB of free space)"
                            messageForm.ShowDialog()
                            Continue While
                        End If
                        If drive.AvailableFreeSpace < 26992893636 Then
                            messageForm.message_txt.Text = "There is not enough space on the selected disk to install PSO2. Please select a different drive. (Requires 36GB of free space)"
                            messageForm.ShowDialog()
                            Continue While
                        End If
                    End If
                Next

                Dim finalYesNo As MsgBoxResult = MsgBox("The program will now install the necessary files, create the folders, and set up the game. Afterwards, the program will automatically begin patching. Click ""OK"" to start.", MsgBoxStyle.OkCancel)
                If finalYesNo = vbCancel Then
                    Helper.WriteDebugInfo("Installation cancelled by user!")
                Else
                    'Office2007StartButton1.Enabled = False  'This is Depreciated.
                    'set the pso2Dir to the install patch
                    Program.Pso2RootDir = (installFolder & "\PHANTASYSTARONLINE2\pso2_bin").Replace("\\", "\")
                    'lblDirectory.Text = Program.Pso2RootDir 'This is depreciated.
                    Show()
                    Focus()
                    Application.DoEvents()
                    Helper.WriteDebugInfo("Downloading DirectX setup...")
                    Try
                        DownloadFile("http://arks-layer.com/docs/dxwebsetup.exe", "dxwebsetup.exe")
                        Helper.WriteDebugInfoSameLine("Done!")
                        Helper.WriteDebugInfo("Checking/Installing DirectX...")
                        Dim processStartInfo As ProcessStartInfo = New ProcessStartInfo() With {.FileName = "dxwebsetup.exe", .Verb = "runas", .Arguments = "/Q", .UseShellExecute = True}
                        Process.Start(processStartInfo).WaitForExit()
                        Helper.WriteDebugInfoSameLine("Done!")
                    Catch ex As Exception
                        Helper.WriteDebugInfo("DirectX installation failed! Please install it later if neccessary!")
                    End Try

                    If File.Exists("dxwebsetup.exe") Then File.Delete("dxwebsetup.exe")
                    'Make a data folder, and a win32 folder under that
                    Directory.CreateDirectory(Program.Pso2RootDir & "\data\win32\")
                    'Download required pso2 stuff
                    Helper.WriteDebugInfo("Downloading PSO2 required files...")
                    DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2launcher.exe.pat", Program.Pso2RootDir & "\pso2launcher.exe")
                    DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2updater.exe.pat", Program.Pso2RootDir & "\pso2updater.exe")
                    DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2.exe.pat", Program.Pso2RootDir & "\pso2.exe")
                    DownloadFile("http://download.pso2.jp/patch_prod/patches/PSO2JP.ini.pat", Program.Pso2RootDir & "\PSO2JP.ini")
                    Helper.WriteDebugInfoSameLine("Done!")
                    'Download Gameguard.des
                    Helper.WriteDebugInfo("Downloading Latest Gameguard file...")
                    DownloadFile("http://download.pso2.jp/patch_prod/patches/GameGuard.des.pat", Program.Pso2RootDir & "\GameGuard.des")
                    'Helper.WriteDebugInfoSameLine(Resources.strDone)
                    Application.DoEvents()

                    RegKey.SetValue(Of String)(RegKey.Pso2Dir, Program.Pso2RootDir)
                    Helper.WriteDebugInfo(Program.Pso2RootDir & " " & "Set your pso2bin")
                    Program.Pso2WinDir = (Program.Pso2RootDir & "\data\win32")
                    If String.IsNullOrEmpty(RegKey.GetValue(Of String)(RegKey.StoryPatchVersion)) Then RegKey.SetValue(Of String)(RegKey.StoryPatchVersion, "Not Installed")
                    If String.IsNullOrEmpty(RegKey.GetValue(Of String)(RegKey.EnPatchVersion)) Then RegKey.SetValue(Of String)(RegKey.EnPatchVersion, "Not Installed")
                    If String.IsNullOrEmpty(RegKey.GetValue(Of String)(RegKey.LargeFilesVersion)) Then RegKey.SetValue(Of String)(RegKey.LargeFilesVersion, "Not Installed")

                    'Check for PSO2 Updates~
                    UpdatePso2(False)

                    messageForm.message_txt.Text = "PSO2 installed, patched to the latest Japanese version, and ready to play!" & vbCrLf & "Press OK to continue."
                    messageForm.ShowDialog()
                    'MsgBox("PSO2 installed, patched to the latest Japanese version, and ready to play!" & vbCrLf & "Press OK to continue.")
                    Refresh()
                End If
            End If

            installPso2 = False
            Program.IsPso2Installed = True
        End While
        Close()
        'End If
    End Sub




    Private Sub UpdatePso2(comingFromOldFiles As Boolean)
        _cancelledFull = False
        If IsPso2WinDirMissing() Then Return
        Dim missingfiles As New List(Of String)
        Dim missingfiles2 As New List(Of String)
        Dim numberofChecks As Integer = 0
        Dim totalfilesize As Long = 0
        Dim testfilesize As String()
        ' lblStatus.Text = ""

        If Directory.Exists(BuildBackupPath(EnglishPatch)) Then
            ' Helper.WriteDebugInfo(Resources.strENBackupFound)
            RestoreBackup(EnglishPatch)
        End If

        If Directory.Exists(BuildBackupPath(LargeFiles)) Then
            'Helper.WriteDebugInfo(Resources.strLFBackupFound)
            RestoreBackup(LargeFiles)
        End If

        If Directory.Exists(BuildBackupPath(StoryPatch)) Then
            ' Helper.WriteDebugInfo(Resources.strStoryBackupFound)
            RestoreBackup(StoryPatch)
        End If

        ' Why is the UI being disabled here, is there something I'm missing? -LightningDragon
        ' LockGui()
        'Helper.WriteDebugInfo(Resources.strDownloadingPatchFile1)

        Application.DoEvents()
        DownloadFile("http://download.pso2.jp/patch_prod/patches/launcherlist.txt", "launcherlist.txt")
        ' Helper.WriteDebugInfoSameLine(Resources.strDone)
        ' Helper.WriteDebugInfo(Resources.strDownloadingPatchFile2)

        Application.DoEvents()
        DownloadFile("http://download.pso2.jp/patch_prod/patches/patchlist.txt", "patchlist.txt")
        ' Helper.WriteDebugInfoSameLine(Resources.strDone)
        ' Helper.WriteDebugInfo(Resources.strDownloadingPatchFile3)

        Application.DoEvents()
        DownloadFile("http://download.pso2.jp/patch_prod/patches_old/patchlist.txt", "patchlist_old.txt")
        'Helper.WriteDebugInfoSameLine(Resources.strDone)
        ' Helper.WriteDebugInfo(Resources.strDownloadingPatchFile4)

        Application.DoEvents()
        Program.Client.DownloadFile("http://arks-layer.com/vanila/version.txt", "version.ver")
        'Helper.WriteDebugInfoSameLine(Resources.strDone)
        Application.DoEvents()
        ' UnlockGui()

        'Const result As MsgBoxResult = MsgBoxResult.No
        'If result = MsgBoxResult.Yes OrElse _comingFromOldFiles Then
        If comingFromOldFiles Then
            'Helper.WriteDebugInfo(Resources.strCheckingforNewContent)
            numberofChecks = 0

            If _cancelledFull Then Return
            For Each line In Helper.GetLines("patchlist.txt")
                If _cancelledFull Then Return
                Dim filename As String() = Regex.Split(line, ".pat")
                Dim truefilename As String = filename(0).Replace("data/win32/", "")
                Dim trueMd5 As String = filename(1).Split(ControlChars.Tab)(2)
                If truefilename <> "GameGuard.des" AndAlso truefilename <> "edition.txt" AndAlso truefilename <> "gameversion.ver" AndAlso truefilename <> "pso2.exe" AndAlso truefilename <> "PSO2JP.ini" AndAlso truefilename <> "script/user_default.pso2" AndAlso truefilename <> "script/user_intel.pso2" Then
                    If Not File.Exists((Program.Pso2WinDir & "\" & truefilename)) Then
                        If _vedaUnlocked Then Helper.WriteDebugInfo("DEBUG: The file " & truefilename & " is missing.")
                        missingfiles.Add(truefilename)
                    ElseIf Helper.GetMd5((Program.Pso2WinDir & "\" & truefilename)) <> trueMd5 Then
                        If _vedaUnlocked Then Helper.WriteDebugInfo("DEBUG: The file " & truefilename & " must be redownloaded.")
                        missingfiles.Add(truefilename)
                    End If
                End If

                numberofChecks += 1
                'lblStatus.Text = (Resources.strCurrentlyCheckingFile & numberofChecks & "")
                Application.DoEvents()
            Next

            Helper.DeleteFile("resume.txt")
            File.AppendAllLines("resume.txt", missingfiles)
            Dim totaldownload As Long = missingfiles.Count
            Dim downloaded As Long = 0
            Dim totaldownloadedthings As Long = 0

            For Each downloadStr In missingfiles
                If _cancelledFull Then Return
                'Download the missing files:
                'WHAT THE FUCK IS GOING ON HERE?
                downloaded += 1
                totaldownloadedthings += _totalsize2
                ' lblStatus.Text = Resources.strDownloading & "" & downloaded & "/" & totaldownload & " (" & Helper.SizeSuffix(totaldownloadedthings) & ")"

                Application.DoEvents()
                _cancelled = False
                DownloadFile(("http://download.pso2.jp/patch_prod/patches/data/win32/" & downloadStr & ".pat"), downloadStr)
                If New FileInfo(downloadStr).Length = 0 Then
                    Helper.Log("File appears to be empty, trying to download from secondary SEGA server")
                    DownloadFile(("http://download.pso2.jp/patch_prod/patches_old/data/win32/" & downloadStr & ".pat"), downloadStr)
                End If

                If _cancelled Then Return
                Helper.DeleteFile((Program.Pso2WinDir & "\" & downloadStr))
                File.Move(downloadStr, (Program.Pso2WinDir & "\" & downloadStr))
                If _vedaUnlocked Then Helper.WriteDebugInfo("DEBUG: Downloaded and installed " & downloadStr & ".")
                Dim linesList As New List(Of String)(File.ReadAllLines("resume.txt"))

                'Remove the line to delete, e.g.
                linesList.Remove(downloadStr)

                File.WriteAllLines("resume.txt", linesList.ToArray())
                Application.DoEvents()
            Next

            If missingfiles.Count = 0 Then Helper.WriteDebugInfo("All systems good. You are up to date.")
            Dim directoryStringthing As String = (Program.Pso2RootDir & "\")
            'Helper.WriteDebugInfo(Resources.strDownloading & "version file...")
            Application.DoEvents()
            _cancelled = False
            Program.Client.DownloadFile("http://arks-layer.com/vanila/version.txt", "version.ver")
            If _cancelled Then Return
            If File.Exists((_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver")) Then Helper.DeleteFile((_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver"))
            File.Copy("version.ver", (_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver"))
            'Helper.WriteDebugInfoAndOk((Resources.strDownloadedandInstalled & "version file"))

            ' Helper.WriteDebugInfo(Resources.strDownloading & "pso2launcher.exe...")
            Application.DoEvents()
            For Each proc As Process In Process.GetProcessesByName("pso2launcher")
                If proc.MainWindowTitle = "PHANTASY STAR ONLINE 2" AndAlso proc.MainModule.ToString() = "ProcessModule (pso2launcher.exe)" Then proc.Kill()
            Next

            DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2launcher.exe.pat", "pso2launcher.exe")
            If _cancelled Then Return
            If File.Exists((directoryStringthing & "pso2launcher.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((directoryStringthing & "pso2launcher.exe"))
            File.Move("pso2launcher.exe", (directoryStringthing & "pso2launcher.exe"))
            'Helper.WriteDebugInfoAndOk((Resources.strDownloadedandInstalled & "pso2launcher.exe"))
            ' Helper.WriteDebugInfo(Resources.strDownloading & "pso2updater.exe...")
            Application.DoEvents()
            For Each proc As Process In Process.GetProcessesByName("pso2updater")
                If proc.MainModule.ToString() = "ProcessModule (pso2updater.exe)" Then proc.Kill()
            Next

            DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2updater.exe.pat", "pso2updater.exe")
            If _cancelled Then Return
            If File.Exists((directoryStringthing & "pso2updater.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((directoryStringthing & "pso2updater.exe"))
            File.Move("pso2updater.exe", (directoryStringthing & "pso2updater.exe"))
            ' Helper.WriteDebugInfoAndOk((Resources.strDownloadedandInstalled & "pso2updater.exe"))
            Application.DoEvents()

            ' Helper.WriteDebugInfo(Resources.strDownloading & "pso2.exe...")
            For Each proc As Process In Process.GetProcessesByName("pso2")
                If proc.MainModule.ToString() = "ProcessModule (pso2.exe)" Then proc.Kill()
            Next

            DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2.exe.pat", "pso2.exe")
            If _cancelled Then Return

            If File.Exists((directoryStringthing & "pso2.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((directoryStringthing & "pso2.exe"))
            File.Move("pso2.exe", (directoryStringthing & "pso2.exe"))
            If _cancelledFull Then Return
            ' Helper.WriteDebugInfoAndOk((Resources.strDownloadedandInstalled & "pso2.exe"))
            RegKey.SetValue(Of String)(RegKey.StoryPatchVersion, "Not Installed")
            RegKey.SetValue(Of String)(RegKey.EnPatchVersion, "Not Installed")
            RegKey.SetValue(Of String)(RegKey.LargeFilesVersion, "Not Installed")
            RegKey.SetValue(Of String)(RegKey.Pso2PatchlistMd5, Helper.GetMd5("patchlist.txt"))
            '  Helper.WriteDebugInfo(Resources.strGameUpdatedVanilla)
            Helper.DeleteFile("resume.txt")
            RegKey.SetValue(Of String)(RegKey.Pso2RemoteVersion, File.ReadAllLines("version.ver")(0))
            'UnlockGui()

            If Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.RemoveCensor)) Then
                If File.Exists((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c.backup")) Then Computer.FileSystem.DeleteFile((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c.backup"), UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently)
                Computer.FileSystem.RenameFile((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c"), "ffbff2ac5b7a7948961212cefd4d402c.backup")
                ' Helper.WriteDebugInfoAndOk(Resources.strRemoving & "Censor...")
            End If

            'Helper.WriteDebugInfoAndOk(Resources.strallDone)
            Return
        Else
            '  TopMost = chkAlwaysOnTop.Checked
        End If

        If _cancelledFull Then Return
        Dim mergedPatches = MergePatches()
        ' Helper.WriteDebugInfo(Resources.strCheckingforAllFiles)

        mergedPatches.Remove("GameGuard.des")
        mergedPatches.Remove("PSO2JP.ini")
        mergedPatches.Remove("script/user_default.pso2")
        mergedPatches.Remove("script/user_intel.pso2")
        mergedPatches.Remove("")

        If mergedPatches.ContainsKey("pso2.exe") Then
            mergedPatches.Remove("pso2.exe")
        End If

        Dim dataPath = Program.Pso2RootDir & "\data\win32\"
        Dim length = mergedPatches.Count
        'Dim oldmax = PBMainBar.Maximum
        'PBMainBar.Maximum = length
        _cancelled = False

        Dim fileLengths = New DirectoryInfo(dataPath).EnumerateFiles().ToDictionary(Function(fileinfo) fileinfo.Name, Function(fileinfo) fileinfo.Length)
        Dim fileNames = fileLengths.Keys

        For Each kvp In mergedPatches

            If _cancelled Then
                ' PBMainBar.Text = ""
                ' PBMainBar.Value = 0
                ' PBMainBar.Maximum = oldmax
                _cancelled = False
                Return
            End If

            '   lblStatus.Text = (Resources.strCurrentlyCheckingFile & numberofChecks)
            '   PBMainBar.Text = numberofChecks & " / " & length
            If (numberofChecks Mod 8) = 0 Then Application.DoEvents()
            numberofChecks += 1
            '  PBMainBar.Value += 1

            If Not fileNames.Contains(kvp.Key) Then
                If _vedaUnlocked Then Helper.WriteDebugInfo("DEBUG: The file " & (dataPath & kvp.Key) & "is missing.")
                testfilesize = kvp.Value.Split(ControlChars.Tab)
                totalfilesize += Convert.ToInt32(testfilesize(1))
                missingfiles2.Add(kvp.Key)
                Continue For
            End If

            testfilesize = kvp.Value.Split(ControlChars.Tab)
            Dim fileSize = Convert.ToInt32(testfilesize(1))

            If fileSize <> fileLengths(kvp.Key) Then
                If _vedaUnlocked Then Helper.WriteDebugInfo("DEBUG: The file " & kvp.Key & " must be redownloaded.")
                totalfilesize += fileSize
                missingfiles2.Add(kvp.Key)
                Continue For
            End If

            Using stream = New FileStream(dataPath & kvp.Key, FileMode.Open)
                If Helper.GetMd5(stream) <> testfilesize(2) Then
                    If _vedaUnlocked Then Helper.WriteDebugInfo("DEBUG: The file " & kvp.Key & " must be redownloaded.")
                    totalfilesize += fileSize
                    missingfiles2.Add(kvp.Key)
                End If
            End Using
        Next

        'PBMainBar.Text = ""
        'PBMainBar.Value = 0
        'PBMainBar.Maximum = oldmax

        Dim totaldownload2 As Long = missingfiles2.Count
        Dim downloaded2 As Long = 0
        Dim totaldownloaded As Long = 0
        Helper.DeleteFile("resume.txt")
        File.WriteAllLines("resume.txt", missingfiles2.ToArray())

        For Each downloadStr In missingfiles2
            If _cancelledFull Then Return
            'Download the missing files:
            'WHAT THE FUCK IS GOING ON HERE?
            downloaded2 += 1
            totaldownloaded += _totalsize2

            'lblStatus.Text = Resources.strDownloading & "" & downloaded2 & "/" & totaldownload2 & " (" & Helper.SizeSuffix(totaldownloaded) & " / " & Helper.SizeSuffix(totalfilesize) & ")"

            Application.DoEvents()
            DownloadFile(("http://download.pso2.jp/patch_prod/patches/data/win32/" & downloadStr & ".pat"), downloadStr)
            If New FileInfo(downloadStr).Length = 0 Then
                Helper.Log("File appears to be empty, trying to download from secondary SEGA server")
                DownloadFile(("http://download.pso2.jp/patch_prod/patches_old/data/win32/" & downloadStr & ".pat"), downloadStr)
            End If
            If New FileInfo(downloadStr).Length = 0 Then
                Helper.DeleteFile(downloadStr)
                DownloadFile(("http://download.pso2.jp/patch_prod/patches_old/data/win32/" & downloadStr & ".pat"), downloadStr)
            End If

            If File.Exists(downloadStr) Then
                Helper.DeleteFile((Program.Pso2WinDir & "\" & downloadStr))
                File.Move(downloadStr, (Program.Pso2WinDir & "\" & downloadStr))
                If _vedaUnlocked Then Helper.WriteDebugInfo("DEBUG: Downloaded and installed " & downloadStr & ".")
                Dim linesList As New List(Of String)(File.ReadAllLines("resume.txt"))

                'Remove the line to delete, e.g.
                linesList.Remove(downloadStr)

                File.WriteAllLines("resume.txt", linesList.ToArray())
            End If
            Application.DoEvents()
        Next

        If missingfiles.Count = 0 Then Helper.WriteDebugInfo("You are up to date.")
        Dim directoryString As String = (Program.Pso2RootDir & "\")
        ' Helper.WriteDebugInfo(Resources.strDownloading & "version file...")
        Application.DoEvents()
        Program.Client.DownloadFile("http://arks-layer.com/vanila/version.txt", "version.ver")
        If File.Exists((_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver")) Then Helper.DeleteFile((_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver"))
        File.Copy("version.ver", (_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver"))
        Helper.WriteDebugInfoAndOk(("It has been downloaded and installed." & "version file"))

        Helper.WriteDebugInfo("Downloading pso2launcher.exe...")
        Application.DoEvents()
        DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2launcher.exe.pat", "pso2launcher.exe")
        If File.Exists((directoryString & "pso2launcher.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((directoryString & "pso2launcher.exe"))
        File.Move("pso2launcher.exe", (directoryString & "pso2launcher.exe"))
        Helper.WriteDebugInfoAndOk(("It has been downloaded and installed pso2launcher.exe"))
        Helper.WriteDebugInfo("Downloading pso2updater.exe...")
        Application.DoEvents()

        Helper.WriteDebugInfo("Downloading pso2.exe...")
        DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2.exe.pat", "pso2.exe")
        If _cancelled Then Return

        If File.Exists((directoryString & "pso2.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((directoryString & "pso2.exe"))
        File.Move("pso2.exe", (directoryString & "pso2.exe"))
        If _cancelledFull Then Return
        Helper.WriteDebugInfoAndOk(("Downloaded and installed pso2.exe"))

        RegKey.SetValue(Of String)(RegKey.StoryPatchVersion, "Not Installed")
        RegKey.SetValue(Of String)(RegKey.EnPatchVersion, "Not Installed")
        RegKey.SetValue(Of String)(RegKey.LargeFilesVersion, "Not Installed")
        RegKey.SetValue(Of String)(RegKey.Pso2PatchlistMd5, Helper.GetMd5("patchlist.txt"))
        Helper.WriteDebugInfo("The game has been updated succesfully.")
        Helper.DeleteFile("resume.txt")
        RegKey.SetValue(Of String)(RegKey.Pso2RemoteVersion, File.ReadAllLines("version.ver")(0))
        'UnlockGui()

        If Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.RemoveCensor)) Then
            If File.Exists((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c.backup")) Then Computer.FileSystem.DeleteFile((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c.backup"), UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently)
            Computer.FileSystem.RenameFile((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c"), "ffbff2ac5b7a7948961212cefd4d402c.backup")
            Helper.WriteDebugInfoAndOk("Removing Censor...")
        End If

        Helper.WriteDebugInfoAndOk("Complete")
    End Sub
End Class