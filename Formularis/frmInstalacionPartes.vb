Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmInstalacionPartes
    Dim oDTC As DTCDataContext
    Dim oLinqPropuesta_Linea As Propuesta_Linea


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pPropuesta_Linea As Propuesta_Linea, ByRef pDTC As DTCDataContext)

        Try

            Me.AplicarDisseny()
            Me.ToolForm.M.Botons.tGuardar.SharedProps.Visible = False

            oLinqPropuesta_Linea = pPropuesta_Linea

            oDTC = pDTC
            Call CargaGrid_Lineas()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

#End Region

#Region "Grid Lineas"
    Public Sub CargaGrid_Lineas()
        Dim DT As DataTable
        DT = BD.RetornaDataTable("Select * From C_Parte_Reparacion WHERE ParteActivo=1 and ID_Propuesta_Linea=" & oLinqPropuesta_Linea.ID_Propuesta_Linea, True)
        Me.GRD_Lineas.M.clsUltraGrid.Cargar(DT)
        ' Dim Lineas = oLinqPropuesta_Linea.par
    End Sub

    Private Sub GRD_Lineas_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Lineas.M_ToolGrid_ToolVisualitzarDobleClickRow
        Try
            If Me.GRD_Lineas.GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            'Dim ID_Parte_Reparacion As Integer = Me.GRD_Lineas.GRID.Selected.Rows(0).Cells("ID_Parte_Reparacion").Value
            Dim ID_Parte As Integer = Me.GRD_Lineas.GRID.Selected.Rows(0).Cells("ID_Parte").Value

            Dim frmParte As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
            frmParte.Entrada(ID_Parte)
            frmParte.FormObrir(Me, True)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region




End Class