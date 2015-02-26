Public Class clsInterfaceFlags
    Public Property oCheckCashing As New clsDataModule.clsInterface()
    Public Property WorkstationID As Integer
    Public Property TimeOutSecs As Integer
    Public Property TestMode As Boolean = False
    Public Property TestType As String = ""
    Public Property TestReview As Boolean = False
    ''' <summary>
    ''' Flow Process Flags
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Register As Boolean = False
    Public Property NewPhotoID As Boolean = False
    Public Property ResumeReg As Boolean = False
    Public Property ScrapDragon As Boolean = False
    Public Property NewCard As Boolean = False
    Public Property CardManagement As Boolean = False
    Public Property PhotoIDAuto As Boolean = False
    Public Property SkipCheckReview As Boolean = False
    Public Property SkipCheckVerify As Boolean = False
    Public Property backoutID As Integer = 0
    Public Property BillPay As Boolean = False


    ''' <summary>
    ''' Error case flags
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property WrongCard As Boolean = False
    Public Property CheckReviewedTimeOut As Boolean = False
    Public Property ShowPinMismatchError As Boolean = False
    Public Property EnteringPin As Boolean = False
    Public Property VPSecondTry As Boolean = False
    Public Property EPSecondTry As Boolean = True
    ''' <summary>
    ''' Input
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActiveTimer As String = ""

    ''' <summary>
    ''' Check Cashing Process Flags
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MultiCheck As Boolean = False
    Public Property CheckCash As Boolean = False
    Public Property CheckCaptured As Boolean = False
    Public Property EndCheckCash As Boolean = False
    Public Property ShowCheckScanError As Boolean = False
    Public Property ShowOCRError As Boolean = False
    Public Property FlowLocation As Location = Location.CashCheck

    ''' <summary>
    ''' Bill Pay Process Flags
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property isNewPayee As Boolean = True

    Public Enum Location
        NewCard = 1
        NewReg = 2
        CashCheck = 3
        NewPhotoID = 4
    End Enum
    Public Sub New()

    End Sub
    Public Sub New(ByVal iWksID)
        TimeOutSecs = CType(oCheckCashing.GetKioskSettings("TimeOut", iWksID), Integer)
    End Sub
End Class
