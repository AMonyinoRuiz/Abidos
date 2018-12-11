Public Class clsPilaFormularis
    Dim oPila As ArrayList

    Public Sub New()
        oPila = New ArrayList
    End Sub

    Public Sub AfegirFormulari(ByRef pFormulari As M_GenericForm.frmBase)
        oPila.Add(pFormulari)
    End Sub

    Public Function ObrirFormulari(ByRef pNom As Type, Optional ByVal pParametroEntrada As Integer = 0) As M_GenericForm.frmBase
        Dim _frm As M_GenericForm.frmBase

        For Each _frm In oPila
            If _frm.GetType = pNom AndAlso _frm.Visible = False Then
                If _frm.Name = "frmInstalacion" Then 'si es un formulari de tipus d'instalació llavors compravarem que el paràmetre d'entrada coincideixi amb fomrulari (si es instalació o futura instalació)
                    If CObj(_frm).oTipoEntrada = pParametroEntrada Then
                        _frm.Visible = True
                        Return _frm
                    End If
                Else
                    _frm.Visible = True
                    Return _frm
                End If
            End If
        Next

        Dim _frm2 As Object
        _frm2 = Activator.CreateInstance(pNom)
        Return _frm2

    End Function


    Public Function OcultarFormulari(ByRef pFormulari As M_GenericForm.frmBase) As Boolean
        OcultarFormulari = False
        Dim _frm As New M_GenericForm.frmBase
        For Each _frm In oPila
            If _frm.Equals(pFormulari) Then
                _frm.Visible = False
                Return True
            End If
        Next
    End Function

    Public Sub NetejarPila()
        Dim _frm As M_GenericForm.frmBase
        For Each _frm In oPila
            _frm.FormTancar()
        Next
        oPila.Clear()
    End Sub

End Class
