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
	Public SupportedResolutions() As String = {"1920x1080", "1280x1024", "1280x960", "1024x768", "800x600", "1280x720", "1600x900", "1280x800", "1440x1080", "1152x864", "1440x900", "1680x1050"}
	Public Settings() As String = {"Yes", "Yes", "No", "Yes", "Yes", "Yes", "Yes", "No", "No", "No", "No", "Yes", "Yes", "Yes", "Yes", "Yes", "Yes", "No"}
	Public CrosshairColorR As Double
	Public CrosshairColorG As Double
	Public CrosshairColorB As Double
	Public CrosshairUse5 As Boolean
	Public Height As String
	Public Width As String
	Public location As String = System.Environment.GetCommandLineArgs()(0)
	Public appName As String = System.IO.Path.GetFileName(location)
	Public DPI As Integer = My.Settings.DPI

	Sub Main()
		SaveSettings()
		My.Settings.Save()
		Dim FileName As String = System.AppDomain.CurrentDomain.BaseDirectory & "\" + appName + ".txt"
		Settings = File.ReadAllLines(FileName)
		Console.Clear()
		Console.Title = "CS:GO CFG Generator"
		Console.WriteLine("Welcome to the CFG generator.")
		Console.WriteLine("1. Generate")
		Console.WriteLine("2. Generate Options")
		Console.WriteLine("3. Set CFG Path")
		Console.WriteLine("4. Set file name")
		Console.WriteLine("5. Write to CFG's?: {0}", WriteCFG)
		Dim UserInput As String = Console.ReadLine
		Select Case UserInput
			Case 1
				Generate()
			Case 2
				GenerateSetup()
			Case 3
				SetCFGPath()
			Case 4
				SetFileNames()
			Case 5
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
		Console.WriteLine("18. Use Crosshair Color 5?: {0}", Settings(17))
		Console.WriteLine("19. DPI: {0}", DPI)
		Console.WriteLine(vbCrLf + "Do not enter a value to continue.")
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
		Console.Clear()
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
					CrosshairSize = Rand.Next(0, 5)
					Random5050(RandomValue1, RandomValue2, CrosshairSize)
					If CrosshairSize < 0.5 Then
						CrosshairSize = 0.5
					End If
					Console.WriteLine("Size: {0}", CrosshairSize)
				End If

				'Gap
				If i = 1 Then
					CrosshairGap = Rand.Next(-3, 1)
					Random5050(RandomValue1, RandomValue2, CrosshairGap)
					Console.WriteLine("Gap: {0}", CrosshairGap)
				End If

				'Style
				If i = 2 Then
					CrosshairStyle = Rand.Next(0, 7)
					Console.WriteLine("Style: {0}", CrosshairStyle)
				End If

				'Color
				If i = 3 Then
					If Settings(17) = "Yes" Then
						CrosshairColor = Rand.Next(0, 6)
					Else
						CrosshairColor = Rand.Next(0, 5)
					End If
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
					CrosshairThickness = Rand.Next(0, 3)
					Random5050(RandomValue1, RandomValue2, CrosshairThickness)
					Console.WriteLine("Thickness: {0}", CrosshairThickness)
				End If

				'Outline
				If i = 5 Then
					CrosshairOutline = Rand.Next(0, 2)
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
					RandomValue1 = Rand.Next(0, 2)
					If RandomValue1 = "0" Then
						ViewmodelBob = 5
					Else
						ViewmodelBob = 21
					End If
					Console.WriteLine("Bob: {0}", ViewmodelBob)
				End If

				If i = 12 Then
					RandomValue1 = Rand.Next(0, 2)
					If RandomValue1 = "0" Then
						BobLat = 0.4
					Else
						BobLat = 0

					End If

					If Not Settings(12) = "Yes" And Settings(13) = "Yes" Then
						Console.WriteLine("Bob Lat: {0}", BobLat)
					End If
				End If

					If i = 13 Then
					RandomValue1 = Rand.Next(0, 2)
					If RandomValue1 = "0" Then
						BobVert = 0.25
					Else
						BobVert = 0
					End If

					If Not Settings(12) = "Yes" And Settings(13) = "Yes" Then
						Console.WriteLine("Bob Vert: {0}", BobVert)
					End If
				End If

				If Settings(12) = "Yes" And Settings(13) = "Yes" And i = 14 Then
					If BobLat = "0.4" Then
						BobVert = 0.25
					Else
						BobVert = 0
					End If
					Console.WriteLine("Bob Lat: {0}", BobLat)
					Console.WriteLine("Bob Vert: {0}", BobVert)
				End If

				If i = 14 Then
					RandomValue1 = Rand.Next(0, 2)
					If RandomValue1 = "0" Then
						Righthand = 0
					Else
						Righthand = 1
					End If
					Console.WriteLine("Righthand: {0}", Righthand)
				End If

				If i = 15 Then
					RandomValue1 = Rand.Next(1, 11)
					Dim ResArray() As String = SupportedResolutions
					Dim ChosenRes(ResArray.Length) As String
					RandomValue1 = Rand.Next(0, ResArray.Length)
					For j = 0 To ResArray.Length - 1
						While ChosenRes.Contains(ResArray(RandomValue1))
							RandomValue1 = Rand.Next(0, ResArray.Length)
						End While
						ChosenRes(j) = ResArray(RandomValue1)
					Next

					Console.WriteLine(vbCrLf + "Resolution:")
					Console.WriteLine(ChosenRes(0))
					Console.WriteLine("")

					Height = ChosenRes(0).Split("x")(0)
					Width = ChosenRes(0).Split("x")(1)
				End If

				If i = 16 Then
					Sens = Rand.Next(1, 4)
					If Sens = 3 Then Sens = 2
					RandomValue1 = Rand.Next(0, 2)
					RandomValue2 = Rand.Next(0, 3)
					Dim SensDecimal As Decimal = Rand.NextDouble
					If RandomValue2 = "0" Then
						SensDecimal = Decimal.Round(SensDecimal, 2)
					Else
						SensDecimal = Decimal.Round(SensDecimal, 1)
					End If
					If RandomValue1 = "0" Then
						Sens = Sens - SensDecimal
					Else
						Sens = Sens + SensDecimal
					End If
					If Sens < 1 Then
						Sens = 1 + SensDecimal
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
		Console.WriteLine("Current path: {0}", CFGPath)
		Console.WriteLine("Do you want to change it? Y/N")
		Threading.Thread.Sleep(250)
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
		Console.WriteLine("Select the one you want to change:")
		Console.WriteLine("1. Config Name: {0}", CFGName)
		Console.WriteLine("2. Exit")
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
		Dim FileName As String = System.AppDomain.CurrentDomain.BaseDirectory & "\" + appName + ".txt"
		IO.File.WriteAllLines(FileName, Settings)
	End Sub
End Module
