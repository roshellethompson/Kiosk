Public Class clsSettings
    Public Property WorkstationID As Integer = 0
    Public Property QueueTimeOut As Integer = 0
    Public Property SimulateAll As Boolean = False
    Public Property SimSnapShell As Boolean = False
    Public Property SimCardReader As Boolean = False
    Public Property SimCheckScanner As Boolean = False
    Public Property SimPinPad As Boolean = False
    Public Property TestMode As Boolean = False
    Public Property PrePaidOnly As Boolean = False
    Public Property SimCheckVerify As Boolean = False
    Public Property SimQueue As Boolean = False
    Public Property TimeOutSecs As Integer = 0
    Public Property SimScrapYard As Boolean = False
    Public Property oCheckCashing = New clsDataModule.clsInterface()
    Public Property HMIReloadable As String = ""
    Public Property HMINonReloadable As String = ""
    Public Property HMIAnonymous As String = ""
    Public Property DNCToReview As Boolean = False

    Public Sub New()

    End Sub

    Public Sub New(ByVal iWorkStationID As Integer)

        WorkstationID = iWorkStationID
        QueueTimeOut = oCheckCashing.GetKioskSettings("QueueTimeout", WorkstationID)
        SimulateAll = CType(oCheckCashing.GetKioskSettings("SimulateAll", WorkstationID), Integer)
        If Not SimulateAll Then
            SimSnapShell = CType(oCheckCashing.GetKioskSettings("SimSnapShell", WorkstationID), Integer)
            SimCardReader = CType(oCheckCashing.GetKioskSettings("SimCardReader", WorkstationID), Integer)
            SimCheckScanner = CType(oCheckCashing.GetKioskSettings("SimCheckScanner", WorkstationID), Integer)
            SimPinPad = CType(oCheckCashing.GetKioskSettings("SimPinPad", WorkstationID), Integer)
            TestMode = CType(oCheckCashing.GetKioskSettings("TestMode", WorkstationID), Integer)
            PrePaidOnly = CType(oCheckCashing.GetKioskSettings("PrePaidOnly", WorkstationID), Integer)
            SimCheckVerify = CType(oCheckCashing.GetKioskSettings("SimCheckVerify", WorkstationID), Integer)
        Else
            SimSnapShell = True
            SimCardReader = True
            SimCheckScanner = True
            SimPinPad = True
            TestMode = True
        End If
        SimQueue = oCheckCashing.GetKioskSettings("SimQueue", WorkstationID)
        TimeOutSecs = oCheckCashing.GetKioskSettings("Timeout", WorkstationID)
        SimScrapYard = oCheckCashing.GetKioskSettings("SimScrapYard", WorkstationID)
        DNCToReview = oCheckCashing.GetKioskSettings("DNCToReview", WorkstationID)

    End Sub
End Class
