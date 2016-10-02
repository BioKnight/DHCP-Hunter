Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class frm_Main
    Const LISTEN_PORT As Integer = 8902

    Private udp_Listener As UdpClient
    Private done As Boolean = False

    Private Sub btn_Broadcast_Click(sender As Object, e As EventArgs) Handles btn_Broadcast.Click

    End Sub

    Private Sub send_Broadcast()

    End Sub

    Private Sub listen_For_Reply()

        udp_Listener = New UdpClient(LISTEN_PORT)
        Dim msg_End_Point As New IPEndPoint(IPAddress.Any, LISTEN_PORT)

        Try
            While Not done
                Console.WriteLine("Waiting for broadcast")
                Dim bytes As Byte() = udp_Listener.Receive(msg_End_Point)
                Console.WriteLine("Received broadcast from {0} :", msg_End_Point.ToString())
                Console.WriteLine("IP {0}", msg_End_Point.Address
                Dim message As String = Encoding.ASCII.GetString(bytes, 0, bytes.Length)
                If message = "EndServer Auth 8902" Then
                    Exit While
                End If
                lst_Servers.Items.Add(message)
                Console.WriteLine()
            End While
        Catch e As Exception
            Console.WriteLine(e.ToString())
        Finally
            udp_Listener.Close()
        End Try
    End Sub

End Class
