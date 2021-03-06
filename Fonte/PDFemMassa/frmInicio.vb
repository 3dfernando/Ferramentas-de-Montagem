﻿Imports Inventor
Imports System.Drawing.Printing
Imports System.IO

Public Class frmInicio

#Region "Variáveis e Objetos"
    Public Shared ThisApplication As Inventor.Application
    Public AddinPDF As Inventor.TranslatorAddIn
    Public AddinDXF As Inventor.TranslatorAddIn
    Public AddinDWG As Inventor.TranslatorAddIn

    Public PDFContext As Inventor.TranslationContext
    Public PDFOptions As Inventor.NameValueMap
    Public PDFDataMedium As Inventor.DataMedium

    Public DWGContext As Inventor.TranslationContext
    Public DWGOptions As Inventor.NameValueMap
    Public DWGDataMedium As Inventor.DataMedium

    Public DXFContext As Inventor.TranslationContext
    Public DXFOptions As Inventor.NameValueMap
    Public DXFDataMedium As Inventor.DataMedium

    Private FormLoaded As Boolean = False
    Private InterromperExecucao As Boolean = False
#End Region

#Region "Eventos"

    Private Sub cmdCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelar.Click
        CancelaTudo()
    End Sub

    Private Sub frmInicio_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Inicializar()
    End Sub

    Private Sub cmdListaArquivos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdListaArquivos.Click
        Dim Form As New frmListaArquivos
        Form.ShowDialog()
    End Sub

    Private Sub cmdOpcoes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOpcoes.Click
        Dim Form As New frmOpcoesGeral
        Form.Abas.SelectTab(0)
        Form.ShowDialog()
    End Sub

    Private Sub cmdEscolherPasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEscolherPasta.Click
        'Mostra uma janela para escolher a pasta recipiente dos arquivos exportados
        Dim Caminho As String
        DialogoPasta.ShowDialog()

        Caminho = DialogoPasta.SelectedPath

        If Not Caminho = vbNullString Then
            If Strings.Right(Caminho, 1) = "\" Then
                txtCaminhoPasta.Text = Caminho
            Else
                txtCaminhoPasta.Text = Caminho & "\"
            End If
            optPastaSelecionada.Checked = True
        Else
            optPastaOrigem.Checked = True
        End If
    End Sub

    Private Sub cmdOpcoesDWG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOpcoesDWG.Click
        Dim Form As New frmOpcoesGeral
        Form.Abas.SelectTab(3)
        Form.ShowDialog()
    End Sub

    Private Sub cmdOpcoesImpressao_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOpcoesImpressao.Click
        Dim Form As New frmOpcoesGeral
        Form.Abas.SelectTab(4)
        Form.ShowDialog()
    End Sub

    Private Sub optArquivosSelecionados_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optArquivosSelecionados.CheckedChanged
        If optArquivosSelecionados.Checked = True And FormLoaded Then
            cmdListaArquivos_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub optPastaSelecionada_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optPastaSelecionada.CheckedChanged
        If optPastaSelecionada.Checked = True Then
            cmdEscolherPasta_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub cmdExecutar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExecutar.Click
        'Executa os comandos configurados. Imprime, cria PDFs e DWGs/DXFs.

        Dim QtdArquivos As Integer
        Dim QtdArquivosSelecionados As Integer = 0
        Dim ArquivoAtual As Integer = 0
        Dim ArquivoSelecionadoAtual As Integer = 0
        Dim Ini As Integer = 0
        Dim Fim As Integer = 0
        Dim Stp As Integer = 1

        Dim TimerIni As Double
        Dim TimerFim As Double
        Dim TempoExecucao As Double
        Dim StrTempoExecucao As String

        Dim PastaSalvar As String = ""
        Dim Documento As Inventor.DrawingDocument

        'Começa a contar o tempo
        TimerIni = Timer

        'Se não existe arquivo de inicialização para DWG/DXF, então nem começa!
        If (chkDWG.Checked Or chkDXF.Checked) And (My.Settings.DWGIniFile = "" Or _
                                (Not System.IO.File.Exists(My.Settings.DWGIniFile))) Then
            MsgBox("DWG/DXF config file not found!" & vbCrLf & vbCrLf &
                    "You need to select in the settings window a DWG/DXF config file!",
                    MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "Critical Error!")

            GoTo Final
        End If

        If optArquivosSelecionados.Checked Then
            QtdArquivos = ListaArquivos.Length

            'Descobre quantos arquivos estão selecionados
            For ArquivoAtual = 0 To QtdArquivos - 1
                If ListaArquivos(ArquivoAtual).Checked Then
                    QtdArquivosSelecionados = QtdArquivosSelecionados + 1
                End If
            Next

            'Zera a progressbar
            lblProgresso.Text = "Executing... 0/" & Trim(Str(QtdArquivosSelecionados))
            pgbProgresso.Value = 0
            pgbProgresso.Maximum = QtdArquivosSelecionados

            'Seta a opção "Silent Operation"
            ThisApplication.SilentOperation = My.Settings.SilentMode

            'Seta a opção "Inverter Ordem de Exportação"
            If My.Settings.InverterOrdem Then
                Ini = QtdArquivos - 1
                Fim = 0
                Stp = -1
            Else
                Ini = 0
                Fim = QtdArquivos - 1
                Stp = 1
            End If


            For ArquivoAtual = Ini To Fim Step Stp
                If InterromperExecucao Then
                    'Termina de contar o tempo, e manda uma mensagem
                    TimerFim = Timer

                    TempoExecucao = Int(TimerFim - TimerIni)
                    StrTempoExecucao = Trim(Str(Int(TempoExecucao / 60))) & ":" & _
                                            Trim(Str(TempoExecucao - Int(TempoExecucao / 60) * 60))
                    MsgBox("Task cancelled by the user!" & vbCrLf & "Performance: " &
                        Trim(Str(ArquivoSelecionadoAtual)) & " Files in " & StrTempoExecucao)
                    Exit Sub
                End If

                'Seta o local da pasta
                If optPastaOrigem.Checked Then
                    PastaSalvar = CaminhoArquivo(ListaArquivos(ArquivoAtual).CaminhoCompleto)
                ElseIf optPastaSelecionada.Checked Then
                    PastaSalvar = txtCaminhoPasta.Text
                End If

                If ListaArquivos(ArquivoAtual).Checked Then
                    'O arquivo precisa estar marcado!

                    'Esta é a pasta onde será salvo o arquivo exportado.

                    'Abre o arquivo no Inventor.
                    Documento = ThisApplication.Documents.Open( _
                                ListaArquivos(ArquivoAtual).CaminhoCompleto)

                    ExportaFormatos(Documento, PastaSalvar, ArquivoAtual)

                    System.Windows.Forms.Application.DoEvents()

                    If My.Settings.FecharDesenhos Then
                        Documento.Close(My.Settings.IgnorarSalvar)
                    End If

                    ArquivoSelecionadoAtual = ArquivoSelecionadoAtual + 1
                End If

                'Atualiza a progressbar
                lblProgresso.Text = "Executing... " & Trim(Str(ArquivoSelecionadoAtual)) _
                                                & "/" & Trim(Str(QtdArquivosSelecionados))
                pgbProgresso.Value = ArquivoSelecionadoAtual

                System.Windows.Forms.Application.DoEvents()
            Next

            'Atualiza a progressbar
            lblProgresso.Text = "Idle."
            pgbProgresso.Value = 0

        ElseIf optEstaSecao.Checked Then
            'Descobre quantos arquivos tem para imprimir
            Dim DocumentoQualquer As Inventor.Document
            QtdArquivosSelecionados = 0

            For Each DocumentoQualquer In ThisApplication.Documents
                If DocumentoQualquer.DocumentType = Inventor.DocumentTypeEnum.kDrawingDocumentObject Then
                    QtdArquivosSelecionados = QtdArquivosSelecionados + 1
                End If
            Next

            'Zera a progressbar
            lblProgresso.Text = "Executing... 0/" & Trim(Str(QtdArquivosSelecionados))
            pgbProgresso.Value = 0
            ArquivoSelecionadoAtual = 0
            pgbProgresso.Maximum = QtdArquivosSelecionados

            For Each DocumentoQualquer In ThisApplication.Documents
                If DocumentoQualquer.DocumentType = Inventor.DocumentTypeEnum.kDrawingDocumentObject Then
                    If InterromperExecucao Then
                        'Termina de contar o tempo, e manda uma mensagem
                        TimerFim = Timer

                        TempoExecucao = Int(TimerFim - TimerIni)
                        StrTempoExecucao = Trim(Str(Int(TempoExecucao / 60))) & ":" & _
                                                Trim(Str(TempoExecucao - Int(TempoExecucao / 60) * 60))
                        MsgBox("Task cancelled by the user!" & vbCrLf & "Performance: " &
                            Trim(Str(ArquivoSelecionadoAtual)) & " Files in " & StrTempoExecucao)
                        GoTo Final
                    End If

                    PastaSalvar = CaminhoArquivo(DocumentoQualquer.FullFileName)

                    ExportaFormatos(DocumentoQualquer, PastaSalvar)

                    System.Windows.Forms.Application.DoEvents()

                    If My.Settings.FecharDesenhos Then
                        DocumentoQualquer.Close(My.Settings.IgnorarSalvar)
                    End If

                    ArquivoSelecionadoAtual = ArquivoSelecionadoAtual + 1

                    'Atualiza a Progressbar
                    lblProgresso.Text = "Executing... " & Trim(Str(ArquivoSelecionadoAtual)) _
                                                    & "/" & Trim(Str(QtdArquivosSelecionados))
                    pgbProgresso.Value = ArquivoSelecionadoAtual

                    System.Windows.Forms.Application.DoEvents()
                End If
            Next

        End If


        'Termina de contar o tempo, e manda uma mensagem
        TimerFim = Timer

        TempoExecucao = Int(TimerFim - TimerIni)
        StrTempoExecucao = Trim(Str(Int(TempoExecucao / 60))) & ":" & _
                                Trim(Str(TempoExecucao - Int(TempoExecucao / 60) * 60))
        MsgBox("Task completed successfully!" & vbCrLf & "Performance: " &
            Trim(Str(ArquivoSelecionadoAtual)) & " Files in " & StrTempoExecucao)

        ThisApplication.SilentOperation = False

