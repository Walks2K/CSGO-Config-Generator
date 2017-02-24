Imports System.IO
Module Module1
    Public CrosshairSize As Double
    Public CrosshairGap As Double
    Public CrosshairStyle As Integer
    Public CrosshairColor As Double
    Public CrosshairColorR As Double
    Public CrosshairColorG As Double
    Public CrosshairColorB As Double
    Public CrosshairThickness As Double
    Public CrosshairOutline As Double
    Public CrosshairOutlineThickness As Double
    Public ViewmodelFOV As Double
    Public ViewmodelX As Double
    Public ViewmodelY As Double
    Public ViewmodelZ As Double
    Public ViewmodelBob As Double
    Public Righthand As Double
    Public Resolution As String
    Public CFGPath As String = "C:\Program Files (x86)\Steam\SteamApps\common\Counter-Strike Global Offensive\csgo\cfg"
    Public WriteCFG As Boolean = False
    Public xHairCFG As String = "randomxhair"
    Public ViewmodelCFG As String = "randomviewmodel"
    Public AllinOneCFG As String = "randomallinone"
    Public Rand As New Random
    Public Cvars() As String = {"No", "No", "4", "No", "No", "No", "No", "No", "No", "No", "68", "2.5", "0", "-1.5", "No", "No", "No"}
    Sub Main()
        Console.Clear()
        For i = 0 To 16
            If i < 15 Then
                If Not IsNumeric(Cvars(i)) Then
                    Cvars(i) = "No"
                End If
            End If
        Next

        Console.Title = "CS:GO CFG Generator"
        Console.WriteLine("Welcome to the CFG generator.")
        Console.WriteLine("1. Crosshair")
        Console.WriteLine("2. Viewmodel")
        Console.WriteLine("3. Resolution")
        Console.WriteLine("4. All in One")
        Console.WriteLine("5. Set CFG Path")
        Console.WriteLine("6. Set file names")
        Console.WriteLine("7. Write to CFG's?: {0}", WriteCFG)
        Console.WriteLine("8. Preset Cvars")
        Dim UserInput As String = Console.ReadLine
        Select Case UserInput
            Case 1
                Crosshair()
            Case 2
                Viewmodel()
            Case 3
                ResolutionSub()
            Case 4
                AllInOne()
            Case 5
                SetCFGPath()
            Case 6
                SetFileNames()
            Case 7
                If WriteCFG = False Then
                    WriteCFG = True
                    Main()
                Else
                    WriteCFG = False
                    Main()
                End If
            Case 8
                PresetCvars()
            Case Else
                Main()
        End Select
        Console.ReadLine()
    End Sub

    Sub Crosshair()
        Console.Clear()
        Dim objStreamWriter As StreamWriter = New StreamWriter(CFGPath + "\" + xHairCFG + ".cfg")
        Dim DecimalRand1 As Integer = Rand.Next(1, 11)
        Dim DecimalRand2 As Integer = Rand.Next(1, 3)

        If Not IsNumeric(Cvars(0)) Then
            CrosshairSize = Rand.Next(1, 5)
            If DecimalRand1 > 5 Then
                If DecimalRand1 = 1 Then
                    CrosshairSize = CrosshairSize - 0.5
                Else
                    CrosshairSize = CrosshairSize + 0.5
                End If
            End If
        Else
            CrosshairSize = Cvars(0)
        End If
        If Not IsNumeric(Cvars(1)) Then
            CrosshairGap = Rand.Next(-2, 3)
            DecimalRand1 = Rand.Next(1, 11)
            DecimalRand2 = Rand.Next(1, 3)
            If DecimalRand1 > 5 Then
                If DecimalRand1 = 1 Then
                    CrosshairGap = CrosshairGap - 0.5
                Else
                    CrosshairGap = CrosshairGap + 0.5
                End If
            End If
        Else
            CrosshairGap = Cvars(1)
        End If

        If Not IsNumeric(Cvars(2)) Then
            CrosshairStyle = Rand.Next(0, 7)
        Else
            CrosshairStyle = Cvars(2)
        End If

        If Not IsNumeric(Cvars(3)) Then
            CrosshairColor = Rand.Next(1, 6)
            If CrosshairColor = 5 Then
                If Not IsNumeric(Cvars(4)) Then
                    CrosshairColorR = Rand.Next(0, 256)
                End If
                If Not IsNumeric(Cvars(5)) Then
                    CrosshairColorG = Rand.Next(0, 256)
                End If
                If Not IsNumeric(Cvars(6)) Then
                    CrosshairColorB = Rand.Next(0, 256)
                End If
            End If
        Else
            CrosshairColor = Cvars(3)
            If CrosshairColor = 5 Then
                CrosshairColorR = Cvars(4)
                CrosshairColorG = Cvars(5)
                CrosshairColorB = Cvars(6)
            End If
        End If
        DecimalRand1 = Rand.Next(1, 11)
        DecimalRand2 = Rand.Next(1, 3)
        If Not IsNumeric(Cvars(7)) Then
            CrosshairThickness = Rand.Next(0, 2)
            If DecimalRand1 > 5 Then
                If DecimalRand1 = 1 Then
                    CrosshairThickness = CrosshairThickness - 0.5
                Else
                    CrosshairThickness = CrosshairThickness + 0.5
                End If
            End If
        Else
            CrosshairThickness = Cvars(7)
        End If

        If Not IsNumeric(Cvars(8)) Then
            CrosshairOutline = Rand.Next(0, 2)
        Else
            CrosshairOutline = Cvars(8)
        End If

        DecimalRand2 = Rand.Next(1, 3)
        If Not IsNumeric(Cvars(9)) Then
            If DecimalRand2 = 1 Then
                CrosshairOutlineThickness = 0.5
            Else
                CrosshairOutlineThickness = 1
            End If
        Else
            CrosshairOutlineThickness = Cvars(9)
        End If

        Console.WriteLine("Size: {0}", CrosshairSize)
        Console.WriteLine("Gap: {0}", CrosshairGap)
        Console.WriteLine("Style: {0}", CrosshairStyle)
        Console.WriteLine("Color: {0}", CrosshairColor)
        If CrosshairColor = 5 Then
            Console.WriteLine("Color R: {0}", CrosshairColorR)
            Console.WriteLine("Color G: {0}", CrosshairColorG)
            Console.WriteLine("Color B: {0}", CrosshairColorB)
        End If
        Console.WriteLine("Thickness: {0}", CrosshairThickness)
        Console.WriteLine("Outline: {0}", CrosshairOutline)
        Console.WriteLine("Outline Thickness: {0}", CrosshairOutlineThickness)


        If WriteCFG = True Then
            objStreamWriter.WriteLine("cl_crosshairsize {0}", CrosshairSize)
            objStreamWriter.WriteLine("cl_crosshairgap {0}", CrosshairGap)
            objStreamWriter.WriteLine("cl_crosshairstyle {0}", CrosshairStyle)
            objStreamWriter.WriteLine("cl_crosshaircolor {0}", CrosshairColor)
            If CrosshairColor = 5 Then
                objStreamWriter.WriteLine("cl_crosshaircolor_r {0}", CrosshairColorR)
                objStreamWriter.WriteLine("cl_crosshaircolor_g {0}", CrosshairColorG)
                objStreamWriter.WriteLine("cl_crosshaircolor_b {0}", CrosshairColorB)
            End If
            objStreamWriter.WriteLine("cl_crosshairthickness {0}", CrosshairThickness)
            objStreamWriter.WriteLine("cl_crosshair_drawoutline {0}", CrosshairOutline)
            objStreamWriter.WriteLine("cl_crosshair_outlinethickness {0}", CrosshairOutlineThickness)
            objStreamWriter.Close()
        End If

        objStreamWriter.Close()
        Console.WriteLine("Do you want to reroll? Y/N")
        Dim UserInput As String = Console.ReadLine
        While UserInput <> "Y" And UserInput <> "N"
            Console.WriteLine("That is not valid, try again.")
            UserInput = Console.ReadLine
        End While
        If UserInput = "Y" Then
            Crosshair()
        Else
            Main()
        End If
    End Sub

    Sub Viewmodel()
        Console.Clear()
        Dim objStreamWriter As StreamWriter = New StreamWriter(CFGPath + "\" + ViewmodelCFG + ".cfg")
        Dim DecimalRand1 As Integer = Rand.Next(1, 11)
        Dim DecimalRand2 As Integer = Rand.Next(1, 4)

        If Not IsNumeric(Cvars(10)) Then
            If DecimalRand2 = 1 Then
                ViewmodelFOV = 60
            End If
            If DecimalRand2 = 2 Then
                ViewmodelFOV = 65
            End If
            If DecimalRand2 = 3 Then
                ViewmodelFOV = 68
            End If
        Else
            ViewmodelFOV = Cvars(10)
        End If

        If Not IsNumeric(Cvars(11)) Then
            DecimalRand1 = Rand.Next(1, 3)
            ViewmodelX = Rand.Next(0, 3)
            If DecimalRand1 > 5 Then
                If DecimalRand1 = 1 Then
                    ViewmodelX = ViewmodelX - 0.5
                Else
                    ViewmodelX = ViewmodelX + 0.5
                End If
            End If
        Else
            ViewmodelX = Cvars(11)
        End If

        If Not IsNumeric(Cvars(12)) Then
            DecimalRand1 = Rand.Next(1, 11)
            DecimalRand2 = Rand.Next(1, 3)
            ViewmodelY = Rand.Next(-2, 3)
            If DecimalRand1 > 5 Then
                If DecimalRand1 = 1 Then
                    ViewmodelY = ViewmodelY - 0.5
                Else
                    ViewmodelY = ViewmodelY + 0.5
                End If
            End If
            If ViewmodelY = 2.5 Then
                ViewmodelY = 2
            End If
        Else
            ViewmodelY = Cvars(12)
        End If

        If Not IsNumeric(Cvars(13)) Then
            DecimalRand1 = Rand.Next(1, 11)
            DecimalRand2 = Rand.Next(1, 3)
            ViewmodelZ = Rand.Next(-2, 0)
            If DecimalRand1 > 5 Then
                If DecimalRand1 = 1 Then
                    ViewmodelZ = ViewmodelZ - 0.5
                Else
                    ViewmodelZ = ViewmodelZ + 0.5
                End If
            End If
        Else
            ViewmodelZ = Cvars(13)
        End If

        If Not IsNumeric(Cvars(14)) Then
            DecimalRand2 = Rand.Next(1, 3)
            If DecimalRand2 = 1 Then
                ViewmodelBob = 5
            Else
                ViewmodelBob = 21
            End If
        Else
            ViewmodelBob = Cvars(14)
        End If

        If Not IsNumeric(Cvars(15)) Then
            DecimalRand2 = Rand.Next(0, 2)
            If DecimalRand2 = 0 Then
                Righthand = 0
            Else
                Righthand = 1
            End If
        Else
            Righthand = Cvars(15)
        End If
        Console.WriteLine("FOV: {0}", ViewmodelFOV)
        Console.WriteLine("X Offset: {0}", ViewmodelX)
        Console.WriteLine("Y Offset: {0}", ViewmodelY)
        Console.WriteLine("Z Offset: {0}", ViewmodelZ)
        Console.WriteLine("Bob: {0}", ViewmodelBob)
        Console.WriteLine("Righthand: {0}", Righthand)

        If WriteCFG = True Then
            objStreamWriter.WriteLine("viewmodel_fov {0}", ViewmodelFOV)
            objStreamWriter.WriteLine("viewmodel_offset_x {0}", ViewmodelX)
            objStreamWriter.WriteLine("viewmodel_offset_y {0}", ViewmodelY)
            objStreamWriter.WriteLine("viewmodel_offset_z {0}", ViewmodelZ)
            objStreamWriter.WriteLine("cl_bob_lower_amt {0}", ViewmodelBob)
            objStreamWriter.WriteLine("cl_righthand {0}", Righthand)
            objStreamWriter.Close()
        End If

        objStreamWriter.Close()
        Console.WriteLine("Do you want to reroll? Y/N")
        Dim UserInput As String = Console.ReadLine
        While UserInput <> "Y" And UserInput <> "N"
            Console.WriteLine("That is not valid, try again.")
            UserInput = Console.ReadLine
        End While
        If UserInput = "Y" Then
            Viewmodel()
        Else
            Main()
        End If
    End Sub

    Sub ResolutionSub()
        Console.Clear()
        Dim DecimalRand1 As Integer = Rand.Next(1, 11)
        Dim ResArray() As String = {"1920x1080", "1280x1024", "1280x960", "1024x768", "800x600", "1280x720", "1600x900"}
        Dim ChosenRes(ResArray.Length) As String
        If Cvars(15) = "No" Then
            For i = 0 To ResArray.Length - 1
                DecimalRand1 = Rand.Next(0, ResArray.Length)
                While ChosenRes.Contains(ResArray(DecimalRand1))
                    DecimalRand1 = Rand.Next(0, ResArray.Length)
                End While
                ChosenRes(i) = ResArray(DecimalRand1)
            Next
        Else
            ChosenRes(0) = Cvars(15)
        End If

        Console.WriteLine(ChosenRes(0))

        Console.WriteLine("Do you want to reroll? Y/N")
        Dim UserInput As String = Console.ReadLine
        While UserInput <> "Y" And UserInput <> "N"
            Console.WriteLine("That is not valid, try again.")
            UserInput = Console.ReadLine
        End While
        If UserInput = "Y" Then
            ResolutionSub()
        Else
            Main()
        End If
    End Sub

    Sub AllInOne()
        Console.Clear()
        Dim objStreamWriter As StreamWriter = New StreamWriter(CFGPath + "\" + AllinOneCFG + ".cfg")
        Dim DecimalRand1 As Integer = Rand.Next(1, 11)
        Dim DecimalRand2 As Integer = Rand.Next(1, 3)

        'Size
        If Not IsNumeric(Cvars(0)) Then
            CrosshairSize = Rand.Next(1, 5)
            If DecimalRand1 > 5 Then
                If DecimalRand2 = 1 Then
                    CrosshairSize = CrosshairSize - 0.5
                End If
                If DecimalRand2 = 2 Then
                    CrosshairSize = CrosshairSize + 0.5
                End If
            End If
        End If

        'Gap
        If Not IsNumeric(Cvars(1)) Then
            CrosshairGap = Rand.Next(-4, 3)
            DecimalRand1 = Rand.Next(1, 11)
            DecimalRand2 = Rand.Next(1, 3)
            If DecimalRand1 > 5 Then
                If DecimalRand2 = 1 Then
                    CrosshairGap = CrosshairGap - 0.5
                End If

                If DecimalRand2 = 2 Then
                    CrosshairGap = CrosshairGap + 0.5
                End If
            End If
        Else
            CrosshairGap = Cvars(1)
        End If

        'Style
        If Not IsNumeric(Cvars(2)) Then
            CrosshairStyle = Rand.Next(0, 7)
        Else
            CrosshairStyle = Cvars(2)
        End If

        'Colour
        If Not IsNumeric(Cvars(3)) Then
            CrosshairColor = Rand.Next(1, 6)
            If CrosshairColor = 5 Then
                If Not IsNumeric(Cvars(4)) Then
                    CrosshairColorR = Rand.Next(0, 256)
                End If
                If Not IsNumeric(Cvars(5)) Then
                    CrosshairColorG = Rand.Next(0, 256)
                End If
                If Not IsNumeric(Cvars(6)) Then
                    CrosshairColorB = Rand.Next(0, 256)
                End If
            End If
        Else
            CrosshairColor = Cvars(3)
            If CrosshairColor = 5 Then
                CrosshairColorR = Cvars(4)
                CrosshairColorG = Cvars(5)
                CrosshairColorB = Cvars(6)
            End If
        End If
        DecimalRand1 = Rand.Next(1, 11)
        DecimalRand2 = Rand.Next(1, 3)

        'Thickness
        If Not IsNumeric(Cvars(7)) Then
            CrosshairThickness = Rand.Next(0, 3)
            If DecimalRand1 > 5 Then
                If DecimalRand2 = 1 Then
                    CrosshairThickness = CrosshairThickness - 0.5
                Else
                    CrosshairThickness = CrosshairThickness + 0.5
                End If
            End If
        Else
            CrosshairThickness = Cvars(7)
        End If

        'Outline
        If Not IsNumeric(Cvars(8)) Then
            CrosshairOutline = Rand.Next(0, 2)
        Else
            CrosshairOutline = Cvars(8)
        End If

        DecimalRand2 = Rand.Next(1, 3)
        If Not IsNumeric(Cvars(9)) Then
            If DecimalRand2 = 1 Then
                CrosshairOutlineThickness = 0.5
            Else
                CrosshairOutlineThickness = 1
            End If
        Else
            CrosshairOutlineThickness = Cvars(9)
        End If

        Console.WriteLine("Size: {0}", CrosshairSize)
        Console.WriteLine("Gap: {0}", CrosshairGap)
        Console.WriteLine("Style: {0}", CrosshairStyle)
        Console.WriteLine("Color: {0}", CrosshairColor)
        If CrosshairColor = 5 Then
            Console.WriteLine("Color R: {0}", CrosshairColorR)
            Console.WriteLine("Color G: {0}", CrosshairColorG)
            Console.WriteLine("Color B: {0}", CrosshairColorB)
        End If
        Console.WriteLine("Thickness: {0}", CrosshairThickness)
        Console.WriteLine("Outline: {0}", CrosshairOutline)
        Console.WriteLine("Outline Thickness: {0}", CrosshairOutlineThickness)
        Console.WriteLine("")

        DecimalRand1 = Rand.Next(1, 11)
        DecimalRand2 = Rand.Next(1, 4)

        'Viewmodel
        If Not IsNumeric(Cvars(10)) Then
            If DecimalRand2 = 1 Then
                ViewmodelFOV = 60
            End If
            If DecimalRand2 = 2 Then
                ViewmodelFOV = 65
            End If
            If DecimalRand2 = 3 Then
                ViewmodelFOV = 68
            End If
        Else
            ViewmodelFOV = Cvars(10)
        End If

        If Not IsNumeric(Cvars(11)) Then
            DecimalRand1 = Rand.Next(1, 3)
            ViewmodelX = Rand.Next(0, 3)
            If DecimalRand1 > 5 Then
                If DecimalRand1 = 1 Then
                    ViewmodelX = ViewmodelX - 0.5
                Else
                    ViewmodelX = ViewmodelX + 0.5
                End If
            End If
        Else
            ViewmodelX = Cvars(11)
        End If

        If Not IsNumeric(Cvars(12)) Then
            DecimalRand1 = Rand.Next(1, 11)
            DecimalRand2 = Rand.Next(1, 3)
            ViewmodelY = Rand.Next(-2, 3)
            If DecimalRand1 > 5 Then
                If DecimalRand1 = 1 Then
                    ViewmodelY = ViewmodelY - 0.5
                Else
                    ViewmodelY = ViewmodelY + 0.5
                End If
            End If
            If ViewmodelY = 2.5 Then
                ViewmodelY = 2
            End If
        Else
            ViewmodelY = Cvars(12)
        End If

        If Not IsNumeric(Cvars(13)) Then
            DecimalRand1 = Rand.Next(1, 11)
            DecimalRand2 = Rand.Next(1, 3)
            ViewmodelZ = Rand.Next(-2, 0)
            If DecimalRand1 > 5 Then
                If DecimalRand1 = 1 Then
                    ViewmodelZ = ViewmodelZ - 0.5
                Else
                    ViewmodelZ = ViewmodelZ + 0.5
                End If
            End If
        Else
            ViewmodelZ = Cvars(13)
        End If

        If Not IsNumeric(Cvars(14)) Then
            DecimalRand2 = Rand.Next(1, 3)
            If DecimalRand2 = 1 Then
                ViewmodelBob = 5
            Else
                ViewmodelBob = 21
            End If
        Else
            ViewmodelBob = Cvars(14)
        End If

        If Not IsNumeric(Cvars(15)) Then
            DecimalRand2 = Rand.Next(0, 2)
            If DecimalRand2 = 0 Then
                Righthand = 0
            Else
                Righthand = 1
            End If
        Else
            Righthand = Cvars(15)
        End If

        Console.WriteLine("Viewmodel:")
        Console.WriteLine("FOV: {0}", ViewmodelFOV)
        Console.WriteLine("X Offset: {0}", ViewmodelX)
        Console.WriteLine("Y Offset: {0}", ViewmodelY)
        Console.WriteLine("Z Offset: {0}", ViewmodelZ)
        Console.WriteLine("Bob: {0}", ViewmodelBob)
        Console.WriteLine("Righthand: {0}", Righthand)
        Console.WriteLine("")

        'Resolution
        DecimalRand1 = Rand.Next(1, 11)
        Dim ResArray() As String = {"1920x1080", "1280x1024", "1280x960", "1024x768", "800x600", "1280x720", "1600x900", "1920x720"}
        Dim ChosenRes(ResArray.Length) As String
        If Cvars(15) = "No" Then
            For i = 0 To ResArray.Length - 1
                DecimalRand1 = Rand.Next(0, ResArray.Length)
                While ChosenRes.Contains(ResArray(DecimalRand1))
                    DecimalRand1 = Rand.Next(0, ResArray.Length)
                End While
                ChosenRes(i) = ResArray(DecimalRand1)
            Next
        Else
            ChosenRes(0) = Cvars(15)
        End If

        Console.WriteLine("Resolution:")
        Console.WriteLine(ChosenRes(0))
        Console.WriteLine("")

        Dim Height As String = ChosenRes(0).Split("x")(0)
        Dim Width As String = ChosenRes(0).Split("x")(1)

        If WriteCFG = True Then
            objStreamWriter.WriteLine("cl_crosshairsize {0}", CrosshairSize)
            objStreamWriter.WriteLine("cl_crosshairgap {0}", CrosshairGap)
            objStreamWriter.WriteLine("cl_crosshairstyle {0}", CrosshairStyle)
            objStreamWriter.WriteLine("cl_crosshaircolor {0}", CrosshairColor)
            If CrosshairColor = 5 Then
                objStreamWriter.WriteLine("cl_crosshaircolor_r {0}", CrosshairColorR)
                objStreamWriter.WriteLine("cl_crosshaircolor_g {0}", CrosshairColorG)
                objStreamWriter.WriteLine("cl_crosshaircolor_b {0}", CrosshairColorB)
            End If
            objStreamWriter.WriteLine("cl_crosshairthickness {0}", CrosshairThickness)
            objStreamWriter.WriteLine("cl_crosshair_drawoutline {0}", CrosshairOutline)
            objStreamWriter.WriteLine("cl_crosshair_outlinethickness {0}", CrosshairOutlineThickness)
            objStreamWriter.WriteLine("viewmodel_fov {0}", ViewmodelFOV)
            objStreamWriter.WriteLine("viewmodel_offset_x {0}", ViewmodelX)
            objStreamWriter.WriteLine("viewmodel_offset_y {0}", ViewmodelY)
            objStreamWriter.WriteLine("viewmodel_offset_z {0}", ViewmodelZ)
            objStreamWriter.WriteLine("cl_bob_lower_amt {0}", ViewmodelBob)
            objStreamWriter.WriteLine("cl_righthand {0}", Righthand)
            objStreamWriter.WriteLine("mat_setvideomode " + Height + " " + Width + " 0")
            objStreamWriter.Close()
        End If

        objStreamWriter.Close()
        Console.WriteLine("Do you want to reroll? Y/N")
        Dim UserInput As String = Console.ReadLine
        While UserInput <> "Y" And UserInput <> "N"
            Console.WriteLine("That is not valid, try again.")
            UserInput = Console.ReadLine
        End While
        If UserInput = "Y" Then
            AllInOne()
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

        Main()
    End Sub
    Sub SetFileNames()
        Console.Clear()
        Console.WriteLine("Select the one you want to change:")
        Console.WriteLine("1. Crosshair: {0}", xHairCFG)
        Console.WriteLine("2. Viewmodel {0}", ViewmodelCFG)
        Console.WriteLine("3. All in One: {0}", AllinOneCFG)
        Console.WriteLine("4. Exit")
        Dim UserInput As String = Console.ReadLine
        Select Case UserInput
            Case 1
                Console.Clear()
                Console.WriteLine("Enter your new file name now:")
                xHairCFG = Console.ReadLine
                SetFileNames()
            Case 2
                Console.Clear()
                Console.WriteLine("Enter your new file name now:")
                ViewmodelCFG = Console.ReadLine
                SetFileNames()
            Case 3
                Console.Clear()
                Console.WriteLine("Enter your new file name now:")
                AllinOneCFG = Console.ReadLine
                SetFileNames()
            Case 4
                Main()
            Case Else
                SetFileNames()
        End Select
    End Sub

    Sub PresetCvars()
        Console.Clear()
        Console.WriteLine("Which one would you like to change?")
        Console.WriteLine("Setting it to a non numeric value will void it.")
        Console.WriteLine("The only exception is Resolution, which using No will void it.")
        Console.WriteLine("")
        Console.WriteLine("1. Size: {0}", Cvars(0))
        Console.WriteLine("2. Gap: {0}", Cvars(1))
        Console.WriteLine("3. Style: {0}", Cvars(2))
        Console.WriteLine("4. Color: {0}", Cvars(3))
        Console.WriteLine("5. Color R: {0}", Cvars(4))
        Console.WriteLine("6. Color G: {0}", Cvars(5))
        Console.WriteLine("7. Color B: {0}", Cvars(6))
        Console.WriteLine("8. Thickness: {0}", Cvars(7))
        Console.WriteLine("9. Outline: {0}", Cvars(8))
        Console.WriteLine("10. Outline Thickness: {0}", Cvars(9))
        Console.WriteLine("11. FOV: {0}", Cvars(10))
        Console.WriteLine("12. X Offset: {0}", Cvars(11))
        Console.WriteLine("13. Y Offset: {0}", Cvars(12))
        Console.WriteLine("14. Z Offset: {0}", Cvars(13))
        Console.WriteLine("15. Bob: {0}", Cvars(14))
        Console.WriteLine("16. Righthand: {0}", Cvars(15))
        Console.WriteLine("17. Resolution: {0}", Cvars(15))
        Console.WriteLine("18. Exit")
        Dim UserInput As String = Console.ReadLine
        Console.Clear()
        Select Case UserInput
            Case 1
                Console.WriteLine("1. Size: {0}", Cvars(0))
                Cvars(0) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(0))
                PresetCvars()
            Case 2
                Console.WriteLine("2. Gap: {0}", Cvars(1))
                Cvars(1) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(1))
                PresetCvars()
            Case 3
                Console.WriteLine("3. Style: {0}", Cvars(2))
                Cvars(2) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(2))
                PresetCvars()
            Case 4
                Console.WriteLine("4. Color: {0}", Cvars(3))
                Cvars(3) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(3))
                PresetCvars()
            Case 5
                Console.WriteLine("5. Color R: {0}", Cvars(4))
                Cvars(4) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(4))
                PresetCvars()
            Case 6
                Console.WriteLine("6. Color G: {0}", Cvars(5))
                Cvars(5) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(5))
                PresetCvars()
            Case 7
                Console.WriteLine("7. Color B: {0}", Cvars(6))
                Cvars(6) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(6))
                PresetCvars()
            Case 8
                Console.WriteLine("8. Thickness: {0}", Cvars(7))
                Cvars(7) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(7))
                PresetCvars()
            Case 9
                Console.WriteLine("9. Outline: {0}", Cvars(8))
                Cvars(8) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(8))
                PresetCvars()
            Case 10
                Console.WriteLine("10. Outline Thickness: {0}", Cvars(9))
                Cvars(9) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(9))
                PresetCvars()
            Case 11
                Console.WriteLine("11. FOV: {0}", Cvars(10))
                Cvars(10) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(10))
                PresetCvars()
            Case 12
                Console.WriteLine("12. X Offset: {0}", Cvars(11))
                Cvars(11) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(11))
                PresetCvars()
            Case 13
                Console.WriteLine("13. Y Offset: {0}", Cvars(12))
                Cvars(12) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(12))
                PresetCvars()
            Case 14
                Console.WriteLine("14. Z Offset: {0}", Cvars(13))
                Cvars(13) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(13))
                PresetCvars()
            Case 15
                Console.WriteLine("15. Bob: {0}", Cvars(14))
                Cvars(14) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(14))
                PresetCvars()
            Case 16
                Console.WriteLine("16. Righthand: {0}", Cvars(15))
                Cvars(15) = Console.ReadLine
                If Not Cvars(15) = 1 And Not Cvars(15) = 0 Then
                    Console.WriteLine("That is not a valid input.")
                Else
                    Console.WriteLine("The new preset value is {0}", Cvars(15))
                End If
                Console.ReadLine()
                PresetCvars()
            Case 17
                Console.WriteLine("For resolutions, please enter them in this format.")
                Console.WriteLine("{Height}x{Width} e.g. 1024x768")
                Console.WriteLine("16. Resolution: {0}", Cvars(16))
                Cvars(15) = Console.ReadLine
                Console.WriteLine("The new preset value is {0}", Cvars(16))
                PresetCvars()
            Case 18
                Main()
            Case Else
                PresetCvars()
        End Select
    End Sub
End Module
