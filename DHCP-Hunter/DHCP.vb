Imports System.Net
'  This class contains all DHCP functions and packets for both client and server DHCP roles.

Public Class DHCP

#Region "Constructors"
    Public Sub New(lan_IP As String)
        subnet_IP = lan_IP
    End Sub

#End Region

#Region "Destructors"
    Public Sub Dispose()
        If (cls_UDP IsNot Nothing) Then cls_UDP.StopListener()
        cls_UDP = Nothing
    End Sub
#End Region

#Region "Enums Region"


    Public Enum DHCP_Msg_Type
        DHCPDISCOVER = 1
        DHCPOFFER = 2
        DHCPREQUEST = 3
        DHCPDECLINE = 4
        DHCPACK = 5
        DHCPNAK = 6
        DHCPRELEASE = 7
        DHCPINFORM = 8
    End Enum

    Public Enum DHCPOptionEnum  ''refer To the rfc2132.txt For vendor specific info
        SubnetMask = 1
        TimeOffset = 2
        Router = 3
        TimeServer = 4
        NameServer = 5
        DomainNameServer = 6
        LogServer = 7
        CookieServer = 8
        LPRServer = 9
        ImpressServer = 10
        ResourceLocServer = 11
        HostName = 12
        BootFileSize = 13
        MeritDump = 14
        DomainName = 15
        SwapServer = 16
        RootPath = 17
        ExtensionsPath = 18
        IpForwarding = 19
        NonLocalSourceRouting = 20
        PolicyFilter = 21
        MaximumDatagramReAssemblySize = 22
        DefaultIPTimeToLive = 23
        PathMTUAgingTimeout = 24
        PathMTUPlateauTable = 25
        InterfaceMTU = 26
        AllSubnetsAreLocal = 27
        BroadcastAddress = 28
        PerformMaskDiscovery = 29
        MaskSupplier = 30
        PerformRouterDiscovery = 31
        RouterSolicitationAddress = 32
        StaticRoute = 33
        TrailerEncapsulation = 34
        ARPCacheTimeout = 35
        EthernetEncapsulation = 36
        TCPDefaultTTL = 37
        TCPKeepaliveInterval = 38
        TCPKeepaliveGarbage = 39
        NetworkInformationServiceDomain = 40
        NetworkInformationServers = 41
        NetworkTimeProtocolServers = 42
        VendorSpecificInformation = 43
        NetBIOSoverTCPIPNameServer = 44
        NetBIOSoverTCPIPDatagramDistributionServer = 45
        NetBIOSoverTCPIPNodeType = 46
        NetBIOSoverTCPIPScope = 47
        XWindowSystemFontServer = 48
        XWindowSystemDisplayManager = 49
        RequestedIPAddress = 50
        IPAddressLeaseTime = 51
        OptionOverload = 52
        DHCPMessageTYPE = 53
        ServerIdentifier = 54
        ParameterRequestList = 55
        Message = 56
        MaximumDHCPMessageSize = 57
        RenewalTimeValue_T1 = 58
        RebindingTimeValue_T2 = 59
        Vendorclassidentifier = 60
        ClientIdentifier = 61
        NetworkInformationServicePlusDomain = 64
        NetworkInformationServicePlusServers = 65
        TFTPServerName = 66
        BootfileName = 67
        MobileIPHomeAgent = 68
        SMTPServer = 69
        POP3Server = 70
        NNTPServer = 71
        DefaultWWWServer = 72
        DefaultFingerServer = 73
        DefaultIRCServer = 74
        StreetTalkServer = 75
        STDAServer = 76
        END_Option = 255
    End Enum

#End Region