Final:
        If My.Settings.FecharAuto Then
            Me.Close()
        End If

    End Sub

#End Region

#Region "Procedimentos e Funções"

    Private Sub CancelaTudo()
        InterromperExecucao = True
        System.Windows.Forms.Application.DoEvents()
        Me.Close()
    End Sub

    Private Sub Inicializar()
        'Seta no escopo do Form a aplicação do inventor
        ThisApplication = PDFemMassa.StandardAddInServer.m_inventorApplication

        'Coloca na progress bar o texto correto
        lblProgresso.Text = "Idle."
        pgbProgresso.Value = 0

        'Primeiro Grupo de Comandos: Origem dos Arquivos
        Dim ArquivoQualquer As Document
        If ThisApplication.Documents.Count = 0 Then
            For Each ArquivoQualquer In ThisApplication.Documents
                If ArquivoQualquer.DocumentType = DocumentTypeEnum.kDrawingDocumentObject Then
                    optEstaSecao.Checked = True
                    optEstaSecao.Enabled = True
                    optArquivosSelecionados.Checked = False
                    GoTo Grupo2
                End If
            Next
            optEstaSecao.Checked = False
            optEstaSecao.Enabled = False
            optArquivosSelecionados.Checked = True
        Else
            optEstaSecao.Checked = True
            optEstaSecao.Enabled = True
            optArquivosSelecionados.Checked = False
        End If

