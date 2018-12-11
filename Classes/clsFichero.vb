Public Class clsFichero
    Public Shared Function DuplicaFichero(ByVal pFichero As Archivo) As Archivo
        With pFichero
            Dim _NewArchivo As New Archivo
            _NewArchivo.Activo = .Activo
            _NewArchivo.CampoBinario = .CampoBinario
            _NewArchivo.Color = .Color
            _NewArchivo.Descripcion = .Descripcion
            _NewArchivo.Fecha = .Fecha
            _NewArchivo.ID_Usuario = .ID_Usuario
            _NewArchivo.Ruta_Fichero = .Ruta_Fichero
            _NewArchivo.Tamaño = .Tamaño
            _NewArchivo.Tipo = .Tipo
            Return _NewArchivo
        End With
    End Function
End Class
