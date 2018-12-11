Imports System.Reflection
Imports System.Data.Linq
Imports System.Data.Linq.Mapping

Public Class FrmNewGrid


    Public Sub Entrada()
        Me.AplicarDisseny()
    End Sub

    Private Sub UltraButton1_Click(sender As Object, e As EventArgs) Handles UltraButton1.Click
        Dim oDTC As New DTCDataContext(BD.Conexion)
        Me.GridControl1.DataSource = BD.RetornaDataTable("Select * From C_Producto_Pantalla_Producto")
        Me.GridView1.OptionsView.ColumnAutoWidth = False
        'Me.GridControl1.DataSource = oDTC.Propuesta
        'Dim DTS As New DataSet
        'BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is null and ID_Propuesta=78")   '
        'Dim i As Integer = 0
        'Dim Fin As Integer = 6


        'For i = 0 To Fin
        '    BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=78", "Propuesta_Linea" & i + 2, i, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
        'Next

        'Me.GridControl1.DataSource = BD.RetornaDataTable("Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is null and ID_Propuesta=78")   '
        ' Me.GridControl1.ShowPrintPreview()
        'Me.GridControl1.DataSource = Nothing

    End Sub

    Private Sub UltraButton2_Click(sender As Object, e As EventArgs) Handles UltraButton2.Click
        Dim Juan As New Parte_Horas
        Dim oDTC As New DTCDataContext(BD.Conexion)
        Dim pepe As IEnumerable(Of Parte_Horas) = From taula In oDTC.Parte_Horas Where taula.ID_Parte = 103 Select taula

        Me.GRD.M.clsUltraGrid.CargarIEnumerable(pepe)



        MsgBox(LinQEsRequerit(Me.GRD.GRID.DataSource, "ID_Personal"))
        ' MsgBox(LinQEsRequerit(pepe.FirstOrDefault, "ID_Personal"))

    End Sub


    'Dim oDTC As New DTCDataContext(BD.Conexion)

    'Dim _Parte As Parte
    '    For Each _Parte In oDTC.Parte
    'Dim _EmpInstalacion As Instalacion_Emplazamiento
    '        For Each _EmpInstalacion In _Parte.Instalacion.Instalacion_Emplazamiento
    'Dim _Emp As New Parte_Instalacion_Emplazamiento
    '            _Emp.ID_Parte = _Parte.ID_Parte
    '            _Emp.ID_Instalacion_Emplazamiento = _EmpInstalacion.ID_Instalacion_Emplazamiento
    '            _Parte.Parte_Instalacion_Emplazamiento.Add(_Emp)
    '            oDTC.Parte_Instalacion_Emplazamiento.InsertOnSubmit(_Emp)
    '        Next
    '    Next
    '    oDTC.SubmitChanges()

    Public Shared Function GetLengthLimit(obj As Object, field As String) As Integer
        Dim dblenint As Integer = 0 ' default value = we can't determine the length
        Dim type As Type = obj.GetType()
        Dim prop As PropertyInfo = type.GetProperty(field)

        ' Find the Linq "C"c attribute
        ' e.g. [Column(Storage="_FileName", DbType="NChar(256) NOT NULL", CanBeNull=false)]
        Dim info As Object() = prop.GetCustomAttributes(GetType(ColumnAttribute), True)
        ' Assume there is just one
        If info.Length = 1 Then
            Dim ca As ColumnAttribute = CType(info(0), ColumnAttribute)
            Dim dbtype As String = ca.DbType
            If dbtype.StartsWith("NChar") OrElse dbtype.StartsWith("NVarChar") Then
                Dim index1 As Integer = dbtype.IndexOf("(")
                Dim index2 As Integer = dbtype.IndexOf(")")
                Dim dblen As String = dbtype.Substring(index1 + 1, index2 - index1 - 1)
                Integer.TryParse(dblen, dblenint)
            End If

        End If
        Return dblenint
    End Function



    Public Shared Function LinQEsRequerit(obj As Object, field As String) As Boolean
        Try
            Dim pepe As Object = obj.GetType.GetGenericArguments(0)
            Dim prop As PropertyInfo

            For Each prop In pepe.declaredProperties
                If prop.Name = field Then
                    Dim info As Object() = prop.GetCustomAttributes(GetType(ColumnAttribute), True)
                    If info.Length = 1 Then
                        Dim ca As ColumnAttribute = CType(info(0), ColumnAttribute)
                        Return ca.CanBeNull
                    End If
                    Exit For
                End If
            Next

        Catch ex As Exception
            MsgBox("Error en la función LinQEsRequerit")
        End Try
    End Function






End Class