Grupo2:
        'Segundo Grupo de Comandos: Destino dos Arquivos
        'Sem necessidade de inicializar

        'Terceiro Grupo de Comandos: Tipos de Arquivo
        'Inicializa o exportador de PDF e checa se ele inicializou corretamente
        AddinPDF = ThisApplication.ApplicationAddIns.ItemById("{0AC6FD96-2F4D-42CE-8BE0-8AEA580399E4}")
        If AddinPDF Is Nothing Then
            chkPDF.Enabled = False
        Else
            chkPDF.Enabled = True
        End If

        'Inicializa o exportador de DXF e checa se ele inicializou corretamente
        AddinDXF = ThisApplication.ApplicationAddIns.ItemById("{C24E3AC4-122E-11D5-8E91-0010B541CD80}")
        If AddinDXF Is Nothing Then
            chkDXF.Enabled = False
        Else
            chkDXF.Enabled = True
        End If
        chkDXF.Checked = False

        'Inicializa o exportador de DWG e checa se ele inicializou corretamente
        AddinDWG = ThisApplication.ApplicationAddIns.ItemById("{C24E3AC2-122E-11D5-8E91-0010B541CD80}")
        If AddinDWG Is Nothing Then
            chkDWG.Enabled = False
        Else
            chkDWG.Enabled = True
        End If
        chkDWG.Checked = False

        'Enumera as impressoras
        Dim Impressora As String
        Dim SemPDF As Boolean = True
        Dim PDFCreator As Boolean = False

        cmbImpressoras.Items.Clear()
        If Not PrinterSettings.InstalledPrinters.Count = 0 Then
            For Each Impressora In PrinterSettings.InstalledPrinters
                cmbImpressoras.Items.Add(Impressora)
                cmbImpressoras2.Items.Add(Impressora)
                If Impressora = "PDFCreator" Then
                    cmbImpressoras2.Text = Impressora
                    SemPDF = False
                    PDFCreator = True
                ElseIf InStr(Impressora, "PDF", CompareMethod.Text) <> 0 And Not PDFCreator Then
                    cmbImpressoras2.Text = Impressora
                    SemPDF = False
                End If
            Next

            'Coloca foco na impressora default na Impressora 1
            Dim PrintSett As New System.Drawing.Printing.PrinterSettings
            cmbImpressoras.Text = PrintSett.PrinterName
            'Caso a impressora 2 não tenha conseguido nenhuma impressora PDF, coloca impressora default
            If SemPDF Then
                cmbImpressoras2.Text = PrintSett.PrinterName
            End If
        Else
            'Nenhuma impressora instalada!
            cmbImpressoras.Text = ""
            chkImprimir.Checked = False
            chkImprimir.Enabled = False
        End If

        FormLoaded = True
    End Sub

    Private Function CriaPDF(ByVal Documento As Inventor.DrawingDocument, _
                             ByVal PastaSalvar As String) As Boolean
        Try
            AddinPDF = ThisApplication.ApplicationAddIns.ItemById("{0AC6FD96-2F4D-42CE-8BE0-8AEA580399E4}")
            PDFContext = ThisApplication.TransientObjects.CreateTranslationContext
            PDFOptions = ThisApplication.TransientObjects.CreateNameValueMap
            PDFDataMedium = ThisApplication.TransientObjects.CreateDataMedium

            PDFContext.Type = IOMechanismEnum.kFileBrowseIOMechanism

            If AddinPDF.HasSaveCopyAsOptions(Documento, PDFContext, PDFOptions) Then
                'Seta as opções: Monochrome
                If My.Settings.PDFMonochrome Then
                    PDFOptions.Value("All_Colors_AS_Black") = 1
                Else
                    PDFOptions.Value("All_Colors_AS_Black") = 0
                End If
                'Seta as opções: Remover espessuras de linha
                If My.Settings.PDFLineweights Then
                    PDFOptions.Value("Remove_Line_Weights") = 1
                Else
                    PDFOptions.Value("Remove_Line_Weights") = 1
                End If
                'Seta as opções: Exportar todas as folhas
                If My.Settings.PDFFolhasExportar = 0 Then
                    PDFOptions.Value("Sheet_Range") = Inventor.PrintRangeEnum.kPrintAllSheets
                ElseIf My.Settings.PDFFolhasExportar = 1 Then
                    PDFOptions.Value("Sheet_Range") = Inventor.PrintRangeEnum.kPrintSheetRange
                    PDFOptions.Value("Custom_Begin_Sheet") = 1
                    PDFOptions.Value("Custom_End_Sheet") = 1
                End If
                'Seta as opções: Resolução Vetorial
                PDFOptions.Value("Vector_Resolution") = TabelaResolucoesPDF(My.Settings.PDFResolução)

            End If

            'Seta o caminho a salvar
            PDFDataMedium.FileName = PastaSalvar & Strings.Left( _
                                NomeDoArquivo(Documento.FullFileName), _
                                NomeDoArquivo(Documento.FullFileName).Length - 3) & "pdf"
            'Salva o arquivo.
            AddinPDF.SaveCopyAs(Documento, PDFContext, PDFOptions, PDFDataMedium)

        Catch ex As Exception
            MsgBox("Unhandled Exception!" & vbCrLf & "Error Description String: " & ex.ToString)
            CriaPDF = False
            Exit Function
        End Try
        CriaPDF = True
    End Function

    Private Function CriaDWG(ByVal Documento As Inventor.DrawingDocument, _
                             ByVal PastaSalvar As String) As Boolean
        Try
            AddinDWG = ThisApplication.ApplicationAddIns.ItemById("{C24E3AC2-122E-11D5-8E91-0010B541CD80}")
            DWGContext = ThisApplication.TransientObjects.CreateTranslationContext
            DWGOptions = ThisApplication.TransientObjects.CreateNameValueMap
            DWGDataMedium = ThisApplication.TransientObjects.CreateDataMedium

            DWGContext.Type = IOMechanismEnum.kFileBrowseIOMechanism

            If AddinDWG.HasSaveCopyAsOptions(Documento, DWGContext, DWGOptions) Then
                DWGOptions.Value("Export_Acad_IniFile") = My.Settings.DWGIniFile
            End If

            'Seta o caminho a salvar
            DWGDataMedium.FileName = PastaSalvar & Strings.Left( _
                                NomeDoArquivo(Documento.FullFileName), _
                                NomeDoArquivo(Documento.FullFileName).Length - 3) & "dwg"
            'Salva o arquivo.
            AddinDWG.SaveCopyAs(Documento, DWGContext, DWGOptions, DWGDataMedium)

        Catch ex As Exception
            MsgBox("Unhandled Exception!" & vbCrLf & "Error Description String: " & ex.ToString)
            CriaDWG = False
            Exit Function
        End Try
        CriaDWG = True

    End Function

    Private Function CriaDXF(ByVal Documento As Inventor.DrawingDocument, _
                             ByVal PastaSalvar As String) As Boolean
        Try
            AddinDXF = ThisApplication.ApplicationAddIns.ItemById("{C24E3AC2-122E-11D5-8E91-0010B541CD80}")
            DXFContext = ThisApplication.TransientObjects.CreateTranslationContext
            DXFOptions = ThisApplication.TransientObjects.CreateNameValueMap
            DXFDataMedium = ThisApplication.TransientObjects.CreateDataMedium

            DXFContext.Type = IOMechanismEnum.kFileBrowseIOMechanism

            If AddinDXF.HasSaveCopyAsOptions(Documento, DXFContext, DXFOptions) Then
                DXFOptions.Value("Export_Acad_IniFile") = My.Settings.DWGIniFile
            End If

            'Seta o caminho a salvar
            DXFDataMedium.FileName = PastaSalvar & Strings.Left( _
                                NomeDoArquivo(Documento.FullFileName), _
                                NomeDoArquivo(Documento.FullFileName).Length - 3) & "dxf"
            'Salva o arquivo.
            AddinDXF.SaveCopyAs(Documento, DXFContext, DXFOptions, DXFDataMedium)

        Catch ex As Exception
            MsgBox("Unhandled Exception" & vbCrLf & "Error Description String: " & ex.ToString)
            CriaDXF = False
            Exit Function
        End Try
        CriaDXF = True

    End Function

    Private Function PlanificaDXF(ByVal Documento As Inventor.DrawingDocument, _
                             ByVal PastaSalvar As String) As Boolean
        Try
            Dim Doc As Document
            Dim Part As PartDocument
            Dim Sheet As SheetMetalComponentDefinition
            Dim Dados As DataIO
            Dim sOut As String
            Dim Planificar As Boolean = False

            For Each Doc In Documento.ReferencedDocuments
                If Doc.DocumentType = DocumentTypeEnum.kPartDocumentObject Then
                    Part = Doc
                    If Part.SubType = "{9C464203-9BAE-11D3-8BAD-0060B0CE6BB4}" Then
                        Sheet = Part.ComponentDefinition

                        If Sheet.HasFlatPattern = False Then
                            Sheet.Unfold()
                            Planificar = True
                        End If

                        Dados = Part.ComponentDefinition.DataIO

                        sOut = "FLAT PATTERN DXF?AcadVersion=2000&OuterProfileLayer=CUT&InteriorProfilesLayer=CUT&InvisibleLayers=IV_TANGENT;IV_BEND;IV_BEND_DOWN"
                        Dados.WriteDataToFile(sOut, PastaSalvar & Strings.Left( _
                                NomeDoArquivo(Documento.FullFileName), _
                                NomeDoArquivo(Documento.FullFileName).Length - 3) & "dxf")

                        If Planificar Then
                            Part.Save()
                            Part.Close()
                        End If

                    End If
                End If
            Next

        Catch ex As Exception
            MsgBox("Unhandled Exception!" & vbCrLf & "Error Description String: " & ex.ToString)
            PlanificaDXF = False
            Exit Function
        End Try
        PlanificaDXF = True

    End Function

    Private Function Imprime(ByVal Documento As Inventor.DrawingDocument, _
                             ByVal Impressora As Integer, _
                             Optional ByVal PosLista As Integer = -1 _
                             ) As Boolean
        Try
            Dim PrintManager As Inventor.DrawingPrintManager
            Dim Folha As Inventor.Sheet
            Dim Inverter As Boolean
            Dim FolhasImprimir As Integer

            PrintManager = Documento.PrintManager

            If Impressora = 1 Then
                PrintManager.Printer = cmbImpressoras.Text

                'Ajusta as settings
                PrintManager.NumberOfCopies = My.Settings.ImpressaoNumCopias
                PrintManager.AllColorsAsBlack = My.Settings.ImpressaoMonochrome
                PrintManager.RemoveLineWeights = My.Settings.ImpressaoLineweights
                PrintManager.Rotate90Degrees = My.Settings.ImpressaoRotacionar90
                Inverter = My.Settings.ImpressaoInverter
                FolhasImprimir = My.Settings.ImpressaoFolhasImprimir
            ElseIf Impressora = 2 Then
                PrintManager.Printer = cmbImpressoras2.Text

                'Ajusta as settings
                PrintManager.NumberOfCopies = My.Settings.ImpressaoNumCopias2
                PrintManager.AllColorsAsBlack = My.Settings.ImpressaoMonochrome2
                PrintManager.RemoveLineWeights = My.Settings.ImpressaoLineweights2
                PrintManager.Rotate90Degrees = My.Settings.ImpressaoRotacionar902
                Inverter = My.Settings.ImpressaoInverter2
                FolhasImprimir = My.Settings.ImpressaoFolhasImprimir2
            Else
                Exit Function
            End If


            PrintManager.Scale = Inventor.PrintScaleModeEnum.kPrintBestFitScale
            PrintManager.PrintRange = Inventor.PrintRangeEnum.kPrintCurrentSheet

            For Each Folha In Documento.Sheets
                Folha.Activate()

                'Verifica se o arquivo é da lista (tem configurações adicionais)
                If Not PosLista = -1 Then
                    If ListaArquivos(PosLista).FormatoPredefinido Then
                        If Inverter Then
                            PrintManager.PaperHeight = Folha.Width
                            PrintManager.PaperWidth = Folha.Height
                        Else
                            PrintManager.PaperHeight = Folha.Height
                            PrintManager.PaperWidth = Folha.Width
                        End If
                    Else
                        If Inverter Then
                            PrintManager.PaperHeight = ListaArquivos(PosLista).FormatoLargura
                            PrintManager.PaperWidth = ListaArquivos(PosLista).FormatoAltura
                        Else
                            PrintManager.PaperHeight = ListaArquivos(PosLista).FormatoAltura
                            PrintManager.PaperWidth = ListaArquivos(PosLista).FormatoLargura
                        End If
                    End If
                    If ListaArquivos(PosLista).PosicaoPredefinida Then
                        PrintManager.Orientation = Folha.Orientation
                    Else
                        If ListaArquivos(PosLista).Posicao = "Retrato" Then
                            PrintManager.Orientation = PrintOrientationEnum.kPortraitOrientation
                        ElseIf ListaArquivos(PosLista).Posicao = "Paisagem" Then
                            PrintManager.Orientation = PrintOrientationEnum.kLandscapeOrientation
                        Else
                            PrintManager.Orientation = PrintOrientationEnum.kDefaultOrientation
                        End If
                    End If
                Else
                    If Inverter Then
                        PrintManager.PaperHeight = Folha.Width
                        PrintManager.PaperWidth = Folha.Height
                    Else
                        PrintManager.PaperHeight = Folha.Height
                        PrintManager.PaperWidth = Folha.Width
                    End If
                    PrintManager.Orientation = Folha.Orientation
                End If

                'Imprime.
                PrintManager.SubmitPrint()

                If FolhasImprimir = 1 Then
                    'Imprime apenas a primeira folha
                    GoTo Final
                End If
            Next

        Catch ex As Exception
            MsgBox("Unhandled Exception!" & vbCrLf & "Error Description String: " & ex.ToString)
            Imprime = False
            Exit Function
        End Try
Final:
        Imprime = True
    End Function

    Private Sub ExportaFormatos(ByVal Documento As Inventor.DrawingDocument, _
                             ByVal PastaSalvar As String, Optional ByVal PosLista As Integer = -1)
        If chkPDF.Checked Then
            'Exporta PDF.
            CriaPDF(Documento, PastaSalvar)
        End If

        If chkDWG.Checked Then
            'Exporta DWG.
            CriaDWG(Documento, PastaSalvar)
        End If

        If chkDXF.Checked Then
            'Exporta DXF.
            CriaDXF(Documento, PastaSalvar)
        End If

        If chkPlanifDXF.Checked Then
            'Exporta Planificação da chapa em DXF.
            PlanificaDXF(Documento, PastaSalvar)
        End If

        If chkImprimir.Checked Then
            'Imprime o arquivo.
            Imprime(Documento, 1, PosLista)
        End If

        If chkImprimir2.Checked Then
            'Imprime o arquivo.
            Imprime(Documento, 2, PosLista)
        End If

    End Sub

#End Region

End Class