#Region "Structures"
    Public Class DHCP_Structures

        Public dhcp_Struct As DHCPstruct
        Public dhcp_Data As DHCPData
        Const OPTION_OFFSET As Integer = 240

        Public Sub New(data As Byte())

            Dim Reader As System.IO.BinaryReader
            Dim Stream As System.IO.MemoryStream = New System.IO.MemoryStream(data, 0, data.Length)

            Try
                ' initalize the binary reader
                Reader = New System.IO.BinaryReader(Stream)
                ' read data
                dhcp_Struct.D_op = Reader.ReadByte()
                dhcp_Struct.D_htype = Reader.ReadByte()
                dhcp_Struct.D_hlen = Reader.ReadByte()
                dhcp_Struct.D_hops = Reader.ReadByte()
                dhcp_Struct.D_xid = Reader.ReadBytes(4)
                dhcp_Struct.D_secs = Reader.ReadBytes(2)
                dhcp_Struct.D_flags = Reader.ReadBytes(2)
                dhcp_Struct.D_ciaddr = Reader.ReadBytes(4)
                dhcp_Struct.D_yiaddr = Reader.ReadBytes(4)
                dhcp_Struct.D_siaddr = Reader.ReadBytes(4)
                dhcp_Struct.D_giaddr = Reader.ReadBytes(4)
                dhcp_Struct.D_chaddr = Reader.ReadBytes(16)
                dhcp_Struct.D_sname = Reader.ReadBytes(64)
                dhcp_Struct.D_file = Reader.ReadBytes(128)
                dhcp_Struct.M_Cookie = Reader.ReadBytes(4)
                dhcp_Struct.D_options = Reader.ReadBytes(data.Length - OPTION_OFFSET)
            Catch
                Console.WriteLine("An error occured in DHCP_Data")
            Finally
                If Stream IsNot Nothing Then Stream.Dispose()
                Stream = Nothing
                Reader = Nothing
            End Try
        End Sub

        Public Structure DHCPData
            Public IPAddr As String
            Public SubMask As String
            Public LeaseTime As UInteger
            Public ServerName As String
            Public MyIP As String
            Public RouterIP As String
            Public DomainIP As String
            Public LogServerIP As String
        End Structure

        Public Structure DHCPstruct
            Public D_op As Byte         ' Op code:   1 = bootRequest, 2 = BootReply
            Public D_htype As Byte      ' Hardware Address Type: 1 = 10MB ethernet
            Public D_hlen As Byte       ' hardware address length: length of MACID
            Public D_hops As Byte       ' Hw options
            Public D_xid As Byte()      ' transaction id (5)
            Public D_secs As Byte()     ' elapsed time from trying To boot (3)
            Public D_flags As Byte()    ' flags (3)
            Public D_ciaddr As Byte()   ' client IP (5)
            Public D_yiaddr As Byte()   ' your client IP (5)
            Public D_siaddr As Byte()   ' Server IP  (5)
            Public D_giaddr As Byte()   ' relay agent IP (5)
            Public D_chaddr As Byte()   ' Client HW address (16)
            Public D_sname As Byte()    ' Optional server host name (64)
            Public D_file As Byte()     ' Boot file name (128)
            Public M_Cookie As Byte()   ' Magic cookie (4)
            Public D_options As Byte()  ' options (rest)
        End Structure


    End Class

#End Region

#Region "Events to Raise"
    ' an event has to call a delegate (function pointer)

#Region "Event Delegates"
    Public Delegate Sub AnnouncedEventHandler(d_DHCP As DHCP_Structures, MacId As String)
    Public Delegate Sub ReleasedEventHandler() ' (cDHCPStruct d_DHCP)
    Public Delegate Sub RequestEventHandler(d_DHCP As DHCP_Structures, MacId As String)
    Public Delegate Sub AssignedEventHandler(IPAdd As String, MacID As String)
#End Region

    Public Event Announced As AnnouncedEventHandler
    Public Event Request As RequestEventHandler
#End Region

#Region "Variables to Call"
    Private cls_UDP As UDP '  the udp snd/rcv Class
    Private subnet_IP As String
#End Region

