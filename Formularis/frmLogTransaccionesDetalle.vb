Imports System.Reflection
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Data.Linq.Mapping

Public Class frmLogTransaccionesDetalle
    Dim oDTC As DTCDataContext

    Public Sub Entrada(ByVal pNomTaula As String, ByVal pNomCampPrincipal As String, ByVal pID As Integer)
        Me.AplicarDisseny()
        Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Visible = False

        Me.GridView1.OptionsView.ColumnAutoWidth = False

        Me.GridView1.OptionsSelection.MultiSelect = False
        Me.GridView1.OptionsSelection.UseIndicatorForSelection = False
        Me.GridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect

        oDTC = New DTCDataContext(BD.Conexion)
        Dim juan As Type
        juan = Type.GetType("Abidos." & pNomTaula)

        Dim _frm2 As Object
        _frm2 = Activator.CreateInstance(juan)

        Me.GridControl1.DataSource = oDTC.GetTable(juan)
        Me.GridView1.ActiveFilterString = "[" & pNomCampPrincipal & "]=" & pID

        Me.GridView1.OptionsBehavior.Editable = False
        Me.GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
        Me.GridView1.OptionsSelection.EnableAppearanceFocusedRow = False
        Me.GridView1.OptionsSelection.EnableAppearanceHideSelection = False


    End Sub




    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub GridView1_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        Dim view As GridView = CType(sender, GridView)

        Dim pt As Point = view.GridControl.PointToClient(Control.MousePosition)

        DoRowDoubleClick(view, pt)
    End Sub

    Private Sub DoRowDoubleClick(ByVal view As GridView, ByVal pt As Point)

        Dim info As GridHitInfo = view.CalcHitInfo(pt)

        If info.InRow OrElse info.InRowCell Then

            If info.Column Is Nothing Then
                Exit Sub
            End If

            If GridView1.GetRowCellValue(info.RowHandle, info.Column) Is Nothing Then
                Exit Sub
            End If


            Dim _prop As System.Reflection.PropertyInfo

            For Each _prop In GridView1.GetRowCellValue(info.RowHandle, info.Column).GetType.GetProperties()
                Dim _info As Object() = _prop.GetCustomAttributes(GetType(ColumnAttribute), True)
                If _info.Length <> 0 Then
                    Dim ca As ColumnAttribute = CType(_info(0), ColumnAttribute)
                    If ca.DbType.Contains("IDENTITY") Then
                        Dim _frm As New frmLogTransaccionesDetalle
                        _frm.Entrada(GridView1.GetRowCellValue(info.RowHandle, info.Column).GetType.Name, _prop.Name, Me.GridView1.GetRowCellValue(info.RowHandle, Me.GridView1.Columns(_prop.Name)))
                        _frm.FormObrir(Me, False)
                        Exit For
                    End If
                End If


            Next


            ' Dim pepe As IEnumerable =
            'Me.UltraGrid1.DataSource = pepe


            ' MessageBox.Show(String.Format("DoubleClick on row: {0}, column: {1}.", info.RowHandle, colCaption))

        End If

    End Sub
End Class