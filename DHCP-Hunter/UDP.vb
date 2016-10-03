Imports System.Net
Imports System.Net.Sockets

Public Class UDP

#Region "Class Variables"
    Private lstn_Port As Int32, snd_Port As Int32 = 0
    Private subnet_IP As String
    Public IsListening As Boolean
    ' call backs for send/recieve!
    Public state As UdpState

#End Region

    Public Sub New()
        IsListening = False
    End Sub

    ' overrides pass the port to listen to/sendto And startup
    Public Sub New(listen_Port As Int32, send_Port As Int32, lan_IP As String)
        Try
            IsListening = False
            lstn_Port = listen_Port
            snd_Port = send_Port
            subnet_IP = lan_IP
            ' StartListener
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

#Region "Class Events"
    Public Delegate Sub DataRcvdEventHandler(DData As Byte(), RIpEndPoint As IPEndPoint)
    Public Event DataRcvd As DataRcvdEventHandler
    Public Delegate Sub ErrEventHandler(Msg As String)
#End Region

    Public Sub StopListener()

    End Sub

    ' structure that shall hold the reference of the call backs
    Public Structure UdpState
        Public e As IPEndPoint ' define an End point
        Public u As UdpClient ' define a client
    End Structure
End Class
