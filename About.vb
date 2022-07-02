
Public Class About

    Public Const MaxSheep As Integer = 20
    Public SheepCount As Integer = 0
    Dim Rotation As Bitmap() = New Bitmap(7) {My.Resources.Frente, My.Resources.Walk_der1, My.Resources.Walk_der2, My.Resources.turnbackfromright, My.Resources.back, My.Resources.turnbackfromleft, My.Resources.Walk_izq2, My.Resources.Walk_izq1}
    Dim Frame As Integer = 0

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        StartNewSheep()
    End Sub

    Private Sub About_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ShowInTaskbar = False
        Me.WindowState = FormWindowState.Minimized
        Me.Visible = False
        Me.Hide()
        StartNewSheep()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        StartNewSheep()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Button2.PerformClick()
    End Sub

    Private Sub RestoreToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestoreToolStripMenuItem.Click
        Me.Show()
        Me.ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
        Me.Visible = True
    End Sub

    Private Sub StartNewSheep()
        If SheepCount >= MaxSheep Then
            MsgBox("Too much Sheeps!", vbExclamation, "FunSheep")
        Else
            Dim NewSheep As New Sheep
            If CheckBox1.Checked = True Then NewSheep.Debug.Enabled = True
            NewSheep.Show()
            SheepCount = SheepCount + 1
        End If
    End Sub


    Private Sub KillAllSheepsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KillAllSheepsToolStripMenuItem.Click
        My.Application.OpenForms.Cast(Of Form)().Except({Me}).ToList().ForEach(Sub(form) form.Close())
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Frame < Rotation.Count - 1 Then
            PictureBox1.Image = Rotation(Frame)
            Frame = Frame + 1
        Else
            PictureBox1.Image = Rotation(Frame)
            Frame = 0
        End If

        Me.Text = "About FunSheep (" & SheepCount.ToString & ")"
    End Sub

    Private Sub About_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim result As DialogResult = MessageBox.Show("Kill all sheeps and close application?", "Please confirm", MessageBoxButtons.YesNo)
        If (result = DialogResult.Yes) Then
            My.Application.OpenForms.Cast(Of Form)().Except({Me}).ToList().ForEach(Sub(form) form.Close())
            NotifyIcon1.Visible = False
            Sheep.wait(2)
            End
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class