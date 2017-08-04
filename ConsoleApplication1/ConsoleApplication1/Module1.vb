Imports System.IO
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
	Public Sens As Double
	Public CFGPath As String = My.Settings.SavedCFGPath
	Public WriteCFG As Boolean = My.Settings.WriteCFG
	Public CFGName As String = My.Settings.FileName
	Public Rand As New Random
	Public SupportedResolutions() As String = {"800x600", "1024x768", "1152x864", "1280x960", "1280x1024", "1280x720", "1600x900", "1920x1080", "1280x800", "1440x900", "1680x1050"}
	Public SupportedResolutionsSettings() As String = My.Settings.SupportedResolutionsSettings.Cast(Of String)().ToArray()
	Public ResolutionAspectRatios() As String = {"4:3", "16:9", "16:10"}
	Public ResolutionAspectRatiosUsed() As String = My.Settings.ResolutionAspectRatiosUsedList.Cast(Of String)().ToArray()
	Public Settings() As String = {"Yes", "Yes", "No", "Yes", "Yes", "Yes", "Yes", "No", "No", "No", "No", "Yes", "Yes", "Yes", "Yes", "Yes", "Yes"}
	Public Version As String = "3.0"
	Public WipeOptions As Boolean = False
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

	Sub Main()
		'Setup
		If File.Exists(VersionFile) Then
			Dim CurrentVersion As String = File.ReadAllText(VersionFile)
			If CurrentVersion <> Version Then
				If WipeOptions = True Then
					File.Delete(SettingsFile)
					IO.File.WriteAllText(VersionFile, Version)
				End If
			End If
		Else
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
		StyleConsole()
		Console.ForegroundColor = ConsoleColor.Green
		Console.Write(vbCrLf + "This is a complete overhaul that gives you complete control over all sides of the generation. I decided it would be better if you could blacklist/whitelist things such as color of the crosshair since yellow really doesn't work for me ever since I tend to lose it a lot and keep crosshair generating to standards that people might actually use. Obviously you can now change it all for yourself so if you wanted a 100 size crosshair to be generated you could. This will be confusing at first to use and things such as disabling a certain aspect ratio disables all the resolutions and you must re-enable them etc but I will look into ways around that. Hope you enjoy this new version! :)" + vbCrLf + vbCrLf + "BiZR" + vbCrLf + vbCrLf)
		Console.ResetColor()
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
			Case Else
				Main()
		End Select
		Console.ReadLine()
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
		Console.WriteLine("18. DPI: {0}", DPI)
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

		While UserInput < 1 Or UserInput > 19
			Console.WriteLine("That is not a valid input, try again.")
			UserInput = Console.ReadLine
		End While

		If UserInput = "18" Then
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

		Dim objStreamWriter As StreamWriter = New StreamWriter(CFGPath + "\" + CFGName + ".cfg")
		Dim RandomValue1 As Integer
		Dim RandomValue2 As Integer
		Dim WriteCrosshair As Boolean = False
		Dim WriteCrosshairDone As Boolean = False
		Dim WriteViewmodel As Boolean = False
		Dim WriteViewmodelDone As Boolean = False

		For i = 0 To 16
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
			End If
		Next

		If WriteCFG = True Then
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
			objStreamWriter.Close()
		End If

		objStreamWriter.Close()
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
		Console.WriteLine("10. Back to Preferences")
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

	Function FlipFlopBoolean(ByRef InputValue)
		If InputValue = True Then
			InputValue = False
		Else
			InputValue = True
		End If
		Return InputValue
	End Function
End Module
