Imports System.IO
Imports Microsoft.SqlServer
Imports System.Net
Imports System.Text.RegularExpressions

Module Module1
	Public CrosshairSize As Double
	Public CrosshairGap As Double
	Public CrosshairStyle As Integer
	Public CrosshairColor As Double
	Public CrosshairThickness As Double
	Public CrosshairOutline As Double
	Public CrosshairOutlineThickness As Double
	Public ViewmodelFOV As Double
	Public ViewmodelX As Double
	Public ViewmodelY As Double
	Public ViewmodelZ As Double
	Public ViewmodelBob As Double
	Public BobLat As Double
	Public BobVert As Double
	Public Righthand As Double
	Public Resolution As String
	Public CrosshairDot As Double
	Public Sens As Double
	Public CFGPath As String = My.Settings.SavedCFGPath
	Public WriteCFG As Boolean = My.Settings.WriteCFG
	Public CFGName As String = My.Settings.FileName
	Public Rand As New Random
	Public SupportedResolutions() As String = {"800x600", "1024x768", "1152x864", "1280x960", "1280x1024", "1280x720", "1600x900", "1920x1080", "1280x800", "1440x900", "1680x1050"}
	Public SupportedResolutionsSettings() As String = My.Settings.SupportedResolutionsSettings.Cast(Of String)().ToArray()
	Public ResolutionAspectRatios() As String = {"4:3", "16:9", "16:10"}
	Public ResolutionAspectRatiosUsed() As String = My.Settings.ResolutionAspectRatiosUsedList.Cast(Of String)().ToArray()
	Public Settings() As String = {"Yes", "Yes", "No", "Yes", "Yes", "Yes", "Yes", "No", "No", "No", "No", "Yes", "Yes", "Yes", "Yes", "Yes", "Yes", "Yes"}
	Public Version As String = "4.0"
	Public WipeOptions As Boolean = True
	Public CrosshairColorR As Double
	Public CrosshairColorG As Double
	Public CrosshairColorB As Double
	Public CrosshairUse5 As Boolean
	Public Height As String
	Public Width As String
	Public location As String = System.Environment.GetCommandLineArgs()(0)
	Public appName As String = System.IO.Path.GetFileName(location)
	Public DPI As Integer = My.Settings.DPI
	Public UseColor0 As Boolean = My.Settings.UseColor0
	Public UseColor1 As Boolean = My.Settings.UseColor1
	Public UseColor2 As Boolean = My.Settings.UseColor2
	Public UseColor3 As Boolean = My.Settings.UseColor3
	Public UseColor4 As Boolean = My.Settings.UseColor4
	Public UseColor5 As Boolean = My.Settings.UseColor5
	Public SettingsFile As String = System.AppDomain.CurrentDomain.BaseDirectory & "\" + appName + ".txt"
	Public VersionFile As String = System.AppDomain.CurrentDomain.BaseDirectory & "\" + appName + ".ver"
	Public UpperSens As Double = My.Settings.UpperSens
	Public LowerSens As Double = My.Settings.LowerSens
	Public DecimalChance As Double = My.Settings.DecimalChance
	Public MatchBob As Boolean = My.Settings.MatchBob
	Public CrosshairSizeMin As Double = My.Settings.CrosshairSizeMin
	Public CrosshairSizeMax As Double = My.Settings.CrosshairSizeMax
	Public CrosshairGapMin As Double = My.Settings.CrosshairGapMin
	Public CrosshairGapMax As Double = My.Settings.CrosshairGapMax
	Public CrosshairThicknessMin As Double = My.Settings.CrosshairThicknessMin
	Public CrosshairThicknessMax As Double = My.Settings.CrosshairThicknessMax
	Public CrosshairOutlineUsed As Boolean = My.Settings.CrosshairOutlineUsed
	Public CrosshairOutlineThicknessMin As Double = My.Settings.CrosshairOutlineThicknessMin
	Public CrosshairOutlineThicknessMax As Double = My.Settings.CrosshairOutlineThicknessMax
	Public CrosshairStyles() As String = My.Settings.CrosshairStyles.Cast(Of String)().ToArray()
	Public BobEnabled() As String = My.Settings.BobEnabled.Cast(Of String)().ToArray()
	Public BobEnabledIndex As Integer = My.Settings.BobEnabledIndex
	Public HandToUse() As String = My.Settings.HandToUse.Cast(Of String)().ToArray()
	Public HandToUseIndex As Integer = My.Settings.HandToUseIndex
	Public FirstTimeRun As Boolean = My.Settings.FirstTimeRun
	Public WriteCFGOnline As Boolean = My.Settings.WriteCFGOnline
	Public Name As String = My.Settings.Name

	Sub Main()
		'Setup
		If File.Exists(VersionFile) Then
			Dim CurrentVersion As String = File.ReadAllText(VersionFile)
			If CurrentVersion <> Version Then
				FirstTimeRun = True
				If WipeOptions = True Then
					File.Delete(SettingsFile)
					IO.File.WriteAllText(VersionFile, Version)
				End If
			End If
		Else
			FirstTimeRun = True
			IO.File.WriteAllText(VersionFile, Version)
		End If

		If File.Exists(SettingsFile) Then
			Settings = File.ReadAllLines(SettingsFile)
		Else
			SaveSettings()
			My.Settings.Save()
		End If

		SaveSettings()

		'Print
		Console.Clear()
		StyleConsole()
		Console.Title = "CS:GO CFG Generator"
		Console.WriteLine("Welcome To the CSGO CFG Generator. This is version: {0}", Version)
		Console.WriteLine("1. Generate")
		Console.WriteLine("2. Generate Options")
		Console.WriteLine("3. Preferences")
		Console.WriteLine("4. Set CFG Path")
		Console.WriteLine("5. Set file name")
		Console.WriteLine("6. Write To CFG's?: {0}", WriteCFG)
		Console.WriteLine("7. Update online CFG? (Uses existing one in CFG Path): {0}", WriteCFGOnline)
		Console.WriteLine("8. Name for online CFG (will overwrite existing ones): {0}", Name)
		Console.WriteLine("9. Download other configs (will overwrite existing ones)")
		Console.WriteLine("?. Secret")
		StyleConsole()
		If FirstTimeRun = True Then
			Console.ForegroundColor = ConsoleColor.Green
			Console.Write(vbCrLf + "Well, seems like now I am actually OUT of ideas/features that could benefit this program further. Other than some QoL changes such as listing the files on the server instead of having to check a web link yourself etc and limiting the amount of config files certain people can upload. Nothing new has come in this update except the new additions. Hope you enjoy! ^_^" + vbCrLf + vbCrLf + "BiZR" + vbCrLf + vbCrLf)
			Console.ResetColor()
			FirstTimeRun = False
			SaveSettings()
		End If
		Dim UserInput As String = Console.ReadLine
		Select Case UserInput
			Case 1
				Generate()
			Case 2
				GenerateSetup()
			Case 3
				Preferences()
			Case 4
				SetCFGPath()
			Case 5
				SetFileNames()
			Case 6
				If WriteCFG = False Then
					WriteCFG = True
					Main()
				Else
					WriteCFG = False
					Main()
				End If
			Case 7
				If WriteCFGOnline = False Then
					WriteCFGOnline = True
					Main()
				Else
					WriteCFGOnline = False
					Main()
				End If
			Case 8
				SetName()
			Case 9
				DownloadConfigs()
			Case 69
				Console.Clear()
				Console.WriteLine(" #####   #####  
#     # #     # 
#       #     # 
######   ###### 
#     #       # 
#     # #     # 
 #####   #####  ")
				Console.WriteLine("
#     #        #####  ####### ######  
 #   #        #     # #     # #     # 
  # #         #       #     # #     # 
   #    ##### #  #### #     # #     # 
  # #         #     # #     # #     # 
 #   #        #     # #     # #     # 
#     #        #####  ####### ######  ")
				Threading.Thread.Sleep(1000)
				MiniMain()
			Case Else
				Main()
		End Select
		Console.ReadLine()
	End Sub

	Sub SetName()
		Console.Clear()
		Console.WriteLine("Please enter the name that you would like your online configs to be called (unique names to prevent overlapping config):")
		Name = Console.ReadLine
		Main()
	End Sub

	Sub GenerateSetup()
		SaveSettings()
		My.Settings.Save()
		Console.Clear()
		StyleConsole()
		Console.WriteLine("Which values would you like to generate?")
		Console.WriteLine("Crosshair Settings:")
		Console.WriteLine("1. Size: {0}", Settings(0))
		Console.WriteLine("2. Gap: {0}", Settings(1))
		Console.WriteLine("3. Style: {0}", Settings(2))
		Console.WriteLine("4. Color: {0}", Settings(3))
		Console.WriteLine("5. Thickness: {0}", Settings(4))
		Console.WriteLine("6. Outline: {0}", Settings(5))
		Console.WriteLine("7. Outline Thickness: {0}", Settings(6))
		Console.WriteLine(vbCrLf + "Viewmodel Settings")
		Console.WriteLine("8. FOV: {0}", Settings(7))
		Console.WriteLine("9. X Offset: {0}", Settings(8))
		Console.WriteLine("10. Y Offset: {0}", Settings(9))
		Console.WriteLine("11. Z Offset: {0}", Settings(10))
		Console.WriteLine("12. Bob Amt: {0}", Settings(11))
		Console.WriteLine("13. Bob Lat: {0}", Settings(12))
		Console.WriteLine("14. Bob Vert: {0}", Settings(13))
		Console.WriteLine("15. Righthand: {0}", Settings(14))
		Console.WriteLine(vbCrLf + "Extra Options:")
		Console.WriteLine("16. Resolution: {0}", Settings(15))
		Console.WriteLine("17. Sensitivity: {0}", Settings(16))
		Console.WriteLine("18. Crosshair Dot: {0}", Settings(17))
		Console.WriteLine("19. DPI: {0}", DPI)
		Console.WriteLine(vbCrLf + "Do not enter a value to continue.")
		StyleConsole()
		Dim UserInputValid As Boolean = False
		Dim UserInput As String

		While UserInputValid = False
			Try
				UserInput = Console.ReadLine()
				If UserInput = "" Then
					UserInputValid = True
					Generate()
				End If
				Convert.ToInt32(UserInput)
				UserInputValid = True
			Catch ex As Exception
				Console.WriteLine("That is not a valid input, try again.")
			End Try
		End While

		While UserInput < 1 Or UserInput > 20
			Console.WriteLine("That is not a valid input, try again.")
			UserInput = Console.ReadLine
		End While

		If UserInput = "19" Then
			Console.Clear()
			Console.WriteLine("Set your new DPI now.")
			DPI = Console.ReadLine
			My.Settings.DPI = DPI
			GenerateSetup()
		Else
			For i = Convert.ToInt32(UserInput) To Convert.ToInt32(UserInput)
				If Settings(i - 1) = "Yes" Then
					Settings(i - 1) = "No"
				Else
					Settings(i - 1) = "Yes"
				End If
				Console.Clear()
				GenerateSetup()
			Next
		End If

		SaveSettings()
		Generate()

	End Sub

	Sub Generate()
		SaveSettings()
		Console.Clear()
		StyleConsole()
		Dim UsedResoltionsIndex() As String = SupportedResolutionsSettings

		If Not ResolutionAspectRatiosUsed(0) = "Yes" Then
			For i = 0 To 4
				UsedResoltionsIndex(i) = "No"
			Next
		End If

		If Not ResolutionAspectRatiosUsed(1) = "Yes" Then
			For i = 5 To 7
				UsedResoltionsIndex(i) = "No"
			Next
		End If

		If Not ResolutionAspectRatiosUsed(2) = "Yes" Then
			For i = 8 To 10
				UsedResoltionsIndex(i) = "No"
			Next
		End If

		Dim RandomValue1 As Integer
		Dim RandomValue2 As Integer
		Dim WriteCrosshair As Boolean = False
		Dim WriteCrosshairDone As Boolean = False
		Dim WriteViewmodel As Boolean = False
		Dim WriteViewmodelDone As Boolean = False

		For i = 0 To 17
			For j = 0 To 6
				If Settings(j) = "Yes" Then
					WriteCrosshair = True
				End If
			Next
			If WriteCrosshair = True And WriteCrosshairDone = False Then
				Console.WriteLine("Crosshair Settings:")
				WriteCrosshairDone = True
			End If
			If Settings(i) = "Yes" Then

				'Size
				If i = 0 Then
					CrosshairSize = Rand.Next(CrosshairSizeMin, CrosshairSizeMax)
					Random5050(RandomValue1, RandomValue2, CrosshairSize)
					If CrosshairSize < CrosshairSizeMin Then
						CrosshairSize = CrosshairSizeMin
					End If
					Console.WriteLine("Size: {0}", CrosshairSize)
				End If

				'Gap
				If i = 1 Then
					CrosshairGap = Rand.Next(CrosshairGapMin, CrosshairGapMax)
					Random5050(RandomValue1, RandomValue2, CrosshairGap)
					Console.WriteLine("Gap: {0}", CrosshairGap)
				End If

				'Style
				If i = 2 Then
					Dim CrosshairStyleValid As Boolean = False
					While CrosshairStyleValid = False
						CrosshairStyle = Rand.Next(0, 7)
						If CrosshairStyles(CrosshairStyle) = "Yes" Then
							CrosshairStyleValid = True
						End If
					End While
					Console.WriteLine("Style: {0}", CrosshairStyle)
				End If

				'Color
				If i = 3 Then
					Dim ValidColors(5) As String
					ValidColors(0) = UseColor0
					ValidColors(1) = UseColor1
					ValidColors(2) = UseColor2
					ValidColors(3) = UseColor3
					ValidColors(4) = UseColor4
					ValidColors(5) = UseColor5
					Dim ValidColor As Boolean = False

					While ValidColor = False
						CrosshairColor = Rand.Next(0, 6)
						If ValidColors(CrosshairColor) = True Then
							ValidColor = True
						End If
					End While

					If CrosshairColor = 5 Then
						CrosshairColorR = Rand.Next(0, 256)
						CrosshairColorG = Rand.Next(0, 256)
						CrosshairColorB = Rand.Next(0, 256)
					End If
					Console.WriteLine("Color: {0}", CrosshairColor)
					If CrosshairColor = 5 Then
						Console.WriteLine("Color R: {0}", CrosshairColorR)
						Console.WriteLine("Color G: {0}", CrosshairColorG)
						Console.WriteLine("Color B: {0}", CrosshairColorB)
					End If
				End If

				'Thickness
				If i = 4 Then
					CrosshairThickness = Rand.Next(CrosshairThicknessMin, CrosshairThicknessMax)
					Random5050(RandomValue1, RandomValue2, CrosshairThickness)
					Console.WriteLine("Thickness: {0}", CrosshairThickness)
				End If

				'Outline
				If i = 5 Then
					If CrosshairOutlineUsed = True Then
						CrosshairOutline = Rand.Next(0, 2)
					Else
						CrosshairOutline = 0
					End If
					Console.WriteLine("Outline: {0}", CrosshairOutline)
				End If

				'Outline Thickness
				If i = 6 Then
					CrosshairOutlineThickness = Rand.Next(0, 2)
					Random5050(RandomValue1, RandomValue2, CrosshairOutlineThickness)

					Console.WriteLine("Outline Thickness: {0}", CrosshairOutlineThickness)
				End If

				If i > 6 Then
					For j = 7 To 13
						If Settings(j) = "Yes" Then
							WriteViewmodel = True
						End If
					Next
					If WriteViewmodel = True And WriteViewmodelDone = False Then
						Console.WriteLine(vbCrLf + "Viewmodel Settings:")
						WriteViewmodelDone = True
					End If
				End If

				If i = 7 Then
					ViewmodelFOV = Rand.Next(54, 69)
					Console.WriteLine("FOV: {0}", ViewmodelFOV)
				End If

				If i = 8 Then
					ViewmodelX = Rand.Next(-2, 3)
					Random5050(RandomValue1, RandomValue2, ViewmodelX)
					Console.WriteLine("X Offset: {0}", ViewmodelX)
				End If

				If i = 9 Then
					ViewmodelY = Rand.Next(-2, 3)
					Random5050(RandomValue1, RandomValue2, ViewmodelY)
					Console.WriteLine("Y Offset: {0}", ViewmodelY)
				End If

				If i = 10 Then
					ViewmodelZ = Rand.Next(-2, 3)
					Random5050(RandomValue1, RandomValue2, ViewmodelZ)
					Console.WriteLine("Z Offset: {0}", ViewmodelZ)
				End If

				If i = 11 Then
					If BobEnabled(BobEnabledIndex) = "Yes" Then
						ViewmodelBob = 21
					ElseIf BobEnabled(BobEnabledIndex) = "No" Then
						ViewmodelBob = 5
					Else
						RandomValue1 = Rand.Next(0, 2)
						If RandomValue1 = "0" Then
							ViewmodelBob = 21
						Else
							ViewmodelBob = 5
						End If
					End If
					Console.WriteLine("Bob: {0}", ViewmodelBob)
				End If

				If i = 12 Then
					If BobEnabled(BobEnabledIndex) = "Yes" Then
						BobLat = 0.4
					ElseIf BobEnabled(BobEnabledIndex) = "No" Then
						BobLat = 0
					Else
						RandomValue1 = Rand.Next(0, 2)
						If RandomValue1 = "0" Then
							BobLat = 0.4
						Else
							BobLat = 0
						End If
					End If

					If Not Settings(12) = "Yes" And Settings(13) = "Yes" And MatchBob = True Then
						Console.WriteLine("Bob Lat: {0}", BobLat)
					End If
				End If

				If i = 13 Then
					If BobEnabled(BobEnabledIndex) = "Yes" Then
						BobVert = 0.25
					ElseIf BobEnabled(BobEnabledIndex) = "No" Then
						BobVert = 0
					Else
						RandomValue1 = Rand.Next(0, 2)
						If RandomValue1 = "0" Then
							BobVert = 0.25
						Else
							BobVert = 0
						End If
					End If

					If Not Settings(12) = "Yes" And Settings(13) = "Yes" And MatchBob = True Then
						Console.WriteLine("Bob Vert: {0}", BobVert)
					End If
				End If

				If Settings(12) = "Yes" And Settings(13) = "Yes" And i = 14 Then
					If MatchBob = True Then
						If BobLat = "0.4" Then
							BobVert = 0.25
						Else
							BobVert = 0
						End If
					End If
					Console.WriteLine("Bob Lat: {0}", BobLat)
					Console.WriteLine("Bob Vert: {0}", BobVert)
				End If

				If i = 14 Then
					If HandToUse(HandToUseIndex) = "Right" Then
						Righthand = 1
					ElseIf HandToUse(HandToUseIndex) = "No" Then
						Righthand = 0
					Else
						RandomValue1 = Rand.Next(0, 2)
						If RandomValue1 = "0" Then
							Righthand = 0
						Else
							Righthand = 1
						End If
					End If
					Console.WriteLine("Righthand: {0}", Righthand)
				End If

				If i = 15 Then
					Dim ChosenRes As String
					Dim ResolutionValid As Boolean = False
					While ResolutionValid = False
						RandomValue1 = Rand.Next(0, 11)
						If UsedResoltionsIndex(RandomValue1) = "Yes" Then
							ChosenRes = SupportedResolutions(RandomValue1)
							ResolutionValid = True
						End If
					End While


					Console.WriteLine(vbCrLf + "Resolution:")
					Console.WriteLine(ChosenRes)
					Console.WriteLine("")

					Height = ChosenRes.Split("x")(0)
					Width = ChosenRes.Split("x")(1)
				End If

				If i = 16 Then
					Sens = Rand.Next(LowerSens, UpperSens) / 100
					RandomValue1 = Rand.Next(0, 100)
					If RandomValue1 < DecimalChance Then
						Sens = Math.Round(Sens, 1)
						Sens = Sens + 0.05
					Else
						Sens = Math.Round(Sens, 1)
					End If
					Sens = Sens * (400 / DPI)
					Console.WriteLine("Sens: {0}", Sens)
				End If

				If i = 17 Then
					CrosshairDot = Rand.Next(0, 2)
					Console.WriteLine("Crosshair Dot: {0}", CrosshairDot)
				End If
			End If
		Next

		If WriteCFG = True Then
			Dim objStreamWriter As StreamWriter = New StreamWriter(CFGPath + "\" + CFGName + ".cfg")
			If Settings(0) = "Yes" Then
				objStreamWriter.WriteLine("cl_crosshairsize {0}", CrosshairSize)
			End If
			If Settings(1) = "Yes" Then
				objStreamWriter.WriteLine("cl_crosshairgap {0}", CrosshairGap)
			End If
			If Settings(2) = "Yes" Then
				objStreamWriter.WriteLine("cl_crosshairstyle {0}", CrosshairStyle)
			End If
			If Settings(3) = "Yes" Then
				objStreamWriter.WriteLine("cl_crosshaircolor {0}", CrosshairColor)
				If CrosshairColor = 5 Then
					objStreamWriter.WriteLine("cl_crosshaircolor_r {0}", CrosshairColorR)
					objStreamWriter.WriteLine("cl_crosshaircolor_g {0}", CrosshairColorG)
					objStreamWriter.WriteLine("cl_crosshaircolor_b {0}", CrosshairColorB)
				End If
			End If
			If Settings(4) = "Yes" Then
				objStreamWriter.WriteLine("cl_crosshairthickness {0}", CrosshairThickness)
			End If
			If Settings(5) = "Yes" Then
				objStreamWriter.WriteLine("cl_crosshair_drawoutline {0}", CrosshairOutline)
			End If
			If Settings(6) = "Yes" Then
				objStreamWriter.WriteLine("cl_crosshair_outlinethickness {0}", CrosshairOutlineThickness)
			End If
			If Settings(7) = "Yes" Then
				objStreamWriter.WriteLine("viewmodel_fov {0}", ViewmodelFOV)
			End If
			If Settings(8) = "Yes" Then
				objStreamWriter.WriteLine("viewmodel_offset_x {0}", ViewmodelX)
			End If
			If Settings(9) = "Yes" Then
				objStreamWriter.WriteLine("viewmodel_offset_y {0}", ViewmodelY)
			End If
			If Settings(10) = "Yes" Then
				objStreamWriter.WriteLine("viewmodel_offset_z {0}", ViewmodelZ)
			End If
			If Settings(11) = "Yes" Then
				objStreamWriter.WriteLine("cl_bob_lower_amt {0}", ViewmodelBob)
			End If
			If Settings(12) = "Yes" Then
				objStreamWriter.WriteLine("cl_bobamt_lat {0}", BobLat)
			End If
			If Settings(13) = "Yes" Then
				objStreamWriter.WriteLine("cl_bobamt_vert {0}", BobVert)
			End If
			If Settings(14) = "Yes" Then
				objStreamWriter.WriteLine("cl_righthand {0}", Righthand)
			End If
			If Settings(15) = "Yes" Then
				objStreamWriter.WriteLine("mat_setvideomode " + Height + " " + Width + " 0")
			End If
			If Settings(16) = "Yes" Then
				objStreamWriter.WriteLine("sensitivity {0}", Sens)
			End If
			If Settings(17) = "Yes" Then
				objStreamWriter.WriteLine("cl_crosshairdot {0}", CrosshairDot)
			End If
			objStreamWriter.Close()
		End If


		If WriteCFGOnline = True Then
			Try
				Dim FullCFG As String = IO.File.ReadAllText(CFGPath + "\" + CFGName + ".cfg")
				Dim SendData As WebRequest = WebRequest.Create("http://95.172.92.86/cfg/post.php?test=" & FullCFG & "&filename=" & Name)
				SendData.GetResponse()
				Console.WriteLine("Online CFG written successfully.")
			Catch ex As Exception
				Console.WriteLine("Failed to write to online CFG.")
			End Try
		End If

		Console.WriteLine("Do you want to generate again? Y/N")
		StyleConsole()
		Dim UserInput As String = Console.ReadLine
		While UserInput <> "Y" And UserInput <> "N"
			Console.WriteLine("That is not valid, try again.")
			UserInput = Console.ReadLine
		End While
		If UserInput = "Y" Then
			Generate()
		Else
			Main()
		End If
	End Sub

	Sub SetCFGPath()
		Console.Clear()
		StyleConsole()
		Console.WriteLine("Current path: {0}", CFGPath)
		Console.WriteLine("Do you want to change it? Y/N")
		StyleConsole()
		Dim UserInput As String = Console.ReadLine
		While UserInput <> "Y" And UserInput <> "N"
			Console.WriteLine("That is not valid, try again.")
			UserInput = Console.ReadLine
		End While
		If UserInput = "Y" Then
			Console.WriteLine("Enter your path now.")
			CFGPath = Console.ReadLine
		End If

		My.Settings.SavedCFGPath = CFGPath
		My.Settings.Save()
		Main()
	End Sub

	Sub SetFileNames()
		Console.Clear()
		StyleConsole()
		Console.WriteLine("Select the one you want to change:")
		Console.WriteLine("1. Config Name: {0}", CFGName)
		Console.WriteLine("2. Exit")
		StyleConsole()
		Dim UserInput As String = Console.ReadLine
		Select Case UserInput
			Case 1
				Console.Clear()
				Console.WriteLine("Enter your new file name now:")
				CFGName = Console.ReadLine
				My.Settings.FileName = CFGName
				My.Settings.Save()
				SetFileNames()
			Case 2
				Main()
			Case Else
				SetFileNames()
		End Select
	End Sub
	Function Random5050(ByRef RandomValue1, ByRef RandomValue2, ByRef InputVar)
		RandomValue1 = Rand.Next(0, 11)
		RandomValue2 = Rand.Next(0, 2)
		If RandomValue1 > 5 Then
			If RandomValue2 = "0" Then
				InputVar = InputVar - 0.5
			Else
				InputVar = InputVar + 0.5
			End If
		End If

		Return InputVar
	End Function

	Sub SaveSettings()
		IO.File.WriteAllLines(SettingsFile, Settings)
		My.Settings.SupportedResolutionsSettings.Clear()
		My.Settings.SupportedResolutionsSettings.AddRange(SupportedResolutionsSettings)
		My.Settings.ResolutionAspectRatiosUsedList.Clear()
		My.Settings.ResolutionAspectRatiosUsedList.AddRange(ResolutionAspectRatiosUsed)
		My.Settings.UpperSens = UpperSens
		My.Settings.LowerSens = LowerSens
		My.Settings.FileName = CFGName
		My.Settings.SavedCFGPath = CFGPath
		My.Settings.DPI = DPI
		My.Settings.UseColor0 = UseColor0
		My.Settings.UseColor1 = UseColor1
		My.Settings.UseColor2 = UseColor2
		My.Settings.UseColor3 = UseColor3
		My.Settings.UseColor4 = UseColor4
		My.Settings.UseColor5 = UseColor5
		My.Settings.DecimalChance = DecimalChance
		My.Settings.WriteCFG = WriteCFG
		My.Settings.MatchBob = MatchBob
		My.Settings.CrosshairSizeMin = CrosshairSizeMin
		My.Settings.CrosshairSizeMax = CrosshairSizeMax
		My.Settings.CrosshairGapMin = CrosshairGapMin
		My.Settings.CrosshairGapMax = CrosshairGapMax
		My.Settings.CrosshairStyles.Clear()
		My.Settings.CrosshairStyles.AddRange(CrosshairStyles)
		My.Settings.CrosshairThicknessMin = CrosshairThicknessMin
		My.Settings.CrosshairThicknessMax = CrosshairThicknessMax
		My.Settings.CrosshairOutlineUsed = CrosshairOutlineUsed
		My.Settings.CrosshairOutlineThicknessMin = CrosshairOutlineThicknessMin
		My.Settings.CrosshairOutlineThicknessMax = CrosshairOutlineThicknessMax
		My.Settings.BobEnabled.Clear()
		My.Settings.BobEnabled.AddRange(BobEnabled)
		My.Settings.BobEnabledIndex = BobEnabledIndex
		My.Settings.HandToUse.Clear()
		My.Settings.HandToUse.AddRange(HandToUse)
		My.Settings.HandToUseIndex = HandToUseIndex
		My.Settings.FirstTimeRun = FirstTimeRun
		My.Settings.WriteCFGOnline = WriteCFGOnline
		My.Settings.Name = Name
		My.Settings.Save()
	End Sub

	Sub Preferences()
		SaveSettings()
		Console.Clear()
		StyleConsole()
		Console.WriteLine("Which preferences would you like to change?")
		Console.WriteLine("1. Color Whitelist")
		Console.WriteLine("2. Resolution Whitelist")
		Console.WriteLine("3. Sensitivity Bounds/Decimal Chances")
		Console.WriteLine("4. Viewmodel Presets")
		Console.WriteLine("5. All Other Crosshair Settings")
		Console.WriteLine("6. Back to Menu")
		StyleConsole()
		Dim UserInput As String
		UserInput = Console.ReadLine
		Select Case UserInput
			Case 1
				UseColors()
			Case 2
				ResolutionWhitelist()
			Case 3
				SensLimits()
			Case 4
				ViewmodelPresets()
			Case 5
				CrosshairBounds()
			Case 6
				Main()
			Case Else
				Preferences()
		End Select
	End Sub

	Sub UseColors()
		SaveSettings()
		Console.Clear()
		StyleConsole()
		Console.WriteLine("Which colours would you like to use?")
		Console.WriteLine("1. Color 0: {0}", UseColor0)
		Console.WriteLine("2. Color 1: {0}", UseColor1)
		Console.WriteLine("3. Color 2: {0}", UseColor2)
		Console.WriteLine("4. Color 3: {0}", UseColor3)
		Console.WriteLine("5. Color 4: {0}", UseColor4)
		Console.WriteLine("6. Color 5: {0}", UseColor5)
		Console.WriteLine("7. Back to Preferences")
		StyleConsole()
		Dim UserInput As String
		UserInput = Console.ReadLine
		Select Case UserInput
			Case 1
				FlipFlopBoolean(UseColor0)
				UseColors()
			Case 2
				FlipFlopBoolean(UseColor1)
				UseColors()
			Case 3
				FlipFlopBoolean(UseColor2)
				UseColors()
			Case 4
				FlipFlopBoolean(UseColor3)
				UseColors()
			Case 5
				FlipFlopBoolean(UseColor4)
				UseColors()
			Case 6
				FlipFlopBoolean(UseColor5)
				UseColors()
			Case 7
				Preferences()
			Case Else
				UseColors()
		End Select
	End Sub

	Sub ResolutionWhitelist()
		SaveSettings()
		Console.Clear()
		StyleConsole()
		Console.WriteLine("Please choose an option.")
		Console.WriteLine("1. Enabled Aspect Ratios")
		Console.WriteLine("2. Enabled Resolutions")
		Console.WriteLine("3. Back to Preferences")
		StyleConsole()
		Dim UserInput As String = Console.ReadLine
		Select Case UserInput
			Case 1
				AspectRatio()
			Case 2
				Resolutions()
			Case 3
				Preferences()
			Case Else
				ResolutionWhitelist()
		End Select
	End Sub

	Sub AspectRatio()
		SaveSettings()
		Console.Clear()
		StyleConsole()
		Console.WriteLine("1. 4:3 Enabled?: {0}", ResolutionAspectRatiosUsed(0))
		Console.WriteLine("2. 16:9 Enabled?: {0}", ResolutionAspectRatiosUsed(1))
		Console.WriteLine("3. 16:10 Enabled?: {0}", ResolutionAspectRatiosUsed(2))
		Console.WriteLine("4. Back to Resolutions")
		StyleConsole()
		Dim UserInput As String = Console.ReadLine
		Select Case UserInput
			Case 1
				If ResolutionAspectRatiosUsed(0) = "Yes" Then
					ResolutionAspectRatiosUsed(0) = "No"
				Else
					ResolutionAspectRatiosUsed(0) = "Yes"
				End If
				AspectRatio()
			Case 2
				If ResolutionAspectRatiosUsed(1) = "Yes" Then
					ResolutionAspectRatiosUsed(1) = "No"
				Else
					ResolutionAspectRatiosUsed(1) = "Yes"
				End If
				AspectRatio()
			Case 3
				If ResolutionAspectRatiosUsed(2) = "Yes" Then
					ResolutionAspectRatiosUsed(2) = "No"
				Else
					ResolutionAspectRatiosUsed(2) = "Yes"
				End If
				AspectRatio()
			Case 4
				ResolutionWhitelist()
			Case Else
				AspectRatio()
		End Select
	End Sub

	Sub Resolutions()
		SaveSettings()
		Console.Clear()
		StyleConsole()

		Console.WriteLine("You can toggle them by entering their respective numbers. (This list is pulled from aspect ratios you have enabled).")
		If ResolutionAspectRatiosUsed(0) = "Yes" Then
			For i = 0 To 4
				Console.WriteLine("{0}. {1} Enabled?: {2}", i + 1, SupportedResolutions(i), SupportedResolutionsSettings(i))
			Next
		Else
			For i = 0 To 4
			Next
		End If

		If ResolutionAspectRatiosUsed(1) = "Yes" Then
			For i = 5 To 7
				Console.WriteLine("{0}. {1} Enabled?: {2}", i + 1, SupportedResolutions(i), SupportedResolutionsSettings(i))
			Next
		Else
			For i = 5 To 7
			Next
		End If

		If ResolutionAspectRatiosUsed(2) = "Yes" Then
			For i = 8 To 10
				Console.WriteLine("{0}. {1} Enabled?: {2}", i + 1, SupportedResolutions(i), SupportedResolutionsSettings(i))
			Next
		Else
			For i = 8 To 10
			Next
		End If

		Console.WriteLine("12. Back to Preferences")
		StyleConsole()
		Dim UserInput As String = Console.ReadLine
		Dim UserInputInt As Integer
		Try
			UserInputInt = Convert.ToInt32(UserInput)
			UserInputInt = UserInputInt - 1
			If UserInputInt > 11 Or UserInputInt < 0 Then
				Console.WriteLine("That is not a valid number, resetting.")
				Console.ReadLine()
				Resolutions()
			End If
		Catch ex As Exception
			Console.WriteLine("That is not a valid number, resetting.")
			Console.ReadLine()
			Resolutions()
		End Try
		If UserInputInt = 11 Then
			Preferences()
		End If
		If UserInputInt <> 11 And SupportedResolutionsSettings(UserInputInt) = "Yes" Then
			SupportedResolutionsSettings(UserInputInt) = "No"
		Else
			SupportedResolutionsSettings(UserInputInt) = "Yes"
		End If
		Resolutions()
	End Sub

	Sub SensLimits()
		SaveSettings()
		Console.Clear()
		StyleConsole()
		Console.WriteLine("Would you like to set the upper or lower bounds?")
		Console.WriteLine("1. Upper Sens: {0}", UpperSens / 100)
		Console.WriteLine("2. Lower Sens: {0}", LowerSens / 100)
		Console.WriteLine("3. Chance of two decimal places (as % of 100): {0}", DecimalChance)
		Console.WriteLine("4. Back to Preferences")
		StyleConsole()
		Dim UserInput As String
		UserInput = Console.ReadLine
		Select Case UserInput
			Case 1
				Console.Clear()
				Console.WriteLine("What would you like the upper bounds to be?")
				UserInput = Console.ReadLine
				Try
					Convert.ToDouble(UserInput)
					UpperSens = (Convert.ToDouble(UserInput) * 100)
					SensLimits()
				Catch ex As Exception
					Console.WriteLine("That is not a valid number, resetting.")
					Console.ReadLine()
					SensLimits()
				End Try
			Case 2
				Console.Clear()
				Console.WriteLine("What would you like the lower bounds to be?")
				UserInput = Console.ReadLine
				Try
					Convert.ToDouble(UserInput)
					LowerSens = (Convert.ToDouble(UserInput) * 100)
					SensLimits()
				Catch ex As Exception
					Console.WriteLine("That is not a valid number, resetting.")
					Console.ReadLine()
					SensLimits()
				End Try
			Case 3
				Console.Clear()
				Console.WriteLine("What would you like the decimal chance to be?")
				UserInput = Console.ReadLine
				Try
					Convert.ToInt32(UserInput)
					If UserInput > "99" Or UserInput < "1" Then
						Console.WriteLine("That is not valid, resetting.")
						SensLimits()
					Else
						DecimalChance = (Convert.ToInt32(UserInput))
						SensLimits()
					End If
				Catch ex As Exception
					Console.WriteLine("That is not a valid number, resetting.")
					Console.ReadLine()
					SensLimits()
				End Try
			Case 4
				Preferences()
			Case Else
				SensLimits()
		End Select
	End Sub

	Sub ViewmodelPresets()
		SaveSettings()
		Console.Clear()
		StyleConsole()
		Console.WriteLine("What would you like to change?")
		Console.WriteLine("1. Match Bob: {0} (prevents things like bob lat being 0 and bob vert being 0.25).", MatchBob)
		Console.WriteLine("2. Bob Enabled?: {0}", BobEnabled(BobEnabledIndex))
		Console.WriteLine("3. Which hand?: {0}", HandToUse(HandToUseIndex))
		Console.WriteLine("4. Back to Preferences")
		StyleConsole()

		Dim UserInput As String = Console.ReadLine
		Select Case UserInput
			Case 1
				If MatchBob = True Then
					MatchBob = False
				Else
					MatchBob = True
				End If
				ViewmodelPresets()
			Case 2
				If BobEnabledIndex + 1 = 3 Then
					BobEnabledIndex = 0
				Else
					BobEnabledIndex = BobEnabledIndex + 1
				End If
				ViewmodelPresets()
			Case 3
				If HandToUseIndex + 1 = 3 Then
					HandToUseIndex = 0
				Else
					HandToUseIndex = HandToUseIndex + 1
				End If
				ViewmodelPresets()
			Case 4
				Preferences()
			Case Else
				ViewmodelPresets()
		End Select
	End Sub

	Sub CrosshairBounds()
		SaveSettings()
		Console.Clear()
		StyleConsole()
		Console.WriteLine("These values are mostly used to determine the minimum and maximum values that can be generated at the start. There is a good chance they will be 0.5 over the threshold most times.")
		Console.WriteLine("1. Crosshair Size Min: {0}", CrosshairSizeMin)
		Console.WriteLine("2. Crosshair Size Max: {0}", CrosshairSizeMax)
		Console.WriteLine("3. Crosshair Gap Min: {0}", CrosshairGapMin)
		Console.WriteLine("4. Crosshair Gap Max: {0}", CrosshairGapMax)
		Console.WriteLine("5. Crosshair Thickness Min: {0}", CrosshairThicknessMin)
		Console.WriteLine("6. Crosshair Thickness Max: {0}", CrosshairThicknessMax)
		Console.WriteLine("7. Crosshair Enable Outline? (chance of crosshair using outline?): {0}", CrosshairOutlineUsed)
		Console.WriteLine("8. Crosshair Outline Thickness Min: {0}", CrosshairOutlineThicknessMin)
		Console.WriteLine("9. Crosshair Outline Thickness Max: {0}", CrosshairOutlineThicknessMax)
		Console.WriteLine("10. Enabled Styles")
		Console.WriteLine("11. Back to Preferences")
		StyleConsole()
		Dim UserInput As String = Console.ReadLine
		Select Case UserInput
			Case 1
				Console.Clear()
				Console.WriteLine("Please enter your new value now. (Crosshair Size Min)")
				UserInput = Console.ReadLine
				Try
					CrosshairSizeMin = Convert.ToDouble(UserInput)
					Console.Clear()
					Console.WriteLine("New value of {0} set. (Crosshair Size Min)", CrosshairSizeMin)
				Catch ex As Exception
					Console.WriteLine("That is not a valid number, resetting.")
				End Try
			Case 2
				Console.Clear()
				Console.WriteLine("Please enter your new value now. (Crosshair Size Max)")
				UserInput = Console.ReadLine
				Try
					CrosshairSizeMax = Convert.ToDouble(UserInput)
					Console.Clear()
					Console.WriteLine("New value of {0} set. (Crosshair Size Max)", CrosshairSizeMax)
				Catch ex As Exception
					Console.WriteLine("That is not a valid number, resetting.")
				End Try
			Case 3
				Console.Clear()
				Console.WriteLine("Please enter your new value now. (Crosshair Gap Min)")
				UserInput = Console.ReadLine
				Try
					CrosshairGapMin = Convert.ToDouble(UserInput)
					Console.Clear()
					Console.WriteLine("New value of {0} set. (Crosshair Gap Min)", CrosshairGapMin)
				Catch ex As Exception
					Console.WriteLine("That is not a valid number, resetting.")
				End Try
			Case 4
				Console.Clear()
				Console.WriteLine("Please enter your new value now. (Crosshair Gap Max)")
				UserInput = Console.ReadLine
				Try
					CrosshairGapMax = Convert.ToDouble(UserInput)
					Console.Clear()
					Console.WriteLine("New value of {0} set. (Crosshair Gap Max)", CrosshairGapMax)
				Catch ex As Exception
					Console.WriteLine("That is not a valid number, resetting.")
				End Try
			Case 5
				Console.Clear()
				Console.WriteLine("Please enter your new value now. (Crosshair Thickness Min)")
				UserInput = Console.ReadLine
				Try
					CrosshairThicknessMin = Convert.ToDouble(UserInput)
					Console.Clear()
					Console.WriteLine("New value of {0} set. (Crosshair Thickness Min)", CrosshairThicknessMin)
				Catch ex As Exception
					Console.WriteLine("That is not a valid number, resetting.")
				End Try
			Case 6
				Console.Clear()
				Console.WriteLine("Please enter your new value now. (Crosshair Thickness Max)")
				UserInput = Console.ReadLine
				Try
					CrosshairThicknessMax = Convert.ToDouble(UserInput)
					Console.Clear()
					Console.WriteLine("New value of {0} set. (Crosshair Thickness Max)", CrosshairThicknessMax)
				Catch ex As Exception
					Console.WriteLine("That is not a valid number, resetting.")
				End Try
			Case 7
				If CrosshairOutlineUsed = True Then
					CrosshairOutlineUsed = False
				Else
					CrosshairOutlineUsed = True

				End If
			Case 8
				Console.Clear()
				Console.WriteLine("Please enter your new value now. (Crosshair Outline Thickness Min)")
				UserInput = Console.ReadLine
				Try
					CrosshairOutlineThicknessMin = Convert.ToDouble(UserInput)
					Console.Clear()
					Console.WriteLine("New value of {0} set. (Crosshair Outline Thickness Min)", CrosshairOutlineThicknessMin)
				Catch ex As Exception
					Console.WriteLine("That is not a valid number, resetting.")
				End Try
			Case 9
				Console.Clear()
				Console.WriteLine("Please enter your new value now. (Crosshair Outline Thickness Max)")
				UserInput = Console.ReadLine
				Try
					CrosshairOutlineThicknessMax = Convert.ToDouble(UserInput)
					Console.Clear()
					Console.WriteLine("New value of {0} set. (Crosshair Outline Thickness Max)", CrosshairOutlineThicknessMax)
				Catch ex As Exception
					Console.WriteLine("That is not a valid number, resetting.")
				End Try
			Case 10
				Dim InputValid As Boolean = False
				While InputValid = False
					Console.Clear()
					StyleConsole()
					Console.WriteLine("Please enter the number of the style you would like to switch now.")
					For i = 0 To CrosshairStyles.Length - 1
						Console.WriteLine("{0}. Crosshair Style {1}: {2}", i + 1, i, CrosshairStyles(i))
					Next
					Console.WriteLine("8. Back to Crosshair Settings")
					StyleConsole()
					UserInput = Console.ReadLine
					Select Case UserInput
						Case 1 To 7
							If CrosshairStyles(UserInput - 1) = "Yes" Then
								CrosshairStyles(UserInput - 1) = "No"
							Else
								CrosshairStyles(UserInput - 1) = "Yes"
							End If
						Case 8
							CrosshairBounds()
					End Select
					SaveSettings()
				End While
			Case 11
				Preferences()
			Case Else
				CrosshairBounds()
		End Select
		Console.ReadLine()
		CrosshairBounds()
	End Sub

	Sub StyleConsole()
		For i = 0 To Console.WindowWidth - 1
			Console.Write("=")
		Next
	End Sub

	Sub DownloadConfigs()
		Console.Clear()
		StyleConsole()
		Console.WriteLine("Welcome to the CFG downloader, enter the name of the CFG you want to download now.")
		Console.WriteLine("You can find a list of them here: http://95.172.92.86/cfg/")
		Console.WriteLine("The .cfg part is not required. It will be removed automatically if you add it.")
		StyleConsole()

		Dim FileToDownload As String = Console.ReadLine

		If FileToDownload.Contains(".cfg") = True Then
			FileToDownload = FileToDownload.Replace(".cfg", "")
		End If

		Dim Client As WebClient = New WebClient()

		Try
			Dim Reader As StreamReader = New StreamReader(Client.OpenRead("http://95.172.92.86/cfg/" + FileToDownload + ".cfg"))
			Dim DisplayCFG As String = Reader.ReadToEnd
			Dim objStreamWriter As StreamWriter = New StreamWriter(CFGPath + "\" + FileToDownload + ".cfg")
			objStreamWriter.Write(DisplayCFG)
			objStreamWriter.Close()
			Console.Clear()
			StyleConsole()
			Console.WriteLine("This is the config:")
			Console.WriteLine(DisplayCFG)
			Console.WriteLine("Press enter when you are ready to return to the main menu.")
			StyleConsole()
			Console.ReadLine()
			Main()
		Catch ex As Exception
			Console.WriteLine("File failed to download. Press enter to return to the main menu.")
			Console.ReadLine()
			Main()
		End Try
	End Sub

	Function FlipFlopBoolean(ByRef InputValue)
		If InputValue = True Then
			InputValue = False
		Else
			InputValue = True
		End If
		Return InputValue
	End Function

	Public Team1 As String
	Public Team2 As String
	Public Team1Score As Integer
	Public Team2Score As Integer
	Public RandomWinner As Integer = Rand.Next(0, 101)
	Public HalfTime As Boolean = True
	Public KnifeRound As Boolean = True
	Public MapPool(6) As String
	Public ScrambledMapPool(6) As String
	Public SideCT As String
	Public SideT As String
	Public SideCTPercent As Integer
	Public SideTPercent As Integer
	Public Team1Side As String
	Public Team2Side As String
	Public BestOf As Integer
	Public Team1Maps As Integer
	Public Team2Maps As Integer
	Public Map1Score As String
	Public Map2Score As String
	Public Map3Score As String
	Public Map1Winner As String
	Public Map2Winner As String
	Public Map3Winner As String
	Public Map1Loser As String
	Public Map2Loser As String
	Public Map3Loser As String
	Public ChooseMaps As Boolean = False


	Public Sub MiniMain()
		Console.Clear()
		StyleConsole()
		RandomWinner = Rand.Next(0, 101)
		Team1Score = 0
		Team2Score = 0
		Team1 = ""
		Team2 = ""
		Team1Maps = 0
		Team2Maps = 0
		Map1Score = ""
		Map2Score = ""
		Map3Score = ""
		Map1Winner = ""
		Map2Winner = ""
		Map3Winner = ""
		Map1Loser = ""
		Map2Loser = ""
		Map3Loser = ""
		MapPool(0) = "Cache"
		MapPool(1) = "Cobblestone"
		MapPool(2) = "Inferno"
		MapPool(3) = "Mirage"
		MapPool(4) = "Nuke"
		MapPool(5) = "Overpass"
		MapPool(6) = "Train"
		If ChooseMaps = False Then
			Array.Clear(ScrambledMapPool, 0, ScrambledMapPool.Length)
			For i = 0 To 6
				Dim RandomMap As Integer
				RandomMap = Rand.Next(0, 7)
				While ScrambledMapPool.Contains(MapPool(RandomMap))
					RandomMap = Rand.Next(0, 7)
				End While
				ScrambledMapPool(i) = MapPool(RandomMap)
			Next
		End If

		Console.WriteLine("OH HELLO THERE!" & vbCrLf & "This is my score generator for CSGO, not too advanced but enjoyable ;D glad you found it!" & vbCrLf & "Enter your choice now:")
		Console.WriteLine("1. Setup Game")
		Console.WriteLine("2. Configure Options")
		Console.WriteLine("3. Select Maps")
		Console.WriteLine("4. Manual Maps?: {0}", ChooseMaps)
		Console.WriteLine("5. Back to CFG Generator")
		StyleConsole()
		Dim UserChoice As String = Console.ReadLine
		Select Case UserChoice
			Case 1
				GameSetup()
			Case 2
				Configure()
			Case 3
				Maps()
			Case 4
				If ChooseMaps = True Then
					ChooseMaps = False
				Else
					ChooseMaps = True
				End If
				MiniMain()
			Case 5
				Main()
			Case Else
				MiniMain()
		End Select
	End Sub
	Sub Configure()
		Console.Clear()
		Console.WriteLine("1. Half Time: {0}", HalfTime)
		Console.WriteLine("2. Knife Round: {0}", KnifeRound)
		Console.WriteLine("9. Home")

		Dim UserChoice As String = Console.ReadLine
		Select Case UserChoice
			Case 1
				If HalfTime = True Then
					HalfTime = False
				Else
					HalfTime = True
				End If
				Configure()
			Case 2
				If KnifeRound = True Then
					KnifeRound = False
				Else
					KnifeRound = True
				End If
				Configure()
			Case 9
				MiniMain()
			Case Else
				Configure()
		End Select
	End Sub
	Sub GameSetup()
		Console.Clear()
		Console.WriteLine("Is overtime possible in the game you want to predict? Y/N")
		Dim Overtime As String = Console.ReadLine
		While Overtime <> "Y" And Overtime <> "N"
			Console.Clear()
			Console.WriteLine("That is not valid, please try again now.")
			Console.WriteLine("Is overtime possible in the game you want to predict? Y/N")
			Overtime = Console.ReadLine
		End While
		Console.Clear()
		Console.WriteLine("Please enter the name of the first team now.")
		Team1 = Console.ReadLine
		Console.WriteLine("Please enter the name of the second team now.")
		Team2 = Console.ReadLine
		Console.Clear()
		Console.WriteLine("Do you want a BO1 or BO3?")
		Console.WriteLine("1. BO1")
		Console.WriteLine("2. BO3")
		BestOf = Console.ReadLine
		While BestOf <> "1" And BestOf <> "2"
			Console.WriteLine("That is not valid, please try again now.")
			Console.WriteLine("Do you want a BO1 or BO3?")
			Console.WriteLine("1. BO1")
			Console.WriteLine("2. BO3")
			BestOf = Console.ReadLine
		End While
		If BestOf = 1 Then
			BestOf = 0
		Else
			BestOf = 2
		End If
		If Overtime = "Y" Then
			OvertimeTrue()
		End If

		If Overtime = "N" Then
			OvertimeFalse()
		End If
	End Sub
	Sub OvertimeTrue()
		MapPool(0) = "Cache"
		MapPool(1) = "Cobblestone"
		MapPool(2) = "Inferno"
		MapPool(3) = "Mirage"
		MapPool(4) = "Nuke"
		MapPool(5) = "Overpass"
		MapPool(6) = "Train"
		If ChooseMaps = False Then
			Array.Clear(ScrambledMapPool, 0, ScrambledMapPool.Length)
			For i = 0 To 6
				Dim RandomMap As Integer
				RandomMap = Rand.Next(0, 7)
				While ScrambledMapPool.Contains(MapPool(RandomMap))
					RandomMap = Rand.Next(0, 7)
				End While
				ScrambledMapPool(i) = MapPool(RandomMap)
			Next
		End If
		Dim StartingSide As String = "0"
		Console.Clear()
		Console.WriteLine("What format will the overtime be?" & vbCrLf & "1. MR6" & vbCrLf & "2. MR10")
		Dim OvertimeFormat As String = Console.ReadLine
		While OvertimeFormat <> "1" And OvertimeFormat <> "2"
			Console.WriteLine("That is not valid, please try again now.")
			OvertimeFormat = Console.ReadLine
		End While

		For MapsPlayed As Integer = 0 To BestOf
			Team1Score = 0
			Team2Score = 0
			Console.Clear()
			Console.WriteLine("The map is: {0}", ScrambledMapPool(MapsPlayed))
			If BestOf > 0 Then
				Console.WriteLine("Map {0} / 3", MapsPlayed + 1)
				If MapsPlayed = 0 Then
					Console.WriteLine("Map 1: {0}, Map 2: {1}, Map 3: {2}", ScrambledMapPool(0), ScrambledMapPool(1), ScrambledMapPool(2))
				End If
				If MapsPlayed = 1 Then
					Console.ForegroundColor = ConsoleColor.Red
					Console.Write("Map 1: {0}, ", ScrambledMapPool(0))
					Console.ForegroundColor = ConsoleColor.Gray
					Console.Write("Map 2: {0}, Map 3: {1}", ScrambledMapPool(1), ScrambledMapPool(2))
				End If
				If MapsPlayed = 2 Then
					Console.ForegroundColor = ConsoleColor.Red
					Console.Write("Map 1: {0}, Map 2: {1}, ", ScrambledMapPool(0), ScrambledMapPool(1))
					Console.ForegroundColor = ConsoleColor.Gray
					Console.Write("Map 3: {0}", ScrambledMapPool(2))
				End If
				Console.WriteLine("")
				Console.WriteLine("Series Score: {0}: {1} - {2}: {3}", Team1, Team1Maps, Team2, Team2Maps)
			End If
			If ScrambledMapPool(MapsPlayed) = "Cache" Then
				SideCTPercent = 54
				SideTPercent = 46
			End If

			If ScrambledMapPool(MapsPlayed) = "Inferno" Then
				SideCTPercent = 49
				SideTPercent = 51
			End If

			If ScrambledMapPool(MapsPlayed) = "Cobblestone" Then
				SideCTPercent = 52
				SideTPercent = 48
			End If

			If ScrambledMapPool(MapsPlayed) = "Mirage" Then
				SideCTPercent = 54
				SideTPercent = 46
			End If

			If ScrambledMapPool(MapsPlayed) = "Nuke" Then
				SideCTPercent = 60
				SideTPercent = 40
			End If

			If ScrambledMapPool(MapsPlayed) = "Overpass" Then
				SideCTPercent = 57
				SideTPercent = 43
			End If

			If ScrambledMapPool(MapsPlayed) = "Train" Then
				SideCTPercent = 70
				SideTPercent = 30
			End If

			If KnifeRound = True Then
				RandomWinner = Rand.Next(0, 101)
				If RandomWinner >= 50 Then
					Console.WriteLine("{0} have won the knife round.", Team1)
					If SideCTPercent > SideTPercent Then
						Console.WriteLine("{0} have went with the CT side.", Team1)
						SideCT = Team1
						SideT = Team2
						Team1Side = "CT"
						Team2Side = "T"
					Else
						Console.WriteLine("{0} have went with the T side.", Team1)
						SideT = Team1
						SideCT = Team2
						Team2Side = "CT"
						Team1Side = "T"
					End If
				Else
					Console.WriteLine("{0} have won the knife round.", Team2)
					If SideCTPercent > SideTPercent Then
						Console.WriteLine("{0} have went with the CT side.", Team2)
						SideCT = Team2
						SideT = Team1
						Team2Side = "CT"
						Team1Side = "T"
					Else
						Console.WriteLine("{0} have went with the T side.", Team2)
						SideT = Team2
						SideCT = Team1
						Team1Side = "CT"
						Team2Side = "T"
					End If
					SideCT = Team2
					SideT = Team1
				End If
			Else
				Console.WriteLine("Select the starting side of {0} now.", Team1)
				Console.WriteLine("1. CT")
				Console.WriteLine("2. T")
				StartingSide = Console.ReadLine
				While StartingSide <> "1" And StartingSide <> "2"
					Console.WriteLine("That is not valid, try again.")
					StartingSide = Console.ReadLine
				End While
				If StartingSide = "1" Then
					SideCT = Team1
					Team1Side = "CT"
					SideT = Team2
					Team2Side = "T"
				Else
					SideT = Team1
					Team1Side = "T"
					SideCT = Team2
					Team2Side = "CT"
				End If
			End If

			System.Threading.Thread.Sleep(3000)
			Dim LO3 As Integer = 3
			Dim Countdown
			For Countdown = 1 To 3
				Console.WriteLine("Going live in {0}...", LO3)
				LO3 = LO3 - 1
				System.Threading.Thread.Sleep(1000)
			Next

			Console.Clear()


			If OvertimeFormat = "1" Then
				'Winning Scores
				Dim WinningScores(10) As Integer
				WinningScores(0) = "16"
				For WinningScoresLoop As Integer = 1 To 10 Step 1
					WinningScores(WinningScoresLoop) = 16 + (3 * WinningScoresLoop)
				Next

				'Max Rounds // DEPRECATED
				Dim MaxRounds(10) As Integer
				MaxRounds(0) = "30"
				For WinningScoresLoop As Integer = 1 To 10 Step 1
					MaxRounds(WinningScoresLoop) = 30 + (6 * WinningScoresLoop)
				Next

				'Half times
				Dim HalfTimes(10) As Integer
				HalfTimes(0) = "16"
				For WinningScoresLoop As Integer = 1 To 10 Step 1
					HalfTimes(WinningScoresLoop) = 28 + (3 * WinningScoresLoop)
				Next

				'Start game
				Dim i As Integer = 0
				Do Until WinningScores.Contains(Team1Score) AndAlso Team2Score < Team1Score - 1 Or WinningScores.Contains(Team2Score) AndAlso Team1Score < Team2Score - 1
					i = i + 1
					'Wait between round beginnings and endings
					If i > 1 Then
						Console.WriteLine("")
						System.Threading.Thread.Sleep(Rand.Next(500, 2001))
					End If
					'Halftime logic
					If HalfTimes.Contains(i) Then
						If SideCT = Team1 And SideT = Team2 Then
							SideCT = Team2
							SideT = Team1
							Team1Side = "T"
							Team2Side = "CT"
						Else
							SideCT = Team1
							SideT = Team2
							Team1Side = "CT"
							Team2Side = "T"
						End If
					End If
					If HalfTimes.Contains(i) And HalfTime = True Then
						Dim Timer As Integer = 5
						System.Threading.Thread.Sleep(Rand.Next(500, 1501))
						Console.Clear()
						Console.WriteLine("Breaking for half time...")
						System.Threading.Thread.Sleep(Rand.Next(500, 1501))
						For HalfTimeCount = 1 To 5
							Console.WriteLine("Resuming in {0}...", Timer)
							System.Threading.Thread.Sleep(1000)
							Timer = Timer - 1
							If HalfTimeCount = 5 Then
								Console.Clear()
							End If
						Next
						Console.WriteLine("The score is currently ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
						System.Threading.Thread.Sleep(Rand.Next(500, 1501))
					End If
					'Begin round + round mechanics
					RandomWinner = Rand.Next(0, 101)
					If Team1Score = 45 And Team2Score < 45 Then
						RandomWinner = 100
					End If
					If Team2Score = 45 And Team1Score < 45 Then
						RandomWinner = 0
					End If
					Console.WriteLine("Round {0} ({1}) has begun...", i, ScrambledMapPool(MapsPlayed))
					System.Threading.Thread.Sleep(Rand.Next(1000, 2001))
					If RandomWinner >= SideTPercent Then
						If SideCT = Team1 Then
							Team1Score = Team1Score + 1
							Console.WriteLine("{0} has taken the round.", Team1)
							Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
						Else
							Team2Score = Team2Score + 1
							Console.WriteLine("{0} has taken the round.", Team2)
							Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
						End If
					Else
						If SideT = Team1 Then
							Team1Score = Team1Score + 1
							Console.WriteLine("{0} has taken the round.", Team1)
							Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
						Else
							Team2Score = Team2Score + 1
							Console.WriteLine("{0} has taken the round.", Team2)
							Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
						End If
					End If
				Loop
				If BestOf = 0 Then
					System.Threading.Thread.Sleep(Rand.Next(500, 1501))
					DetermWin()
				End If
				System.Threading.Thread.Sleep(Rand.Next(1500, 3001))
				If Team1Score > Team2Score Then
					Team1Maps = Team1Maps + 1
					Console.WriteLine("{0} have took Map {1}", Team1, ScrambledMapPool(MapsPlayed))
				Else
					Team2Maps = Team2Maps + 1
					Console.WriteLine("{0} have took Map {1}", Team2, ScrambledMapPool(MapsPlayed))
				End If

				If MapsPlayed = 0 And Team1Score > Team2Score Then
					Map1Winner = Team1
					Map1Loser = Team2
					Map1Score = String.Join(" - ", Team1Score, Team2Score)
				End If

				If MapsPlayed = 0 And Team1Score < Team2Score Then
					Map1Winner = Team2
					Map1Loser = Team1
					Map1Score = String.Join(" - ", Team2Score, Team1Score)
				End If

				If MapsPlayed = 1 And Team1Score > Team2Score Then
					Map2Winner = Team1
					Map2Loser = Team2
					Map2Score = String.Join(" - ", Team1Score, Team2Score)
				End If

				If MapsPlayed = 1 And Team1Score < Team2Score Then
					Map2Winner = Team2
					Map2Loser = Team1
					Map2Score = String.Join(" - ", Team2Score, Team1Score)
				End If

				If MapsPlayed = 2 And Team1Score > Team2Score Then
					Map3Winner = Team1
					Map3Loser = Team2
					Map3Score = String.Join(" - ", Team1Score, Team2Score)
				End If

				If MapsPlayed = 2 And Team1Score < Team2Score Then
					Map3Winner = Team2
					Map3Loser = Team1
					Map3Score = String.Join(" - ", Team2Score, Team1Score)
				End If

				If Team1Maps = 2 Then
					System.Threading.Thread.Sleep(Rand.Next(1500, 3001))
					Exit For
				End If
				If Team2Maps = 2 Then
					System.Threading.Thread.Sleep(Rand.Next(1500, 3001))
					Exit For
				End If
				System.Threading.Thread.Sleep(Rand.Next(3000, 6001))
			Else
				'Winning Scores
				Dim WinningScores(10) As Integer
				WinningScores(0) = "16"
				For WinningScoresLoop As Integer = 1 To 10 Step 1
					WinningScores(WinningScoresLoop) = 16 + (5 * WinningScoresLoop)
				Next

				'Max Rounds // DEPRECATED
				Dim MaxRounds(10) As Integer
				MaxRounds(0) = "30"
				For WinningScoresLoop As Integer = 1 To 10 Step 1
					MaxRounds(WinningScoresLoop) = 30 + (10 * WinningScoresLoop)
				Next

				'Half times
				Dim HalfTimes(10) As Integer
				HalfTimes(0) = "16"
				For WinningScoresLoop As Integer = 1 To 10 Step 1
					HalfTimes(WinningScoresLoop) = 26 + (5 * WinningScoresLoop)
				Next

				'Start game
				Dim i As Integer = 0
				Do Until WinningScores.Contains(Team1Score) AndAlso Team2Score < Team1Score - 1 Or WinningScores.Contains(Team2Score) AndAlso Team1Score < Team2Score - 1
					i = i + 1
					'Wait between round beginnings and endings
					If i > 1 Then
						Console.WriteLine("")
						System.Threading.Thread.Sleep(Rand.Next(500, 2001))
					End If
					'Halftime logic
					If HalfTimes.Contains(i) Then
						If SideCT = Team1 And SideT = Team2 Then
							SideCT = Team2
							SideT = Team1
							Team1Side = "T"
							Team2Side = "CT"
						Else
							SideCT = Team1
							SideT = Team2
							Team1Side = "CT"
							Team2Side = "T"
						End If
					End If
					If HalfTimes.Contains(i) And HalfTime = True Then
						Dim Timer As Integer = 5
						System.Threading.Thread.Sleep(Rand.Next(500, 1501))
						Console.Clear()
						Console.WriteLine("Breaking for half time...")
						System.Threading.Thread.Sleep(Rand.Next(500, 1501))
						For HalfTimeCount = 1 To 5
							Console.WriteLine("Resuming in {0}...", Timer)
							System.Threading.Thread.Sleep(1000)
							Timer = Timer - 1
							If HalfTimeCount = 5 Then
								Console.Clear()
							End If
						Next
						Console.WriteLine("The score is currently ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
						System.Threading.Thread.Sleep(Rand.Next(500, 1501))
					End If
					'Begin round + round mechanics
					RandomWinner = Rand.Next(0, 101)
					If Team1Score = 45 And Team2Score < 45 Then
						RandomWinner = 100
					End If
					If Team2Score = 45 And Team1Score < 45 Then
						RandomWinner = 0
					End If
					Console.WriteLine("Round {0} ({1}) has begun...", i, ScrambledMapPool(MapsPlayed))
					System.Threading.Thread.Sleep(Rand.Next(1000, 2001))
					If RandomWinner >= SideTPercent Then
						If SideCT = Team1 Then
							Team1Score = Team1Score + 1
							Console.WriteLine("{0} has taken the round.", Team1)
							Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
						Else
							Team2Score = Team2Score + 1
							Console.WriteLine("{0} has taken the round.", Team2)
							Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
						End If
					Else
						If SideT = Team1 Then
							Team1Score = Team1Score + 1
							Console.WriteLine("{0} has taken the round.", Team1)
							Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
						Else
							Team2Score = Team2Score + 1
							Console.WriteLine("{0} has taken the round.", Team2)
							Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
						End If
					End If
				Loop
				If BestOf = 0 Then
					System.Threading.Thread.Sleep(Rand.Next(500, 1501))
					DetermWin()
				End If
				System.Threading.Thread.Sleep(Rand.Next(1500, 3001))
				If Team1Score > Team2Score Then
					Team1Maps = Team1Maps + 1
					Console.WriteLine("{0} have took Map {1}", Team1, ScrambledMapPool(MapsPlayed))
				Else
					Team2Maps = Team2Maps + 1
					Console.WriteLine("{0} have took Map {1}", Team2, ScrambledMapPool(MapsPlayed))
				End If
				If Team1Maps = 2 Then
					System.Threading.Thread.Sleep(Rand.Next(1500, 3001))
					Exit For
				End If
				If Team2Maps = 2 Then
					System.Threading.Thread.Sleep(Rand.Next(1500, 3001))
					Exit For
				End If
				System.Threading.Thread.Sleep(Rand.Next(3000, 6001))
			End If
		Next
		System.Threading.Thread.Sleep(Rand.Next(500, 1501))
		Console.Clear()
		Dim Winner As String = ""
		If Team1Maps > Team2Maps Then
			Winner = Team1
		Else
			Winner = Team2
		End If
		If Winner <> "Draw" Then
			Console.WriteLine("The winner is {0}.", Winner)
		End If
		If Winner = Team1 Then
			Console.WriteLine("The series score ended as {0} - {1}.", Team1Maps, Team2Maps)
		Else
			Console.WriteLine("The series score ended as {0} - {1}.", Team2Maps, Team1Maps)
		End If
		Console.WriteLine("Map Scores:")
		Console.WriteLine("Map 1 ({0}): {1}: {2} {3}", ScrambledMapPool(0), Map1Winner, Map1Score, Map1Loser)
		Console.WriteLine("Map 2 ({0}): {1}: {2} {3}", ScrambledMapPool(1), Map2Winner, Map2Score, Map2Loser)
		If Not String.IsNullOrEmpty(Map3Winner) Then
			Console.WriteLine("Map 3 ({0}): {1}: {2} {3}", ScrambledMapPool(2), Map3Winner, Map3Score, Map3Loser)
		End If
		Console.ReadLine()
		MiniMain()
	End Sub
	Sub OvertimeFalse()
		MapPool(0) = "Cache"
		MapPool(1) = "Cobblestone"
		MapPool(2) = "Inferno"
		MapPool(3) = "Mirage"
		MapPool(4) = "Nuke"
		MapPool(5) = "Overpass"
		MapPool(6) = "Train"
		If ChooseMaps = False Then
			Array.Clear(ScrambledMapPool, 0, ScrambledMapPool.Length)
			For i = 0 To 6
				Dim RandomMap As Integer
				RandomMap = Rand.Next(0, 7)
				While ScrambledMapPool.Contains(MapPool(RandomMap))
					RandomMap = Rand.Next(0, 7)
				End While
				ScrambledMapPool(i) = MapPool(RandomMap)
			Next
		End If
		Dim StartingSide As String = "0"
		For MapsPlayed As Integer = 0 To BestOf
			Team1Score = 0
			Team2Score = 0
			Console.Clear()
			Console.WriteLine("The map is: {0}", ScrambledMapPool(MapsPlayed))
			If BestOf > 0 Then
				Console.WriteLine("Map {0} / 3", MapsPlayed + 1)
				If MapsPlayed = 0 Then
					Console.WriteLine("Map 1: {0}, Map 2: {1}, Map 3: {2}", ScrambledMapPool(0), ScrambledMapPool(1), ScrambledMapPool(2))
				End If
				If MapsPlayed = 1 Then
					Console.ForegroundColor = ConsoleColor.Red
					Console.Write("Map 1: {0}, ", ScrambledMapPool(0))
					Console.ForegroundColor = ConsoleColor.Gray
					Console.Write("Map 2: {0}, Map 3: {1}", ScrambledMapPool(1), ScrambledMapPool(2))
				End If
				If MapsPlayed = 2 Then
					Console.ForegroundColor = ConsoleColor.Red
					Console.Write("Map 1: {0}, Map 2: {1}, ", ScrambledMapPool(0), ScrambledMapPool(1))
					Console.ForegroundColor = ConsoleColor.Gray
					Console.Write("Map 3: {0}", ScrambledMapPool(2))
				End If
				Console.WriteLine("")
				Console.WriteLine("Series Score: {0}: {1} - {2}: {3}", Team1, Team1Maps, Team2, Team2Maps)
			End If
			If ScrambledMapPool(0) = "Cache" Then
				SideCTPercent = 54
				SideTPercent = 46
			End If

			If ScrambledMapPool(0) = "Inferno" Then
				SideCTPercent = 49
				SideTPercent = 51
			End If

			If ScrambledMapPool(0) = "Cobblestone" Then
				SideCTPercent = 52
				SideTPercent = 48
			End If

			If ScrambledMapPool(0) = "Mirage" Then
				SideCTPercent = 54
				SideTPercent = 46
			End If

			If ScrambledMapPool(0) = "Nuke" Then
				SideCTPercent = 60
				SideTPercent = 40
			End If

			If ScrambledMapPool(0) = "Overpass" Then
				SideCTPercent = 57
				SideTPercent = 43
			End If

			If ScrambledMapPool(0) = "Train" Then
				SideCTPercent = 70
				SideTPercent = 30
			End If

			If KnifeRound = True Then
				RandomWinner = Rand.Next(0, 101)
				If RandomWinner >= 50 Then
					Console.WriteLine("{0} have won the knife round.", Team1)
					If SideCTPercent > SideTPercent Then
						Console.WriteLine("{0} have went with the CT side.", Team1)
						SideCT = Team1
						SideT = Team2
						Team1Side = "CT"
						Team2Side = "T"
					Else
						Console.WriteLine("{0} have went with the T side.", Team1)
						SideT = Team1
						SideCT = Team2
						Team2Side = "CT"
						Team1Side = "T"
					End If
				Else
					Console.WriteLine("{0} have won the knife round.", Team2)
					If SideCTPercent > SideTPercent Then
						Console.WriteLine("{0} have went with the CT side.", Team2)
						SideCT = Team2
						SideT = Team1
						Team2Side = "CT"
						Team1Side = "T"
					Else
						Console.WriteLine("{0} have went with the T side.", Team2)
						SideT = Team2
						SideCT = Team1
						Team1Side = "CT"
						Team2Side = "T"
					End If
					SideCT = Team2
					SideT = Team1
				End If
			Else
				Console.WriteLine("Select the starting side of {0} now.", Team1)
				Console.WriteLine("1. CT")
				Console.WriteLine("2. T")
				StartingSide = Console.ReadLine
				While StartingSide <> "1" And StartingSide <> "2"
					Console.WriteLine("That is not valid, try again.")
					StartingSide = Console.ReadLine
				End While
				If StartingSide = "1" Then
					SideCT = Team1
					Team1Side = "CT"
					SideT = Team2
					Team2Side = "T"
				Else
					SideT = Team1
					Team1Side = "T"
					SideCT = Team2
					Team2Side = "CT"
				End If
			End If

			System.Threading.Thread.Sleep(3000)
			Dim LO3 As Integer = 3
			Dim Countdown
			For Countdown = 1 To 3
				Console.WriteLine("Going live in {0}...", LO3)
				LO3 = LO3 - 1
				System.Threading.Thread.Sleep(1000)
			Next

			Console.Clear()
			'Winning Scores
			Dim WinningScores(10) As Integer
			WinningScores(0) = "16"
			For WinningScoresLoop As Integer = 1 To 10 Step 1
				WinningScores(WinningScoresLoop) = 16 + (3 * WinningScoresLoop)
			Next

			'Max Rounds // DEPRECATED
			Dim MaxRounds(10) As Integer
			MaxRounds(0) = "30"
			For WinningScoresLoop As Integer = 1 To 10 Step 1
				MaxRounds(WinningScoresLoop) = 30 + (6 * WinningScoresLoop)
			Next

			'Half times
			Dim HalfTimes(10) As Integer
			HalfTimes(0) = "16"
			For WinningScoresLoop As Integer = 1 To 10 Step 1
				HalfTimes(WinningScoresLoop) = 28 + (3 * WinningScoresLoop)
			Next

			'Start game
			Dim i As Integer = 0
			Do Until WinningScores.Contains(Team1Score) AndAlso Team2Score < Team1Score - 1 Or WinningScores.Contains(Team2Score) AndAlso Team1Score < Team2Score - 1
				i = i + 1
				'Wait between round beginnings and endings
				If i > 1 Then
					Console.WriteLine("")
					System.Threading.Thread.Sleep(Rand.Next(500, 2001))
				End If
				'Halftime logic
				If HalfTimes.Contains(i) Then
					If SideCT = Team1 And SideT = Team2 Then
						SideCT = Team2
						SideT = Team1
						Team1Side = "T"
						Team2Side = "CT"
					Else
						SideCT = Team1
						SideT = Team2
						Team1Side = "CT"
						Team2Side = "T"
					End If
				End If
				If HalfTimes.Contains(i) And HalfTime = True Then
					Dim Timer As Integer = 5
					System.Threading.Thread.Sleep(Rand.Next(500, 1501))
					Console.Clear()
					Console.WriteLine("Breaking for half time...")
					System.Threading.Thread.Sleep(Rand.Next(500, 1501))
					For HalfTimeCount = 1 To 5
						Console.WriteLine("Resuming in {0}...", Timer)
						System.Threading.Thread.Sleep(1000)
						Timer = Timer - 1
						If HalfTimeCount = 5 Then
							Console.Clear()
						End If
					Next
					Console.WriteLine("The score is currently ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
					System.Threading.Thread.Sleep(Rand.Next(500, 1501))
				End If
				'Begin round + round mechanics
				RandomWinner = Rand.Next(0, 101)
				If Team1Score = 15 And Team2Score = 15 Then
					Exit For
				End If
				Console.WriteLine("Round {0} ({1}) has begun...", i, ScrambledMapPool(MapsPlayed))
				System.Threading.Thread.Sleep(Rand.Next(1000, 2001))
				If RandomWinner >= SideTPercent Then
					If SideCT = Team1 Then
						Team1Score = Team1Score + 1
						Console.WriteLine("{0} has taken the round.", Team1)
						Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
					Else
						Team2Score = Team2Score + 1
						Console.WriteLine("{0} has taken the round.", Team2)
						Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
					End If
				Else
					If SideT = Team1 Then
						Team1Score = Team1Score + 1
						Console.WriteLine("{0} has taken the round.", Team1)
						Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
					Else
						Team2Score = Team2Score + 1
						Console.WriteLine("{0} has taken the round.", Team2)
						Console.WriteLine("The score is now ({0}) {1}: {2} - ({3}) {4}: {5}", Team1Side, Team1, Team1Score, Team2Side, Team2, Team2Score)
					End If
				End If
			Loop
			If BestOf = 0 Then
				System.Threading.Thread.Sleep(Rand.Next(500, 1501))
				Exit Sub
			End If
			System.Threading.Thread.Sleep(Rand.Next(1500, 3001))
			If Team1Score > Team2Score Then
				Team1Maps = Team1Maps + 1
				Console.WriteLine("{0} have took Map {1}", Team1, ScrambledMapPool(MapsPlayed))
			Else
				Team2Maps = Team2Maps + 1
				Console.WriteLine("{0} have took Map {1}", Team2, ScrambledMapPool(MapsPlayed))
			End If

			If MapsPlayed = 0 And Team1Score > Team2Score Then
				Map1Winner = Team1
				Map1Loser = Team2
				Map1Score = String.Join(" - ", Team1Score, Team2Score)
			End If

			If MapsPlayed = 0 And Team1Score < Team2Score Then
				Map1Winner = Team2
				Map1Loser = Team1
				Map1Score = String.Join(" - ", Team2Score, Team1Score)
			End If

			If MapsPlayed = 1 And Team1Score > Team2Score Then
				Map2Winner = Team1
				Map2Loser = Team2
				Map2Score = String.Join(" - ", Team1Score, Team2Score)
			End If

			If MapsPlayed = 1 And Team1Score < Team2Score Then
				Map2Winner = Team2
				Map2Loser = Team1
				Map2Score = String.Join(" - ", Team2Score, Team1Score)
			End If

			If MapsPlayed = 2 And Team1Score > Team2Score Then
				Map3Winner = Team1
				Map3Loser = Team2
				Map3Score = String.Join(" - ", Team1Score, Team2Score)
			End If

			If MapsPlayed = 2 And Team1Score < Team2Score Then
				Map3Winner = Team2
				Map3Loser = Team1
				Map3Score = String.Join(" - ", Team2Score, Team1Score)
			End If

			If Team1Maps = 2 Then
				System.Threading.Thread.Sleep(Rand.Next(1500, 3001))
				Exit For
			End If
			If Team2Maps = 2 Then
				System.Threading.Thread.Sleep(Rand.Next(1500, 3001))
				Exit For
			End If
			System.Threading.Thread.Sleep(Rand.Next(3000, 6001))
		Next
		System.Threading.Thread.Sleep(Rand.Next(500, 1501))
		Console.Clear()
		Dim Winner As String = ""
		If Team1Maps > Team2Maps Then
			Winner = Team1
		Else
			Winner = Team2
		End If
		If Winner <> "Draw" Then
			Console.WriteLine("The winner is {0}.", Winner)
		End If
		If Winner = Team1 Then
			Console.WriteLine("The series score ended as {0} - {1}.", Team1Maps, Team2Maps)
		Else
			Console.WriteLine("The series score ended as {0} - {1}.", Team2Maps, Team1Maps)
		End If
		Console.WriteLine("Map Scores:")
		Console.WriteLine("Map 1 ({0}): {1}: {2} {3}", ScrambledMapPool(0), Map1Winner, Map1Score, Map1Loser)
		Console.WriteLine("Map 2 ({0}): {1}: {2} {3}", ScrambledMapPool(1), Map2Winner, Map2Score, Map2Loser)
		If Not String.IsNullOrEmpty(Map3Winner) Then
			Console.WriteLine("Map 3 ({0}): {1}: {2} {3}", ScrambledMapPool(2), Map3Winner, Map3Score, Map3Loser)
		End If
		Console.ReadLine()
		MiniMain()
	End Sub
	Sub DetermWin()
		Console.Clear()
		Dim Winner As String = ""
		If Team1Score > Team2Score Then
			Winner = Team1
		Else
			Winner = Team2
		End If
		If Team1Score = Team2Score Then
			Winner = "Draw"
		End If
		If Winner <> "Draw" Then
			Console.WriteLine("The winner is {0}.", Winner)
		Else
			Console.WriteLine("The game ended as a draw.")
		End If
		If Winner = Team1 Then
			Console.WriteLine("Map Score:")
			Console.WriteLine("Map 1 ({0}): {1}: {2} - {3}: {4}", ScrambledMapPool(0), Team1, Team1Score, Team2, Team2Score)
		Else
			Console.WriteLine("Map Score:")
			Console.WriteLine("Map 1 ({0}): {1}: {2} - {3}: {4}", ScrambledMapPool(0), Team2, Team2Score, Team1, Team1Score)
		End If
		Console.ReadLine()
		MiniMain()
	End Sub
	Sub Maps()
		ChooseMaps = True
		MapPool(0) = "Cache"
		MapPool(1) = "Cobblestone"
		MapPool(2) = "Inferno"
		MapPool(3) = "Mirage"
		MapPool(4) = "Nuke"
		MapPool(5) = "Overpass"
		MapPool(6) = "Train"
		Array.Clear(ScrambledMapPool, 0, ScrambledMapPool.Length)
		For i = 0 To 2
			Console.Clear()
			Console.WriteLine("Choose map {0}:", i + 1)
			For maps As Integer = 0 To 6
				Console.Write("{0}. ", maps)
				Console.Write(MapPool(maps))
				Console.WriteLine("")
			Next
			Dim UserMap As String
			UserMap = Console.ReadLine
			While ScrambledMapPool.Contains(MapPool(UserMap))
				Console.WriteLine("That is already part of the map pool, try again.")
				UserMap = Console.ReadLine
			End While
			For maps As Integer = 0 To 6
				If UserMap = maps Then
					ScrambledMapPool(i) = MapPool(UserMap)
				End If
			Next
		Next
		Console.Clear()
		For i = 0 To 2
			Console.WriteLine(ScrambledMapPool(i))
		Next
		Console.ReadLine()
		MiniMain()
	End Sub
End Module
