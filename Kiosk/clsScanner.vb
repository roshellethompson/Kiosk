Imports DigitekWrapper
Imports System.Text
Imports System
Imports System.Configuration

Public Class clsScanner
    Inherits Scanner
    Dim oFlags As New clsFlags
    Dim oCheck As New DigitekWrapper.Scanner
    Private mlngItems As Integer = 0
    Private mstrFileExt As String = String.Empty
    Private Const mstrBMP As String = "BMP"
    Private Const mstrJPG As String = "JPG"
    Private Const mstrTIF As String = "TIF"
    Dim sFrontJPEG As String = String.Empty
    Dim sBackJPEG As String = String.Empty
    Dim strFront As String = String.Empty
    Dim strBack As String = String.Empty
    Dim strCheckNum As String
    Dim strRouteNum As String
    Dim strAcctNum As String


    Dim oCheckCashing As New clsDataModule.clsInterface
    ''' <summary>
    ''' The scanned check number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CheckNum As String
        Get
            Return strCheckNum
        End Get
        Set(ByVal value As String)
            strCheckNum = value
        End Set
    End Property
    ''' <summary>
    ''' The scanned route number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RouteNum As String
        Get
            Return strRouteNum
        End Get
        Set(ByVal value As String)
            strRouteNum = value
        End Set
    End Property
    ''' <summary>
    ''' The scanned account number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AcctNum As String
        Get
            Return (strAcctNum)
        End Get
        Set(ByVal value As String)
            strAcctNum = value
        End Set
    End Property
    Private d1 As Scanner.MICRCallBackDelegate

    ''' <summary>
    ''' Scan the check
    ''' </summary>
    ''' <param name="strCustID"></param>
    ''' <param name="strTransID"></param>
    ''' <param name="strBlockID"></param>
    ''' <remarks></remarks>
    Public Sub scan(ByVal strCustID As String, ByVal strTransID As String, ByVal strBlockID As String)
        '/ Declares for DCCScan

        Dim sMICRData As New StringBuilder(255)
        Dim sOCRConf = New StringBuilder(255)
        '            //// Declares and setup for Scan to memory

        Dim imgmemPtrFront As Integer = 0 '; //VB method of declaring a pointer to memory buffer holding Front BW Image
        Dim imgmemPtrBack As Integer = 0 '; //VB method of declaring a pointer to memory buffer holding Back BW Image
        Dim imgmemPtrLen As Integer = 0
        Dim ans As Boolean = MemoryGet(imgmemhwndFront, imgmemPtrFront)
        '; //assigning and naming Front image memory buffer using above assignments
        ans = MemoryGet(imgmemhwndBack, imgmemPtrBack)
        '; //assigning and naming Back image memory buffer using above assignments

        Dim lFinalImageQuality As Integer = 0
        Dim lFinalContrast As Integer = 0
        Dim Ret As Integer = 0
        'int IQA = 0;
        Dim tstart As Integer = 0
        Dim tend As Integer = 0
        Dim ibatchitems As Integer = 0
        ' Declare DocStatus as User Defined Type
        Dim DocStatus As typDocStatus = typDocStatus.CreateInstance()
        Dim MICRDataLbl As String = String.Empty

        Ret = funcSetUpCallBack(TS200_CB_EVENT_VBMICR, Nothing)

        ScanLastItemFlag = 0 '; //// Flag identifying last item in hopper has been scanned
        ibatchitems = 0 '; //// Scanned item counter
        tstart = Environment.TickCount '; //// set time started variable to NOW
        bPrintBMPFlag = True
        sDispImgT = "FJ"
        sDispImgB = "FJ"
        iSPR = 1
        Scan2MemYN = True
        '        //// Begin Scan Loop
        While (Ret >= 0)
            ' //// 'No Scan Error or Empty Feeder
            If (ScanLastItemFlag >= 1) Then
                ' //// If this is the last image to scan
                Ret = BUICGetParam(CFG_MISC_SCANBATCH_ENABLE)
                If (Ret <> 0) Then
                    ' ////  If BatchScan is still on then
                    Ret = BUICSetParam(CFG_MISC_SCANBATCH_ENABLE, 0)
                    ScanLastItemFlag = 2
                    '; //// Set flag so we know to Start BatchScan again when done scanning
                Else
                    Exit While
                End If
            End If

            '//Have we set Print BMP on config screen for inkjet endorsing

            '//// Set Image File Naming Templates for std tif images
            strFront = mstrPath + "\\Images\\" + "F" + (mlngItems + 1).ToString("0000000") + ".tif"
            strBack = mstrPath + "\\Images\\" + "B" + (mlngItems + 1).ToString("0000000") + ".tif"
            '//// Set Image File Naming Templates for std jpg images
            'sFrontJPEG = mstrPath + "\\Images\\" + "F" + (mlngItems + 1).ToString("0000000") + ".jpg"
            'sBackJPEG = mstrPath + "\\Images\\" + "B" + (mlngItems + 1).ToString("0000000") + ".jpg"
            sFrontJPEG = oCheckCashing.GetKioskSettings("ScannedCheckImagePath", oFlags.WorkstationID) + "F" + strCustID + "_" + strBlockID + ".jpg"
            sBackJPEG = oCheckCashing.GetKioskSettings("ScannedCheckImagePath", oFlags.WorkstationID) + "B" + strCustID + "_" + strBlockID + ".jpg"

            '    //// Start the scanner
            BUICSetParam(CFG_SCAN_MODE, CFG_SCAN_MODE_HOLD)


            Ret = DCCScan("MD:" + imgmemPtrFront.ToString(), "MD:" + imgmemPtrBack.ToString(), sFrontJPEG, sBackJPEG, sMICRData, lFinalImageQuality, lFinalContrast, DocStatus)
            'Ret = BUICScan(BUIC_SCAN_BOTH_CODE, imgmemPtrFront.ToString, sFrontJPEG, imgmemPtrBack.ToString, sBackJPEG, strMicrData, lpCodeLen.ToString)
            If Ret < 0 Then
                sFrontJPEG = ""
                sBackJPEG = ""
                Exit While
            End If
            '        //Get OCR Data from MICR Line and fill MICRData buffer with results
            Ret = DCCScanGetOCRMicr(sMICRData, sOCRConf)
            If Ret > 0 Then
                Dim strINfo As String = sMICRData.ToString
                Dim strMICR As String() = strINfo.Split(":")
                CheckNum = strMICR(0).Replace(" ", "").Replace("<", "").Replace(">", "")
                RouteNum = strMICR(1)
                AcctNum = strMICR(2).Replace(">", "").Replace("<", "")
                CheckNum = Trim(CheckNum)
                RouteNum = Trim(RouteNum)
                AcctNum = Trim(AcctNum)
            Else
                Exit While
            End If
            ibatchitems += 1
            mlngItems += 1
            stopScan()
        End While
        '        //Second Scan call for CX30 Scan/Park/Return Mode
        Dim tempRefParam As Boolean = True


    End Sub
    ''' <summary>
    ''' Turn off scanning
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub stopScan()
        ScanLastItemFlag = 1 '//// Reset last item scanned flag
        AutoScanFlag = False '//// Turn off autoscan
    End Sub
    ''' <summary>
    ''' Return the path to the front of check image
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFrontPath() As String
        Return sFrontJPEG
    End Function
    ''' <summary>
    ''' REturn the path to the back of check image
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBackPath() As String
        Return sBackJPEG
    End Function
    Public Sub Clear()
        sBackJPEG = ""
        sFrontJPEG = ""
        strAcctNum = ""
        strRouteNum = ""
        strCheckNum = ""
    End Sub
    ''' <summary>
    ''' Eject the check into the kiosk
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub EjectForward()
        If Not oFlags.SimCheckScanner Then
            BUICEjectPocket(EJECT_FORWARD, 0)
        End If
        oFlags.CheckCaptured = True
    End Sub
    ''' <summary>
    ''' Eject the check back to the customer
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub EjectReverse()
        If Not oFlags.SimCheckScanner Then
            BUICEjectPocket(EJECT_REVERSE, 0)
        End If
        oFlags.CheckCaptured = False
    End Sub
    Public Function Init() As Integer
        clsScanner.mstrPath = oCheckCashing.GetKioskSettings("ScannerPath", oFlags.WorkstationID)
        Dim Ret As Integer = clsScanner.BUICSetParamString(clsScanner.CFG_INIPATH, clsScanner.mstrPath + "BUICSCAN.INI")
        Ret = clsScanner.BUICInit()
        Return Ret
    End Function

    Public Sub New()


    End Sub
End Class
