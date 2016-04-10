Imports Microsoft.VisualBasic.FileIO
Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Runtime.InteropServices
Imports mock2Launcher.My

Public Class mocklauncher

    Const EnglishPatch = "English Patch"
    Const StoryPatch = "Story Patch"
    Const LargeFiles = "Large Files"

    Dim selectedLanguagePatch As String = "JP"

    Private allowCoolMove As Boolean = False
    Private myCoolPoint As New Point

    'Dim address As String = "http://162.206.115.127:64080/pso2/version.txt" 'Checks downloads to the latest version of the launcher
    'Dim currentversion As String = Me.ProductVersion
    'Dim client As WebClient = New WebClient
    'Private IsFormBeingDragged As Boolean = False

    Dim filepath As String = CurDir()

    Dim fps As String

    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    Public Const HT_CAPTION As Integer = &H2

    'ReadOnly _pso2OptionsFrm As FrmPso2Options
    'ReadOnly _optionsFrm As FrmOptions
    ReadOnly _documents As String = (Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) & "\")
    ReadOnly _usersettingsfile As String = (_documents & "SEGA\PHANTASYSTARONLINE2\user.pso2")
    Dim finalPath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString & "\SEGA\PHANTASYSTARONLINE2\" & "user.pso2"
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


    Sub New()
        Try
            ' This call is required by the designer.
            InitializeComponent()

            language_cb.SelectedIndex = 0

            ' Add any initialization after the InitializeComponent() call.
            Program.MainForm = Me

            Dim regValue As Integer

            regValue = RegKey.GetValue(Of Integer)(RegKey.TextBoxBgColor)
            If regValue = 0 Then RegKey.SetValue(Of UInteger)(RegKey.TextBoxBgColor, 4294967295)
            ' If regValue <> 0 Then rtbDebug.BackColor = Color.FromArgb(Convert.ToInt32(regValue))

            regValue = RegKey.GetValue(Of Integer)(RegKey.TextBoxColor)
            ' If regValue <> 0 Then rtbDebug.ForeColor = Color.FromArgb(Convert.ToInt32(regValue))

            If String.IsNullOrEmpty(RegKey.GetValue(Of String)(RegKey.UseOldProgressBar.ToString)) Then RegKey.SetValue(Of Boolean)(RegKey.UseOldProgressBar, False)

        Catch ex As Exception

        End Try



    End Sub


    Private Sub mocklauncher_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Check for previous instance and exit if found.'
        'check for update
        Dim LatestAppVersion
        Dim wc As New Net.WebClient
        latestAppVersion = wc.DownloadString("http://162.206.115.127:64080/pso2/version.txt")
        'label1.Text = Text
        ' Textbox2.Text = reader.ReadToEnd
        'MsgBox(Text)
        If LatestAppVersion <> ProductVersion Then

            'Me.Hide()
            ApplicationUpdater.ShowDialog()
        Else
            ' MsgBox("No new updates.           ")
        End If

        tmrWaitingforPSO2.Interval = 20000
        Try


            Try
                If File.Exists(filepath & "/mock2updated.exe") = True Then
                    File.Delete(filepath & "/mock2updated.exe")
                Else
                    '
                End If
            Catch ex As Exception
                'Ignore this case as it may fail due to permission level.
            End Try

            lblDirectory.Text = Program.Pso2RootDir

            If Not File.Exists("7za.exe") Then
                Helper.WriteDebugInfo("Downloading 7za.exe...")
                Application.DoEvents()
                DownloadFile(Program.FreedomUrl & "7za.exe", "7za.exe")
            End If

            For index = 1 To 5
                If Helper.GetMd5("7za.exe") <> "42BADC1D2F03A8B1E4875740D3D49336" Then
                    Helper.WriteDebugInfo("Your corrupt.")
                    Application.DoEvents()
                    DownloadFile(Program.FreedomUrl & "7za.exe", "7za.exe")
                Else
                    Exit For
                End If
            Next

            If Not File.Exists("UnRar.exe") Then
                Helper.WriteDebugInfo("Downloading UnRar.exe...")
                Application.DoEvents()
                DownloadFile(Program.FreedomUrl & "UnRAR.exe", "UnRAR.exe")
            End If

            For index = 1 To 5
                If Helper.GetMd5("UnRar.exe") <> "0C83C1293723A682577E3D0B21562B79" Then
                    Helper.WriteDebugInfo("UnRar.exe Is corrupt.")
                    Application.DoEvents()
                    DownloadFile(Program.FreedomUrl & "UnRAR.exe", "UnRAR.exe")
                Else
                    Exit For
                End If
            Next

            Helper.DeleteDirectory("TEMPSTORYAIDAFOOL")
            Helper.DeleteFile("launcherlist.txt")
            Helper.DeleteFile("patchlist.txt")
            Helper.DeleteFile("patchlist_old.txt")

            'Added in precede files. Stupid ass SEGA.
            Helper.DeleteFile("patchlist0.txt")
            Helper.DeleteFile("patchlist1.txt")
            Helper.DeleteFile("patchlist2.txt")
            Helper.DeleteFile("patchlist3.txt")
            Helper.DeleteFile("patchlist4.txt")
            Helper.DeleteFile("patchlist5.txt")
            Helper.DeleteFile("precede.txt")
            Helper.DeleteFile("ServerConfig.txt")
            Helper.DeleteFile("precede_apply.txt")
            Helper.DeleteFile("version.ver")
            Helper.DeleteFile("Story MD5HashList.txt")
            Helper.DeleteFile("PluginMD5HashList.txt")
            Helper.DeleteFile("working.txt")
            Helper.DeleteFile("gnfieldstatus.txt")




            If File.Exists("Resume.txt") Then
                Dim yesNoResume As MsgBoxResult = MsgBox("It seems that the last patching attempt was interrupted. Would you Like To Resume patching?", vbYesNo)
                If yesNoResume = MsgBoxResult.Yes Then
                    ResumePatching()
                Else
                    Helper.DeleteFile("Resume.txt")
                End If


            End If



            If Program.WayuIsAFailure Then
                Helper.WriteDebugInfo("Skipping downloads For Wayu!")
            Else
                If String.IsNullOrEmpty(RegKey.GetValue(Of String)(RegKey.UseItemTranslation)) Then
                    RegKey.SetValue(Of Boolean)(RegKey.UseItemTranslation, True)
                End If

                Program.UseItemTranslation = Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.UseItemTranslation))

                If Directory.Exists(Program.Pso2RootDir & "\plugins\") = False Then
                    Helper.WriteDebugInfoAndOk("Setting up plugin system...")
                    Directory.CreateDirectory(Program.Pso2RootDir & "\plugins\")
                    Directory.CreateDirectory(Program.Pso2RootDir & "\plugins\disabled\")
                End If

                If Not Dns.GetHostEntry("gs001.pso2gs.net").AddressList(0).ToString().Contains("210.189.") And File.Exists(Program.Pso2RootDir & "\plugins\disabled\PSO2Proxy.dll") = True And File.Exists(Program.Pso2RootDir & "\plugins\PSO2Proxy.dll") = False Then
                    Helper.WriteDebugInfo("PSO2Proxy usage detected! Auto-enabling PSO2Proxy plugin.")
                    File.Move((Program.Pso2RootDir & "\plugins\disabled\PSO2Proxy.dll"), (Program.Pso2RootDir & "\plugins\PSO2Proxy.dll"))
                End If
            End If
            CheckForPluginUpdates()


            ListViewEx1.Clear()
            ' make a reference to a directory
            Dim di As New IO.DirectoryInfo(Program.Pso2RootDir & "\plugins\")

            Dim diar1 As IO.FileInfo() = di.GetFiles()
            Dim dra As IO.FileInfo
            Dim DisplayName As String = ""
            'list the names of all files in the specified directory
            For Each dra In diar1
                DisplayName = dra.Name.Replace("translator.dll", "Item Translation").Replace("PSO2TitleTranslator.dll", "Title Translation").Replace("PSO2DamageDump.dll", "Damage Parser").Replace("PSO2Proxy.dll", "PSO2Proxy Plugin")
                ListViewEx1.Items.Add(DisplayName).Checked = True
            Next

            Dim di2 As New IO.DirectoryInfo(Program.Pso2RootDir & " \plugins\disabled\")
            Dim diar2 As IO.FileInfo() = di2.GetFiles()
            Dim dra2 As IO.FileInfo

            'list the names of all files in the specified directory
            For Each dra2 In diar2
                DisplayName = dra2.Name.Replace("translator.dll", "Item Translation").Replace("PSO2TitleTranslator.dll", "Title Translation").Replace("PSO2DamageDump.dll", "Damage Parser").Replace("PSO2Proxy.dll", "PSO2Proxy Plugin")
                ListViewEx1.Items.Add(DisplayName).Checked = False
            Next



            Try
                If Not File.Exists(_usersettingsfile) Then
                    File.WriteAllText(_usersettingsfile, Program.MainForm.txtPSO2DefaultINI.Text)
                    Helper.WriteDebugInfo("Generating New PSO2 Settings file... Done!")
                End If

                Dim devM As External.Devmode
                devM.dmDeviceName = New String(Chr(0), 32)
                devM.dmFormName = New String(Chr(0), 32)
                devM.dmSize = CShort(Marshal.SizeOf(GetType(External.Devmode)))

                Dim modeIndex As Integer = 0
                ' 0 = The first mode
                While External.EnumDisplaySettings(Nothing, modeIndex, devM)
                    ' Mode found
                    If Not screenResolution_cb.Items.Contains(devM.dmPelsWidth & "x" & devM.dmPelsHeight) Then screenResolution_cb.Items.Add(devM.dmPelsWidth & "x" & devM.dmPelsHeight)

                    ' The next mode
                    modeIndex += 1
                End While



                Dim currentHeight As String = ReadIniSetting("Height3d")
                Dim currentWidth As String = ReadIniSetting("Width3d")

                Dim fullRes As String = currentWidth & "x" & currentHeight

                screenResolution_cb.Text = fullRes
                simpleSettigns_tb.Value = Convert.ToInt32(ReadIniSetting("DrawLevel"))
                bgmVol_trkb.Value = Convert.ToInt32(ReadIniSetting("Bgm"))
                seVol_tb.Value = Convert.ToInt32(ReadIniSetting("Se"))
                voiceVol_tb.Value = Convert.ToInt32(ReadIniSetting("Voice"))
                vidVol_tb.Value = Convert.ToInt32(ReadIniSetting("Movie"))
                If ReadIniSetting("Surround") = "True" Then surroundON_rb.Checked = True
                If ReadIniSetting("Surround") = "True" Then surroundOFF_rb.Checked = True
                If ReadIniSetting("TextureResolution") = "2" Then highResTextures_rb.Checked = True
                If ReadIniSetting("TextureResolution") = "1" Then standardRes_rb.Checked = True
                If ReadIniSetting("TextureResolution") = "0" Then compactRes_rb.Checked = True 'Convert.ToInt32(ReadIniSetting("TextureResolution"))
                charSize_cb.SelectedIndex = Convert.ToInt32(ReadIniSetting("InterfaceSize"))
                'fpsRate_cb.Text = ReadIniSetting("FrameKeep") & " FPS"
                If ReadIniSetting("FrameKeep") = "30" Then fpsRate_cb.SelectedIndex = 0
                If ReadIniSetting("FrameKeep") = "60" Then fpsRate_cb.SelectedIndex = 1
                If ReadIniSetting("FrameKeep") = "90" Then fpsRate_cb.SelectedIndex = 2
                If ReadIniSetting("FrameKeep") = "120" Then fpsRate_cb.SelectedIndex = 3
                If ReadIniSetting("FrameKeep") = "0" Then fpsRate_cb.SelectedIndex = 4
                'If fpsRate_cb.Text = "0 FPS" Then fpsRate_cb.Text = "Unlimited FPS"
                If ReadIniSetting("ShaderQuality") = "True" Then shaderStandard_rb.Checked = True
                If ReadIniSetting("ShaderQuality") = "False" Then sharderSimple_rb.Checked = True
                If ReadIniSetting("MoviePlay") = "True" Then enableVideos_rb.Checked = True
                If ReadIniSetting("MoviePlay") = "False" Then disableVideos_rb.Checked = True
                If ReadIniSetting("FullScreen") = "False" Then
                    windowed_rb.Checked = True
                End If
                If ReadIniSetting("FullScreen") = "True" Then
                    fullScreen_rb.Checked = True
                End If
                If ReadIniSetting("Fullscreen") = "False" And ReadIniSetting("VirtualFullScreen") = "True" Then
                    virtualFullscreen_rb.Checked = True
                    screenResolution_cb.Enabled = False
                    'Disable resolution thingie
                End If
                'ComboBoxEx5.Text = ReadINISetting("Width", 240) & "x" & ReadINISetting("Height", 240)
                If Not screenResolution_cb.Items.Contains(screenResolution_cb.Text) Then screenResolution_cb.SelectedIndex = 0
                CheckBoxX1.Checked = False
                If ReadIniSetting("Y") = "99999" Then
                    If ReadIniSetting("X") = "99999" Then
                        CheckBoxX1.Checked = True
                    End If
                End If




            Catch ex As Exception

                IO.File.WriteAllBytes(finalPath, Resources.user)
                'My.Resources.user.(finalPath)

                messageForm.message_txt.Text = (ex.Message & vbCrLf & "This Is due To your local user settings file To be either corrupt Or erased. Rest assured, this has been replaced by a fresh New file With Default SEGA settings applied.")
                messageForm.ShowDialog()


                Dim devM As External.Devmode
                devM.dmDeviceName = New String(Chr(0), 32)
                devM.dmFormName = New String(Chr(0), 32)
                devM.dmSize = CShort(Marshal.SizeOf(GetType(External.Devmode)))

                Dim modeIndex As Integer = 0
                ' 0 = The first mode
                While External.EnumDisplaySettings(Nothing, modeIndex, devM)
                    ' Mode found
                    If Not screenResolution_cb.Items.Contains(devM.dmPelsWidth & "x" & devM.dmPelsHeight) Then screenResolution_cb.Items.Add(devM.dmPelsWidth & "x" & devM.dmPelsHeight)

                    ' The next mode
                    modeIndex += 1
                End While



                Dim currentHeight As String = ReadIniSetting("Height3d")
                Dim currentWidth As String = ReadIniSetting("Width3d")

                Dim fullRes As String = currentWidth & "x" & currentHeight

                screenResolution_cb.Text = fullRes
                simpleSettigns_tb.Value = Convert.ToInt32(ReadIniSetting("DrawLevel"))
                bgmVol_trkb.Value = Convert.ToInt32(ReadIniSetting("Bgm"))
                seVol_tb.Value = Convert.ToInt32(ReadIniSetting("Se"))
                voiceVol_tb.Value = Convert.ToInt32(ReadIniSetting("Voice"))
                vidVol_tb.Value = Convert.ToInt32(ReadIniSetting("Movie"))
                If ReadIniSetting("Surround") = "True" Then surroundON_rb.Checked = True
                If ReadIniSetting("Surround") = "True" Then surroundOFF_rb.Checked = True
                If ReadIniSetting("TextureResolution") = "2" Then highResTextures_rb.Checked = True
                If ReadIniSetting("TextureResolution") = "1" Then standardRes_rb.Checked = True
                If ReadIniSetting("TextureResolution") = "0" Then compactRes_rb.Checked = True 'Convert.ToInt32(ReadIniSetting("TextureResolution"))
                charSize_cb.SelectedIndex = Convert.ToInt32(ReadIniSetting("InterfaceSize"))
                'fpsRate_cb.Text = ReadIniSetting("FrameKeep") & " FPS"
                If ReadIniSetting("FrameKeep") = "30" Then fpsRate_cb.SelectedIndex = 0
                If ReadIniSetting("FrameKeep") = "60" Then fpsRate_cb.SelectedIndex = 1
                If ReadIniSetting("FrameKeep") = "90" Then fpsRate_cb.SelectedIndex = 2
                If ReadIniSetting("FrameKeep") = "120" Then fpsRate_cb.SelectedIndex = 3
                If ReadIniSetting("FrameKeep") = "0" Then fpsRate_cb.SelectedIndex = 4
                'If fpsRate_cb.Text = "0 FPS" Then fpsRate_cb.Text = "Unlimited FPS"
                If ReadIniSetting("ShaderQuality") = "True" Then shaderStandard_rb.Checked = True
                If ReadIniSetting("ShaderQuality") = "False" Then sharderSimple_rb.Checked = True
                If ReadIniSetting("MoviePlay") = "True" Then enableVideos_rb.Checked = True
                If ReadIniSetting("MoviePlay") = "False" Then disableVideos_rb.Checked = True
                If ReadIniSetting("FullScreen") = "False" Then
                    windowed_rb.Checked = True
                End If
                If ReadIniSetting("FullScreen") = "True" Then
                    fullScreen_rb.Checked = True
                End If
                If ReadIniSetting("Fullscreen") = "False" And ReadIniSetting("VirtualFullScreen") = "True" Then
                    virtualFullscreen_rb.Checked = True
                    screenResolution_cb.Enabled = False
                    'Disable resolution thingie
                End If
                'ComboBoxEx5.Text = ReadINISetting("Width", 240) & "x" & ReadINISetting("Height", 240)
                If Not screenResolution_cb.Items.Contains(screenResolution_cb.Text) Then screenResolution_cb.SelectedIndex = 0
                CheckBoxX1.Checked = False
                If ReadIniSetting("Y") = "99999" Then
                    If ReadIniSetting("X") = "99999" Then
                        CheckBoxX1.Checked = True
                    End If
                End If
            Finally
                ResumeLayout(False)

            End Try


            'load our settings when we start our launcher.
            ' simpleSettigns_tb.Value = Settings.SimpleDraw
            language_cb.SelectedIndex = Settings.Language
            ' enableVideos_rb.Checked = Settings.vidPlaybackYes
            ' disableVideos_rb.Checked = Settings.vidPlaybackNo
            ' windowed_rb.Checked = Settings.Window
            ' fullScreen_rb.Checked = Settings.Fullscreen
            ' virtualFullscreen_rb.Checked = Settings.virtualFullscreen
            ' screenResolution_cb.SelectedIndex = Settings.ScreenSize
            ' fpsRate_cb.SelectedIndex = Settings.maxFPS
            ' charSize_cb.SelectedIndex = Settings.charSize
            ' highResTextures_rb.Checked = Settings.textResolutionHigh
            ' standardRes_rb.Checked = Settings.standardTextRes
            ' compactRes_rb.Checked = Settings.compactTextRes
            ' shaderStandard_rb.Checked = Settings.ShadersHigh
            ' sharderSimple_rb.Checked = Settings.ShadersLow
            ' bgmVol_trkb.Value = Settings.bgmVolume
            ' seVol_tb.Value = Settings.seVolume
            ' voiceVol_tb.Value = Settings.voiceVolume
            ' vidVol_tb.Value = Settings.videoPlaybackVol
            ' surroundON_rb.Checked = Settings.surroundEnabled
            ' surroundOFF_rb.Checked = Settings.surroundDisabled
            ' ListViewEx1.FocusedItem.Text = "Title Translation" = Settings.titleTranslator
            ' ListViewEx1.FocusedItem.Text = "Item Translation" = Settings.itemTranslator
            ' ListViewEx1.FocusedItem.Text = "Damage Parser" = Settings.dmgParser
            ' ListViewEx1.FocusedItem.Text = "PSO2Proxy Plugin" = Settings.pso2ProxyPlugin

            ' Update our labels that are used for measurments.
            bgmVol_lbl.Text = bgmVol_trkb.Value.ToString()
            seVol_lbl.Text = seVol_tb.Value.ToString()
            voiceVol_lbl.Text = voiceVol_tb.Value.ToString()
            vidVol_lbl.Text = vidVol_tb.Value.ToString()

            'set our dialog windows.
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub ResumePatching()

        If Not File.Exists("Resume.txt") Then
            'Helper.WriteDebugInfo(Resources.strCannotFindResume)
            Return
        End If

        _cancelledFull = False

        Try
            Dim missingfiles As New List(Of String)

            'Helper.WriteDebugInfoAndOk(Resources.strFoundIncompleteJob)
            If _cancelledFull Then Return
            For Each line In Helper.GetLines("Resume.txt")
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
            'CheckForStoryUpdates()
        Catch ex As Exception
            MsgBox("ERROR - " & ex.Message.ToString)
        End Try
    End Sub

    Public Sub CheckForPluginUpdates()
        DownloadFile("http://107.170.16.100/Plugins/PluginMD5HashList.txt", "PluginMD5HashList.txt")
        Using oReader As StreamReader = File.OpenText("PluginMD5HashList.txt")
            Dim strNewDate As String = oReader.ReadLine()
            RegKey.SetValue(Of String)(RegKey.NewPluginVersionTemp, strNewDate)
            RegKey.SetValue(Of String)(RegKey.NewPluginVersion, strNewDate)

            If strNewDate <> RegKey.GetValue(Of String)(RegKey.PluginVersion) Or (Directory.GetFiles(Program.Pso2RootDir & "\plugins\").Count = 0 And Directory.GetFiles(Program.Pso2RootDir & "\plugins\disabled").Count = 0) Or File.Exists(Program.Pso2RootDir & "\pso2h.dll") = False Or File.Exists(Program.Pso2RootDir & "\translation_titles.bin") = False Or File.Exists(Program.Pso2RootDir & "\translation.bin") = False Then
                'Update plugins [AIDA]

                Dim missingfiles As New List(Of String)
                Dim numberofChecks As Integer = 0
                Dim truefilename As String
                Dim filename As String()
                Dim FinalExportString As String = ""
                Helper.WriteDebugInfo("Beginning plugin update...")
                'Move all plugins to the disabled folder instead. [AIDA]

                RegKey.SetValue(Of String)(RegKey.PluginsEnabled, FinalExportString)
                For Each fi As FileInfo In New DirectoryInfo(Program.Pso2RootDir & "\plugins\").GetFiles
                    File.Move(fi.FullName, Path.Combine(Program.Pso2RootDir & "\plugins\disabled", fi.Name))
                    FinalExportString += fi.Name & ","
                Next
                If FinalExportString.Length > 0 Then FinalExportString = FinalExportString.Remove(FinalExportString.Length - 1, 1)
                RegKey.SetValue(Of String)(RegKey.PluginsEnabled, FinalExportString)
                While Not (oReader.EndOfStream)
                    filename = oReader.ReadLine().Split(","c)
                    truefilename = filename(0)

                    If truefilename = "pso2h.dll" Or truefilename = "translation_titles.bin" Or truefilename = "translation.bin" Then
                        If Not File.Exists((Program.Pso2RootDir & "\" & truefilename)) Then
                            missingfiles.Add(truefilename)
                        ElseIf Helper.GetMd5((Program.Pso2RootDir & "\" & truefilename)) <> filename(1) Then
                            missingfiles.Add(truefilename)
                        End If
                    Else
                        If Not File.Exists((Program.Pso2RootDir & "\plugins\disabled\" & truefilename)) Then
                            missingfiles.Add(truefilename)
                        ElseIf Helper.GetMd5((Program.Pso2RootDir & "\plugins\disabled\" & truefilename)) <> filename(1) Then
                            missingfiles.Add(truefilename)
                        End If
                    End If
                    numberofChecks += 1
                    'lblStatus.Text = (Resources.strCurrentlyCheckingFile & numberofChecks & "")
                    Application.DoEvents()
                End While

                Helper.WriteDebugInfo("Downloading/Installing updates...")
                Dim totaldownload As Long = missingfiles.Count
                Dim downloaded As Long = 0

                For Each downloadStr As String In missingfiles
                    'Download the missing files:
                    downloaded += 1
                    'lblStatus.Text = Resources.strUpdating & downloaded & "/" & totaldownload
                    Application.DoEvents()
                    _cancelled = False
                    DownloadFile(("http://107.170.16.100/Plugins/" & downloadStr), downloadStr)
                    If _cancelled Then Return
                    'Delete the existing file FIRST
                    If Not File.Exists(downloadStr) Then
                        Helper.WriteDebugInfoAndFailed("File " & downloadStr & " does not exist! Perhaps it wasn't downloaded properly?")
                    End If
                    If downloadStr = "pso2h.dll" Or downloadStr = "translation_titles.bin" Or downloadStr = "translation.bin" Then
                        If Environment.CurrentDirectory <> Program.Pso2RootDir Then
                            Helper.DeleteFile((Program.Pso2RootDir & "\" & downloadStr))
                            File.Move(downloadStr, (Program.Pso2RootDir & "\" & downloadStr))
                        End If
                    Else
                        If downloadStr = "PSO2Proxy.dll" Then
                            'If Not Dns.GetHostEntry("gs001.pso2gs.net").AddressList(0).ToString().Contains("210.189.") Then
                            If Not Dns.GetHostEntry("gs001.pso2gs.net").AddressList(0).ToString().Contains("210.189.") Then
                                Helper.WriteDebugInfo("PSO2Proxy usage detected! Auto-enabling PSO2Proxy plugin...")
                                File.Move(downloadStr, (Program.Pso2RootDir & "\plugins\" & downloadStr))
                            Else
                                Helper.DeleteFile((Program.Pso2RootDir & "\plugins\disabled\" & downloadStr))
                                File.Move(downloadStr, (Program.Pso2RootDir & "\plugins\disabled\" & downloadStr))
                            End If
                        Else
                            Helper.DeleteFile((Program.Pso2RootDir & "\plugins\disabled\" & downloadStr))
                            File.Move(downloadStr, (Program.Pso2RootDir & "\plugins\disabled\" & downloadStr))
                        End If
                    End If

                    If File.Exists(downloadStr) = True And Environment.CurrentDirectory <> Program.Pso2RootDir Then Helper.DeleteFile(downloadStr)
                    Application.DoEvents()
                Next
                'Restore the plugins to their proper folders now
                'If there's enabled plugins, do stuff.
                Dim FileToMove As String = ""
                If RegKey.GetValue(Of String)(RegKey.PluginsEnabled).ToString.Length > 1 Then
                    'Check to see if it's more than one file by seeing if there are any commas
                    If RegKey.GetValue(Of String)(RegKey.PluginsEnabled).Contains(",") = False Then
                        'It's just one file
                        'MsgBox("One plugin enabled - " & RegKey.GetValue(Of String)(RegKey.PluginsEnabled).ToString)
                        FileToMove = RegKey.GetValue(Of String)(RegKey.PluginsEnabled).ToString
                        If File.Exists(Program.Pso2RootDir & "\plugins\" & FileToMove) = True Then Helper.DeleteFile((Program.Pso2RootDir & "\plugins\" & FileToMove))
                        If File.Exists(Program.Pso2RootDir & "\plugins\disabled\" & FileToMove) Then File.Move((Program.Pso2RootDir & "\plugins\disabled\" & FileToMove), (Program.Pso2RootDir & "\plugins\" & FileToMove))
                    Else
                        'It's multiple files
                        Dim EnabledPlugins() As String = RegKey.GetValue(Of String)(RegKey.PluginsEnabled).Split(CType(",", Char()))
                        For Each EnabledFilename In EnabledPlugins
                            'MsgBox(EnabledFilename)
                            If File.Exists(Program.Pso2RootDir & "\plugins\" & EnabledFilename) = True Then Helper.DeleteFile((Program.Pso2RootDir & "\plugins\" & EnabledFilename))
                            If File.Exists(Program.Pso2RootDir & "\plugins\disabled\" & EnabledFilename) Then File.Move((Program.Pso2RootDir & "\plugins\disabled\" & EnabledFilename), (Program.Pso2RootDir & "\plugins\" & EnabledFilename))
                        Next
                    End If
                Else
                    'MsgBox("No plugins enabled!")
                End If

                Helper.WriteDebugInfoAndOk("Plugins updated successfully.")
                RegKey.SetValue(Of String)(RegKey.PluginVersion, RegKey.GetValue(Of String)(RegKey.NewPluginVersionTemp))
                RegKey.SetValue(Of String)(RegKey.NewPluginVersionTemp, "")
            Else
                Helper.WriteDebugInfoAndOk("No plugin updates available.")
            End If
        End Using
        If File.Exists("PluginMD5HashList.txt") = True Then Helper.DeleteFile("PluginMD5HashList.txt")
    End Sub

    Private Sub bgmVol_trkb_Scroll(sender As Object, e As EventArgs)
        bgmVol_lbl.Text = bgmVol_trkb.Value.ToString()
    End Sub

    Private Sub seVol_tb_Scroll(sender As Object, e As EventArgs)
        seVol_lbl.Text = seVol_tb.Value.ToString()
    End Sub

    Private Sub voiceVol_tb_Scroll(sender As Object, e As EventArgs)
        voiceVol_lbl.Text = voiceVol_tb.Value.ToString()
    End Sub

    Private Sub vidVol_tb_Scroll(sender As Object, e As EventArgs)
        vidVol_lbl.Text = vidVol_tb.Value.ToString()
    End Sub





    Private Sub virtualFullscreen_rb_CheckedChanged(sender As Object, e As EventArgs)
        If virtualFullscreen_rb.Checked = True Then
            screenResolution_cb.Enabled = False
        Else
            screenResolution_cb.Enabled = True
        End If
    End Sub

    Private Function ReadIniSetting(settingToRead As String, Optional ByVal lineToStartAt As Integer = 0) As String
        Try
            'Dim returnValue = ""
            'If INICache.TryGetValue(SettingToRead, returnValue) Then Return returnValue

            Dim textLines As String() = File.ReadAllLines(_usersettingsfile)
            For i As Integer = lineToStartAt To (textLines.Length - 1)
                If Not String.IsNullOrEmpty(textLines(i)) Then
                    If textLines(i).Contains(" " & settingToRead & " ") Then
                        Dim strLine As String = textLines(i).Replace(vbTab, "")
                        Dim strReturn As String() = strLine.Split("="c)
                        Dim finalString As String = strReturn(1).Replace("""", "").Replace(","c, "").Replace(" "c, "")
                        'If FinalString IsNot Nothing Then INICache.Add(SettingToRead, FinalString)
                        Return finalString
                    End If
                End If
            Next
        Catch ex As Exception
            Helper.Log(ex.Message)
            Helper.WriteDebugInfo(Resources.strERROR & ex.Message)
            messageForm.message_txt.Text = ex.Message
            messageForm.ShowDialog()
        End Try
        Return ""
    End Function

    Private Sub ListViewEx1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListViewEx1.SelectedIndexChanged
        lblPluginInfo.Text = ""
        btnConfigure.Visible = False
        If ListViewEx1.FocusedItem.Text = "Title Translation" Then
            lblPluginInfo.Text =
                "Translates the titles in-game." & vbCrLf &
                "Plugin Author: Variant" & vbCrLf &
                "DLL Name: PSO2TitleTranslator.dll" & vbCrLf &
                "Support URL: N/A"
            Exit Sub
        End If
        If ListViewEx1.FocusedItem.Text = "Item Translation" Then
            lblPluginInfo.Text =
                "Translates the items in-game." & vbCrLf &
                "Plugin Author: arcnmx/Variant/Raven123" & vbCrLf &
                "DLL Name: translator.dll" & vbCrLf &
                "Support URL: N/A"
            btnConfigure.Visible = True
            Exit Sub
        End If
        If ListViewEx1.FocusedItem.Text = "Damage Parser" Then
            lblPluginInfo.Text =
                "DPS (Damage Per Second) Parser plugin. Exports damage logs into a damagelogs folder where pso2.exe is, in excel format." & vbCrLf &
                "Plugin Author: Variant" & vbCrLf &
                "DLL Name: PSO2DamageDump.dll" & vbCrLf &
                "Support URL: N/A"
            Exit Sub
        End If
        If ListViewEx1.FocusedItem.Text = "PSO2Proxy Plugin" Then
            lblPluginInfo.Text =
                "PSO2Proxy plugin. Allows people from SEA and other blocked regions to connect to PSO2JP." & vbCrLf &
                "Plugin Author: Alam_Squeeze" & vbCrLf &
                "DLL Name: PSO2Proxy.dll" & vbCrLf &
                "Support URL: http://pso2proxy.cyberkitsune.net"
            Exit Sub
        End If
    End Sub



    Private Sub quit_btn_Click(sender As Object, e As EventArgs) Handles quit_btn.Click
        quit_btn.BackgroundImage = Resources._207__Japanese__Japan__
        Close()
    End Sub




    Private Sub OnDownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        Dim totalSize As Long = e.TotalBytesToReceive
        _totalsize2 = totalSize
        Dim downloadedBytes As Long = e.BytesReceived
        Dim percentage As Integer = e.ProgressPercentage
        'PBMainBar.Value = percentage
        'PBMainBar.Text = (Resources.strDownloaded & Helper.SizeSuffix(downloadedBytes) & " / " & Helper.SizeSuffix(totalSize) & " (" & percentage & "%) - " & Resources.strRightClickforOptions)
        'Put your progress UI here, you can cancel download by uncommenting the line below
    End Sub

    Private Sub OnFileDownloadCompleted(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
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
                'Close()
            End If
        End While
    End Sub

    ' Private Shared Function IsPso2WinDirMissing() As Boolean
    '     If Program.Pso2RootDir = "lblDirectory" OrElse Not Directory.Exists(Program.Pso2WinDir) Then
    '         MsgBox("Locate your pso2 win32 folder.")
    '         Helper.SelectPso2Directory()
    '         Return True
    '     End If
    '     Return False
    ' End Function

    Private Sub Play_btn_Click(sender As Object, e As EventArgs) Handles Play_btn.Click
        'Play_btn.BackgroundImage = Resources._170
        If Helper.CheckIfRunning("pso2") Then Return
        Try
            'If IsPso2WinDirMissing() Then Return
            Hide()

            If Not File.Exists(Program.Pso2RootDir & "\pso2.exe") Then

                messageForm.locatePSO2()
                messageForm.ShowDialog()

                Return
            End If

            ' DLS.CancelAsync()
            ' _cancelled = True



            If Not Program.transOverride Then Helper.DeleteFile(Program.Pso2RootDir & "\ddraw.dll")
            If Not Program.transOverride Then File.WriteAllBytes(Program.Pso2RootDir & "\ddraw.dll", Resources.ddraw)
            Dim startInfo As ProcessStartInfo = New ProcessStartInfo With {.FileName = (Program.Pso2RootDir & "\pso2.exe"), .Arguments = "+0x33aca2b9", .UseShellExecute = False}
            startInfo.EnvironmentVariables("-pso2") = "+0x01e3f1e9"
            Dim shell As Process = New Process With {.StartInfo = startInfo}

            shell.Start()


            If Program.GNFieldActive = True Then
                Process.Start("GN Field.exe")
                Thread.Sleep(100)
                End
            End If

            'This code is no longer run because Gameguard sucks cock.
            'Maybe SEGA doesn't? WHO KNOWS. IT'S BACK IN.
            Try
                shell.Start()
            Catch ex As Exception
                messageForm.message_txt.Text = ex.Message.ToString & " Making second attempt."
                messageForm.ShowDialog()
                DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2.exe.pat", "pso2.exe")
                If File.Exists((Program.Pso2RootDir & "\pso2.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((Program.Pso2RootDir & "\pso2.exe"))
                File.Move("pso2.exe", (Program.Pso2RootDir & "\pso2.exe"))
                Helper.WriteDebugInfoSameLine("Complete.")
                shell.Start()
            End Try


            Dim hWnd As IntPtr = External.FindWindow("Phantasy Star Online 2", Nothing)

            SkipDialogs = False
            'tmrWaitingforPSO2.Enabled = True
            tmrWaitingforPSO2.Start()

            Do While hWnd = IntPtr.Zero
                hWnd = External.FindWindow("Phantasy Star Online 2", Nothing)
                Thread.Sleep(10)
                Application.DoEvents()
                If SkipDialogs = True Then Exit Do
            Loop

            If Not Program.transOverride Then Helper.DeleteFile(Program.Pso2RootDir & "\ddraw.dll")
            tmrWaitingforPSO2.Stop()
            tmrWaitingforPSO2.Enabled = False

            If RegKey.GetValue(Of String)(RegKey.SteamMode) = "True" Then
                File.Copy(Program.Pso2RootDir & "\pso2.exe", Program.Pso2RootDir & "\pso2.exe_backup", True)
                Do While Helper.IsFileInUse(Program.Pso2RootDir & "\pso2.exe")
                    Thread.Sleep(1000)
                Loop
                File.Copy(Program.Pso2RootDir & "\pso2.exe_backup", Program.Pso2RootDir & "\pso2.exe", True)
                File.Delete(Program.Pso2RootDir & "\pso2.exe_backup")
            End If


            'Dim p() As Process
            'p = Process.GetProcessesByName("nProtect GameGuard Error Report Rev 158")

            Dim pList() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessesByName("ggerror.des")
            For Each proc As System.Diagnostics.Process In pList
                ' Dim resp As MsgBoxResult
                ' resp = MsgBox("Terminate " & proc.ProcessName & "?", MsgBoxStyle.YesNo, "Terminate?")
                ' If resp = MsgBoxResult.Yes Then
                '
                ' End If
                proc.Kill()
            Next



            Dim game() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessesByName("pso2")
            For Each proc2 As System.Diagnostics.Process In game
                ' Dim resp As MsgBoxResult
                ' resp = MsgBox("Terminate " & proc.ProcessName & "?", MsgBoxStyle.YesNo, "Terminate?")
                ' If resp = MsgBoxResult.Yes Then
                '
                ' End If
                'proc2
                Close()
            Next
            Close()


        Catch ex As Exception
            Helper.Log(ex.Message.ToString & " Stack Trace: " & ex.StackTrace)
            messageForm.message_txt.Text = "There was a problem. " & ex.Message.ToString & " Stack Trace: " & ex.StackTrace
            messageForm.ShowDialog()
            Show()
        End Try
    End Sub


    Private Sub Play_btn_MouseEnter(sender As Object, e As EventArgs) Handles Play_btn.MouseEnter
        Play_btn.BackgroundImage = Resources._171ENPLAY1
    End Sub

    Private Sub Play_btn_MouseLeave(sender As Object, e As EventArgs) Handles Play_btn.MouseLeave
        Play_btn.BackgroundImage = Resources._172EN
    End Sub




    Private Sub tmrWaitingforPSO2_Tick(sender As Object, e As EventArgs) Handles tmrWaitingforPSO2.Tick
        tmrWaitingforPSO2.Stop()
        tmrWaitingforPSO2.Enabled = False

        Dim YesNo As MsgBoxResult = MsgBox("Seems like PSO2 hasn't started yet. This could be caused by a Gameguard Issue. Would you like pso2launcher to try to solve the issue and relaunch the game?", vbYesNo)
        If YesNo = MsgBoxResult.No Then
            tmrWaitingforPSO2.Stop()
            tmrWaitingforPSO2.Enabled = False
            Exit Sub
        End If
        If YesNo = MsgBoxResult.Yes Then
            SkipDialogs = True
            'btnGameguard.RaiseClick()
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

    Private Sub btnLargeFilesTRANSAM_Click(sender As Object, e As EventArgs) Handles btnLargeFilesTRANSAM.Click
        'Install Large Files with TRANSAM to cut down on net costs for Agrajag and friends.
        'Need to speak with Agrajag and get some files before I can do this, though.
        Try
            If IsPso2WinDirMissing() Then Return

            ' Create a match using regular exp<b></b>ressions
            ' Spit out the value plucked from the code
            Dim LFDate As String = Program.Client.DownloadString("http://108.61.203.33/freedom/patches/largefilesTRANSAM.txt") 'The freedomURL path to download the Large files.

            Helper.WriteDebugInfoAndOk("Downloading Large Files info... ")
            DownloadFile("http://108.61.203.33/freedom/patches/lf.stripped.db.7z", "lf.stripped.db.7z")
            Dim processStartInfo2 As New ProcessStartInfo With
            {
                .FileName = (Program.StartPath & "\7za.exe"),
                .Verb = "runas",
                .Arguments = ("e -y lf.stripped.db.7z"),
                .WindowStyle = ProcessWindowStyle.Hidden,
            .UseShellExecute = True
            }
            Process.Start(processStartInfo2).WaitForExit()
            Dim DBMD5 As String = Helper.GetMd5("lf.stripped.db")
            Helper.WriteDebugInfoAndOk("Downloading Trans-Am tool... ")
            DownloadFile("http://108.61.203.33/freedom/pso2-transam.exe", "pso2-transam.exe")

            'execute pso2-transam stuff with -b flag for backup
            Dim processStartInfo As ProcessStartInfo = New ProcessStartInfo() With {.FileName = "pso2-transam.exe", .Verb = "runas"}
            'If Directory.Exists(backupdir) Then
            'Dim counter = Computer.FileSystem.GetFiles(backupdir)
            'If counter.Count > 0 Then
            processStartInfo.Arguments = ("-t largefiles-" & LFDate & " lf.stripped.db " & """" & Program.Pso2WinDir & """")
            Clipboard.SetText(processStartInfo.Arguments)
            'Else
            'Helper.Log("[TRANSAM] Creating backup directory")
            'Directory.CreateDirectory(backupdir)
            'Helper.WriteDebugInfo(Resources.strCreatingBackupDirectory)
            'processStartInfo.Arguments = ("-b " & """" & backupdir & """" & " -t story-eng-" & strStoryPatchLatestBase & " pso2.stripped.db " & """" & Program.Pso2WinDir & """")
            'End If
            'End If

            'We don't need to make backups anymore
            'If Not Directory.Exists(backupdir) Then
            ' Helper.Log("[TRANSAM] Creating backup directory")
            ' Directory.CreateDirectory(backupdir)
            ' Helper.WriteDebugInfo(Resources.strCreatingBackupDirectory)
            ' processStartInfo.Arguments = ("-b " & """" & backupdir & """" & " -t story-eng-" & strStoryPatchLatestBase & " pso2.stripped.db " & """" & Program.Pso2WinDir & """")
            ' End If

            processStartInfo.UseShellExecute = False
            Helper.Log("[TRANSAM] Starting shitstorm")
            processStartInfo.Arguments = processStartInfo.Arguments.Replace("\", "/")
            Helper.Log("TRANSM parameters: " & processStartInfo.Arguments & vbCrLf & "TRANSAM Working Directory: " & processStartInfo.WorkingDirectory)
            Helper.Log("[TRANSAM] Program started")
            If Helper.GetMd5("lf.stripped.db") <> DBMD5 Then
                MsgBox("ERROR: Files have been modified since download. Aborting...")
                Exit Sub
            End If
            Process.Start(processStartInfo).WaitForExit()
            Helper.DeleteFile("lf.stripped.db")
            Helper.DeleteFile("pso2-transam.exe")
            External.FlashWindow(Handle, True)
            'Story Patch 3-12-2014.rar
            RegKey.SetValue(Of String)(RegKey.LargeFilesVersion, LFDate.Replace("-", "/"))
            Helper.WriteDebugInfo("Large Files Patch installed!")
        Catch ex As Exception
            MsgBox("ERROR - " & ex.Message.ToString)
        End Try
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
        Dim url As String = Program.Client.DownloadString("http://108.61.203.33/freedom/patches/largefiles.txt") 'This is the original FreedomURL string
        DownloadPatch(url, LargeFiles, "LargeFiles.rar", RegKey.LargeFilesVersion, Resources.strWouldYouLikeToBackupLargeFiles, Resources.strWouldYouLikeToUse)
        'Else
        'Helper.WriteDebugInfo("Download was cancelled due To incompatibility.")
        'End If
    End Sub

    Private Sub btnNewShit_Click(sender As Object, e As EventArgs)
        Helper.WriteDebugInfo("Starting TRANS-AM BURST system...")
        'All done!

        'arcnmx: It first creates a new empty patchlist. It loops through each item in
        'p (the new one freshly downloaded).

        'arcnmx: If, searching by filename, it can't find an entry in po (the old currently-installed
        'patchlist) or it finds an entry but the filesize/md5 is different, it puts the entry from p
        'in the new list.

        'arcnmx: Then the new list only contains the files that changed, and need to be downloaded.

        'Have two patchlists (the old merged SOME OF THE THINGS) (B) and the new patchlist downloaded
        'from SEGA (A), merged.

        'Read the first line from A, search until the end of B. If found, and the lines are different,
        'add it to missing files. If it's not there, add to missing files.

        'download all missingfiles

        'if file "currentpatchlist.txt" is not found then build list like SEGA's.

        _cancelledFull = False
        If IsPso2WinDirMissing() Then Return
        'lblStatus.Text = ""

        If Directory.Exists(BuildBackupPath(EnglishPatch)) Then
            'If File.Exists("win32list_DO_NOT_DELETE_ME.txt") Then File.Delete("win32list_DO_NOT_DELETE_ME.txt")
            ' Helper.WriteDebugInfo(Resources.strENBackupFound)
            RestoreBackup(EnglishPatch)
        End If

        If Directory.Exists(BuildBackupPath(LargeFiles)) Then
            'If File.Exists("win32list_DO_NOT_DELETE_ME.txt") Then File.Delete("win32list_DO_NOT_DELETE_ME.txt")
            'Helper.WriteDebugInfo(Resources.strLFBackupFound)
            RestoreBackup(LargeFiles)
        End If

        If Directory.Exists(BuildBackupPath(StoryPatch)) Then
            'If File.Exists("win32list_DO_NOT_DELETE_ME.txt") Then File.Delete("win32list_DO_NOT_DELETE_ME.txt")
            ' Helper.WriteDebugInfo(Resources.strStoryBackupFound)
            RestoreBackup(StoryPatch)
        End If

        Dim totalfiles = Directory.GetFiles(Program.Pso2RootDir & "\data\win32\")

        If File.Exists("win32list_DO_NOT_DELETE_ME.txt_temp") Then File.Delete("win32list_DO_NOT_DELETE_ME.txt_temp")

        If Not File.Exists("win32list_DO_NOT_DELETE_ME.txt") Then
            Helper.WriteDebugInfo("Building file list... ")
            Helper.WriteDebugInfo("This will only be done once, as long as you don't delete the 'win32list_DO_NOT_DELETE_ME.txt' file")
            Dim di As New DirectoryInfo(Program.Pso2RootDir & "\data\win32\")

            Using writer As New StreamWriter("win32list_DO_NOT_DELETE_ME.txt_temp", True)
                Dim count As Integer = 0
                For Each dra In di.GetFiles()
                    If _cancelledFull Then Return
                    writer.WriteLine("data/win32/" & dra.Name & ".pat" & vbTab & dra.Length & vbTab & Helper.GetMd5(Program.Pso2RootDir & "\data\win32\" & dra.Name))
                    count += 1
                    'lblStatus.Text = "Building first time list of win32 files (" & count & "/" & totalfiles.Length & ")"
                    Application.DoEvents()
                Next
            End Using

            FileSystem.RenameFile("win32list_DO_NOT_DELETE_ME.txt_temp", "win32list_DO_NOT_DELETE_ME.txt")

            Helper.WriteDebugInfoSameLine("Done!")
        End If

        'LockGui()
        ' Helper.WriteDebugInfo(Resources.strDownloadingPatchFile1)
        Application.DoEvents()
        DownloadFile("http://download.pso2.jp/patch_prod/patches/launcherlist.txt", "launcherlist.txt")
        '  Helper.WriteDebugInfoSameLine(Resources.strDone)
        ' Helper.WriteDebugInfo(Resources.strDownloadingPatchFile2)
        Application.DoEvents()
        DownloadFile("http://download.pso2.jp/patch_prod/patches/patchlist.txt", "patchlist.txt")
        ' Helper.WriteDebugInfoSameLine(Resources.strDone)
        ' Helper.WriteDebugInfo(Resources.strDownloadingPatchFile3)
        Application.DoEvents()
        DownloadFile("http://download.pso2.jp/patch_prod/patches_old/patchlist.txt", "patchlist_old.txt")
        ' Helper.WriteDebugInfoSameLine(Resources.strDone)
        ' Helper.WriteDebugInfo(Resources.strDownloadingPatchFile4)
        Application.DoEvents()
        Program.Client.DownloadFile("http://arks-layer.com/vanila/version.txt", "version.ver")
        ' Helper.WriteDebugInfoSameLine(Resources.strDone)
        Application.DoEvents()
        'UnlockGui()
        Dim mergedPatches = MergePatches()

        'Rewrite this to support the new format

        Dim segaLine As String
        Dim segaFilename As String
        Dim missingfiles As New List(Of String)
        Dim oldarray = File.ReadAllLines("win32list_DO_NOT_DELETE_ME.txt")

        For i As Integer = 0 To mergedPatches.Count

            If _cancelledFull Then Return
            segaLine = mergedPatches.Values(i)
            If String.IsNullOrEmpty(segaLine) Then Continue For

            segaFilename = segaLine.Remove(segaLine.IndexOf(".pat", StringComparison.Ordinal)).Replace("data/win32/", "")
            ' lblStatus.Text = "Checking file " & i + 1 & " / " & mergedPatches.Count
            If missingfiles.Count > 0 Then lblStatus.Text &= " (missing files found: " & missingfiles.Count & ")"
            Application.DoEvents()
            If Not oldarray.Contains(segaLine) And segaLine.Contains("user_default.pso2") = False And segaLine.Contains("edition.txt") = False And segaLine.Contains("GameGuard.des") = False And segaLine.Contains("gameversion.ver") = False And segaLine.Contains("pso2.exe") = False And segaLine.Contains("PSO2JP.ini") = False And segaLine.Contains("script/user_intel.pso2") = False And segaLine.Contains("ffbff2ac5b7a7948961212cefd4d402c") = False Then
                missingfiles.Add(segaFilename)
            End If
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
            'lblStatus.Text = Resources.strDownloading & "" & downloaded & "/" & totaldownload & " (" & Helper.SizeSuffix(totaldownloadedthings) & ")"

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
            ' lblStatus.Text = Resources.strDownloading & "" & downloaded & "/" & totaldownload & " (" & Helper.SizeSuffix(totaldownloadedthings) & ")"

            Application.DoEvents()
            Application.DoEvents()
        Next
        If missingfiles.Count = 0 Then Helper.WriteDebugInfo("You appear up to date.")
        Dim directoryStringthing As String = (Program.Pso2RootDir & "\")

        'Helper.WriteDebugInfo(Resources.strDownloading & "version file...")
        Application.DoEvents()
        _cancelled = False
        Program.Client.DownloadFile("http://arks-layer.com/vanila/version.txt", "version.ver")
        If _cancelled Then Return
        If File.Exists((_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver")) Then Helper.DeleteFile((_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver"))
        File.Copy("version.ver", (_myDocuments & "\SEGA\PHANTASYSTARONLINE2\version.ver"))
        ' Helper.WriteDebugInfoAndOk((Resources.strDownloadedandInstalled & "version file"))

        'Helper.WriteDebugInfo(Resources.strDownloading & "pso2launcher.exe...")
        Application.DoEvents()
        For Each proc As Process In Process.GetProcessesByName("pso2launcher")
            If proc.MainWindowTitle = "PHANTASY STAR ONLINE 2" AndAlso proc.MainModule.ToString() = "ProcessModule (pso2launcher.exe)" Then proc.Kill()
        Next

        DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2launcher.exe.pat", "pso2launcher.exe")
        If _cancelled Then Return
        If File.Exists((directoryStringthing & "pso2launcher.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((directoryStringthing & "pso2launcher.exe"))
        File.Move("pso2launcher.exe", (directoryStringthing & "pso2launcher.exe"))
        ' Helper.WriteDebugInfoAndOk((Resources.strDownloadedandInstalled & "pso2launcher.exe"))
        'Helper.WriteDebugInfo(Resources.strDownloading & "pso2updater.exe...")
        Application.DoEvents()
        For Each proc As Process In Process.GetProcessesByName("pso2updater")
            If proc.MainModule.ToString() = "ProcessModule (pso2updater.exe)" Then proc.Kill()
        Next

        DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2updater.exe.pat", "pso2updater.exe")
        If _cancelled Then Return
        If File.Exists((directoryStringthing & "pso2updater.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((directoryStringthing & "pso2updater.exe"))
        File.Move("pso2updater.exe", (directoryStringthing & "pso2updater.exe"))
        'Helper.WriteDebugInfoAndOk((Resources.strDownloadedandInstalled & "pso2updater.exe"))
        Application.DoEvents()

        'Helper.WriteDebugInfo(Resources.strDownloading & "pso2.exe...")
        For Each proc As Process In Process.GetProcessesByName("pso2")
            If proc.MainModule.ToString() = "ProcessModule (pso2.exe)" Then proc.Kill()
        Next

        DownloadFile("http://download.pso2.jp/patch_prod/patches/pso2.exe.pat", "pso2.exe")
        If _cancelled Then Return

        If File.Exists((directoryStringthing & "pso2.exe")) AndAlso Program.StartPath <> Program.Pso2RootDir Then Helper.DeleteFile((directoryStringthing & "pso2.exe"))
        File.Move("pso2.exe", (directoryStringthing & "pso2.exe"))
        If _cancelledFull Then Return

        ' Helper.WriteDebugInfoAndOk((Resources.strDownloadedandInstalled & "pso2.exe"))


        If RegKey.GetValue(Of String)(RegKey.StoryPatchVersion) <> "Not Installed" Then RegKey.SetValue(Of String)(RegKey.StoryPatchVersion, "Not Updated")
        If RegKey.GetValue(Of String)(RegKey.EnPatchVersion) <> "Not Installed" Then RegKey.SetValue(Of String)(RegKey.EnPatchVersion, "Not Updated")
        If RegKey.GetValue(Of String)(RegKey.LargeFilesVersion) <> "Not Installed" Then RegKey.SetValue(Of String)(RegKey.LargeFilesVersion, "Not Updated")

        RegKey.SetValue(Of String)(RegKey.Pso2PatchlistMd5, Helper.GetMd5("patchlist.txt"))
        Helper.WriteDebugInfo("Game updated to the latest version. Don't forget to re-install/update the patches, as some of the files might have been untranslated.")
        Helper.DeleteFile("resume.txt")
        RegKey.SetValue(Of String)(RegKey.Pso2RemoteVersion, File.ReadAllLines("version.ver")(0))
        ' UnlockGui()

        If Convert.ToBoolean(RegKey.GetValue(Of String)(RegKey.RemoveCensor)) And File.Exists((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c")) Then
            If File.Exists((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c.backup")) Then Computer.FileSystem.DeleteFile((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c.backup"), UIOption.OnlyErrorDialogs, RecycleOption.DeletePermanently)
            Computer.FileSystem.RenameFile((Program.Pso2WinDir & "\" & "ffbff2ac5b7a7948961212cefd4d402c"), "ffbff2ac5b7a7948961212cefd4d402c.backup")
            'Helper.WriteDebugInfoAndOk(Resources.strRemoving & "Censor...")
        End If

        Helper.WriteDebugInfo("Updating win32 list...")

        'Write new win32 here
        'Take in the old file, search for a file that was downloaded.
        'Replace that line with the new filesize or make a new entry if the file wasn't found in the old one.
        'SHOULD be much faster than writing a new win32 file
        If File.Exists("new_win32.txt") Then File.Delete("new_win32.txt")
        If File.Exists("missingfiles.txt") Then File.Delete("missingfiles.txt")
        Dim FILE_NAME As String = ""
        Dim Found As Boolean = False
        System.IO.File.WriteAllText("new_win32.txt", System.IO.File.ReadAllText("win32list_DO_NOT_DELETE_ME.txt"))
        For i As Integer = 0 To missingfiles.Count - 1
            Found = False
            File.AppendAllText("missingfiles.txt", missingfiles(i) & vbNewLine)
            For Each match As Match In Regex.Matches(System.IO.File.ReadAllText("new_win32.txt"), "data/win32/" & missingfiles(i).ToString & ".*")
                'MsgBox(match.Value)
                Dim info As New FileInfo(Program.Pso2RootDir & "\data\win32\" & missingfiles(i).ToString)
                System.IO.File.WriteAllText("new_win32.txt", System.IO.File.ReadAllText("new_win32.txt").Replace(match.Value, "data/win32/" & missingfiles(i).ToString & ".pat" & vbTab & info.Length & vbTab & Helper.GetMd5(Program.Pso2RootDir & "\data\win32\" & missingfiles(i).ToString)))
                Found = True
            Next
            If Found = False Then
                Dim info As New FileInfo(Program.Pso2RootDir & "\data\win32\" & missingfiles(i).ToString)
                System.IO.File.WriteAllText("new_win32.txt", System.IO.File.ReadAllText("new_win32.txt") & vbNewLine & "data/win32/" & missingfiles(i).ToString & ".pat" & vbTab & info.Length & vbTab & Helper.GetMd5(Program.Pso2RootDir & "\data\win32\" & missingfiles(i).ToString))
            End If
        Next

        If File.Exists("win32list_DO_NOT_DELETE_ME.txt") Then File.Delete("win32list_DO_NOT_DELETE_ME.txt")
        FileSystem.RenameFile("new_win32.txt", "win32list_DO_NOT_DELETE_ME.txt")
        Helper.WriteDebugInfoSameLine(" Done!")
        'Helper.WriteDebugInfoAndOk(Resources.strallDone)

        'MsgBox(missingfiles.Count)
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

    Private Shared Function BuildBackupPath(ByVal patchName As String) As String
        Return Program.Pso2WinDir & "\backup\" & patchName & "\"
    End Function

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

    Private Sub quit_btn_MouseEnter(sender As Object, e As EventArgs) Handles quit_btn.MouseEnter
        quit_btn.BackgroundImage = Resources._208__Japanese__Japan__
    End Sub

    Private Sub quit_btn_MouseLeave(sender As Object, e As EventArgs) Handles quit_btn.MouseLeave
        quit_btn.BackgroundImage = Resources._209__Japanese__Japan__
    End Sub



    Private Sub settings_btn_Click(sender As Object, e As EventArgs) Handles settings_btn.Click
        settings_btn.BackgroundImage = Resources._164EN

        Main.Visible = False
        settingsPanel.Visible = True


    End Sub

    Private Sub settings_btn_MouseEnter(sender As Object, e As EventArgs) Handles settings_btn.MouseEnter
        settings_btn.BackgroundImage = Resources._165EN
    End Sub
    Private Sub settings_btn_MouseLeave(sender As Object, e As EventArgs) Handles settings_btn.MouseLeave
        settings_btn.BackgroundImage = Resources._166EN
    End Sub

    Private Sub return_btn_Click(sender As Object, e As EventArgs) Handles return_btn.Click
        Simple_btn.BackgroundImage = Resources._158EN

        settingsPanel.Visible = False
        Main.Visible = True

        'RESET OUR OTHER BUTTONS!!!
        Simple_btn.Enabled = False
        language_btn.Enabled = True
        screenSettings_btn.Enabled = True
        textures_btn.Enabled = True
        sound_btn.Enabled = True
        network_btn.Enabled = True

        'RESET OUR OTHER PANELS!!!
        simpleSettings_pnl.Visible = True
        language_pnl.Visible = False
        screen_pnl.Visible = False
        textures_pnl.Visible = False
        sound_pnl.Visible = False
        network_pnl.Visible = False

        'load our settings when we start our launcher.
        simpleSettigns_tb.Value = Settings.SimpleDraw
        language_cb.SelectedIndex = Settings.Language
        enableVideos_rb.Checked = Settings.vidPlaybackYes
        disableVideos_rb.Checked = Settings.vidPlaybackNo
        windowed_rb.Checked = Settings.Window
        fullScreen_rb.Checked = Settings.Fullscreen
        virtualFullscreen_rb.Checked = Settings.virtualFullscreen
        screenResolution_cb.SelectedIndex = Settings.ScreenSize
        fpsRate_cb.SelectedIndex = Settings.maxFPS
        charSize_cb.SelectedIndex = Settings.charSize
        highResTextures_rb.Checked = Settings.textResolutionHigh
        standardRes_rb.Checked = Settings.standardTextRes
        compactRes_rb.Checked = Settings.compactTextRes
        shaderStandard_rb.Checked = Settings.ShadersHigh
        sharderSimple_rb.Checked = Settings.ShadersLow
        bgmVol_trkb.Value = Settings.bgmVolume
        seVol_tb.Value = Settings.seVolume
        voiceVol_tb.Value = Settings.voiceVolume
        vidVol_tb.Value = Settings.videoPlaybackVol
        surroundON_rb.Checked = Settings.surroundEnabled
        surroundOFF_rb.Checked = Settings.surroundDisabled
        ' ListViewEx1.FocusedItem.Text = "Title Translation" = Settings.titleTranslator
        ' ListViewEx1.FocusedItem.Text = "Item Translation" = Settings.itemTranslator
        ' ListViewEx1.FocusedItem.Text = "Damage Parser" = Settings.dmgParser
        ' ListViewEx1.FocusedItem.Text = "PSO2Proxy Plugin" = Settings.pso2ProxyPlugin

        ' Update our labels that are used for measurments.
        bgmVol_lbl.Text = bgmVol_trkb.Value.ToString()
        seVol_lbl.Text = seVol_tb.Value.ToString()
        voiceVol_lbl.Text = voiceVol_tb.Value.ToString()
        vidVol_lbl.Text = vidVol_tb.Value.ToString()

        'set our dialog windows.

    End Sub

    Private Sub return_btn_MouseEnter(sender As Object, e As EventArgs) Handles return_btn.MouseEnter
        return_btn.BackgroundImage = Resources._135EN
    End Sub




    ' Our settings panels with each tab and functionality................................

    Private Sub Simple_btn_Click(sender As Object, e As EventArgs) Handles Simple_btn.Click
        ''Simple Button function once clicked.
        Simple_btn.BackgroundImage = Resources._158EN


        ''ESET OUR OTHER BUTTONS!!!
        Simple_btn.Enabled = False
        language_btn.Enabled = True
        screenSettings_btn.Enabled = True
        textures_btn.Enabled = True
        sound_btn.Enabled = True
        network_btn.Enabled = True

        ''RESET OUR OTHER PANELS!!!
        simpleSettings_pnl.Visible = True
        language_pnl.Visible = False
        screen_pnl.Visible = False
        textures_pnl.Visible = False
        sound_pnl.Visible = False
        network_pnl.Visible = False


        ''RESET OUR OTHER BUTTONS!!!
        language_btn.BackgroundImage = Resources._148EN
        screenSettings_btn.BackgroundImage = Resources._157EN
        textures_btn.BackgroundImage = Resources._151EN
        sound_btn.BackgroundImage = Resources._163EN
        network_btn.BackgroundImage = Resources._219EN
    End Sub

    Private Sub Simple_btn_MouseEnter(sender As Object, e As EventArgs) Handles Simple_btn.MouseEnter
        Simple_btn.BackgroundImage = Resources._159EN
    End Sub

    Private Sub Simple_btn_MouseLeave(sender As Object, e As EventArgs) Handles Simple_btn.MouseLeave
        If Simple_btn.Enabled = False Then

            Simple_btn.BackgroundImage = Resources._158EN

        Else

            Simple_btn.BackgroundImage = Resources._160EN
        End If
    End Sub

    'Language
    Private Sub language_btn_Click(sender As Object, e As EventArgs) Handles language_btn.Click
        'Simple Button function once clicked.

        language_btn.BackgroundImage = Resources._146EN

        'RESET OUR OTHER BUTTONS!!!
        Simple_btn.Enabled = True
        language_btn.Enabled = False
        screenSettings_btn.Enabled = True
        textures_btn.Enabled = True
        sound_btn.Enabled = True
        network_btn.Enabled = True

        'RESET OUR OTHER PANELS!!!
        simpleSettings_pnl.Visible = False
        language_pnl.Visible = True
        screen_pnl.Visible = False
        textures_pnl.Visible = False
        sound_pnl.Visible = False
        network_pnl.Visible = False

        'RESET OUR OTHER BUTTONS!!!
        Simple_btn.BackgroundImage = Resources._160EN
        screenSettings_btn.BackgroundImage = Resources._157EN
        textures_btn.BackgroundImage = Resources._151EN
        sound_btn.BackgroundImage = Resources._163EN
        network_btn.BackgroundImage = Resources._219EN
    End Sub

    Private Sub language_btn_MouseEnter(sender As Object, e As EventArgs) Handles language_btn.MouseEnter
        language_btn.BackgroundImage = Resources._147EN
    End Sub

    Private Sub language_btn_MouseLeave(sender As Object, e As EventArgs) Handles language_btn.MouseLeave
        If language_btn.Enabled = False Then

            language_btn.BackgroundImage = Resources._146EN

        Else

            language_btn.BackgroundImage = Resources._148EN
        End If
    End Sub


    'Screen Settings
    Private Sub screenSettings_btn_Click(sender As Object, e As EventArgs) Handles screenSettings_btn.Click
        screenSettings_btn.BackgroundImage = Resources._155EN

        'RESET OUR OTHER BUTTONS!!!
        Simple_btn.Enabled = True
        language_btn.Enabled = True
        screenSettings_btn.Enabled = False
        textures_btn.Enabled = True
        sound_btn.Enabled = True
        network_btn.Enabled = True

        'RESET OUR OTHER PANELS!!!
        simpleSettings_pnl.Visible = False
        language_pnl.Visible = False
        screen_pnl.Visible = True
        textures_pnl.Visible = False
        sound_pnl.Visible = False
        network_pnl.Visible = False

        'RESET OUR OTHER BUTTONS!!!
        Simple_btn.BackgroundImage = Resources._160EN
        language_btn.BackgroundImage = Resources._148EN
        'screenSettings_btn.BackgroundImage = Properties.Resources._157;
        textures_btn.BackgroundImage = Resources._151EN
        sound_btn.BackgroundImage = Resources._163EN
        network_btn.BackgroundImage = Resources._219EN
    End Sub

    Private Sub screenSettings_btn_MouseEnter(sender As Object, e As EventArgs) Handles screenSettings_btn.MouseEnter
        screenSettings_btn.BackgroundImage = Resources._156EN
    End Sub

    Private Sub screenSettings_btn_MouseLeave(sender As Object, e As EventArgs) Handles screenSettings_btn.MouseLeave
        If screenSettings_btn.Enabled = False Then

            screenSettings_btn.BackgroundImage = Resources._155EN

        Else

            screenSettings_btn.BackgroundImage = Resources._157EN
        End If
    End Sub



    'Texture quality and Shader Quality
    Private Sub textures_btn_Click(sender As Object, e As EventArgs) Handles textures_btn.Click
        textures_btn.BackgroundImage = Resources._149EN

        'RESET OUR OTHER BUTTONS!!!
        Simple_btn.Enabled = True
        language_btn.Enabled = True
        screenSettings_btn.Enabled = True
        textures_btn.Enabled = False
        sound_btn.Enabled = True
        network_btn.Enabled = True

        'RESET OUR OTHER PANELS!!!
        simpleSettings_pnl.Visible = False
        language_pnl.Visible = False
        screen_pnl.Visible = False
        textures_pnl.Visible = True
        sound_pnl.Visible = False
        network_pnl.Visible = False

        'RESET OUR OTHER BUTTONS!!!
        Simple_btn.BackgroundImage = Resources._160EN
        language_btn.BackgroundImage = Resources._148EN
        screenSettings_btn.BackgroundImage = Resources._157EN
        'textures_btn.BackgroundImage = Properties.Resources._151;
        sound_btn.BackgroundImage = Resources._163EN
        network_btn.BackgroundImage = Resources._219EN
    End Sub

    Private Sub textures_btn_MouseEnter(sender As Object, e As EventArgs) Handles textures_btn.MouseEnter
        textures_btn.BackgroundImage = Resources._150EN
    End Sub

    Private Sub textures_btn_MouseLeave(sender As Object, e As EventArgs) Handles textures_btn.MouseLeave
        If textures_btn.Enabled = False Then

            textures_btn.BackgroundImage = Resources._149EN

        Else

            textures_btn.BackgroundImage = Resources._151EN
        End If
    End Sub


    'Sound Settings
    Private Sub sound_btn_Click(sender As Object, e As EventArgs) Handles sound_btn.Click
        sound_btn.BackgroundImage = Resources._161EN

        'RESET OUR OTHER BUTTONS!!!
        Simple_btn.Enabled = True
        language_btn.Enabled = True
        screenSettings_btn.Enabled = True
        textures_btn.Enabled = True
        sound_btn.Enabled = False
        network_btn.Enabled = True

        'RESET OUR OTHER PANELS!!!
        simpleSettings_pnl.Visible = False
        language_pnl.Visible = False
        screen_pnl.Visible = False
        textures_pnl.Visible = False
        sound_pnl.Visible = True
        network_pnl.Visible = False

        'RESET OUR OTHER BUTTONS!!!
        Simple_btn.BackgroundImage = Resources._160EN
        language_btn.BackgroundImage = Resources._148EN
        screenSettings_btn.BackgroundImage = Resources._157EN
        textures_btn.BackgroundImage = Resources._151EN
        'sound_btn.BackgroundImage = Properties.Resources._163;

        network_btn.BackgroundImage = Resources._219EN
    End Sub

    Private Sub sound_btn_MouseEnter(sender As Object, e As EventArgs) Handles sound_btn.MouseEnter
        sound_btn.BackgroundImage = Resources._162EN
    End Sub

    Private Sub sound_btn_MouseLeave(sender As Object, e As EventArgs) Handles sound_btn.MouseLeave
        If sound_btn.Enabled = False Then
            sound_btn.BackgroundImage = Resources._161EN
        Else
            sound_btn.BackgroundImage = Resources._163EN
        End If
    End Sub

    Private Sub language_cb_SelectedIndexChanged(sender As Object, e As EventArgs) Handles language_cb.SelectedIndexChanged


        If language_cb.SelectedIndex = 0 Then
            ' Japanese(JP)
            ' Restore our patches back to default.
            selectedLanguagePatch = "JP"
        End If

        If language_cb.SelectedIndex = 1 Then
            ' English(US)
            ' Download EN Patch
            selectedLanguagePatch = "EN"
        End If

        If language_cb.SelectedIndex = 2 Then
            ' English(US Complete)
            ' Download EN Patch, Full Trans-AM patch.
            selectedLanguagePatch = "ENFULL"
        End If


    End Sub
    Private Sub DownloadEnglishPatch()
        ' Here we parse the text file before passing it to the DownloadPatch function.
        ' The Using statement will dispose "net" as soon as we're done with it.
        ' If we decide not to, we can do away with "url" and just pass net.DownloadString in as the parameter.
        ' Furthermore, we could also parse it from within the function.
        Dim url As String = Program.Client.DownloadString("http://108.61.203.33/freedom/patches/enpatch.txt") 'This contains the original freedomURL for patching.
        DownloadPatch(url, EnglishPatch, "ENPatch.rar", RegKey.EnPatchVersion, Resources.strBackupEN, "Please select the pre-downloaded EN Patch RAR file")
    End Sub
    Private Sub DownloadPatch(patchUrl As String, patchName As String, patchFile As String, versionStr As String, msgBackup As String, msgSelectArchive As String)
        saveSettings_btn.Enabled = False
        return_btn.Enabled = False
        Simple_btn.Enabled = False
        language_btn.Enabled = False
        screenSettings_btn.Enabled = False
        textures_btn.Enabled = False
        sound_btn.Enabled = False
        network_btn.Enabled = False
        ListViewEx1.Enabled = False
        simpleSettings_pnl.Enabled = False
        language_pnl.Enabled = False
        screen_pnl.Enabled = False
        textures_pnl.Enabled = False
        sound_pnl.Enabled = False
        network_pnl.Enabled = False


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
            lblStatus.Text = "Installing Patch."
            ' messageForm.Show()


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
                lblStatus.Text = "Installed english patch."
                ' messageForm.Show()
                If Not String.IsNullOrEmpty(versionStr) Then RegKey.SetValue(Of String)(versionStr, strVersion)
            End If
            If backupyesno = MsgBoxResult.Yes Then
                External.FlashWindow(Handle, True)
                'Helper.WriteDebugInfo(("English patch " & Resources.strInstalledUpdatedBackup & backupPath))
                lblStatus.Text = "Installed updated backup."
                ' messageForm.Show()
                If Not String.IsNullOrEmpty(versionStr) Then RegKey.SetValue(Of String)(versionStr, strVersion)
            End If
            Helper.DeleteFile(patchName)
            'UnlockGui()

            lblStatus.Text = "English patch installed."
        Catch ex As Exception

            messageForm.message_txt.Text = ex.Message.ToString & " Stack Trace: " & ex.StackTrace
            messageForm.ShowDialog()

        End Try

        'Enable our buttons after patching
        lblStatus.Visible = False
        saveSettings_btn.Enabled = True
        return_btn.Enabled = True
        Simple_btn.Enabled = True
        language_btn.Enabled = True
        screenSettings_btn.Enabled = True
        textures_btn.Enabled = True
        sound_btn.Enabled = True
        network_btn.Enabled = True
        ListViewEx1.Enabled = True
        simpleSettings_pnl.Enabled = True
        language_pnl.Enabled = True
        screen_pnl.Enabled = True
        textures_pnl.Enabled = True
        sound_pnl.Enabled = True
        network_pnl.Enabled = True

        If selectedLanguagePatch = "ENFULL" Then
            downloadTransAM()
            lblStatus.Visible = True

        End If
    End Sub

    Public Sub downloadTransAM()
        'Install Large Files with TRANSAM to cut down on net costs for Agrajag and friends.
        'Need to speak with Agrajag and get some files before I can do this, though.
        lblStatus.Text = "Working.. Please wait..."

        'Disable our buttons while patching..
        saveSettings_btn.Enabled = False
        return_btn.Enabled = False
        Simple_btn.Enabled = False
        language_btn.Enabled = False
        screenSettings_btn.Enabled = False
        textures_btn.Enabled = False
        sound_btn.Enabled = False
        network_btn.Enabled = False
        ListViewEx1.Enabled = False
        simpleSettings_pnl.Enabled = False
        language_pnl.Enabled = False
        screen_pnl.Enabled = False
        textures_pnl.Enabled = False
        sound_pnl.Enabled = False
        network_pnl.Enabled = False

        Try
            If IsPso2WinDirMissing() Then Return

            ' Create a match using regular exp<b></b>ressions
            ' Spit out the value plucked from the code
            Dim LFDate As String = Program.Client.DownloadString("http://108.61.203.33/freedom/patches/largefilesTRANSAM.txt") 'The freedomURL path to download the Large files.

            Helper.WriteDebugInfoAndOk("Downloading Large Files info... ")
            DownloadFile("http://108.61.203.33/freedom/patches/lf.stripped.db.7z", "lf.stripped.db.7z")
            Dim processStartInfo2 As New ProcessStartInfo With
            {
                .FileName = (Program.StartPath & "\7za.exe"),
                .Verb = "runas",
                .Arguments = ("e -y lf.stripped.db.7z"),
                .WindowStyle = ProcessWindowStyle.Hidden,
            .UseShellExecute = True
            }
            Process.Start(processStartInfo2).WaitForExit()
            Dim DBMD5 As String = Helper.GetMd5("lf.stripped.db")
            Helper.WriteDebugInfoAndOk("Downloading Trans-Am tool... ")
            DownloadFile("http://108.61.203.33/freedom/pso2-transam.exe", "pso2-transam.exe")

            'execute pso2-transam stuff with -b flag for backup
            Dim processStartInfo As ProcessStartInfo = New ProcessStartInfo() With {.FileName = "pso2-transam.exe", .Verb = "runas"}
            'If Directory.Exists(backupdir) Then
            'Dim counter = Computer.FileSystem.GetFiles(backupdir)
            'If counter.Count > 0 Then
            processStartInfo.Arguments = ("-t largefiles-" & LFDate & " lf.stripped.db " & """" & Program.Pso2WinDir & """")
            Clipboard.SetText(processStartInfo.Arguments)
            'Else
            'Helper.Log("[TRANSAM] Creating backup directory")
            'Directory.CreateDirectory(backupdir)
            'Helper.WriteDebugInfo(Resources.strCreatingBackupDirectory)
            'processStartInfo.Arguments = ("-b " & """" & backupdir & """" & " -t story-eng-" & strStoryPatchLatestBase & " pso2.stripped.db " & """" & Program.Pso2WinDir & """")
            'End If
            'End If

            'We don't need to make backups anymore
            'If Not Directory.Exists(backupdir) Then
            ' Helper.Log("[TRANSAM] Creating backup directory")
            ' Directory.CreateDirectory(backupdir)
            ' Helper.WriteDebugInfo(Resources.strCreatingBackupDirectory)
            ' processStartInfo.Arguments = ("-b " & """" & backupdir & """" & " -t story-eng-" & strStoryPatchLatestBase & " pso2.stripped.db " & """" & Program.Pso2WinDir & """")
            ' End If

            processStartInfo.UseShellExecute = False
            Helper.Log("[TRANSAM] Starting shitstorm")
            processStartInfo.Arguments = processStartInfo.Arguments.Replace("\", "/")
            Helper.Log("TRANSM parameters: " & processStartInfo.Arguments & vbCrLf & "TRANSAM Working Directory: " & processStartInfo.WorkingDirectory)
            Helper.Log("[TRANSAM] Program started")
            If Helper.GetMd5("lf.stripped.db") <> DBMD5 Then
                MsgBox("ERROR: Files have been modified since download. Aborting...")
                Exit Sub
            End If
            Process.Start(processStartInfo).WaitForExit()
            Helper.DeleteFile("lf.stripped.db")
            Helper.DeleteFile("pso2-transam.exe")
            External.FlashWindow(Handle, True)
            'Story Patch 3-12-2014.rar
            RegKey.SetValue(Of String)(RegKey.LargeFilesVersion, LFDate.Replace("-", "/"))
            Helper.WriteDebugInfo("Large Files Patch installed!")

            lblStatus.Text = "Full English patch installed."
        Catch ex As Exception
            MsgBox("ERROR - " & ex.Message.ToString)
        End Try

        'Enable our buttons after patching
        lblStatus.Visible = False
        saveSettings_btn.Enabled = True
        return_btn.Enabled = True
        Simple_btn.Enabled = True
        language_btn.Enabled = True
        screenSettings_btn.Enabled = True
        textures_btn.Enabled = True
        sound_btn.Enabled = True
        network_btn.Enabled = True
        ListViewEx1.Enabled = True

        simpleSettings_pnl.Enabled = True
        language_pnl.Enabled = True
        screen_pnl.Enabled = True
        textures_pnl.Enabled = True
        sound_pnl.Enabled = True
        network_pnl.Enabled = True
    End Sub


    Private Sub btnConfigure_Click(sender As Object, e As EventArgs) Handles btnConfigure.Click

        messageForm.message_txt.Text = "No settings to configure for Items. Please wait for next avaialble update."
        messageForm.ShowDialog()
        ' If ListViewEx1.FocusedItem.Text = "Item Translation" Then FrmItemConfig.Show()
    End Sub

    Private Sub saveSettings_btn_Click(sender As Object, e As EventArgs) Handles saveSettings_btn.Click
        saveSettings_btn.BackgroundImage = Resources._140EN

        Me.Cursor = Cursors.WaitCursor

        lblStatus.Visible = True
        lblStatus.Text = "Working.. Please wait..."
        'saveSettings_btn.Enabled = False
        'return_btn.Enabled = False
        'Simple_btn.Enabled = False
        'language_btn.Enabled = False
        'screenSettings_btn.Enabled = False
        'textures_btn.Enabled = False
        'sound_btn.Enabled = False
        'network_btn.Enabled = False

        '  simpleSettings_pnl.Enabled = False
        '  language_pnl.Enabled = False
        '  screen_pnl.Enabled = False
        '  textures_pnl.Enabled = False
        '  sound_pnl.Enabled = False
        '  network_pnl.Enabled = False



        Try




            If shaderStandard_rb.Checked = True Then SaveIniSetting("ShaderQuality", "true")
            If sharderSimple_rb.Checked = True Then SaveIniSetting("ShaderQuality", "false")
            Helper.Log("Saving Movie Play...")
            If enableVideos_rb.Checked = True Then SaveIniSetting("MoviePlay", "true")
            If disableVideos_rb.Checked = True Then SaveIniSetting("MoviePlay", "false")


            'Window
            If windowed_rb.Checked = True Then
                Helper.Log("Saving Window Mode (Windowed)...")
                SaveIniSetting("FullScreen", "false")
                SaveIniSetting("VirtualFullScreen", "false")
            End If

            If fullScreen_rb.Checked = True Then
                Helper.Log("Saving Window Mode (Fullscreen)...")
                SaveIniSetting("FullScreen", "true")
                SaveIniSetting("VirtualFullScreen", "false")
            End If

            If virtualFullscreen_rb.Checked = True Then
                Helper.Log("Saving Window Mode (Virtual Fullscreen)...")
                SaveIniSetting("FullScreen", "false")
                SaveIniSetting("VirtualFullScreen", "true")
            End If

            'Surround Sound
            If surroundON_rb.Checked = True Then
                SaveIniSetting("Surround", "true")
            End If
            If surroundOFF_rb.Checked = True Then
                SaveIniSetting("Surround", "false")
            End If

            'Texture Resolutions
            If highResTextures_rb.Checked = True Then
                SaveIniSetting("TextureResolution", "2")
            End If
            If standardRes_rb.Checked = True Then
                SaveIniSetting("TextureResolution", "1")
            End If
            If compactRes_rb.Checked = True Then
                SaveIniSetting("TextureResolution", "0")
            End If

            'Interface Size
            If charSize_cb.SelectedIndex = 0 Then
                SaveIniSetting("InterfaceSize", "0")
            End If
            If charSize_cb.SelectedIndex = 1 Then
                SaveIniSetting("InterfaceSize", "1")
            End If
            If charSize_cb.SelectedIndex = 2 Then
                SaveIniSetting("InterfaceSize", "2")
            End If


            If Not screenResolution_cb.Items.Contains(screenResolution_cb.Text) Then
                messageForm.message_txt.Text = "Please select a supported resolution!"
                messageForm.ShowDialog()
                ' MsgBox("Please select a supported resolution!")
                Return
            End If

            Helper.Log("Saving Resolution...")
            'If ComboBoxEx5.SelectedText <> "x" Then
            Dim strResolution As String = screenResolution_cb.SelectedItem.ToString()

            Dim realResolution As String() = strResolution.Split("x"c)
            SaveResolutionWidth(realResolution(0))
            SaveResolutionHeight(realResolution(1))
            SaveResolutionWidth3D(realResolution(0))
            SaveResolutionHeight3D(realResolution(1))
            'End If


            'Dim fps As String = fpsRate_cb.SelectedItem.ToString().Replace("fps", "").Replace("Unlimited", "0")

            Helper.Log("Saving FPS...")
            'SaveIniSetting("FrameKeep", fps)

            If fpsRate_cb.SelectedIndex = 0 Then
                SaveIniSetting("FrameKeep", "30")
            End If
            If fpsRate_cb.SelectedIndex = 1 Then
                SaveIniSetting("FrameKeep", "60")
            End If
            If fpsRate_cb.SelectedIndex = 2 Then
                SaveIniSetting("FrameKeep", "90")
            End If
            If fpsRate_cb.SelectedIndex = 3 Then
                SaveIniSetting("FrameKeep", "120")
            End If
            If fpsRate_cb.SelectedIndex = 4 Then
                SaveIniSetting("FrameKeep", "0")
            End If

            Helper.Log("Saving Volume...")
            SaveIniSetting("Bgm", bgmVol_trkb.Value.ToString())
            SaveIniSetting("Voice", voiceVol_tb.Value.ToString())
            SaveIniSetting("Movie", vidVol_tb.Value.ToString())
            SaveIniSetting("Se", seVol_tb.Value.ToString())
            SaveIniSetting("DrawLevel", simpleSettigns_tb.Value.ToString())

            If CheckBoxX1.Checked Then
                Helper.Log("Disabling Interface...")
                If ReadIniSetting("X") <> "99999" Then
                    If ReadIniSetting("Y") <> "99999" Then
                        RegKey.SetValue(Of String)(RegKey.OldX, ReadIniSetting("X"))
                        RegKey.SetValue(Of String)(RegKey.OldY, ReadIniSetting("Y"))
                        SaveIniSetting("X", "99999")
                        SaveIniSetting("Y", "99999")
                    End If
                End If
            End If

            If Not CheckBoxX1.Checked Then
                Helper.Log("Enabling Interface...")
                If ReadIniSetting("X") = "99999" Then
                    If ReadIniSetting("Y") = "99999" Then
                        SaveIniSetting("X", RegKey.GetValue(Of String)(RegKey.OldX))
                        SaveIniSetting("Y", RegKey.GetValue(Of String)(RegKey.OldY))
                    End If
                End If
            End If


            My.Settings.SimpleDraw = simpleSettigns_tb.Value
            My.Settings.Language = language_cb.SelectedIndex
            My.Settings.vidPlaybackYes = enableVideos_rb.Checked
            My.Settings.vidPlaybackNo = disableVideos_rb.Checked
            My.Settings.Window = windowed_rb.Checked
            My.Settings.Fullscreen = fullScreen_rb.Checked
            My.Settings.virtualFullscreen = virtualFullscreen_rb.Checked
            My.Settings.ScreenSize = screenResolution_cb.SelectedIndex
            My.Settings.maxFPS = fpsRate_cb.SelectedIndex
            My.Settings.charSize = charSize_cb.SelectedIndex
            My.Settings.textResolutionHigh = highResTextures_rb.Checked
            My.Settings.standardTextRes = standardRes_rb.Checked
            My.Settings.compactTextRes = compactRes_rb.Checked
            My.Settings.ShadersHigh = shaderStandard_rb.Checked
            My.Settings.ShadersLow = sharderSimple_rb.Checked
            My.Settings.bgmVolume = bgmVol_trkb.Value
            My.Settings.seVolume = seVol_tb.Value
            My.Settings.voiceVolume = voiceVol_tb.Value
            My.Settings.videoPlaybackVol = vidVol_tb.Value
            My.Settings.surroundEnabled = surroundON_rb.Checked
            My.Settings.surroundDisabled = surroundOFF_rb.Checked
            ' My.Settings.titleTranslator = ListViewEx1.FocusedItem.Text = "Title Translation"
            ' My.Settings.itemTranslator = ListViewEx1.FocusedItem.Text = "Item Translation"
            ' My.Settings.dmgParser = ListViewEx1.FocusedItem.Text = "Damage Parser"
            ' My.Settings.pso2ProxyPlugin = ListViewEx1.FocusedItem.Text = "PSO2Proxy Plugin"
            My.Settings.Save()

            If selectedLanguagePatch = "JP" Then
                'restore files
            End If
            If selectedLanguagePatch = "EN" Then
                DownloadEnglishPatch()
            End If
            If selectedLanguagePatch = "ENFULL" Then
                DownloadEnglishPatch()
                'Thread.Sleep(10000)
                'DownloadLargeFiles()
            End If

            messageForm.message_txt.Text = "Settings have been saved successfully."
            messageForm.ShowDialog()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            messageForm.message_txt.Text = ex.Message
            messageForm.ShowDialog()
        End Try



        ' saveSettings_btn.Enabled = True
        ' return_btn.Enabled = True
        ' Simple_btn.Enabled = True
        ' language_btn.Enabled = True
        ' screenSettings_btn.Enabled = True
        ' textures_btn.Enabled = True
        ' sound_btn.Enabled = True
        ' network_btn.Enabled = True

        ' simpleSettings_pnl.Enabled = True
        ' language_pnl.Enabled = True
        ' screen_pnl.Enabled = True
        ' textures_pnl.Enabled = True
        ' sound_pnl.Enabled = True
        ' network_pnl.Enabled = True
        lblStatus.Visible = False
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub SaveIniSetting(settingToSave As String, value As String)
        Try
            'INICache(SettingToSave) = Value

            TextBoxX1.Text = ""
            Dim settingString As String = File.ReadAllText(_usersettingsfile)
            Dim textLines As String() = settingString.Split(Environment.NewLine.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            Dim i As Integer
            Dim j As Integer
            For i = 0 To (textLines.Length - 1)
                If textLines(i).Contains(" " & settingToSave & " ") Then
                    Dim strLine As String = textLines(i).Replace(vbTab, "")
                    Dim strReturn As String() = strLine.Split("="c)
                    Dim finalString As String = strReturn(1).Replace("""", "").Replace(","c, "")
                    textLines(i) = textLines(i).Replace(finalString, (" " & value))
                    For j = 0 To textLines.Length
                        If j + 1 = textLines.Length Then
                            TextBoxX1.AppendText("}")
                            File.Delete(_usersettingsfile)
                            File.WriteAllText(_usersettingsfile, TextBoxX1.Text)
                            Return
                        End If
                        TextBoxX1.AppendText(textLines(j) & vbCrLf)
                    Next j
                End If
            Next i
        Catch ex As Exception
            Helper.Log(ex.Message)
            Helper.WriteDebugInfo(Resources.strERROR & ex.Message)
        End Try
    End Sub

    Private Sub SaveResolutionHeight(value As String)
        Try
            TextBoxX1.Text = ""
            Dim settingString As String = File.ReadAllText(_usersettingsfile)
            Dim textLines As String() = settingString.Split(Environment.NewLine.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            Dim i As Integer
            Dim j As Integer
            Dim contains As Boolean = False
            For i = 0 To (textLines.Length - 1)
                If textLines(i).Contains("Windows = {") Then
                    For x As Integer = 1 To 9
                        If textLines(i + x).Contains("Height =") Then
                            i += x
                            contains = True
                            Exit For
                        End If
                    Next x

                    If Not contains Then
                        Helper.WriteDebugInfo("Couldn't find Height in user settings. This is OKAY. If you notice your resolution not changing, try resetting your PSO2 Settings to default. If everything works, feel free to ignore this error.")
                        Return
                    End If

                    Dim strLine As String = textLines(i).Replace(vbTab, "")
                    Dim strReturn As String() = strLine.Split("="c)
                    Dim finalString As String = strReturn(1).Replace("""", "").Replace(","c, "")
                    textLines(i) = textLines(i).Replace(finalString, (" " & value))
                    For j = 0 To textLines.Length
                        If j + 1 = textLines.Length Then
                            TextBoxX1.AppendText("}")
                            File.Delete(_usersettingsfile)
                            File.WriteAllText(_usersettingsfile, TextBoxX1.Text)
                            Return
                        End If
                        TextBoxX1.AppendText(textLines(j) & vbCrLf)
                    Next j
                End If
            Next i
        Catch ex As Exception
            Helper.Log(ex.Message)
            Helper.WriteDebugInfo(Resources.strERROR & ex.Message)
        End Try
    End Sub

    Private Sub SaveResolutionWidth(value As String)
        Try
            TextBoxX1.Text = ""
            Dim settingString As String = File.ReadAllText(_usersettingsfile)
            Dim textLines As String() = settingString.Split(Environment.NewLine.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            Dim i As Integer
            Dim j As Integer
            Dim contains As Boolean = False
            For i = 0 To (textLines.Length - 1)
                If textLines(i).Contains("Windows = {") Then
                    For x As Integer = 1 To 9
                        If textLines(i + x).Contains("Width =") Then
                            i += x
                            contains = True
                            Exit For
                        End If
                    Next x

                    If Not contains Then
                        Helper.WriteDebugInfo("Couldn't find Width in user settings. This is OKAY. If you notice your resolution not changing, try resetting your PSO2 Settings to default. If everything works, feel free to ignore this error.")
                        Return
                    End If

                    Dim strLine As String = textLines(i).Replace(vbTab, "")
                    Dim strReturn As String() = strLine.Split("="c)
                    Dim finalString As String = strReturn(1).Replace("""", "").Replace(","c, "")
                    textLines(i) = textLines(i).Replace(finalString, (" " & value))

                    For j = 0 To textLines.Length
                        If j + 1 = textLines.Length Then
                            TextBoxX1.AppendText("}")
                            File.Delete(_usersettingsfile)
                            File.WriteAllText(_usersettingsfile, TextBoxX1.Text)
                            Return
                        End If
                        TextBoxX1.AppendText(textLines(j) & vbCrLf)
                    Next j

                End If
            Next i
        Catch ex As Exception
            Helper.Log(ex.Message)
            Helper.WriteDebugInfo(Resources.strERROR & ex.Message)
        End Try
    End Sub

    Private Sub SaveResolutionHeight3D(value As String)
        Try
            TextBoxX1.Text = ""
            Dim settingString As String = File.ReadAllText(_usersettingsfile)
            Dim textLines As String() = settingString.Split(Environment.NewLine.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            Dim i As Integer
            Dim j As Integer
            Dim contains As Boolean = False
            For i = 0 To (textLines.Length - 1)
                If textLines(i).Contains("Windows = {") Then
                    For x As Integer = 1 To 9
                        If textLines(i + x).Contains("Height3d =") Then
                            i += x
                            contains = True
                            Exit For
                        End If
                    Next x

                    If Not contains Then
                        Helper.WriteDebugInfo("Couldn't find Height3D in user settings. This is OKAY. If you notice your resolution not changing, try resetting your PSO2 Settings to default. If everything works, feel free to ignore this error.")
                        Return
                    End If

                    Dim strLine As String = textLines(i).Replace(vbTab, "")
                    Dim strReturn As String() = strLine.Split("="c)
                    Dim finalString As String = strReturn(1).Replace("""", "").Replace(","c, "")
                    textLines(i) = textLines(i).Replace(finalString, (" " & value))
                    For j = 0 To textLines.Length
                        If j + 1 = textLines.Length Then
                            TextBoxX1.AppendText("}")
                            File.Delete(_usersettingsfile)
                            File.WriteAllText(_usersettingsfile, TextBoxX1.Text)
                            Return
                        End If
                        TextBoxX1.AppendText(textLines(j) & vbCrLf)
                    Next j
                End If
            Next i
        Catch ex As Exception
            Helper.Log(ex.Message)
            Helper.WriteDebugInfo(Resources.strERROR & ex.Message)
        End Try
    End Sub

    Private Sub SaveResolutionWidth3D(value As String)
        Try
            TextBoxX1.Text = ""
            Dim settingString As String = File.ReadAllText(_usersettingsfile)
            Dim textLines As String() = settingString.Split(Environment.NewLine.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            Dim i As Integer
            Dim j As Integer
            Dim contains As Boolean = False
            For i = 0 To (textLines.Length - 1)
                If textLines(i).Contains("Windows = {") Then
                    For x As Integer = 1 To 9
                        If textLines(i + x).Contains("Width3d =") Then
                            i += x
                            contains = True
                            Exit For
                        End If
                    Next x

                    If Not contains Then
                        Helper.WriteDebugInfo("Couldn't find Width3D in user settings. This is OKAY. If you notice your resolution not changing, try resetting your PSO2 Settings to default. If everything works, feel free to ignore this error.")
                        Return
                    End If

                    Dim strLine As String = textLines(i).Replace(vbTab, "")
                    Dim strReturn As String() = strLine.Split("="c)
                    Dim finalString As String = strReturn(1).Replace("""", "").Replace(","c, "")
                    textLines(i) = textLines(i).Replace(finalString, (" " & value))

                    For j = 0 To textLines.Length
                        If j + 1 = textLines.Length Then
                            TextBoxX1.AppendText("}")
                            File.Delete(_usersettingsfile)
                            File.WriteAllText(_usersettingsfile, TextBoxX1.Text)
                            Return
                        End If
                        TextBoxX1.AppendText(textLines(j) & vbCrLf)
                    Next j

                End If
            Next i
        Catch ex As Exception
            Helper.Log(ex.Message)
            Helper.WriteDebugInfo(Resources.strERROR & ex.Message)
        End Try
    End Sub
    Private Sub ListViewEx1_Click(sender As Object, e As EventArgs) Handles ListViewEx1.Click
        savePlugins.Visible = True

        lblPluginInfo.Text = ""
        btnConfigure.Visible = False
        If ListViewEx1.FocusedItem.Text = "Title Translation" Then
            lblPluginInfo.Text =
                "Translates the titles in-game." & vbCrLf &
                "Plugin Author: Variant" & vbCrLf &
                "DLL Name: PSO2TitleTranslator.dll" & vbCrLf &
                "Support URL: N/A"
            Exit Sub
        End If
        If ListViewEx1.FocusedItem.Text = "Item Translation" Then
            lblPluginInfo.Text =
                "Translates the items in-game." & vbCrLf &
                "Plugin Author: arcnmx/Variant/Raven123" & vbCrLf &
                "DLL Name: translator.dll" & vbCrLf &
                "Support URL: N/A"
            btnConfigure.Visible = True
            Exit Sub
        End If
        If ListViewEx1.FocusedItem.Text = "Damage Parser" Then
            lblPluginInfo.Text =
                "DPS (Damage Per Second) Parser plugin. Exports damage logs into a damagelogs folder where pso2.exe is, in excel format." & vbCrLf &
                "Plugin Author: Variant" & vbCrLf &
                "DLL Name: PSO2DamageDump.dll" & vbCrLf &
                "Support URL: N/A"
            Exit Sub
        End If
        If ListViewEx1.FocusedItem.Text = "PSO2Proxy Plugin" Then
            lblPluginInfo.Text =
                "PSO2Proxy plugin. Allows people from SEA and other blocked regions to connect to PSO2JP." & vbCrLf &
                "Plugin Author: Alam_Squeeze" & vbCrLf &
                "DLL Name: PSO2Proxy.dll" & vbCrLf &
                "Support URL: http://pso2proxy.cyberkitsune.net"
            Exit Sub
        End If
    End Sub

    Private Sub saveSettings_btn_MouseEnter(sender As Object, e As EventArgs) Handles saveSettings_btn.MouseEnter
        saveSettings_btn.BackgroundImage = Resources._141EN
    End Sub

    Private Sub saveSettings_btn_MouseLeave(sender As Object, e As EventArgs) Handles saveSettings_btn.MouseLeave
        saveSettings_btn.BackgroundImage = Resources._142EN
    End Sub

    Private Sub virtualFullscreen_rb_CheckedChanged_1(sender As Object, e As EventArgs) Handles virtualFullscreen_rb.CheckedChanged
        If virtualFullscreen_rb.Checked = True Then
            screenResolution_cb.Enabled = False
        Else
            screenResolution_cb.Enabled = True
        End If
    End Sub

    Private Sub bgmVol_trkb_Scroll_1(sender As Object, e As EventArgs) Handles bgmVol_trkb.Scroll
        bgmVol_lbl.Text = bgmVol_trkb.Value.ToString()
    End Sub

    Private Sub seVol_tb_Scroll_1(sender As Object, e As EventArgs) Handles seVol_tb.Scroll
        seVol_lbl.Text = seVol_tb.Value.ToString()
    End Sub

    Private Sub voiceVol_tb_Scroll_1(sender As Object, e As EventArgs) Handles voiceVol_tb.Scroll
        voiceVol_lbl.Text = voiceVol_tb.Value.ToString()
    End Sub

    Private Sub vidVol_tb_Scroll_1(sender As Object, e As EventArgs) Handles vidVol_tb.Scroll
        vidVol_lbl.Text = vidVol_tb.Value.ToString()
    End Sub

    Private Sub savePlugins_Click(sender As Object, e As EventArgs) Handles savePlugins.Click
        'Move all plugins to the base folder, then sort according to the list.
        Dim FinalExportString As String = ""
        For Each fi As FileInfo In New DirectoryInfo(Program.Pso2RootDir & "\plugins\disabled\").GetFiles
            File.Move(fi.FullName, Path.Combine(Program.Pso2RootDir & "\plugins\", fi.Name))
        Next

        For l_index As Integer = 0 To ListViewEx1.Items.Count - 1
            Dim filename As String = CStr(ListViewEx1.Items(l_index).Text).Replace("Item Translation", "translator.dll").Replace("Title Translation", "PSO2TitleTranslator.dll").Replace("Damage Parser", "PSO2DamageDump.dll").Replace("PSO2Proxy Plugin", "PSO2Proxy.dll")
            If ListViewEx1.Items(l_index).Checked = True Then
                'Enable
                If filename = "translator.dll" Then
                    If Not File.Exists(Program.Pso2RootDir & "\translation.cfg") Then
                        File.WriteAllText(Program.Pso2RootDir & "\translation.cfg", "TranslationPath:translation.bin")
                    End If

                    'Start the shitstorm
                    Dim builtFile As New List(Of String)
                    For Each line In Helper.GetLines(Program.Pso2RootDir & "\translation.cfg")
                        If line.Contains("TranslationPath:") Then line = "TranslationPath:translation.bin"
                        builtFile.Add(line)
                    Next
                    File.WriteAllLines(Program.Pso2RootDir & "\translation.cfg", builtFile.ToArray())

                    Program.UseItemTranslation = True
                    RegKey.SetValue(Of Boolean)(RegKey.UseItemTranslation, Program.UseItemTranslation)
                End If
                'Do nothing! :D
            Else
                'Disable

                If filename = "translator.dll" Then
                    'Helper.WriteDebugInfoAndOk(Resources.strDeletingItemCache)
                    'Helper.WriteDebugInfoSameLine(Resources.strDone)
                    Dim builtFile As New List(Of String)
                    For Each line In Helper.GetLines(Program.Pso2RootDir & "\translation.cfg")
                        If line.Contains("TranslationPath:") Then line = "TranslationPath:"
                        builtFile.Add(line)
                    Next
                    File.WriteAllLines(Program.Pso2RootDir & "\translation.cfg", builtFile.ToArray())
                    Program.UseItemTranslation = False
                    RegKey.SetValue(Of Boolean)(RegKey.UseItemTranslation, Program.UseItemTranslation)
                End If
                File.Move(Program.Pso2RootDir & "\plugins\" & filename, Path.Combine(Program.Pso2RootDir & "\plugins\disabled\", filename))
            End If
        Next
        For Each fi As FileInfo In New DirectoryInfo(Program.Pso2RootDir & "\plugins\").GetFiles
            FinalExportString += fi.Name & ","
        Next
        If FinalExportString.Length > 0 Then FinalExportString = FinalExportString.Remove(FinalExportString.Length - 1, 1)
        RegKey.SetValue(Of String)(RegKey.PluginsEnabled, FinalExportString)
        'FrmMain.WriteDebugInfoAndOk("Plugin changes saved!")
        messageForm.message_txt.Text = "Applied plugin settings."
        messageForm.ShowDialog()
        savePlugins.Visible = False
    End Sub

    Private Sub defaults_btn_Click(sender As Object, e As EventArgs) Handles defaults_btn.Click


        defaults_btn.BackgroundImage = Resources._137EN
        Thread.Sleep(100)

        'My.Resources.user.(finalPath)

        YesNoMessage.message_txt.Text = ("Settings will return to defaults. Are you sure?")
        YesNoMessage.ShowDialog()

        ' If YesNoMessage.ok_yesM = True Then
        '     MessageBox.Show("The Yes Button was clicked.")
        ' End If
        If My.Settings.resetSettings = True Then
            IO.File.WriteAllBytes(finalPath, Resources.user)
            'MessageBox.Show("The Yes Button was clicked.")
            Thread.Sleep(100)
            Dim devM As External.Devmode
            devM.dmDeviceName = New String(Chr(0), 32)
            devM.dmFormName = New String(Chr(0), 32)
            devM.dmSize = CShort(Marshal.SizeOf(GetType(External.Devmode)))

            Dim modeIndex As Integer = 0
            ' 0 = The first mode
            While External.EnumDisplaySettings(Nothing, modeIndex, devM)
                ' Mode found
                If Not screenResolution_cb.Items.Contains(devM.dmPelsWidth & "x" & devM.dmPelsHeight) Then screenResolution_cb.Items.Add(devM.dmPelsWidth & "x" & devM.dmPelsHeight)

                ' The next mode
                modeIndex += 1
            End While



            Dim currentHeight As String = ReadIniSetting("Height3d")
            Dim currentWidth As String = ReadIniSetting("Width3d")

            Dim fullRes As String = currentWidth & "x" & currentHeight

            screenResolution_cb.Text = fullRes
            simpleSettigns_tb.Value = Convert.ToInt32(ReadIniSetting("DrawLevel"))
            bgmVol_trkb.Value = Convert.ToInt32(ReadIniSetting("Bgm"))
            seVol_tb.Value = Convert.ToInt32(ReadIniSetting("Se"))
            voiceVol_tb.Value = Convert.ToInt32(ReadIniSetting("Voice"))
            vidVol_tb.Value = Convert.ToInt32(ReadIniSetting("Movie"))
            If ReadIniSetting("Surround") = "True" Then surroundON_rb.Checked = True
            If ReadIniSetting("Surround") = "True" Then surroundOFF_rb.Checked = True
            If ReadIniSetting("TextureResolution") = "2" Then highResTextures_rb.Checked = True
            If ReadIniSetting("TextureResolution") = "1" Then standardRes_rb.Checked = True
            If ReadIniSetting("TextureResolution") = "0" Then compactRes_rb.Checked = True 'Convert.ToInt32(ReadIniSetting("TextureResolution"))
            charSize_cb.SelectedIndex = Convert.ToInt32(ReadIniSetting("InterfaceSize"))
            'fpsRate_cb.Text = ReadIniSetting("FrameKeep") & " FPS"
            If ReadIniSetting("FrameKeep") = "30" Then fpsRate_cb.SelectedIndex = 0
            If ReadIniSetting("FrameKeep") = "60" Then fpsRate_cb.SelectedIndex = 1
            If ReadIniSetting("FrameKeep") = "90" Then fpsRate_cb.SelectedIndex = 2
            If ReadIniSetting("FrameKeep") = "120" Then fpsRate_cb.SelectedIndex = 3
            If ReadIniSetting("FrameKeep") = "0" Then fpsRate_cb.SelectedIndex = 4
            'If fpsRate_cb.Text = "0 FPS" Then fpsRate_cb.Text = "Unlimited FPS"
            If ReadIniSetting("ShaderQuality") = "true" Then shaderStandard_rb.Checked = True
            If ReadIniSetting("ShaderQuality") = "false" Then sharderSimple_rb.Checked = True
            If ReadIniSetting("MoviePlay") = "true" Then enableVideos_rb.Checked = True
            If ReadIniSetting("MoviePlay") = "false" Then disableVideos_rb.Checked = True
            If ReadIniSetting("FullScreen") = "false" Then
                windowed_rb.Checked = True
            End If
            If ReadIniSetting("FullScreen") = "true" Then
                fullScreen_rb.Checked = True
            End If
            If ReadIniSetting("Fullscreen") = "false" And ReadIniSetting("VirtualFullScreen") = "true" Then
                virtualFullscreen_rb.Checked = True
                screenResolution_cb.Enabled = False
                'Disable resolution thingie
            End If
            'ComboBoxEx5.Text = ReadINISetting("Width", 240) & "x" & ReadINISetting("Height", 240)
            If Not screenResolution_cb.Items.Contains(screenResolution_cb.Text) Then screenResolution_cb.SelectedIndex = 0
            CheckBoxX1.Checked = False
            If ReadIniSetting("Y") = "99999" Then
                If ReadIniSetting("X") = "99999" Then
                    CheckBoxX1.Checked = True
                End If
            End If

        Else
            'MessageBox.Show("The Cancel Button was clicked.")

        End If







    End Sub

    Private Sub defaults_btn_MouseEnter(sender As Object, e As EventArgs) Handles defaults_btn.MouseEnter
        defaults_btn.BackgroundImage = Resources._138EN
    End Sub

    Private Sub defaults_btn_MouseLeave(sender As Object, e As EventArgs) Handles defaults_btn.MouseLeave
        defaults_btn.BackgroundImage = Resources._139EN
    End Sub

    Private Sub Main_MouseDown(sender As Object, e As MouseEventArgs) Handles Main.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If
    End Sub

    Private Sub settingsPanel_MouseDown(sender As Object, e As MouseEventArgs) Handles settingsPanel.MouseDown
        If e.Button = MouseButtons.Left Then
            ReleaseCapture()
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0)
        End If
    End Sub

    Private Sub website_btn_Click(sender As Object, e As EventArgs) Handles website_btn.Click
        website_btn.BackgroundImage = Resources._167EN
        messageForm.message_txt.Text = "Running pso2launcher to perform standard filecheck to fix any errors."
        messageForm.ShowDialog()

        ' Dim prooo = New Process(lblDirectory.Text & "\pso2launcher.exe")


        'RegKey.SetValue(Of String)(RegKey.Pso2Dir,
        Try
            Hide()
            Process.Start(lblDirectory.Text & "\pso2launcher.exe")
            Timer1.Start()
        Catch ex As Exception
            messageForm.message_txt.Text = ex.Message
            messageForm.ShowDialog()
        End Try





    End Sub

    Private Sub website_btn_MouseEnter(sender As Object, e As EventArgs) Handles website_btn.MouseEnter
        website_btn.BackgroundImage = Resources._168EN
    End Sub

    Private Sub website_btn_MouseLeave(sender As Object, e As EventArgs) Handles website_btn.MouseLeave
        website_btn.BackgroundImage = Resources._169EN
    End Sub

    Private Sub about_btn_Click(sender As Object, e As EventArgs) Handles about_btn.Click
        about_btn.BackgroundImage = Resources._173EN
        Close()
    End Sub

    Private Sub about_btn_MouseEnter(sender As Object, e As EventArgs) Handles about_btn.MouseEnter
        about_btn.BackgroundImage = Resources._174EN
    End Sub

    Private Sub about_btn_MouseLeave(sender As Object, e As EventArgs) Handles about_btn.MouseLeave
        about_btn.BackgroundImage = Resources._175EN
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For Each proc As Process In Process.GetProcessesByName("pso2launcher.exe")
            If proc.MainWindowTitle = "PHANTASY STAR ONLINE 2" AndAlso proc.MainModule.ToString() = "ProcessModule (PHANTASY STAR ONLINE 2)" Then
                proc.Kill()

                Timer1.Stop()

                Show()
            End If
        Next
    End Sub

    Private Sub quit_btn_MouseDown(sender As Object, e As MouseEventArgs) Handles quit_btn.MouseDown
        quit_btn.BackgroundImage = Resources._207__Japanese__Japan__
    End Sub

    Private Sub Play_btn_MouseDown(sender As Object, e As MouseEventArgs) Handles Play_btn.MouseDown
        Play_btn.BackgroundImage = Resources._170ENPLAY1
    End Sub

    Private Sub settings_btn_MouseDown(sender As Object, e As MouseEventArgs) Handles settings_btn.MouseDown
        settings_btn.BackgroundImage = Resources._164EN
    End Sub

    Private Sub website_btn_MouseDown(sender As Object, e As MouseEventArgs) Handles website_btn.MouseDown
        website_btn.BackgroundImage = Resources._167EN
    End Sub

    Private Sub about_btn_MouseDown(sender As Object, e As MouseEventArgs) Handles about_btn.MouseDown
        about_btn.BackgroundImage = Resources._173EN
    End Sub

    Private Sub saveSettings_btn_MouseDown(sender As Object, e As MouseEventArgs) Handles saveSettings_btn.MouseDown
        saveSettings_btn.BackgroundImage = Resources._140EN
    End Sub

    Private Sub defaults_btn_MouseDown(sender As Object, e As MouseEventArgs) Handles defaults_btn.MouseDown
        defaults_btn.BackgroundImage = Resources._137EN
    End Sub

    Private Sub return_btn_MouseDown(sender As Object, e As MouseEventArgs) Handles return_btn.MouseDown
        return_btn.BackgroundImage = Resources._134EN
    End Sub

    Private Sub return_btn_MouseLeave(sender As Object, e As EventArgs) Handles return_btn.MouseLeave
        return_btn.BackgroundImage = Resources._136EN
    End Sub

    Private Sub Main_Paint(sender As Object, e As PaintEventArgs) Handles Main.Paint

    End Sub

    Private Sub website2_btn_Click(sender As Object, e As EventArgs) Handles website2_btn.Click
        Process.Start("http://pso2.jp/")
    End Sub

    Private Sub website2_btn_MouseDown(sender As Object, e As MouseEventArgs) Handles website2_btn.MouseDown
        website2_btn.BackgroundImage = Resources._143EN
    End Sub

    Private Sub website2_btn_MouseEnter(sender As Object, e As EventArgs) Handles website2_btn.MouseEnter
        website2_btn.BackgroundImage = Resources._144EN
    End Sub

    Private Sub website2_btn_MouseLeave(sender As Object, e As EventArgs) Handles website2_btn.MouseLeave
        website2_btn.BackgroundImage = Resources._145EN
    End Sub

    Private Sub arksDonation_Click(sender As Object, e As EventArgs)
        Process.Start("https://www.paypal.com/us/cgi-bin/webscr?cmd=_flow&SESSION=bHVv-EgQOJuJbqubcxMNeSwD5056s0dYJduOlbKWyek007EuA15qJfFGLYm&dispatch=5885d80a13c0db1f8e263663d3faee8d6625bf1e8bd269586d425cc639e26c6a")
    End Sub

    Private Sub donationLiveOriginal_Click(sender As Object, e As EventArgs) Handles donationLiveOriginal.Click
        Process.Start("https://www.paypal.com/us/cgi-bin/webscr?cmd=_flow&SESSION=DfyZM-HDhelG3HnVe31_Lyw_HIv9Z9NglxktnI6al3bsxpcr65JzAbWiTCO&dispatch=5885d80a13c0db1f8e263663d3faee8d6625bf1e8bd269586d425cc639e26c6a")
    End Sub

    Private Sub settingsPanel_Paint(sender As Object, e As PaintEventArgs) Handles settingsPanel.Paint

    End Sub
End Class
