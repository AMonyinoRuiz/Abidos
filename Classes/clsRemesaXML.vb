Public Class clsRemesaXML
    Dim oDTC As DTCDataContext
    Dim oLinqRemesa As Remesa
    Dim oLinqEmpresa As Empresa
    Dim oRutaFitxer As String


    'Variables usades
    Dim _EmpresaNom As String
    Dim _EmpresaAdreça As String
    Dim _EmpresaPoblacioProvincia As String

    Dim _RemesaIdentificador As String
    Dim _RemesaIdentificadorSuperUnic As String
    Dim _RemesaDataHoraCreacio As String
    Dim _RemesaDataCobro As String
    Dim _RemesaNumRebuts As Integer
    Dim _RemesaImportRebuts As String
    Dim _RemesaTipus As String  ' *** TIPUS D'ESQUEMA (COR1 O B2B)
    Dim _RemesaRecurrencia As String  '*** OJO TIPUS DE REBUT -rECCURENT, PUNTUAL, PRIMER, ÚLTIM) 
    Dim _RemesaIban As String
    Dim _RemesaBic As String
    Public Sub New(ByRef pDTC As DTCDataContext, ByRef pRemesa As Remesa, ByVal pRuta As String)

        oRutaFitxer = pRuta
        pDTC = New DTCDataContext(BD.Conexion) 'Fem això pq si no no s'actualitza la vista C_Remesa_ExportacionXML
        oDTC = pDTC

        Dim _Remesa As Remesa = pRemesa
        oLinqRemesa = oDTC.Remesa.Where(Function(F) F Is _Remesa).FirstOrDefault
        oLinqEmpresa = oDTC.Empresa.FirstOrDefault

        _EmpresaNom = Mid(TreureCaracters(oLinqEmpresa.Nombre), 1, 70) 'màxim 70 caràcters
        _EmpresaAdreça = TreureCaracters(oLinqEmpresa.Direccion)
        _EmpresaPoblacioProvincia = TreureCaracters(oLinqEmpresa.Poblacion & " - " & oLinqEmpresa.Provincia)

        _RemesaIdentificador = oLinqRemesa.ID_Remesa
        _RemesaIdentificadorSuperUnic = RetornaIdentificadorAcreedorComplert(oLinqEmpresa.NIF)
        _RemesaDataHoraCreacio = CDate(Now.Date).ToString("yyyy-MM-dd") & "T" & FormatDateTime(Now.Hour & ":" & Now.Minute, DateFormat.ShortTime) & ":00"
        _RemesaDataCobro = CDate(oLinqRemesa.FechaRemesa).ToString("yyyy-MM-dd")
        _RemesaNumRebuts = oLinqRemesa.C_Remesa_ExportacionXML.Count
        _RemesaImportRebuts = Util.Comes_Per_Punts(Math.Round(CDbl(oLinqRemesa.C_Remesa_ExportacionXML.Sum(Function(f) f.Importe)), 2))
        _RemesaTipus = "COR1" '"B2B"
        _RemesaRecurrencia = "RCUR"
        _RemesaIban = oLinqRemesa.Empresa_CuentaBancaria.NumeroCuenta
        _RemesaBic = oLinqRemesa.Empresa_CuentaBancaria.BIC

    End Sub

    Public Sub GenerarFitxer()
        Try

            Dim writer As New System.Xml.XmlTextWriter(oRutaFitxer, System.Text.Encoding.UTF8)
            writer.WriteStartDocument(True)
            writer.Indentation = 2
            writer.WriteStartElement("Document")
            writer.WriteAttributeString("xmlns", "xsi", Nothing, "http://www.w3.org/2001/XMLSchema-instance")
            writer.WriteAttributeString("xmlns", Nothing, Nothing, "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")
            writer.WriteStartElement("CstmrDrctDbtInitn")
            'Inicio cabecera
            writer.WriteStartElement("GrpHdr")
            'Regla de uso: El acreedor/presentador debe asegurarse que esta referencia es única para cada entidad destino del mensaje de presentación para un período de tiempo previamente acordado.'
            writer.WriteStartElement("MsgId")
            writer.WriteString(_RemesaIdentificador)    '***
            writer.WriteEndElement()
            writer.WriteStartElement("CreDtTm")
            writer.WriteString(_RemesaDataHoraCreacio)   '*** 
            writer.WriteEndElement()

            'Número de operaciones individuales que contiene el mensaje.Por razones técnicas puede ser conveniente establecer un límite máximo en el número de operaciones a incluir en cada fichero, que deberá comunicarle su entidad.'
            writer.WriteStartElement("NbOfTxs")
            writer.WriteString(_RemesaNumRebuts)   '*** OJO EL NÚMERO DE REBUTS ENCARA NO EL SABEM PQ HAUREM D'AGRUPAR
            writer.WriteEndElement()
            'Tiene 18 dígitos, 2 de ellos son decimales. Los dígitos correspondientes a los decimales irán separados por un punto'
            writer.WriteStartElement("CtrlSum")
            writer.WriteString(_RemesaImportRebuts) '*** OJO el mateix que abans amb el número de rebuts
            writer.WriteEndElement()
            writer.WriteStartElement("InitgPty")
            writer.WriteStartElement("Nm")
            writer.WriteString(Mid(_EmpresaNom, 1, 70))   '***  només es permet 70 carácerters
            writer.WriteEndElement()
            writer.WriteStartElement("Id")
            writer.WriteStartElement("OrgId")
            writer.WriteStartElement("Othr")
            'Definición: Identificación única e inequívoca de la parte.
            writer.WriteStartElement("Id")
            writer.WriteString(_RemesaIdentificadorSuperUnic)  '***
            writer.WriteEndElement()
            writer.WriteStartElement("SchmeNm")
            writer.WriteStartElement("Cd")
            writer.WriteString(_RemesaTipus) ' *** TIPUS D'ESQUEMA (COR1 O B2B)
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("PmtInf")
            writer.WriteStartElement("PmtInfId")
            writer.WriteString(_RemesaIdentificador)  '*** OJO Identificador únic de la remesa
            writer.WriteEndElement()
            writer.WriteStartElement("PmtMtd")
            writer.WriteString("DD")
            writer.WriteEndElement()
            writer.WriteStartElement("NbOfTxs")
            writer.WriteString(_RemesaNumRebuts)  '*** OJO EL NÚMERO DE REBUTS ENCARA NO EL SABEM PQ HAUREM D'AGRUPAR
            writer.WriteEndElement()
            writer.WriteStartElement("CtrlSum")
            'Tiene 18 dígitos, 2 de ellos son decimales. Los dígitos correspondientes a los decimales irán separados por un punto'
            writer.WriteString(_RemesaImportRebuts) '*** OJO el mateix que abans amb el número de rebuts

            writer.WriteEndElement()
            writer.WriteStartElement("PmtTpInf")
            writer.WriteStartElement("SvcLvl")
            writer.WriteStartElement("Cd")
            writer.WriteString("SEPA")
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("LclInstrm")
            writer.WriteStartElement("Cd")
            writer.WriteString(_RemesaTipus) ' *** TIPUS D'ESQUEMA (COR1 O B2B)
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("SeqTp")
            writer.WriteString(_RemesaRecurrencia)   '*** OJO TIPUS DE REBUT -rECCURENT, PUNTUAL, PRIMER, ÚLTIM) 
            writer.WriteEndElement()
            writer.WriteStartElement("CtgyPurp")
            writer.WriteStartElement("Cd")
            writer.WriteString("SUPP")
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("ReqdColltnDt")
            writer.WriteString(_RemesaDataCobro)   '***  Fecha de ejecución solicitada 
            writer.WriteEndElement()
            writer.WriteStartElement("Cdtr")
            writer.WriteStartElement("Nm")
            writer.WriteString(_EmpresaNom)  '***
            writer.WriteEndElement()
            writer.WriteStartElement("PstlAdr")
            writer.WriteStartElement("Ctry")
            writer.WriteString("ES")
            writer.WriteEndElement()
            writer.WriteStartElement("AdrLine")
            writer.WriteString(_EmpresaAdreça)  '***
            writer.WriteEndElement()
            writer.WriteStartElement("AdrLine")
            writer.WriteString(_EmpresaPoblacioProvincia) '***
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("CdtrAcct")
            writer.WriteStartElement("Id")
            writer.WriteStartElement("IBAN")
            writer.WriteString(_RemesaIban) '***
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("CdtrAgt")
            writer.WriteStartElement("FinInstnId")
            writer.WriteStartElement("BIC")
            writer.WriteString(_RemesaBic) '*** OJO BIC DEL BANC QUE GESTIONARÀ LA REMESA EX. BSABESBBXXX")
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("ChrgBr")
            writer.WriteString("SLEV")
            writer.WriteEndElement()

            Dim _Rebut As C_Remesa_ExportacionXML

            For Each _Rebut In oLinqRemesa.C_Remesa_ExportacionXML
                Call CrearXMLRebut(writer, _Rebut)
            Next
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndDocument()
            writer.Close()
            Mensaje.Mostrar_Mensaje("Exportación realizada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function CrearXMLRebut(ByRef writer As System.Xml.XmlTextWriter, ByRef pRebut As C_Remesa_ExportacionXML) As String

        With pRebut
            writer.WriteStartElement("DrctDbtTxInf")
            writer.WriteStartElement("PmtId")
            writer.WriteStartElement("EndToEndId")
            writer.WriteString(pRebut.ID_Entrada_Vencimiento)  '*** 
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("InstdAmt")
            writer.WriteAttributeString("Ccy", "EUR")
            writer.WriteString(Util.Comes_Per_Punts(pRebut.Importe)) '*** 
            writer.WriteEndElement()
            writer.WriteStartElement("DrctDbtTx")
            writer.WriteStartElement("MndtRltdInf")
            writer.WriteStartElement("MndtId")
            writer.WriteString(pRebut.ID_Entrada_Vencimiento)  '*** 
            writer.WriteEndElement()
            writer.WriteStartElement("DtOfSgntr")
            writer.WriteString("2009-10-31") '***
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("CdtrSchmeId")
            writer.WriteStartElement("Id")
            writer.WriteStartElement("PrvtId")
            writer.WriteStartElement("Othr")
            writer.WriteStartElement("Id")
            writer.WriteString(_RemesaIdentificadorSuperUnic) '*** IDENTIFICADOR + NIF DEL QUE GENERA LA REMESA"
            writer.WriteEndElement()
            writer.WriteStartElement("SchmeNm")
            writer.WriteStartElement("Prtry")
            writer.WriteString("SEPA")
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("DbtrAgt")
            writer.WriteStartElement("FinInstnId")
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("Dbtr")
            writer.WriteStartElement("Nm")
            writer.WriteString(TreureCaracters(pRebut.Cliente_Nombre)) '*** Nom del client
            writer.WriteEndElement()
            writer.WriteStartElement("PstlAdr")
            writer.WriteStartElement("Ctry")
            writer.WriteString(Mid(pRebut.Cliente_NumeroCuentaBancaria, 1, 2))  '*** Pais
            writer.WriteEndElement()
            writer.WriteStartElement("AdrLine")
            writer.WriteString(TreureCaracters(pRebut.Cliente_Direccion))  '*** Adreça del client
            writer.WriteEndElement()
            writer.WriteStartElement("AdrLine" & " ")
            writer.WriteString(TreureCaracters(pRebut.Cliente_Poblacion))  '*** Población del cliente
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("DbtrAcct")
            writer.WriteStartElement("Id")
            writer.WriteStartElement("IBAN")
            writer.WriteString(pRebut.Cliente_NumeroCuentaBancaria)   '*** Iban del client
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("Purp")
            writer.WriteStartElement("Cd")
            writer.WriteString("SUPP")
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteStartElement("RmtInf")
            writer.WriteStartElement("Ustrd")
            writer.WriteString(Mid("Recibo de: " & TreureCaracters(oLinqEmpresa.Nombre) & " sobre factura/s: " & pRebut.NumFacturas, 1, 139)) '*** TEXT LLIURE DEL REBUT
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.WriteEndElement()
        End With
        Return 1
    End Function

    Public Function RetornaIdentificadorAcreedorComplert(ByVal pCIF As String) As String
        Try
            Dim _CIF As String = pCIF
            Dim _NumTransformat As String = UCase(_CIF) & "ES00"
            Dim _NumTransformatANumeros As String = ""
            Dim _NumCalcul As Long
            Dim _Resto As Integer
            Dim _Sufix As Integer

            Dim _Caracter As Char
            For Each _Caracter In _NumTransformat.ToArray  'La llestra A es el codi 65 i la Z el 90  la A hauria de ser 10 i la Z 35
                If Asc(_Caracter) >= 65 And Asc(_Caracter) <= 90 Then
                    _NumTransformatANumeros = _NumTransformatANumeros & Asc(_Caracter) - 55
                Else
                    _NumTransformatANumeros = _NumTransformatANumeros & _Caracter
                End If
            Next

            _NumCalcul = CDbl(_NumTransformatANumeros)
            _Resto = _NumCalcul Mod 97
            _Sufix = 98 - _Resto



            Return "ES" & _Sufix & "000" & pCIF

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Function TreureCaracters(cadena As String) As String
        If cadena Is Nothing Then
            Return Nothing
        End If
        cadena = cadena.Replace("ñ", "")
        cadena = cadena.Replace("Ñ", "")
        cadena = cadena.Replace("á", "a")
        cadena = cadena.Replace("á", "a")
        cadena = cadena.Replace("é", "e")
        cadena = cadena.Replace("è", "e")
        cadena = cadena.Replace("í", "i")
        cadena = cadena.Replace("ó", "o")
        cadena = cadena.Replace("ú", "u")
        cadena = cadena.Replace("ñ", "")
        cadena = cadena.Replace("Ñ", "")
        cadena = cadena.Replace("à", "a")
        cadena = cadena.Replace("ò", "a")
        cadena = cadena.Replace("À", "A")
        cadena = cadena.Replace("Á", "A")
        cadena = cadena.Replace("É", "E")
        cadena = cadena.Replace("È", "E")
        cadena = cadena.Replace("Í", "I")
        cadena = cadena.Replace("Ò", "O")
        cadena = cadena.Replace("Ó", "O")
        cadena = cadena.Replace("Ú", "U")
        cadena = cadena.Replace("%", "")
        cadena = cadena.Replace("&", "")
        cadena = cadena.Replace("ª", "")
        cadena = cadena.Replace("º", "")
        cadena = cadena.Replace("Ç", "C")
        cadena = cadena.Replace("ç", "c")
        cadena = cadena.Replace("Ñ", "N")
        cadena = cadena.Replace("ñ", "n")
        cadena = cadena.Replace("`", "")
        cadena = cadena.Replace("´", "")
        cadena = cadena.Replace("'", "")
        cadena = cadena.Replace("'", "")
        cadena = cadena.Replace(Chr(145), " ")

        Return cadena
    End Function

    Private Function ValidacionsPerGenerarFitxer() As Boolean
        Try
            ValidacionsPerGenerarFitxer = True


            Dim _C_ExportacionXML As C_Remesa_ExportacionXML = oDTC.C_Remesa_ExportacionXML.Where(Function(F) F.Importe <= 0).FirstOrDefault

            If _C_ExportacionXML Is Nothing = False Then
                Mensaje.Mostrar_Mensaje("Imposible generar el fichero, no se pueden generar recibos con valor 0 o negativo. Recibo con cuenta bancaria: " & _C_ExportacionXML.Cliente_NumeroCuentaBancaria & " del cliente: " & _C_ExportacionXML.Cliente_Nombre, M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Return False
            End If

            For Each _C_ExportacionXML In oLinqRemesa.C_Remesa_ExportacionXML.Where(Function(F) F.ID_Remesa = oLinqRemesa.ID_Remesa)
                If Util.IBANValidacion(_C_ExportacionXML.Cliente_NumeroCuentaBancaria) = False Then
                    Mensaje.Mostrar_Mensaje("Imposible generar el fichero, el número de cuenta bancaria: " & _C_ExportacionXML.Cliente_NumeroCuentaBancaria & " del cliente: " & _C_ExportacionXML.Cliente_Nombre & " no es una cuenta bancaria válida", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Return False
                End If
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

End Class




'Public Class Class2
'    Dim writer As New System.Xml.XmlTextWriter("c:\Remesa\fitxer.xml", System.Text.Encoding.UTF8)
'        writer.WriteStartDocument(True)
'        writer.Formatting = Formatting.Indented
'        writer.Indentation = 2
'        writer.WriteStartElement("Document")
'        writer.WriteAttributeString("xmlns", "xsi", Nothing, "http://www.w3.org/2001/XMLSchema-instance")
'        writer.WriteAttributeString("xmlns", Nothing, Nothing, "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02")
'        writer.WriteStartElement("CstmrDrctDbtInitn")
'    'Inicio cabecera
'        writer.WriteStartElement("GrpHdr")
'    'Regla de uso: El acreedor/presentador debe asegurarse que esta referencia es única para cada entidad destino del mensaje de presentación para un período de tiempo previamente acordado.'
'        writer.WriteStartElement("MsgId")
'        writer.WriteString("*** IDENTIFICADOR ÚNIC PER A CADA REMESA)
'        writer.WriteEndElement()
'        writer.WriteStartElement("CreDtTm")
'        writer.WriteString(CDate("***DATA DE LA REMESA").ToString("yyyy-MM-dd") & "T09:30:47")   '2013-10-28T09:58:31
'        writer.WriteEndElement()
'    'Número de operaciones individuales que contiene el mensaje.Por razones técnicas puede ser conveniente establecer un límite máximo en el número de operaciones a incluir en cada fichero, que deberá comunicarle su entidad.'
'        writer.WriteStartElement("NbOfTxs")

'        writer.WriteString("*** NÚMERO DE REBUTS")
'        writer.WriteEndElement()
'    'Tiene 18 dígitos, 2 de ellos son decimales. Los dígitos correspondientes a los decimales irán separados por un punto'
'        writer.WriteStartElement("CtrlSum")
'        writer.WriteString(Util.Comes_Per_Punts("*** IMPORT TOTAL DELS REBUTS**")))
'        writer.WriteEndElement()
'        writer.WriteStartElement("InitgPty")
'        writer.WriteStartElement("Nm")
'        writer.WriteString("*** NOM DE LA EMPRESA QUE GENERA LA REMESA")
'        writer.WriteEndElement()
'        writer.WriteStartElement("Id")
'        writer.WriteStartElement("OrgId")
'        writer.WriteStartElement("Othr")
'    'Definición: Identificación única e inequívoca de la parte.
'        writer.WriteStartElement("Id")
'        writer.WriteString("*** IDENTIFICADOR + NIF DEL QUE GENERA LA REMESA")
'        writer.WriteEndElement()
'        writer.WriteStartElement("SchmeNm")
'        writer.WriteStartElement("Cd")
'        writer.WriteString("*** TIPUS D'ESQUEMA (COR1 O B2B)")
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteStartElement("PmtInf")
'        writer.WriteStartElement("PmtInfId")
'        writer.WriteString("*** IDENTIFICADOR ÚNIC DE TOTA LA REMESA")
'        writer.WriteEndElement()
'        writer.WriteStartElement("PmtMtd")
'        writer.WriteString("DD")
'        writer.WriteEndElement()
'        writer.WriteStartElement("NbOfTxs")
'        writer.WriteString("*** NÚMERO DE REBUTS TOTAL)
'        writer.WriteEndElement()
'        writer.WriteStartElement("CtrlSum")
'    'Tiene 18 dígitos, 2 de ellos son decimales. Los dígitos correspondientes a los decimales irán separados por un punto'
'        writer.WriteString(Util.Comes_Per_Punts("*** IMPORT TOTAL DELS REBUTS"))

'        writer.WriteEndElement()
'        writer.WriteStartElement("PmtTpInf")
'        writer.WriteStartElement("SvcLvl")
'        writer.WriteStartElement("Cd")
'        writer.WriteString("SEPA")
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteStartElement("LclInstrm")
'        writer.WriteStartElement("Cd")
'        writer.WriteString("*** TIPUS D'ESQUEMA (COR1 O B2B)")
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteStartElement("SeqTp")
'        writer.WriteString("*** TIPUS DE REBUT -rECCURENT, PUNTUAL, PRIMER, ÚLTIM) ---RCUR---")
'        writer.WriteEndElement()
'        writer.WriteStartElement("CtgyPurp")
'        writer.WriteStartElement("Cd")
'        writer.WriteString("SUPP")
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteStartElement("ReqdColltnDt")
'        writer.WriteString(CDate("*** DATA DE LA REMESA").ToString("yyyy-MM-dd"))
'        writer.WriteEndElement()
'        writer.WriteStartElement("Cdtr")
'        writer.WriteStartElement("Nm")
'        writer.WriteString("*** NOM DE LA EMPRESA QUE GENERA LA REMESA")
'        writer.WriteEndElement()
'        writer.WriteStartElement("PstlAdr")
'        writer.WriteStartElement("Ctry")
'        writer.WriteString("ES")
'        writer.WriteEndElement()
'        writer.WriteStartElement("AdrLine")
'        writer.WriteString("*** ADREÇA DE LA EMPRESA QUE GENERA LA REMESA")
'        writer.WriteEndElement()
'        writer.WriteStartElement("AdrLine")
'        writer.WriteString("*** POBLACIÓ I PROVINCIA DE LA EMPRESA QUE GENERA LA REMESA")
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteStartElement("CdtrAcct")
'        writer.WriteStartElement("Id")
'        writer.WriteStartElement("IBAN")
'        writer.WriteString("*** IBAN DE LA EMPRESA QUE GENERA LA REMESA")
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteStartElement("CdtrAgt")
'        writer.WriteStartElement("FinInstnId")
'        writer.WriteStartElement("BIC")
'        writer.WriteString("*** BIC DEL BANC QUE GESTIONARÀ LA REMESA EX. BSABESBBXXX")
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteStartElement("ChrgBr")
'        writer.WriteString("SLEV")
'        writer.WriteEndElement()


'        For Each _REBUT In _REMESA
'            Call CrearXML2(writer, _REBUT)
'        Next
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteEndElement()
'        writer.WriteEndDocument()
'        writer.Close()
'        MsgBox("OPERACIÓ REALITZADA")
'    End Sub


'    Private Function CrearXML2(ByRef writer As System.Xml.XmlTextWriter, ByRef pRemesa As REMESA) As String

'        With pRemesa
'            writer.WriteStartElement("DrctDbtTxInf")
'            writer.WriteStartElement("PmtId")
'            writer.WriteStartElement("EndToEndId")
'            writer.WriteString("*** IDENTIFICADOR ÚNIC DEL REBUT")
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteStartElement("InstdAmt")
'            writer.WriteAttributeString("Ccy", "EUR")
'            writer.WriteString(Util.Comes_Per_Punts("*** IMPORT DEL REBUT"))
'            writer.WriteEndElement()
'            writer.WriteStartElement("DrctDbtTx")
'            writer.WriteStartElement("MndtRltdInf")
'            writer.WriteStartElement("MndtId")
'            writer.WriteString("*** IDENTIFICADOR ÚNIC DEL REBUT")
'            writer.WriteEndElement()
'            writer.WriteStartElement("DtOfSgntr")
'            writer.WriteString("*** FECHA FIXE NO SE PQ 2009-10-31")
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteStartElement("CdtrSchmeId")
'            writer.WriteStartElement("Id")
'            writer.WriteStartElement("PrvtId")
'            writer.WriteStartElement("Othr")
'            writer.WriteStartElement("Id")
'            writer.WriteString("*** IDENTIFICADOR + NIF DEL QUE GENERA LA REMESA")
'            writer.WriteEndElement()
'            writer.WriteStartElement("SchmeNm")
'            writer.WriteStartElement("Prtry")
'            writer.WriteString("SEPA")
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteStartElement("DbtrAgt")
'            writer.WriteStartElement("FinInstnId")
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteStartElement("Dbtr")
'            writer.WriteStartElement("Nm")

'            writer.WriteString(TreureCaracters("*** NOM CLIENT"))
'            writer.WriteEndElement()
'            writer.WriteStartElement("PstlAdr")
'            writer.WriteStartElement("Ctry")
'            writer.WriteString(" *** PRIMERS 2 CARÀCTERS DE LA COMPTE CORRENT DEL CLIENT)
'            writer.WriteEndElement()
'            writer.WriteStartElement("AdrLine")
'            writer.WriteString("*** ADREÇA DEL CLIENT")
'            writer.WriteEndElement()
'            writer.WriteStartElement("AdrLine" & " ")
'            writer.WriteString("*** POBLACIÓ DEL CLIENT")
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteStartElement("DbtrAcct")
'            writer.WriteStartElement("Id")
'            writer.WriteStartElement("IBAN")
'            writer.WriteString("*** IBAN DEL CLIENT) 
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteStartElement("Purp")
'            writer.WriteStartElement("Cd")
'            writer.WriteString("SUPP")
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteStartElement("RmtInf")
'            writer.WriteStartElement("Ustrd")
'            writer.WriteString("*** TEXT LLIURE DEL REBUT")
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'            writer.WriteEndElement()
'        End With
'        Return 1
'    End Function

'    Public Function TreureCaracters(cadena As String) As String
'        If cadena Is Nothing Then
'            Return Nothing
'        End If
'        cadena = cadena.Replace("ñ", "")
'        cadena = cadena.Replace("Ñ", "")
'        cadena = cadena.Replace("á", "a")
'        cadena = cadena.Replace("á", "a")
'        cadena = cadena.Replace("é", "e")
'        cadena = cadena.Replace("è", "e")
'        cadena = cadena.Replace("í", "i")
'        cadena = cadena.Replace("ó", "o")
'        cadena = cadena.Replace("ú", "u")
'        cadena = cadena.Replace("ñ", "")
'        cadena = cadena.Replace("Ñ", "")
'        cadena = cadena.Replace("à", "a")
'        cadena = cadena.Replace("ò", "a")
'        cadena = cadena.Replace("À", "A")
'        cadena = cadena.Replace("Á", "A")
'        cadena = cadena.Replace("É", "E")
'        cadena = cadena.Replace("È", "E")
'        cadena = cadena.Replace("Í", "I")
'        cadena = cadena.Replace("Ò", "O")
'        cadena = cadena.Replace("Ó", "O")
'        cadena = cadena.Replace("Ú", "U")
'        cadena = cadena.Replace("%", "")
'        cadena = cadena.Replace("&", "")
'        cadena = cadena.Replace("ª", "")
'        cadena = cadena.Replace("º", "")
'        cadena = cadena.Replace("Ç", "C")
'        cadena = cadena.Replace("ç", "c")
'        cadena = cadena.Replace("Ñ", "N")
'        cadena = cadena.Replace("ñ", "n")
'        cadena = cadena.Replace("`", "")
'        cadena = cadena.Replace("´", "")
'        cadena = cadena.Replace("'", "")
'        cadena = cadena.Replace("'", "")
'        cadena = cadena.Replace(Chr(145), " ")
'        Return cadena
'    End Function
'End Class
