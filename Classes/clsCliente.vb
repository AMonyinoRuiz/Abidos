Public Class clsCliente
    Public Shared Function RetornaContactosDelCliente(ByVal pIDCliente As Integer) As DataTable
        Try
            Return BD.RetornaDataTable("Select * From C_Cliente_Contacto Where ID_Cliente=" & pIDCliente, True)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function
End Class
