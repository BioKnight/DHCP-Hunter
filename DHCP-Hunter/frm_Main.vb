Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Class frm_Main
    Const DHCP_PORT As Integer = 67

    Private UdpBroadcaster As UdpClient
    Private udp_Listener As UdpClient
    Private done As Boolean = False

    Private UdpBroadcasterEndpoint As New IPEndPoint(IPAddress.Broadcast, DHCP_PORT)

    Private Sub btn_Broadcast_Click(sender As Object, e As EventArgs) Handles btn_Broadcast.Click
        StartBroadcastUdpThread()
    End Sub

    Private Sub StartBroadcastUdpThread()
        Dim UdpBroadcastThread As New Thread(AddressOf send_Broadcast)
        UdpBroadcastThread.IsBackground = True
        UdpBroadcastThread.Start()
    End Sub

    Private Sub send_Broadcast()
        Try
            UdpBroadcaster = New UdpClient(4333)
            UdpBroadcaster.EnableBroadcast = True
        Catch
            MsgBox("Error creating UDP client! Port already in use?")
        End Try

        If UdpBroadcaster Is Nothing Then
            Try
                UdpBroadcaster = New UdpClient(4333)
                UdpBroadcaster.EnableBroadcast = True
            Catch
                MsgBox("Error creating UDP Client.", MsgBoxStyle.Critical, "Error")
                Application.Exit()
                Return
            End Try
        End If

        Dim BroadcastBytes() As Byte = System.Text.Encoding.UTF32.GetBytes("")  '   need to send a DHCP discover packet.
        UdpBroadcaster.Send(BroadcastBytes, BroadcastBytes.Length, UdpBroadcasterEndpoint)

    End Sub

    Private Sub StartUdpListener()
        Dim ListenerUdp As New Thread(AddressOf listen_For_Reply)
        ListenerUdp.IsBackground = True
        ListenerUdp.Start()
    End Sub

    Private Sub listen_For_Reply()

        udp_Listener = New UdpClient(DHCP_PORT) '   Can we listen on same port as send?
        Dim msg_End_Point As New IPEndPoint(IPAddress.Any, DHCP_PORT)

        Try
            While Not done
                Console.WriteLine("Waiting for broadcast")
                Dim bytes As Byte() = udp_Listener.Receive(msg_End_Point)
                Console.WriteLine("Received broadcast from {0} :", msg_End_Point.ToString())
                Console.WriteLine("IP {0}", msg_End_Point.Address)
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
