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
    Public Resolution As String
    Public CFGPath As String = "C:\Program Files (x86)\Steam\SteamApps\common\Counter-Strike Global Offensive\csgo\cfg"
    Public WriteCFG As Boolean = False
    Public Rand As New Random
    Sub Main()
        Console.Clear()
        Console.WriteLine("Welcome to the CFG generator.")
        Console.WriteLine("1. Crosshair")
        Console.WriteLine("2. Viewmodel")
        Console.WriteLine("3. Resolution")
        Console.WriteLine("4. All in One")
        Console.WriteLine("5. Set CFG Path")
        Console.WriteLine("6. Write to CFG's?: {0}", WriteCFG)
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

    Sub Crosshair()
        Console.Clear()
        Dim objStreamWriter As StreamWriter = New StreamWriter(CFGPath + "\randomxhair.cfg")
        Dim DecimalRand1 As Integer = Rand.Next(1, 11)
        Dim DecimalRand2 As Integer = Rand.Next(1, 3)

        CrosshairSize = Rand.Next(1, 5)
        If DecimalRand1 > 5 Then
            If DecimalRand1 = 1 Then
                CrosshairSize = CrosshairSize - 0.5
            Else
                CrosshairSize = CrosshairSize + 0.5
            End If
        End If

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

        CrosshairStyle = Rand.Next(0, 7)

        CrosshairColor = Rand.Next(1, 6)
        If CrosshairColor = 5 Then
            CrosshairColorR = Rand.Next(0, 256)
            CrosshairColorG = Rand.Next(0, 256)
            CrosshairColorB = Rand.Next(0, 256)
        End If

        CrosshairThickness = Rand.Next(0, 2)
        DecimalRand1 = Rand.Next(1, 11)
        DecimalRand2 = Rand.Next(1, 3)
        If DecimalRand1 > 5 Then
            If DecimalRand1 = 1 Then
                CrosshairThickness = CrosshairThickness - 0.5
            Else
                CrosshairThickness = CrosshairThickness + 0.5
            End If
        End If

        CrosshairOutline = Rand.Next(0, 2)
        If CrosshairOutline = 1 Then
            DecimalRand2 = Rand.Next(1, 3)
            If DecimalRand2 = 1 Then
                CrosshairOutlineThickness = 0.5
            Else
                CrosshairOutlineThickness = 1
            End If
        End If
        DecimalRand1 = Rand.Next(1, 11)
        DecimalRand2 = Rand.Next(1, 3)
        If DecimalRand1 > 5 Then
            If DecimalRand1 = 1 Then
                CrosshairOutlineThickness = CrosshairOutlineThickness - 0.5
            Else
                CrosshairOutlineThickness = CrosshairOutlineThickness + 0.5
            End If
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
        Dim objStreamWriter As StreamWriter = New StreamWriter(CFGPath + "\randomviewmodel.cfg")
        Dim DecimalRand1 As Integer = Rand.Next(1, 11)
        Dim DecimalRand2 As Integer = Rand.Next(1, 4)

        If DecimalRand2 = 1 Then
            ViewmodelFOV = 60
        End If
        If DecimalRand2 = 2 Then
            ViewmodelFOV = 65
        End If
        If DecimalRand2 = 3 Then
            ViewmodelFOV = 68
        End If

        DecimalRand1 = Rand.Next(1, 3)
        ViewmodelX = Rand.Next(0, 3)
        If DecimalRand1 > 5 Then
            If DecimalRand1 = 1 Then
                ViewmodelX = ViewmodelX - 0.5
            Else
                ViewmodelX = ViewmodelX + 0.5
            End If
        End If

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

        DecimalRand2 = Rand.Next(1, 3)
        If DecimalRand2 = 1 Then
            ViewmodelBob = 5
        Else
            ViewmodelBob = 21
        End If

        Console.WriteLine("FOV: {0}", ViewmodelFOV)
        Console.WriteLine("X Offset: {0}", ViewmodelX)
        Console.WriteLine("Y Offset: {0}", ViewmodelY)
        Console.WriteLine("Z Offset: {0}", ViewmodelZ)
        Console.WriteLine("Bob: {0}", ViewmodelBob)

        If WriteCFG = True Then
            objStreamWriter.WriteLine("viewmodel_fov {0}", ViewmodelFOV)
            objStreamWriter.WriteLine("viewmodel_offset_x {0}", ViewmodelX)
            objStreamWriter.WriteLine("viewmodel_offset_y {0}", ViewmodelY)
            objStreamWriter.WriteLine("viewmodel_offset_z {0}", ViewmodelZ)
            objStreamWriter.WriteLine("cl_bob_lower_amt {0}", ViewmodelBob)
            objStreamWriter.Close()
        End If

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
        Dim ResArray(4) As String
        ResArray(0) = "1920x1080"
        ResArray(1) = "1280x1024"
        ResArray(2) = "1280x960"
        ResArray(3) = "1024x768"
        ResArray(4) = "800x600"
        Dim ChosenRes(4) As String
        For i = 0 To 4
            DecimalRand1 = Rand.Next(0, 5)
            While ChosenRes.Contains(ResArray(DecimalRand1))
                DecimalRand1 = Rand.Next(0, 5)
            End While
            ChosenRes(i) = ResArray(DecimalRand1)
        Next

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
        Dim objStreamWriter As StreamWriter = New StreamWriter(CFGPath + "\randomallinone.cfg")
        Dim DecimalRand1 As Integer = Rand.Next(1, 11)
        Dim DecimalRand2 As Integer = Rand.Next(1, 3)

        CrosshairSize = Rand.Next(1, 5)
        If DecimalRand1 > 5 Then
            If DecimalRand1 = 1 Then
                CrosshairSize = CrosshairSize - 0.5
            Else
                CrosshairSize = CrosshairSize + 0.5
            End If
        End If

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

        CrosshairStyle = Rand.Next(0, 7)

        CrosshairColor = Rand.Next(1, 6)
        If CrosshairColor = 5 Then
            CrosshairColorR = Rand.Next(0, 256)
            CrosshairColorG = Rand.Next(0, 256)
            CrosshairColorB = Rand.Next(0, 256)
        End If

        CrosshairThickness = Rand.Next(0, 2)
        DecimalRand1 = Rand.Next(1, 11)
        DecimalRand2 = Rand.Next(1, 3)
        If DecimalRand1 > 5 Then
            If DecimalRand1 = 1 Then
                CrosshairThickness = CrosshairThickness - 0.5
            Else
                CrosshairThickness = CrosshairThickness + 0.5
            End If
        End If

        CrosshairOutline = Rand.Next(0, 2)
        If CrosshairOutline = 1 Then
            DecimalRand2 = Rand.Next(1, 3)
            If DecimalRand2 = 1 Then
                CrosshairOutlineThickness = 0.5
            Else
                CrosshairOutlineThickness = 1
            End If
        End If
        DecimalRand1 = Rand.Next(1, 11)
        DecimalRand2 = Rand.Next(1, 3)
        If DecimalRand1 > 5 Then
            If DecimalRand1 = 1 Then
                CrosshairOutlineThickness = CrosshairOutlineThickness - 0.5
            Else
                CrosshairOutlineThickness = CrosshairOutlineThickness + 0.5
            End If
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

        If DecimalRand2 = 1 Then
            ViewmodelFOV = 60
        End If
        If DecimalRand2 = 2 Then
            ViewmodelFOV = 65
        End If
        If DecimalRand2 = 3 Then
            ViewmodelFOV = 68
        End If

        DecimalRand1 = Rand.Next(1, 3)
        ViewmodelX = Rand.Next(0, 3)
        If DecimalRand1 > 5 Then
            If DecimalRand1 = 1 Then
                ViewmodelX = ViewmodelX - 0.5
            Else
                ViewmodelX = ViewmodelX + 0.5
            End If
        End If

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

        DecimalRand2 = Rand.Next(1, 3)
        If DecimalRand2 = 1 Then
            ViewmodelBob = 5
        Else
            ViewmodelBob = 21
        End If
        Console.WriteLine("Viewmodel:")
        Console.WriteLine("FOV: {0}", ViewmodelFOV)
        Console.WriteLine("X Offset: {0}", ViewmodelX)
        Console.WriteLine("Y Offset: {0}", ViewmodelY)
        Console.WriteLine("Z Offset: {0}", ViewmodelZ)
        Console.WriteLine("Bob: {0}", ViewmodelBob)
        Console.WriteLine("")
        DecimalRand1 = Rand.Next(1, 11)
        Dim ResArray(4) As String
        ResArray(0) = "1920x1080"
        ResArray(1) = "1280x1024"
        ResArray(2) = "1280x960"
        ResArray(3) = "1024x768"
        ResArray(4) = "800x600"
        Dim ChosenRes(4) As String
        For i = 0 To 4
            DecimalRand1 = Rand.Next(0, 5)
            While ChosenRes.Contains(ResArray(DecimalRand1))
                DecimalRand1 = Rand.Next(0, 5)
            End While
            ChosenRes(i) = ResArray(DecimalRand1)
        Next
        Console.WriteLine("Resolution:")
        Console.WriteLine(ChosenRes(0))
        Console.WriteLine("")

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
            If ChosenRes(0) = "800x600" Then
                objStreamWriter.WriteLine("mat_setvideomode 800 600 0")
            End If
            If ChosenRes(0) = "1024x768" Then
                objStreamWriter.WriteLine("mat_setvideomode 1024 768 0")
            End If
            If ChosenRes(0) = "1280x960" Then
                objStreamWriter.WriteLine("mat_setvideomode 1280 960 0")
            End If
            If ChosenRes(0) = "1280x1024" Then
                objStreamWriter.WriteLine("mat_setvideomode 1280 1024 0")
            End If
            If ChosenRes(0) = "1920x1080" Then
                objStreamWriter.WriteLine("mat_setvideomode 1920 1080 0")
            End If
            objStreamWriter.Close()
        End If

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
End Module
