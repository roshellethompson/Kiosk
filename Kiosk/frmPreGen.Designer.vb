<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPreGen
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
        Me.components = New System.ComponentModel.Container()
        Me.CustomerPhotoIDsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.EzCash_ProtoDataSet = New Kiosk.ezCash_ProtoDataSet()
        Me.CustomerPhotoIDsTableAdapter = New Kiosk.ezCash_ProtoDataSetTableAdapters.CustomerPhotoIDsTableAdapter()
        Me.GridPreReg = New System.Windows.Forms.DataGridView()
        Me.PhotoID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DOB = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.State = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.CustomerPhotoIDsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EzCash_ProtoDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridPreReg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CustomerPhotoIDsBindingSource
        '
        Me.CustomerPhotoIDsBindingSource.DataMember = "CustomerPhotoIDs"
        Me.CustomerPhotoIDsBindingSource.DataSource = Me.EzCash_ProtoDataSet
        '
        'EzCash_ProtoDataSet
        '
        Me.EzCash_ProtoDataSet.DataSetName = "ezCash_ProtoDataSet"
        Me.EzCash_ProtoDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'CustomerPhotoIDsTableAdapter
        '
        Me.CustomerPhotoIDsTableAdapter.ClearBeforeFill = True
        '
        'GridPreReg
        '
        Me.GridPreReg.AllowUserToAddRows = False
        Me.GridPreReg.AllowUserToDeleteRows = False
        Me.GridPreReg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridPreReg.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.PhotoID, Me.DOB, Me.State})
        Me.GridPreReg.Location = New System.Drawing.Point(12, 12)
        Me.GridPreReg.Name = "GridPreReg"
        Me.GridPreReg.Size = New System.Drawing.Size(390, 150)
        Me.GridPreReg.TabIndex = 0
        '
        'PhotoID
        '
        Me.PhotoID.HeaderText = "PhotoID Number"
        Me.PhotoID.Name = "PhotoID"
        '
        'DOB
        '
        Me.DOB.HeaderText = "Date Of Birth"
        Me.DOB.Name = "DOB"
        '
        'State
        '
        Me.State.HeaderText = "State"
        Me.State.Name = "State"
        '
        'frmPreGen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1924, 382)
        Me.Controls.Add(Me.GridPreReg)
        Me.Name = "frmPreGen"
        Me.Text = "frmPreGen"
        CType(Me.CustomerPhotoIDsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EzCash_ProtoDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridPreReg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents EzCash_ProtoDataSet As Kiosk.ezCash_ProtoDataSet
    Friend WithEvents CustomerPhotoIDsBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents CustomerPhotoIDsTableAdapter As Kiosk.ezCash_ProtoDataSetTableAdapters.CustomerPhotoIDsTableAdapter
    Friend WithEvents GridPreReg As System.Windows.Forms.DataGridView
    Friend WithEvents PhotoID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DOB As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents State As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
