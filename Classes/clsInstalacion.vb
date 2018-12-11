Public Class clsInstalacion
    Public Shared Function RetornaContactosDeLaInstalacion(ByVal pIDInstalacion As Integer) As DataTable
        Try
            Return BD.RetornaDataTable("Select * From C_Instalacion_Contacto Where ID_Instalacion=" & pIDInstalacion, True)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Shared Function RetornaPropuestasDeLaInstalacion(ByVal pIDInstalacion As Integer) As DataTable
        Try
            Return BD.RetornaDataTable("Select * From C_Propuesta Where Activo=1 and SeInstalo=0 and ID_Propuesta_Tipo in (1,2) and  ID_Instalacion=" & pIDInstalacion, True)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function
End Class

