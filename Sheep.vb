Imports System.Runtime.InteropServices
Imports VB = Microsoft.VisualBasic

Public Class Sheep
    'SOME API FUNCTIONS TO MANAGE WALKING OVER WINDOWS
    Private Declare Function IsWindowVisible Lib "user32" (ByVal hWnd As IntPtr) As Boolean
    Private Declare Function GetTopWindow Lib "user32.dll" (ByVal hWnd As Long) As Long
    Private Declare Function GetWindow Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal uCmd As UInteger) As IntPtr
    Private Declare Function EnumWindows Lib "user32.dll" (ByVal lpfn As EnumWindowsDelegate, ByVal lParam As Integer) As Boolean
    Private Declare Function GetWindowText Lib "user32" Alias "GetWindowTextA" (ByVal hwnd As IntPtr, ByVal lpString As System.Text.StringBuilder, ByVal cch As Integer) As Integer
    Private Delegate Function EnumWindowsDelegate(ByVal hWnd As System.IntPtr, ByVal parametro As Integer) As Boolean
    Private Declare Function DwmGetWindowAttribute Lib "dwmapi.dll" (ByVal hwnd As IntPtr, ByVal dwAttribute As Integer, <Out> ByRef pvAttribute As RECT, ByVal cbAttribute As Integer) As Integer

    'Better than GetWindowRect() because not messes with windows shadows
    Public Shared Function GetWindowRectangle(ByVal hWnd As IntPtr) As RECT
        Dim rect As RECT
        Dim size As Integer = Marshal.SizeOf(GetType(RECT))
        DwmGetWindowAttribute(hWnd, 9, rect, size)
        Return rect
    End Function

    'STRUCTURE FOR WINDOW RECT
    Public Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    'ANIMATION BITMAP ARRAYS 
    Dim petDrag As Bitmap() = New Bitmap(3) {My.Resources.Atrapado_izq, My.Resources.Atrapado_centro, My.Resources.Atrapado_der, My.Resources.Atrapado_centro}
    Dim petPissFromLeft As Bitmap() = New Bitmap(15) {My.Resources.Walk_izq2, My.Resources.turnbackfromleft, My.Resources.back, My.Resources.Pis1, My.Resources.Pis2, My.Resources.Pis3, My.Resources.Pis4, My.Resources.Pis3, My.Resources.Pis4, My.Resources.Pis3, My.Resources.Pis4, My.Resources.Pis2, My.Resources.Pis1, My.Resources.back, My.Resources.turnbackfromleft, My.Resources.Walk_izq2}
    Dim petPissFromRight As Bitmap() = New Bitmap(15) {My.Resources.Walk_der2, My.Resources.turnbackfromright, My.Resources.back, My.Resources.Pis1, My.Resources.Pis2, My.Resources.Pis3, My.Resources.Pis4, My.Resources.Pis3, My.Resources.Pis4, My.Resources.Pis3, My.Resources.Pis4, My.Resources.Pis2, My.Resources.Pis1, My.Resources.back, My.Resources.turnbackfromright, My.Resources.Walk_der2}
    Dim petWalkLeft As Bitmap() = New Bitmap(1) {My.Resources.Walk_izq2, My.Resources.Walk_izq3}
    Dim petWalkRight As Bitmap() = New Bitmap(1) {My.Resources.Walk_der2, My.Resources.Walk_der3}
    Dim petSleepFromLeft As Bitmap() = New Bitmap(18) {My.Resources.Walk_izq2, My.Resources.Walk_izq1, My.Resources.Frente, My.Resources.Sleeping1, My.Resources.Sleeping1, My.Resources.Sleeping2, My.Resources.Sleeping2, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping2, My.Resources.Sleeping1, My.Resources.Frente, My.Resources.Walk_izq1, My.Resources.Walk_izq2}
    Dim petSleepFromRight As Bitmap() = New Bitmap(18) {My.Resources.Walk_der2, My.Resources.Walk_der1, My.Resources.Frente, My.Resources.Sleeping1, My.Resources.Sleeping1, My.Resources.Sleeping2, My.Resources.Sleeping2, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping3, My.Resources.Sleeping2, My.Resources.Sleeping1, My.Resources.Frente, My.Resources.Walk_der1, My.Resources.Walk_der2}
    Dim GoodBoyFromLeft As Bitmap() = New Bitmap(13) {My.Resources.Walk_izq2, My.Resources.Walk_izq1, My.Resources.Frente, My.Resources.perrito2, My.Resources.perrito, My.Resources.perrito2, My.Resources.perrito, My.Resources.perrito2, My.Resources.perrito, My.Resources.perrito2, My.Resources.perrito, My.Resources.Frente, My.Resources.Walk_izq1, My.Resources.Walk_izq2}
    Dim GoodBoyFromRight As Bitmap() = New Bitmap(13) {My.Resources.Walk_der2, My.Resources.Walk_der1, My.Resources.Frente, My.Resources.perrito2, My.Resources.perrito, My.Resources.perrito2, My.Resources.perrito, My.Resources.perrito2, My.Resources.perrito, My.Resources.perrito2, My.Resources.perrito, My.Resources.Frente, My.Resources.Walk_der1, My.Resources.Walk_der2}
    Dim petYawnFromLeft As Bitmap() = New Bitmap(19) {My.Resources.Walk_izq2, My.Resources.Yawn1, My.Resources.Yawn2, My.Resources.Yawn3, My.Resources.Yawn2, My.Resources.Yawn2, My.Resources.Yawn3, My.Resources.Yawn3, My.Resources.Yawn4, My.Resources.Yawn5, My.Resources.Yawn4, My.Resources.Yawn5, My.Resources.Yawn4, My.Resources.Yawn5, My.Resources.Yawn3, My.Resources.Yawn2, My.Resources.Yawn3, My.Resources.Yawn2, My.Resources.Yawn1, My.Resources.Walk_izq2}
    Dim petYawnFromRight As Bitmap() = New Bitmap(27) {My.Resources.Walk_der2, My.Resources.Walk_der1, My.Resources.Frente, My.Resources.Walk_izq1, My.Resources.Walk_izq2, My.Resources.Yawn1, My.Resources.Yawn2, My.Resources.Yawn3, My.Resources.Yawn2, My.Resources.Yawn2, My.Resources.Yawn3, My.Resources.Yawn3, My.Resources.Yawn4, My.Resources.Yawn5, My.Resources.Yawn4, My.Resources.Yawn5, My.Resources.Yawn4, My.Resources.Yawn5, My.Resources.Yawn3, My.Resources.Yawn2, My.Resources.Yawn3, My.Resources.Yawn2, My.Resources.Yawn1, My.Resources.Walk_izq2, My.Resources.Walk_izq1, My.Resources.Frente, My.Resources.Walk_der1, My.Resources.Walk_der2}
    Dim petRunningLeft As Bitmap() = New Bitmap(1) {My.Resources.Running1, My.Resources.Running2}
    Dim petRunningRight As Bitmap() = New Bitmap(1) {My.Resources.Running3, My.Resources.Running4}
    Dim petCrashLeft As Bitmap() = New Bitmap(9) {My.Resources.crash_left, My.Resources.crash_left2, My.Resources.crash_left3, My.Resources.crash_left4, My.Resources.crash_left5, My.Resources.crash_left6, My.Resources.crash_left7, My.Resources.crash_left8, My.Resources.crash_left9, My.Resources.Walk_izq2}
    Dim petCrashRight As Bitmap() = New Bitmap(9) {My.Resources.crash_right, My.Resources.crash_right2, My.Resources.crash_right3, My.Resources.crash_right4, My.Resources.crash_right5, My.Resources.crash_right6, My.Resources.crash_right7, My.Resources.crash_right8, My.Resources.crash_right9, My.Resources.Walk_der2}
    Dim petTurnRight As Bitmap() = New Bitmap(4) {My.Resources.Walk_izq2, My.Resources.Walk_izq1, My.Resources.Frente, My.Resources.Walk_der1, My.Resources.Walk_der2}
    Dim petTurnLeft As Bitmap() = New Bitmap(4) {My.Resources.Walk_der2, My.Resources.Walk_der1, My.Resources.Frente, My.Resources.Walk_izq1, My.Resources.Walk_izq2}
    Dim petTurnRightFromFalling As Bitmap() = New Bitmap(1) {My.Resources.Walk_der1, My.Resources.Walk_der2}
    Dim petTurnLeftFromFalling As Bitmap() = New Bitmap(1) {My.Resources.Walk_izq1, My.Resources.Walk_izq2}
    Dim petFalling As Bitmap() = New Bitmap(1) {My.Resources.caida, My.Resources.caida2}

    'APPLICATION RUNTIME VARIABLES
    Dim Frame As Integer = 0
    Dim RunningSteps As Integer
    Dim MouseX As Integer
    Dim MouseY As Integer
    Dim randNum As New Random
    Dim FallSpeed As Integer
    Dim BorderLeft As Integer
    Dim BorderRight As Integer

    Dim SheepDirection As String = ""
    Dim Is_Dragging As Boolean = False
    Dim Is_Falling As Boolean = False
    Dim Is_CrashBottom As Boolean = False
    Dim Is_Walking As Boolean = True
    Dim Is_Running As Boolean = False
    Dim Is_Turning As Boolean = False
    Dim Is_TurningFromFall As Boolean = False
    Dim Is_Crashing As Boolean = False
    Dim Is_Yawning As Boolean = False
    Dim Is_Sleeping As Boolean = False
    Dim Is_Pissing As Boolean = False
    Dim Is_GoodBoy As Boolean = False
    Dim Is_Dead As Boolean = False

    'LIST OF OPEN WINDOWS
    Dim colWin As New System.Collections.Specialized.StringDictionary


    Dim ventanaaseguir As IntPtr
    Dim currentWindowSize As RECT

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H2000000
            cp.ExStyle = cp.ExStyle Or &H80      'WS_EX_TOOLWINDOW  <- Remove sheep(s) from ALT-TAB list.
            cp.ExStyle = cp.ExStyle Or &H8       'WS_EX_TOPMOST     <- Set sheep(s) TopMost on Z index.
            cp.ExStyle = cp.ExStyle Or &H80000   'WS_EX_LAYERED     <- Increase overall paint performance (Still not sure if it works).
            'cp.ExStyle = cp.ExStyle Or &H20     'WS_EX_TRANSPARENT <- Do not draw window (Makes sheep unclickable, discarded).
            cp.ExStyle = cp.ExStyle Or &H8000000 'WS_EX_NOACTIVATE  <- prevent focus when created (Not sure if it's nedded).
            cp.Style = cp.Style Or &H80000000    'WS_POPUP          <- Unsure about this shit.
            Return cp
        End Get
    End Property

    Public Sub wait(ByVal seconds As Single)
        Static start As Single = VB.Timer()
        Do While VB.Timer() < start + seconds
            System.Windows.Forms.Application.DoEvents()
        Loop
    End Sub

    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    Private Function EnumerateOpenWindows(ByVal hWnd As System.IntPtr, ByVal parametro As Integer) As Boolean
        Dim Titulo As New System.Text.StringBuilder(New String(" "c, 256))
        Dim ret As Integer = GetWindowText(hWnd, Titulo, Titulo.Length)
        Dim nombreVentana As String = Titulo.ToString.Substring(0, ret)
        If Not hWnd = IntPtr.Zero AndAlso IsWindowVisible(hWnd) AndAlso nombreVentana <> Nothing AndAlso nombreVentana.Length > 0 Then
            colWin.Add(hWnd, nombreVentana)
        End If
        Return True
    End Function

    Private Sub SetRandomDirection()
        Dim Direction As Integer = GetRandom(0, 1)
        Select Case Direction
            Case 0
                SheepDirection = "Left"
            Case 1
                SheepDirection = "Right"
        End Select
    End Sub

    Private Function CheckWindowBordersForY() As Boolean
        For Each s As String In colWin.Keys
            Dim myRect As RECT = GetWindowRectangle(s)
            If Not myRect.Top = 0 Then
                BorderLeft = myRect.Left + 10
                BorderRight = myRect.Right - Me.Width - 10
                If Me.Location.Y + Me.Height < myRect.Top AndAlso
                    Me.Location.Y + Me.Height + 9 >= myRect.Top AndAlso
                    Me.Location.X >= myRect.Left AndAlso
                        Me.Location.X <= myRect.Right AndAlso
                        CheckTopWindow(s) = True Then
                    ventanaaseguir = s
                    currentWindowSize = GetWindowRectangle(s)
                    Me.Location = New Point(Me.Location.X, myRect.Top - Me.Width - 1)
                    Return True
                Else
                    'ventanaaseguir = Nothing
                    BorderLeft = 0
                    BorderRight = Screen.PrimaryScreen.WorkingArea.Width - Me.Width
                End If
            End If
        Next
    End Function

    Private Function CheckTopWindow(ByVal hwndWindow As IntPtr) As Boolean
        If Not hwndWindow = IntPtr.Zero Then
            Dim Ventana As RECT = GetWindowRectangle(hwndWindow)
            Dim NextWindowHandle As IntPtr = GetTopWindow(IntPtr.Zero)
            While NextWindowHandle <> IntPtr.Zero
                If NextWindowHandle = hwndWindow Then
                    Return True
                End If
                Dim Sheepx As RECT = GetWindowRectangle(NextWindowHandle)
                If IsWindowVisible(NextWindowHandle) Then
                    If Sheepx.Top < Ventana.Top AndAlso Sheepx.Bottom > Ventana.Top + 20 Then
                        Return False
                    End If
                End If
                    NextWindowHandle = GetWindow(NextWindowHandle, 2) 'Get next window
            End While
        End If
        Return False
    End Function

    Private Function FollowWindow(ByVal hwndWindow As IntPtr) As Boolean
        If Not hwndWindow = IntPtr.Zero Then
            Dim Rectanguloventana As RECT = GetWindowRectangle(hwndWindow)

            If Rectanguloventana.Top = 0 AndAlso Rectanguloventana.Bottom = 0 Then
                Return False
            End If

            If currentWindowSize.Top <> Rectanguloventana.Top OrElse currentWindowSize.Left <> Rectanguloventana.Left OrElse currentWindowSize.Right <> Rectanguloventana.Right Then

                If Rectanguloventana.Right - Rectanguloventana.Left = currentWindowSize.Right - currentWindowSize.Left Then
                    Top -= (currentWindowSize.Top - Rectanguloventana.Top)
                    Left -= (currentWindowSize.Left - Rectanguloventana.Left)
                Else
                    Top -= (currentWindowSize.Top - Rectanguloventana.Top)
                    Left = Rectanguloventana.Left + (Left - currentWindowSize.Left) * (Rectanguloventana.Right - Rectanguloventana.Left) / (currentWindowSize.Right - currentWindowSize.Left)
                End If

                currentWindowSize = Rectanguloventana

                'Me.Location = New Point(Me.Location.X, Rectanguloventana.Top - Me.Height - 1)
                Return True
            End If
        End If

        'Return False
    End Function

    Private Sub RandomAnimation()
        PauseAllAnimations()
        Dim Ani As Integer = GetRandom(0, 5)
        Select Case Ani
            Case 0
                Is_Pissing = True
            Case 1
                Is_Sleeping = True
            Case 2
                Is_Yawning = True
            Case 3
                RunningSteps = 100
                Is_Running = True
            Case 4
                Is_GoodBoy = True
        End Select
    End Sub

    Public Sub PauseAllAnimations()
        Is_Dragging = False
        Is_Falling = False
        Is_Crashing = False
        Is_CrashBottom = False
        Is_Walking = False
        Is_Running = False
        Is_Turning = False
        Is_TurningFromFall = False
        Is_Yawning = False
        Is_Sleeping = False
        Is_Pissing = False
        Is_GoodBoy = False
        Is_Dead = False
        Frame = 0
        FallSpeed = 0
    End Sub

    Private Sub Debug_Tick(sender As Object, e As EventArgs) Handles Debug.Tick
        ToolTip1.Show(
"CheckForY: " & CheckWindowBordersForY() & vbCrLf &
"CheckTopw: " & CheckTopWindow(ventanaaseguir) & vbCrLf &
"FollowWin: " & FollowWindow(ventanaaseguir) & vbCrLf &
"Anim. Interval: " & Animation.Interval.ToString & vbCrLf &
"SheepDirection: " & SheepDirection & vbCrLf &
"-----------------------------" & vbCrLf &
"Is_Dragging: " & Is_Dragging & vbCrLf &
"Is_Falling: " & Is_Falling & vbCrLf &
"Is_Walking: " & Is_Walking & vbCrLf &
"Is_Running: " & Is_Running & vbCrLf &
"Is_Crashing: " & Is_Crashing & vbCrLf &
"Is_CrashBottom: " & Is_CrashBottom & vbCrLf &
"Is_Sleeping: " & Is_Sleeping & vbCrLf &
"Is_pissing: " & Is_Pissing & vbCrLf &
"Is_Yawning: " & Is_Yawning & vbCrLf &
"Is_GoodBoy: " & Is_GoodBoy & vbCrLf &
"-----------------------------" & vbCrLf &
"Is_Turning: " & Is_Turning & vbCrLf &
"Is_TurningFromFall: " & Is_TurningFromFall & vbCrLf &
"-----------------------------" & vbCrLf &
"Me.X: " & Me.Location.X & vbCrLf &
"Me.Y: " & Me.Location.Y & vbCrLf &
"-----------------------------" & vbCrLf &
"BorderLeft: " & BorderLeft & vbCrLf &
"BorderRight: " & BorderRight,
PictureBox1, -40, -380, 100000)
    End Sub

    Private Sub Sheep_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Set transparency key on form for transparent PNG images
        Me.BackColor = Color.Magenta
        Me.TransparencyKey = Color.Magenta

        'Set a sheep random startup screen position
        Dim x As Integer = GetRandom(300, Screen.PrimaryScreen.WorkingArea.Width - 300)
        Dim y As Integer = GetRandom(0, 300)
        Me.Location = New Point(x, y)

        SetRandomDirection()

        'Load sheep falling image and start timer
        PictureBox1.Image = My.Resources.caida
        Frame = 0
        FallSpeed = 0
        Is_Falling = True
        Animation.Enabled = True
    End Sub

    Private Sub Animation_Tick(sender As Object, e As EventArgs) Handles Animation.Tick
        'Me.TopMost = True




        If Is_Falling = True Then '-------------------------------------------------------------------------------------------------------------------------
            ventanaaseguir = Nothing
            Animation.Interval = 10
            If Frame < petFalling.Count - 1 Then
                PictureBox1.Image = petFalling(Frame)
                Frame = Frame + 1
            Else
                PictureBox1.Image = petFalling(Frame)
                Frame = 0
            End If
            If FallSpeed > 8 Then
                FallSpeed = 8
            Else
                FallSpeed = FallSpeed + 1
            End If
            Dim Y As Integer = Me.Location.Y
            Y = Y + FallSpeed
            Me.Location = New System.Drawing.Point(Me.Location.X, Y)

            If CheckWindowBordersForY() = True Then
                PauseAllAnimations()
                Is_CrashBottom = True
                Exit Sub
            ElseIf Me.Location.Y >= (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) Then
                PauseAllAnimations()
                Me.Location = New Point(Me.Location.X, Screen.PrimaryScreen.WorkingArea.Height - Me.Height)
                Is_CrashBottom = True
                Exit Sub
            End If




        ElseIf Is_CrashBottom = True Then '------------------------------------------------------------------------------------------------------------------------------------
            Animation.Interval = 1000
            PauseAllAnimations()
            PictureBox1.Image = My.Resources.suelo
            wait(2)
            PictureBox1.Image = My.Resources.Frente
            wait(2)
            Is_TurningFromFall = True



        ElseIf Is_TurningFromFall = True Then '---------------------------------------------------------------------------------------------------------------------------
            Animation.Interval = 200
            If SheepDirection = "Left" Then
                If Frame < petTurnLeftFromFalling.Count - 1 Then
                    PictureBox1.Image = petTurnLeftFromFalling(Frame)
                    Frame = Frame + 1
                Else
                    PauseAllAnimations()
                    Is_Walking = True
                    Exit Sub
                End If
            ElseIf SheepDirection = "Right" Then
                If Frame < petTurnRightFromFalling.Count - 1 Then
                    PictureBox1.Image = petTurnRightFromFalling(Frame)
                    Frame = Frame + 1
                Else
                    PauseAllAnimations()
                    Is_Walking = True
                    Exit Sub
                End If
            End If

            If CheckWindowBordersForY() = False And Me.Location.Y < Screen.PrimaryScreen.WorkingArea.Height - Me.Height Then
                PauseAllAnimations()
                PictureBox1.Image = My.Resources.soltar
                wait(1)
                Is_Falling = True
                Exit Sub
            End If






        ElseIf Is_Walking = True Then '---------------------------------------------------------------------------------------------------------------------------
            Animation.Interval = 200

            If SheepDirection = "Left" Then
                If Me.Location.X - 10 <= BorderLeft Then
                    PauseAllAnimations()
                    SheepDirection = "Right"
                    Is_Turning = True
                    Exit Sub
                Else
                    Me.Location = New System.Drawing.Point(Me.Location.X - 3, Me.Location.Y)
                    If Frame < petWalkLeft.Count - 1 Then
                        PictureBox1.Image = petWalkLeft(0)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = petWalkLeft(1)
                        Frame = 0
                    End If
                End If
            ElseIf SheepDirection = "Right" Then
                If Me.Location.X + Me.Width >= BorderRight Then
                    PauseAllAnimations()
                    SheepDirection = "Left"
                    Is_Turning = True
                    Exit Sub
                Else
                    Me.Location = New System.Drawing.Point(Me.Location.X + 3, Me.Location.Y)
                    If Frame < petWalkRight.Count - 1 Then
                        PictureBox1.Image = petWalkRight(0)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = petWalkRight(1)
                        Frame = 0
                    End If
                End If
            End If



            If CheckWindowBordersForY() = False And Me.Location.Y < Screen.PrimaryScreen.WorkingArea.Height - Me.Height Then
                PauseAllAnimations()
                PictureBox1.Image = My.Resources.soltar
                wait(1)
                Is_Falling = True
                Exit Sub
            End If











        ElseIf Is_Turning = True Then '-------------------------------------------------------------------------------------------------------------------------------
            Animation.Interval = 200
            If SheepDirection = "Left" Then
                If Frame < petTurnLeft.Count - 1 Then
                    PictureBox1.Image = petTurnLeft(Frame)
                    Frame = Frame + 1
                Else
                    PauseAllAnimations()
                    Is_Walking = True
                    Exit Sub
                End If
            ElseIf SheepDirection = "Right" Then
                If Frame < petTurnRight.Count - 1 Then
                    PictureBox1.Image = petTurnRight(Frame)
                    Frame = Frame + 1
                Else
                    PauseAllAnimations()
                    Is_Walking = True
                    Exit Sub
                End If
            End If

            If CheckWindowBordersForY() = False And Me.Location.Y < Screen.PrimaryScreen.WorkingArea.Height - Me.Height Then
                PauseAllAnimations()
                PictureBox1.Image = My.Resources.soltar
                wait(1)
                Is_Falling = True
                Exit Sub
            End If



        ElseIf Is_Yawning = True Then '------------------------------------------------------------------------------------------------------------------------------
            Animation.Interval = 1000
            If SheepDirection = "Left" Then
                If Frame < petYawnFromLeft.Count - 1 Then
                    PictureBox1.Image = petYawnFromLeft(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = petYawnFromLeft(Frame)
                    Frame = 0
                End If

            ElseIf SheepDirection = "Right" Then
                If Frame < petYawnFromRight.Count - 1 Then
                    PictureBox1.Image = petYawnFromRight(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = petYawnFromRight(Frame)
                    Frame = 0
                End If
            End If

            If CheckWindowBordersForY() = False And Me.Location.Y < Screen.PrimaryScreen.WorkingArea.Height - Me.Height Then
                PauseAllAnimations()
                PictureBox1.Image = My.Resources.soltar
                wait(1)
                Is_Falling = True
                Exit Sub
            End If



        ElseIf Is_Running = True Then '---------------------------------------------------HERE-----------------------------------------------------------------------------------
            Animation.Interval = 100

            RunningSteps = RunningSteps - 1
            If RunningSteps = 0 Then
                PauseAllAnimations()
                Is_Walking = True
                Exit Sub
            End If

            If SheepDirection = "Left" Then
                If Me.Location.X <= 0 Then
                    PauseAllAnimations()
                    SheepDirection = "Right"
                    Is_Crashing = True
                    Exit Sub
                ElseIf Me.Location.X <= BorderLeft Then
                    PauseAllAnimations()
                    SheepDirection = "Right"
                    Is_Turning = True
                Else
                    Dim X As Integer = Me.Location.X
                    X = X - 5
                    Me.Location = New System.Drawing.Point(X, Me.Location.Y)
                    If Frame = 0 Then
                        PictureBox1.Image = petRunningLeft(0)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = petRunningLeft(1)
                        Frame = 0
                    End If
                End If
            ElseIf SheepDirection = "Right" Then
                If Me.Location.X >= Screen.PrimaryScreen.WorkingArea.Width - Me.Width Then
                    PauseAllAnimations()
                    SheepDirection = "Left"
                    Is_Crashing = True
                ElseIf Me.Location.X >= BorderRight Then
                    PauseAllAnimations()
                    SheepDirection = "Left"
                    Is_Turning = True
                Else
                    Dim X As Integer = Me.Location.X
                    X = X + 5
                    Me.Location = New System.Drawing.Point(X, Me.Location.Y)
                    If Frame = 0 Then
                        PictureBox1.Image = petRunningRight(0)
                        Frame = Frame + 1
                    Else
                        PictureBox1.Image = petRunningRight(1)
                        Frame = 0
                    End If
                End If
            End If

            If CheckWindowBordersForY() = False And Me.Location.Y < Screen.PrimaryScreen.WorkingArea.Height - Me.Height Then
                PauseAllAnimations()
                PictureBox1.Image = My.Resources.soltar
                wait(1)
                Is_Falling = True
                Exit Sub
            End If




        ElseIf Is_Crashing = True Then '------------------------------------------------------------------------------------------------------------------------
            Animation.Interval = 100

            If SheepDirection = "Right" Then
                If Frame < petCrashLeft.Count - 1 Then
                    PictureBox1.Image = petCrashLeft(Frame)
                    Frame = Frame + 1
                    Dim X As Integer = Me.Location.X
                    X = X + 10
                    Me.Location = New System.Drawing.Point(X, Me.Location.Y)
                Else
                    PauseAllAnimations()
                    Is_Turning = True
                End If
            ElseIf SheepDirection = "Left" Then
                If Frame < petCrashRight.Count - 1 Then
                    PictureBox1.Image = petCrashRight(Frame)
                    Frame = Frame + 1
                    Dim X As Integer = Me.Location.X
                    X = X - 10
                    Me.Location = New System.Drawing.Point(X, Me.Location.Y)
                Else
                    PauseAllAnimations()
                    Is_Turning = True
                End If
            End If







        ElseIf Is_Dragging = True Then '---------------------------------------------------------------------------------------------------------------------------------------
            Animation.Interval = 200
            If Frame < petDrag.Count - 1 Then
                PictureBox1.Image = petDrag(Frame)
                Frame = Frame + 1
            Else
                PictureBox1.Image = petDrag(Frame)
                Frame = 0
            End If







        ElseIf Is_Pissing = True Then '------------------------------------------------------------------------------------------------------------------------------------------
            Animation.Interval = 500
            If SheepDirection = "Left" Then
                If Frame < petPissFromLeft.Count - 1 Then
                    PictureBox1.Image = petPissFromLeft(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = petPissFromLeft(Frame)
                    PauseAllAnimations()
                    Is_Walking = True
                End If
            ElseIf SheepDirection = "Right" Then
                If Frame < petPissFromRight.Count - 1 Then
                    PictureBox1.Image = petPissFromRight(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = petPissFromRight(Frame)
                    PauseAllAnimations()
                    Is_Walking = True
                End If
            End If


            If CheckWindowBordersForY() = False And Me.Location.Y < Screen.PrimaryScreen.WorkingArea.Height - Me.Height Then
                PauseAllAnimations()
                PictureBox1.Image = My.Resources.soltar
                wait(1)
                Is_Falling = True
                Exit Sub
            End If



        ElseIf Is_Sleeping = True Then '---------------------------------------------------------------------------------------------------------------------------
            Animation.Interval = 800
            If SheepDirection = "Left" Then
                If Frame < petSleepFromLeft.Count - 1 Then
                    PictureBox1.Image = petSleepFromLeft(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = petSleepFromLeft(Frame)
                    Frame = 0
                    PauseAllAnimations()
                    Is_Walking = True
                End If
            ElseIf SheepDirection = "Right" Then
                If Frame < petSleepFromRight.Count - 1 Then
                    PictureBox1.Image = petSleepFromRight(Frame)
                    Frame = Frame + 1
                Else
                    PictureBox1.Image = petSleepFromRight(Frame)
                    Frame = 0
                    PauseAllAnimations()
                    Is_Walking = True
                End If
            End If


            If CheckWindowBordersForY() = False And Me.Location.Y < Screen.PrimaryScreen.WorkingArea.Height - Me.Height Then
                PauseAllAnimations()
                PictureBox1.Image = My.Resources.soltar
                wait(1)
                Is_Falling = True
                Exit Sub
            End If









        ElseIf Is_GoodBoy = True Then '------------------------------------------------------------------------------------------------------------------------------
            If SheepDirection = "Left" Then
                If Frame < GoodBoyFromLeft.Count - 1 Then
                    PictureBox1.Image = GoodBoyFromLeft(Frame)
                    Frame = Frame + 1
                Else
                    PauseAllAnimations()
                    PictureBox1.Image = GoodBoyFromLeft(Frame)
                    Frame = 0
                    Is_Walking = True
                End If
            ElseIf SheepDirection = "Right" Then
                If Frame < GoodBoyFromRight.Count - 1 Then
                    PictureBox1.Image = GoodBoyFromRight(Frame)
                    Frame = Frame + 1
                Else
                    PauseAllAnimations()
                    PictureBox1.Image = GoodBoyFromRight(Frame)
                    Frame = 0
                    Is_Walking = True
                End If
            End If

            If CheckWindowBordersForY() = False And Me.Location.Y < Screen.PrimaryScreen.WorkingArea.Height - Me.Height Then
                PauseAllAnimations()
                PictureBox1.Image = My.Resources.soltar
                wait(1)
                Is_Falling = True
                Exit Sub
            End If
        ElseIf Is_Dead = True Then
            PauseAllAnimations()
            Animation.Enabled = False
            DoSpecialAnimation.Enabled = False
            PictureBox1.Image = My.Resources.dead

        End If
    End Sub

    Private Sub DoSpecialAnimation_Tick(sender As Object, e As EventArgs) Handles DoSpecialAnimation.Tick
        If Is_Walking = True Then
            RandomAnimation()
        End If
    End Sub

    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown
        If Is_Dead = True Then Exit Sub
        PauseAllAnimations()
        Is_Dragging = True
        MouseX = Windows.Forms.Cursor.Position.X - Me.Left
        MouseY = Windows.Forms.Cursor.Position.Y - Me.Top
    End Sub

    Private Sub PictureBox1_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox1.MouseLeave
        Is_Dragging = False
    End Sub

    Private Sub PictureBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseUp
        If Is_Dead = True Then Exit Sub
        PauseAllAnimations()
        PictureBox1.Image = My.Resources.soltar
        wait(2)
        Is_Falling = True
    End Sub

    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        If Is_Dragging = True Then
            Me.Top = Windows.Forms.Cursor.Position.Y - MouseY
            Me.Left = Windows.Forms.Cursor.Position.X - MouseX
        End If
    End Sub

    Private Sub Sheep_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        PauseAllAnimations()
        Animation.Enabled = False
        DoSpecialAnimation.Enabled = False
        PictureBox1.Image = My.Resources.dead
        wait(3)
        Me.Hide()
        Me.Visible = False
        About.SheepCount = About.SheepCount - 1
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        colWin.Clear()
        EnumWindows(AddressOf EnumerateOpenWindows, 0)

        If Not ventanaaseguir = Nothing AndAlso IsWindowVisible(ventanaaseguir) = True AndAlso CheckTopWindow(ventanaaseguir) = True AndAlso CheckWindowBordersForY() = True Then
            FollowWindow(ventanaaseguir)
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub PictureBox1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDoubleClick
        Me.Close()
    End Sub
End Class
