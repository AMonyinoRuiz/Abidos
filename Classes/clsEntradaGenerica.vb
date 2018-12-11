Public Class clsEntradaGenerica
    Public oEntrada As Entrada
    Dim oDTC As DTCDataContext

    Public Sub New(ByVal pIDEntrada As Integer, ByRef pDTC As DTCDataContext)
        oDTC = pDTC
        oEntrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = pIDEntrada).FirstOrDefault
    End Sub


End Class
