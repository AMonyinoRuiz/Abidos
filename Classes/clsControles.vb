Imports Infragistics.Win
Public Class clsControles
    Private oErrorProvider As ErrorProvider
    Private oCampRequeritTrobat As Boolean = False

    Public Enum EPropietat
        porDefecto = 0
        pValue = 1
        pTag = 2
        pText = 3
    End Enum

    Public ReadOnly Property pCampRequeritTrobat() As Boolean
        Get
            Return oCampRequeritTrobat
        End Get
    End Property


    Public Sub New(ByRef pErrorProvider As System.Windows.Forms.ErrorProvider)
        oErrorProvider = pErrorProvider
    End Sub

    Public Sub ControlBuit(ByRef pControl As Control, Optional ByVal pPropietat As EPropietat = EPropietat.porDefecto)
        Try
            Dim CampRequerit As Boolean = False
            Select Case TypeName(pControl).ToUpper
                Case "M_TextEditor".ToUpper
                    CampRequerit = CaixaTexte(CType(pControl, M_TextEditor.m_TextEditor), pPropietat)

                Case "m_MaskEditor".ToUpper
                    CampRequerit = CaixaTexteMask(CType(pControl, M_MaskEditor.m_MaskEditor), pPropietat)

                Case "UltraDateTimeEditor".ToUpper
                    CampRequerit = DateTime(CType(pControl, UltraWinEditors.UltraDateTimeEditor), pPropietat)

                Case "UltraColorPicker".ToUpper
                    CampRequerit = ColorPicker(CType(pControl, UltraWinEditors.UltraColorPicker), pPropietat)

                Case "UltraComboEditor".ToUpper
                    CampRequerit = ComboEditor(CType(pControl, UltraWinEditors.UltraComboEditor), pPropietat)

                Case "M_RichText".ToUpper
                    CampRequerit = RichText(CType(pControl, M_RichText.M_RichText), pPropietat)
                Case Else
                    MsgBox("Error intern")
            End Select

            If CampRequerit = True Then
                oErrorProvider.SetError(pControl, "El campo es obligatorio !")
                oCampRequeritTrobat = True
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function CaixaTexte(ByRef pControl As M_TextEditor.m_TextEditor, ByVal pPropietat As EPropietat) As Boolean
        If pPropietat = EPropietat.porDefecto Or pPropietat = EPropietat.pText Then
            If pControl.TextLength = 0 Then
                Return True
            End If
        End If

        If pPropietat = EPropietat.pTag Then
            If pControl.Tag Is Nothing Then
                Return True
            End If
        End If
    End Function

    Private Function RichText(ByRef pControl As M_RichText.M_RichText, ByVal pPropietat As EPropietat) As Boolean
        If pPropietat = EPropietat.porDefecto Or pPropietat = EPropietat.pText Then
            If pControl.RichText.Text Is Nothing OrElse pControl.RichText.Text.Trim = "" Then
                Return True
            End If
        End If
    End Function

    Private Function CaixaTexteMask(ByRef pControl As M_MaskEditor.m_MaskEditor, ByVal pPropietat As EPropietat) As Boolean
        If pPropietat = EPropietat.porDefecto Or pPropietat = EPropietat.pValue Then
            If pControl.Value Is Nothing OrElse IsDBNull(pControl.Value) = True Then
                Return True
            End If
        End If

        If pPropietat = EPropietat.pTag Then
            If pControl.Tag Is Nothing Then
                Return True
            End If
        End If
    End Function

    Private Function DateTime(ByRef pControl As UltraWinEditors.UltraDateTimeEditor, ByVal pPropietat As EPropietat) As Boolean
        If pPropietat = EPropietat.porDefecto Or pPropietat = EPropietat.pValue Then
            If pControl.Value Is Nothing Then
                Return True
            End If
        End If
    End Function

    Private Function ColorPicker(ByRef pControl As Infragistics.Win.UltraWinEditors.UltraColorPicker, ByVal pPropietat As EPropietat) As Boolean
        If pPropietat = EPropietat.porDefecto Or pPropietat = EPropietat.pText Then
            If pControl.TextLength = 0 Then
                Return True
            End If
        End If

        If pPropietat = EPropietat.pTag Then
            If pControl.Tag Is Nothing Then
                Return True
            End If
        End If

        If pPropietat = EPropietat.pValue Then
            If pControl.Value Is Nothing Then
                Return True
            End If
        End If
    End Function

    Private Function ComboEditor(ByRef pControl As Infragistics.Win.UltraWinEditors.UltraComboEditor, ByVal pPropietat As EPropietat) As Boolean
        If pPropietat = EPropietat.porDefecto Or pPropietat = EPropietat.pValue Then
            If pControl.TextLength = 0 Then
                Return True
            End If
        End If
    End Function
End Class