#Region "Other functions"

    ' string property to contain the class name
    Private Function ClassName() As String
        Return "DHCP"
    End Function

    ' function to start the DHCP server
    ' port 67 to recieve, 68 to send
    Public Sub StartDHCPServer()

        Try
            '  start the DHCP server
            ' assign the event handlers
            cls_UDP = New UDP(67, 68, subnet_IP)
            AddHandler cls_UDP.DataRcvd, AddressOf cls_UDP.DataRcvdEventHandler(cUdp_DataRcvd)

        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

    End Sub

    ' pass the option type that you require
    ' parse the option data
    ' return the data in a byte of what we need
    Private Function GetOptionData(DHCP_Type As DHCPOptionEnum, DHCP_Structure As DHCP_Structures) As Byte()

        Dim DHCPId As Integer = 0
        Dim DDataID As Byte, DataLength As Byte = 0
        Dim dumpData As Byte()

        Try

            DHCPId = CType(DHCP_Type, Integer)
            ' loop through look for the bit that states that the identifier Is there
            For i = 0 To DHCP_Structure.dhcp_Struct.D_options.Length

                ' at the start we have the code + length
                ' i has the code, i+1 = length of data, i+1+n = data skip
                DDataID = DHCP_Structure.dhcp_Struct.D_options(i)
                If (DDataID = DHCPId) Then

                    DataLength = DHCP_Structure.dhcp_Struct.D_options(i + 1)
                    dumpData = New Byte() {DataLength}
                    Array.Copy(DHCP_Structure.dhcp_Struct.D_options, i + 2, dumpData, 0, DataLength)
                    Return dumpData

                Else

                    DataLength = DHCP_Structure.dhcp_Struct.D_options(i + 1) ' 'length of code
                    i += 1 + DataLength
                End If
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            dumpData = Nothing
        End Try
        Return Nothing
    End Function

    Public Function ByteToString(dByte As Byte(), hLength As Byte) As String

        Dim dString As String

        Try
            dString = String.Empty
            If dByte IsNot Nothing Then
                For i = 0 To hLength
                    dString &= dByte(i).ToString("X2")
                Next
                Return dString
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return String.Empty
        Finally
            dString = Nothing
        End Try
        Return Nothing
    End Function

    ' get the Message type
    ' located in the options stream
    Public Function GetMsgType(cdDHCPs As DHCP_Structures) As DHCP_Msg_Type
        Dim DData As Byte()
        Try
            DData = GetOptionData(DHCPOptionEnum.DHCPMessageTYPE, cdDHCPs)
            If (DData IsNot Nothing) Then
                Return DData(0)
            End If
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Return 0
        End Try
        Return Nothing
    End Function

    Public Sub cUdp_DataRcvd(DData As Byte(), RIpEndPoint As IPEndPoint)

        Dim DHCP_Structure As New DHCP_Structures(DData)
        'Dim DHCPstruct As New DHCP_Structures.DHCPstruct
        Dim MsgTyp As DHCP_Msg_Type
        Dim MacID As String

        Try
            ' data Is now in the structure
            ' get the msg type
            MsgTyp = GetMsgType(DHCP_Structure)
            MacID = ByteToString(DHCP_Structure.dhcp_Struct.D_chaddr, DHCP_Structure.dhcp_Struct.D_hlen) '  (string)ddHcpS.dStruct.D_chaddr
            Select Case MsgTyp
                Case DHCP_Msg_Type.DHCPDISCOVER
                    '  a Mac has requested an IP
                    '  discover Msg Has been sent
                    Announced(DHCP_Structure, MacID)
                Case DHCP_Msg_Type.DHCPREQUEST
                    Request(DHCP_Structure, MacID)
            End Select
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub



    Private Sub CreateOptionStruct(ByRef DHCP_Structure As DHCP_Structures, OptionReplyMsg As DHCP_Msg_Type)

        Dim PReqList As Byte(), t1 As Byte(), LeaseTime As Byte(), MyIp As Byte()

        Try
            ' we look for the parameter request list
            PReqList = GetOptionData(DHCPOptionEnum.ParameterRequestList, DHCP_Structure)
            ' erase the options array, And set the message type to ack
            DHCP_Structure.dhcp_Struct.D_options = Nothing
            CreateOptionElement(DHCPOptionEnum.DHCPMessageTYPE, New Byte() {OptionReplyMsg}, DHCP_Structure.dhcp_Struct.D_options)
            ' server identifier, my IP
            MyIp = IPAddress.Parse(DHCP_Structure.dhcp_Data.MyIP).GetAddressBytes()
            CreateOptionElement(DHCPOptionEnum.ServerIdentifier, MyIp, DHCP_Structure.dhcp_Struct.D_options)


            ' PReqList contains the option data in a byte that Is requested by the unit
            For Each i As Byte In PReqList

                t1 = Nothing
                Select Case i
                    Case DHCPOptionEnum.SubnetMask
                        t1 = IPAddress.Parse(DHCP_Structure.dhcp_Data.SubMask).GetAddressBytes()
                    Case DHCPOptionEnum.Router
                        t1 = IPAddress.Parse(DHCP_Structure.dhcp_Data.RouterIP).GetAddressBytes()
                    Case DHCPOptionEnum.DomainNameServer
                        t1 = IPAddress.Parse(DHCP_Structure.dhcp_Data.DomainIP).GetAddressBytes()
                    Case DHCPOptionEnum.DomainName
                        t1 = System.Text.Encoding.ASCII.GetBytes(DHCP_Structure.dhcp_Data.ServerName)
                    Case DHCPOptionEnum.ServerIdentifier
                        t1 = IPAddress.Parse(DHCP_Structure.dhcp_Data.MyIP).GetAddressBytes()
                    Case DHCPOptionEnum.LogServer
                        t1 = System.Text.Encoding.ASCII.GetBytes(DHCP_Structure.dhcp_Data.LogServerIP)
                    Case DHCPOptionEnum.NetBIOSoverTCPIPNameServer
                End Select
                If t1 IsNot Nothing Then CreateOptionElement(i, t1, DHCP_Structure.dhcp_Struct.D_options)
            Next

            ' lease time
            LeaseTime = New Byte(4)
            LeaseTime[3] = (Byte)(ddHcps.dData.LeaseTime)
                LeaseTime[2] = (Byte)(ddHcps.dData.LeaseTime >> 8)
                LeaseTime[1] = (Byte)(ddHcps.dData.LeaseTime >> 16)
                LeaseTime[0] = (Byte)(ddHcps.dData.LeaseTime >> 24)
                CreateOptionElement(DHCPOptionEnum.IPAddressLeaseTime, LeaseTime, ref ddHcps.dStruct.D_options)
                CreateOptionElement(DHCPOptionEnum.RenewalTimeValue_T1, LeaseTime, ref ddHcps.dStruct.D_options)
                CreateOptionElement(DHCPOptionEnum.RebindingTimeValue_T2, LeaseTime, ref ddHcps.dStruct.D_options)
                ' create the end option
                Array.Resize(ref ddHcps.dStruct.D_options, ddHcps.dStruct.D_options.Length + 1)
                Array.Copy(New Byte()  255 }, 0, ddHcps.dStruct.D_options, ddHcps.dStruct.D_options.Length - 1, 1)
                ' send the data to the unit

        Catch ex As Exception
            Console.WriteLine(ex.Message)
        Finally
            LeaseTime = Nothing
            PReqList = Nothing
            t1 = Nothing
        End Try
    End Sub

    ' mac announced itself, established IP etc....
    ' send the offer to the mac
    Public Sub SendDHCPMessage(DHCP_Msg_Type msgType, cDHCPStruct ddHcpS)

        Byte() Subn, HostID,  DataToSend
           

            ' we shall leave everything as Is structure wise
            ' shall CHANGE the type to OFFER
            ' shall set the client's IP-Address
            Try

            ' change message type to reply
            ddHcpS.dStruct.D_op = 2 ' reply
            ' subnet
            Subn = IPAddress.Parse(ddHcpS.dData.SubMask).GetAddressBytes()
            ' create your ip address
            ddHcpS.dStruct.D_yiaddr = IPAddress.Parse(ddHcpS.dData.IPAddr).GetAddressBytes()

            ' Host ID
            HostID = System.Text.Encoding.ASCII.GetBytes(ddHcpS.dData.ServerName)

            CreateOptionStruct(ref ddHcpS, msgType)
            ' send the data to the unit
            DataToSend = BuildDataStructure(ddHcpS.dStruct)
            cUdp.SendData(DataToSend)
            }
            Catch (Exception ex)
            
                Console.WriteLine(ex.Message)
            }
            Finally

            Subn = Nothing
            ' LeaseTime= nothing
            HostID = Nothing

            DataToSend = Nothing
            }
        End Sub

    ' function to build the data structure to a byte array
    Private Function BuildDataStructure(cDHCPStruct.DHCPstruct ddHcpS) As Byte()

        Byte() mArray

            Try

            mArray = New Byte[0]
                AddOptionElement(New Byte()  ddHcpS.D_op }, ref mArray)
            AddOptionElement(New Byte()  ddHcpS.D_htype }, ref mArray)
            AddOptionElement(New Byte()  ddHcpS.D_hlen }, ref mArray)
            AddOptionElement(New Byte()  ddHcpS.D_hops }, ref mArray)
            AddOptionElement(ddHcpS.D_xid, ref mArray)
            AddOptionElement(ddHcpS.D_secs, ref mArray)
            AddOptionElement(ddHcpS.D_flags, ref mArray)
            AddOptionElement(ddHcpS.D_ciaddr, ref mArray)
            AddOptionElement(ddHcpS.D_yiaddr, ref mArray)
            AddOptionElement(ddHcpS.D_siaddr, ref mArray)
            AddOptionElement(ddHcpS.D_giaddr, ref mArray)
            AddOptionElement(ddHcpS.D_chaddr, ref mArray)
            AddOptionElement(ddHcpS.D_sname, ref mArray)
            AddOptionElement(ddHcpS.D_file, ref mArray)
            AddOptionElement(ddHcpS.M_Cookie, ref mArray)
            AddOptionElement(ddHcpS.D_options, ref mArray)
            Return mArray
            }
            Catch (Exception ex)
            
                Console.WriteLine(ex.Message)
            Return Nothing
            }
            Finally

            mArray = Nothing
            }

        End Function

    Private Sub AddOptionElement(Byte() FromValue, ref Byte() TargetArray)
        
            Try

            If (TargetArray! = Nothing) Then
                Array.Resize(ref TargetArray, TargetArray.Length + FromValue.Length)
            Else
                Array.Resize(ref TargetArray, FromValue.Length)
                Array.Copy(FromValue, 0, TargetArray, TargetArray.Length - FromValue.Length, FromValue.Length)
            }
            Catch (Exception ex)
            
                Console.WriteLine(ex.Message)
            }
        End Sub


    ' create an option message 
    ' shall always append at the end of the message
    Private Sub CreateOptionElement(Code As DHCPOptionEnum, DataToAdd As Byte(), AddtoMe As Byte())

        Dim tOption As Byte()

        Try

            tOption = New Byte[DataToAdd.Length +2]
                ' add the code, And data length
                tOption[0] = (Byte)Code
                tOption[1] = (Byte)DataToAdd.Length
                ' add the code to put in
                Array.Copy(DataToAdd, 0, tOption, 2, DataToAdd.Length)
            ' copy the data to the out array
            If (AddtoMe == Nothing) Then
                Array.Resize(ref AddtoMe, (Int())tOption.Length)
            Else
                Array.Resize(ref AddtoMe, AddtoMe.Length + tOption.Length)
                Array.Copy(tOption, 0, AddtoMe, AddtoMe.Length - tOption.Length, tOption.Length)
            }
            Catch (Exception ex)
            
                Console.WriteLine(ex.Message)
            }

        End Sub

#End Region

End Class
