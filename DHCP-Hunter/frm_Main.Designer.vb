<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lst_Servers = New System.Windows.Forms.ListBox()
        Me.btn_Broadcast = New System.Windows.Forms.Button()
        Me.btn_Exit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lst_Servers
        '
        Me.lst_Servers.FormattingEnabled = True
        Me.lst_Servers.Location = New System.Drawing.Point(0, 0)
        Me.lst_Servers.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.lst_Servers.Name = "lst_Servers"
        Me.lst_Servers.Size = New System.Drawing.Size(270, 277)
        Me.lst_Servers.TabIndex = 0
        '
        'btn_Broadcast
        '
        Me.btn_Broadcast.Location = New System.Drawing.Point(9, 289)
        Me.btn_Broadcast.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btn_Broadcast.Name = "btn_Broadcast"
        Me.btn_Broadcast.Size = New System.Drawing.Size(64, 19)
        Me.btn_Broadcast.TabIndex = 1
        Me.btn_Broadcast.Text = "&Broadcast"
        Me.btn_Broadcast.UseVisualStyleBackColor = True
        '
        'btn_Exit
        '
        Me.btn_Exit.Location = New System.Drawing.Point(205, 289)
        Me.btn_Exit.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btn_Exit.Name = "btn_Exit"
        Me.btn_Exit.Size = New System.Drawing.Size(56, 19)
        Me.btn_Exit.TabIndex = 2
        Me.btn_Exit.Text = "E&xit"
        Me.btn_Exit.UseVisualStyleBackColor = True
        '
        'frm_Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(270, 318)
        Me.Controls.Add(Me.btn_Exit)
        Me.Controls.Add(Me.btn_Broadcast)
        Me.Controls.Add(Me.lst_Servers)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "frm_Main"
        Me.Text = "DHCP-Hunter"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lst_Servers As ListBox
    Friend WithEvents btn_Broadcast As Button
    Friend WithEvents btn_Exit As Button
End Class
