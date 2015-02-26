Imports System.ComponentModel
Imports System
Imports Kiosk.clsScreens.clsPinAction
Imports Kiosk.clsCard
Imports Kiosk.clsCheck
Imports Kiosk.clsQueue
Imports System.Data.SqlClient
Imports IBM
Imports clsDataModule

Public Class clsObjectMemoryDump
    Public Property Dumped As Boolean = False
    Public Function ToDataTable(ByVal oList As List(Of clsCustomer)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsCustomer))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsCustomer In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsCard)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsCard))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsCard In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsCheck)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsCheck))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsCheck In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsCustomer.clsCustomerFlags)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsCustomer.clsCustomerFlags))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsCustomer.clsCustomerFlags In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsQueue)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsQueue))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsQueue In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsError)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsError))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsError In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsCheckFlags)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsCheckFlags))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsCheckFlags In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsCardFlags)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsCardFlags))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsCardFlags In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    Public Function ToDataTable(ByVal oList As List(Of clsScreens)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsScreens))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsScreens In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsCustomerPhotoIDs)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsCustomerPhotoIDs))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsCustomerPhotoIDs In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    Public Function ToDataTable(ByVal oList As List(Of clsLog)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsLog))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsLog In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsScreens.clsPinAction)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsScreens.clsPinAction))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsScreens.clsPinAction In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsScreens.clsPinAction.clsPinActionFlags)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsScreens.clsPinAction.clsPinActionFlags))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsScreens.clsPinAction.clsPinActionFlags In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    Public Function ToDataTable(ByVal oList As List(Of clsInterfaceFlags)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsInterfaceFlags))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsInterfaceFlags In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    Public Function ToDataTable(ByVal oList As List(Of clsInputQueue)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsInputQueue))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsInputQueue In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsSettings)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsSettings))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsSettings In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsJournal)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsJournal))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsJournal In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function ToDataTable(ByVal oList As List(Of clsScreens.clsInputScreen)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsScreens.clsInputScreen))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsScreens.clsInputScreen In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    Public Function ToDataTable(ByVal oList As List(Of clsConn)) As DataTable
        Dim properties As PropertyDescriptorCollection
        properties = TypeDescriptor.GetProperties(GetType(clsConn))
        Dim table As DataTable = New DataTable()
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, prop.PropertyType)
        Next
        For Each item As clsConn In oList
            Dim row As DataRow = table.NewRow()
            For Each pro As PropertyDescriptor In properties
                If (Not pro.GetValue(item) Is Nothing) Then
                    row(pro.Name) = pro.GetValue(item)
                End If
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    
    Public Sub New()

    End Sub
End Class
