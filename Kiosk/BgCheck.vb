
'Purpose: BgCheck class is software interface to communicate with the EVS background validation service
'
'  Steps: 1) Establish instance of BgCheck object
'         2) Call BgCheck.doBackgroundCheck(ByVal SSN As String)
'         3) Use the internal name/DOB fields that are the result

Public Class BgCheck

    Public BGCHECK_SUCCESS = 1
    Public BGCHECK_FAIL = 2

    Public surname As String
    Public firstname As String
    Public DOB As String
    Public DOBMonth As String
    Public DOBDay As String
    Public DOBYear As String
    Public OFACCode As Integer = 0

    'Function: doBackgroundCheck()
    '  Inputs: SSN - Text string with Social Security Number (no punctuation)
    ' Returns: returnCode - BGCHECK_SUCCESS or BGCHECK_FAIL
    ' Purpose: Perform the background check
    '          This allows the calling program to send an SSN and get the BgCheck details
    '
    ' Example: returnCode = bgCheck.doBackgroundCheck("123456789")
    '
    Public Function doBackgroundCheck(ByVal SSN As String) As Integer

        Dim WebSvc As New net.everification.transactions.WrapEVS

        Dim returnString As String

        returnString = WebSvc.GetEVS(SSN, "", "", "", "", "", "", "33884a", "", "", "", "")
        'This calls the web service (passing the necessary parameters) and stores the resulting xml in a string

        'Console.WriteLine("return: " & returnString)

        surname = getXMLTagData(returnString, "SSN_Surname")
        firstname = getXMLTagData(returnString, "SSN_First_Name")
        DOBMonth = getXMLTagData(returnString, "SSN_MonthOfBirth")
        DOBDay = getXMLTagData(returnString, "SSN_DayOfBirth")
        DOBYear = getXMLTagData(returnString, "SSN_YearOfBirth")
        OFACCode = getXMLTagData(returnString, "OFACValidationResult")

        If Not IsNumeric(DOBMonth) Or Not IsNumeric(DOBDay) Or Not IsNumeric(DOBYear) Then
            doBackgroundCheck = BGCHECK_FAIL
            Return False
        End If
        DOB = DOBMonth & DOBDay & DOBYear

        If surname & firstname & DOB = "N/AN/AN/AN/AN/A" Or OFACCode > 1 Then
            doBackgroundCheck = BGCHECK_FAIL
        Else
            doBackgroundCheck = BGCHECK_SUCCESS
        End If

    End Function

    'Function: getXMLTagData()
    '  Inputs: data - raw XML data
    '          tag - the tag to extract
    ' Returns: value - the resulting value found
    ' Purpose: Internal function for parsing XML
    '
    Private Function getXMLTagData(ByVal data As String, ByVal tag As String) As String
        Dim i As Integer
        Dim j As Integer
        i = data.IndexOf("<" & tag & ">")
        j = data.IndexOf("</" & tag & ">")
        If i < 0 Or j < 0 Then
            Return "N/A"
        End If
        Return data.Substring(i + tag.Length + 2, j - i - 2 - tag.Length)
    End Function

End Class