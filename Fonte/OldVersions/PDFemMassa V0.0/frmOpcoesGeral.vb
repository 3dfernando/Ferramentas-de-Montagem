﻿Public Class frmOpcoesGeral

#Region "Variaveis e Objetos"
    Private SettingsLabelsPapelTemp As Collections.Specialized.StringCollection
    Private SettingsTamanhosPapelTemp As Collections.Specialized.StringCollection

    Private SettingsLocaisPesquisaTemp As Collections.Specialized.StringCollection
    Private SettingsPesquisarPrimeiroTemp As Boolean
    Private SettingsPesquisarSubPastas As Boolean
    Private SettingsPesquisarIDW As Boolean
    Private SettingsPesquisarDWG As Boolean

    Private SettingsFecharAutoTemp As Boolean
    Private SettingsSilentModeTemp As Boolean
    Private SettingsIgnorarSaveTemp As Boolean
    Private SettingsFecharDesenhosTemp As Boolean
    Private SettingsInverterOrdem As Boolean

    Private SettingsPDFFolhasExportar As Integer
    Private SettingsPDFResolucao As Integer
    Private SettingsPDFMonochrome As Boolean
    Private SettingsPDFLineweights As Boolean

    Private SettingsIniFile As String

    Private SettingsImpressaoFolhasImprimir As Integer
    Private SettingsImpressaoNumCopias As Integer
    Private SettingsImpressaoLineweights As Integer
    Private SettingsImpressaoMonochrome As Integer
    Private SettingsImpressaoRotacionar90 As Integer
    Private SettingsImpressaoInverter As Integer
    Private SettingsImpressaoAgrupar As Integer

#End Region

#Region "COMANDOS GERAIS"

    Private Sub cmdCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelar.Click
        Me.Close()
    End Sub

    Private Sub frmOpcoesGeral_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Carrega os settings dentro das variáveis temporárias:
        
        SettingsLabelsPapelTemp = My.Settings.LabelsPapel
        SettingsTamanhosPapelTemp = My.Settings.TamanhosPapel

        SettingsLocaisPesquisaTemp = My.Settings.LocaisPadraoPesquisa
        SettingsPesquisarPrimeiroTemp = My.Settings.PesquisarPrimeiro
        SettingsPesquisarSubPastas = My.Settings.PesquisarSubPastas
        SettingsPesquisarIDW = My.Settings.pesquisaridw
        SettingsPesquisarDWG = My.Settings.PesquisarDWG

        SettingsFecharAutoTemp = My.Settings.FecharAuto
        SettingsSilentModeTemp = My.Settings.SilentMode
        SettingsIgnorarSaveTemp = My.Settings.IgnorarSalvar
        SettingsFecharDesenhosTemp = My.Settings.FecharDesenhos
        SettingsInverterOrdem = My.Settings.InverterOrdem

        SettingsPDFFolhasExportar = My.Settings.PDFFolhasExportar
        SettingsPDFResolucao = My.Settings.PDFResolução
        SettingsPDFMonochrome = My.Settings.PDFMonochrome
        SettingsPDFLineweights = My.Settings.PDFLineweights

        SettingsIniFile = My.Settings.DWGIniFile

        SettingsImpressaoFolhasImprimir = My.Settings.ImpressaoFolhasImprimir
        SettingsImpressaoNumCopias = My.Settings.ImpressaoNumCopias
        SettingsImpressaoLineweights = My.Settings.ImpressaoLineweights
        SettingsImpressaoMonochrome = My.Settings.ImpressaoMonochrome
        SettingsImpressaoRotacionar90 = My.Settings.ImpressaoRotacionar90
        SettingsImpressaoAgrupar = My.Settings.ImpressaoAgruparFormatos
        SettingsImpressaoInverter = My.Settings.ImpressaoInverter


        'Carrega os Settings dentro da aba "GERAL"

        chkFecharAuto.Checked = SettingsFecharAutoTemp
        chkSilentMode.Checked = SettingsSilentModeTemp
        chkFecharDesenhos.Checked = SettingsFecharDesenhosTemp
        chkIgnorarSalvar.Checked = SettingsIgnorarSaveTemp
        chkInverterOrdem.Checked = SettingsInverterOrdem

        If chkSilentMode.Checked And chkFecharDesenhos.Checked Then
            chkIgnorarSalvar.Enabled = True
        Else
            chkIgnorarSalvar.Enabled = False
        End If

        'Carrega os Settings dentro da aba "FORMATOS"
        Dim I As Integer
        Dim Count As Integer

        'Checa consistência
        'Se os tamanhos das arrays forem diferentes, pega o menor tamanho
        If My.Settings.LabelsPapel.Count < My.Settings.TamanhosPapel.Count Then
            Count = My.Settings.LabelsPapel.Count
        Else
            Count = My.Settings.TamanhosPapel.Count
        End If

        lstFormatos.Items.Clear()

        For I = 0 To Count - 1
            'Popula a lista
            lstFormatos.Items.Add(My.Settings.LabelsPapel(I))
        Next

        'Carrega os Settings dentro da aba "PESQUISA"

        lstLocaisPadraoPesquisa.Items.Clear()
        For I = 0 To SettingsLocaisPesquisaTemp.Count - 1
            'Popula a lista
            lstLocaisPadraoPesquisa.Items.Add(SettingsLocaisPesquisaTemp(I))
        Next

        chkPesquisaPrimeiro.Checked = SettingsPesquisarPrimeiroTemp
        chkPesquisarSubpastas.Checked = SettingsPesquisarSubPastas
        chkPesquisarDWG.Checked = SettingsPesquisarDWG
        chkPesquisarIDW.Checked = SettingsPesquisarIDW

        'Carrega os Settings dentro da aba "EXPORTAÇÃO"

        cmbFolhasPDF.SelectedIndex = SettingsPDFFolhasExportar
        cmbResolucaoPDF.SelectedIndex = SettingsPDFResolucao
        chkMonochromePDF.Checked = SettingsPDFMonochrome
        chkRemoveLineweightsPDF.Checked = SettingsPDFLineweights

        txtIniFile.Text = SettingsIniFile

        'Carrega os Settings dentro da aba "IMPRESSÃO"

        cmbImpressaoFolhas.SelectedIndex = SettingsImpressaoFolhasImprimir
        updNumCopias.Value = SettingsImpressaoNumCopias
        chkImpressaoRemoveLineweights.Checked = SettingsImpressaoLineweights
        chkImpressaoMonochrome.Checked = SettingsImpressaoMonochrome
        chkImpressaoRotacionar90.Checked = SettingsImpressaoRotacionar90
        chkImpressaoAgruparFormatos.Checked = SettingsImpressaoAgrupar
        chkImpressaoInverter.Checked = SettingsImpressaoInverter

    End Sub

    Private Sub cmdSaveClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveClose.Click
        'Salva as alterações e fecha o form
        Try
            My.Settings.LabelsPapel = SettingsLabelsPapelTemp
            My.Settings.TamanhosPapel = SettingsTamanhosPapelTemp

            My.Settings.LocaisPadraoPesquisa = SettingsLocaisPesquisaTemp
            My.Settings.PesquisarPrimeiro = SettingsPesquisarPrimeiroTemp
            My.Settings.pesquisarsubpastas = SettingsPesquisarSubPastas
            My.Settings.PesquisarIDW = SettingsPesquisarIDW
            My.Settings.PesquisarDWG = SettingsPesquisarDWG

            My.Settings.FecharAuto = SettingsFecharAutoTemp
            My.Settings.SilentMode = SettingsSilentModeTemp
            My.Settings.FecharDesenhos = SettingsFecharDesenhosTemp
            My.Settings.IgnorarSalvar = SettingsIgnorarSaveTemp
            My.Settings.InverterOrdem = SettingsInverterOrdem

            My.Settings.PDFFolhasExportar = SettingsPDFFolhasExportar
            My.Settings.PDFResolução = SettingsPDFResolucao
            My.Settings.PDFMonochrome = SettingsPDFMonochrome
            My.Settings.PDFLineweights = SettingsPDFLineweights

            My.Settings.DWGIniFile = SettingsIniFile

            My.Settings.ImpressaoFolhasImprimir = SettingsImpressaoFolhasImprimir
            My.Settings.ImpressaoNumCopias = SettingsImpressaoNumCopias
            My.Settings.ImpressaoLineweights = SettingsImpressaoLineweights
            My.Settings.ImpressaoMonochrome = SettingsImpressaoMonochrome
            My.Settings.ImpressaoRotacionar90 = SettingsImpressaoRotacionar90
            My.Settings.ImpressaoAgruparFormatos = SettingsImpressaoAgrupar
            My.Settings.ImpressaoInverter = SettingsImpressaoInverter


            My.Settings.Save()
        Catch ex As Exception

        End Try

        Me.Close()
    End Sub

#End Region

#Region "ABA GERAL"

    Private Sub chkFecharAuto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFecharAuto.CheckedChanged
        SettingsFecharAutoTemp = chkFecharAuto.Checked
    End Sub

    Private Sub chkSilentMode_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSilentMode.CheckedChanged
        SettingsSilentModeTemp = chkSilentMode.Checked

        If chkSilentMode.Checked And chkFecharDesenhos.Checked Then
            chkIgnorarSalvar.Enabled = True
        Else
            chkIgnorarSalvar.Enabled = False
            chkIgnorarSalvar.Checked = False
        End If
    End Sub

    Private Sub chkIgnorarSalvar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIgnorarSalvar.CheckedChanged
        SettingsIgnorarSaveTemp = chkIgnorarSalvar.Checked
    End Sub

    Private Sub chkFecharDesenhos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFecharDesenhos.CheckedChanged
        SettingsFecharDesenhosTemp = chkFecharDesenhos.Checked

        If chkSilentMode.Checked And chkFecharDesenhos.Checked Then
            chkIgnorarSalvar.Enabled = True
        Else
            chkIgnorarSalvar.Enabled = False
            chkIgnorarSalvar.Checked = False
        End If
    End Sub

    Private Sub chkInverterOrdem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInverterOrdem.CheckedChanged
        SettingsInverterOrdem = chkInverterOrdem.Checked
    End Sub
#End Region

#Region "ABA FORMATO"

    Private Sub lstFormatos_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstFormatos.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Delete Then
            If Not lstFormatos.SelectedIndex < 0 Or lstFormatos.SelectedIndex > lstFormatos.Items.Count - 1 Then
                cmdExcluirFormato_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub lstFormatos_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstFormatos.SelectedIndexChanged
        'Coloca nos devidos campos, seus valores
        Dim Indice, I, J As Integer
        Dim Label, Tamanho As String
        Dim Unid As String = vbNullString

        Indice = lstFormatos.SelectedIndex
        Try
            Label = SettingsLabelsPapelTemp(Indice)
            Tamanho = SettingsTamanhosPapelTemp(Indice)

            'Descrição:
            'Encontra o primeiro Abre Parênteses "(" e coloca tudo que está antes dele:
            '(Por esta razão, os caracteres parênteses são proibidos na hora de salvar)
            For I = 1 To Label.Length
                If Mid(Label, I, 1) = "(" Then
                    txtDescricao.Text = Strings.Left(Label, I - 2)
                    Exit For
                End If
            Next

            'Dimensões:
            'Altura
            For I = 1 To Tamanho.Length
                If Mid(Tamanho, I, 1) = "x" Then
                    txtAltura.Text = Strings.Left(Tamanho, I - 1)
                    Exit For
                End If
            Next
            'Largura
            For J = I + 1 To Tamanho.Length
                If Mid(Tamanho, J, 1) = "x" Then
                    txtLargura.Text = Mid(Tamanho, I + 1, J - I - 1)
                    Exit For
                End If
            Next

            'Unidade:
            For I = Tamanho.Length - 1 To 1 Step -1
                If Mid(Tamanho, I, 1) = "x" Then
                    Unid = Strings.Right(Tamanho, Tamanho.Length - I)
                    Exit For
                End If
            Next
            If Unid = "mm" Then
                optMilimetros.Checked = True
                optPolegadas.Checked = False
            ElseIf Unid = "in" Then
                optMilimetros.Checked = False
                optPolegadas.Checked = True
            End If
            EnableComandosFormato(True)
        Catch ex As Exception
            'Provavelmente out of bounds!
            EnableComandosFormato(False)
            txtAltura.Text = ""
            txtDescricao.Text = ""
            txtLargura.Text = ""
        End Try

        If Indice = 0 Then cmdUpFormato.Enabled = False
        If Indice = lstFormatos.Items.Count - 1 Then cmdDownFormato.Enabled = False

    End Sub

    Private Sub cmdCancelarFormato_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancelarFormato.Click
        'Seleciona nada
        lstFormatos.ClearSelected()
    End Sub

    Private Sub EnableComandosFormato(ByVal Enable As Boolean)
        'Habilita ou desabilita todos os comandos e textos que são necessários
        txtAltura.Enabled = Enable
        txtDescricao.Enabled = Enable
        txtLargura.Enabled = Enable
        optMilimetros.Enabled = Enable
        optPolegadas.Enabled = Enable
        cmdAlterarFormato.Enabled = Enable
        cmdExcluirFormato.Enabled = Enable
        cmdUpFormato.Enabled = Enable
        cmdDownFormato.Enabled = Enable
        cmdCancelarFormato.Enabled = Enable
    End Sub

    Private Sub cmdAdicionarFormato_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdicionarFormato.Click
        'Adiciona um novo formato Dummy "Formato 100x100"
        SettingsLabelsPapelTemp.Add("Formato (100mm x 100mm)")
        SettingsTamanhosPapelTemp.Add("100x100xmm")
        lstFormatos.Items.Add("Formato (100mm x 100mm)")
        lstFormatos.SelectedIndex = lstFormatos.Items.Count - 1
    End Sub

    Private Sub cmdExcluirFormato_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcluirFormato.Click
        'Exclui uma linha
        Dim Index As Integer
        Index = lstFormatos.SelectedIndex
        SettingsLabelsPapelTemp.RemoveAt(Index)
        SettingsTamanhosPapelTemp.RemoveAt(Index)
        lstFormatos.Items.RemoveAt(Index)

        lstFormatos.ClearSelected()
    End Sub

    Private Sub cmdAlterarFormato_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAlterarFormato.Click
        'Altera uma linha da listbox
        Dim Index As Integer
        Dim LabelNova, TamanhoNovo As String
        Dim UnidLabel As String = vbNullString
        Dim UnidFormato As String = vbNullString

        'Faz as checagens antes de fazer o cadastro!

        If InStr(txtDescricao.Text, "(") > 0 Or InStr(txtDescricao.Text, ")") > 0 Then
            MsgBox("A descrição não pode conter parênteses!", MsgBoxStyle.OkOnly, _
                   "Erro de sintaxe!")
            txtDescricao.Focus()
            Exit Sub
        End If

        If Val(txtAltura.Text) <= 0 Then
            MsgBox("A altura deve ser um número real e maior que zero!", MsgBoxStyle.OkOnly, _
                   "Erro de sintaxe!")
            txtAltura.Focus()
            Exit Sub
        End If

        If Val(txtLargura.Text) <= 0 Then
            MsgBox("A largura deve ser um número real e maior que zero!", MsgBoxStyle.OkOnly, _
                   "Erro de sintaxe!")
            txtLargura.Focus()
            Exit Sub
        End If

        'Altera os campos de número pelos seus valores interpretados, para evitar dúvidas!
        txtLargura.Text = Trim(Str(Val(txtLargura.Text)))
        txtAltura.Text = Trim(Str(Val(txtAltura.Text)))

        'Cadastra.
        Index = lstFormatos.SelectedIndex

        If optMilimetros.Checked = True Then
            UnidLabel = "mm"
            UnidFormato = "mm"
        ElseIf optPolegadas.Checked = True Then
            UnidLabel = """"
            UnidFormato = "in"
        End If

        LabelNova = txtDescricao.Text & " (" & txtAltura.Text & UnidLabel & " x " & _
                    txtLargura.Text & UnidLabel & ")"
        TamanhoNovo = txtAltura.Text & "x" & txtLargura.Text & "x" & UnidFormato

        lstFormatos.Items(Index) = LabelNova
        SettingsLabelsPapelTemp(Index) = LabelNova
        SettingsTamanhosPapelTemp(Index) = TamanhoNovo

        'Seleciona nada.
        lstFormatos.ClearSelected()
    End Sub

    Private Sub cmdUpFormato_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpFormato.Click
        'Move a seleção uma linha para cima
        Dim Index As Integer

        Index = lstFormatos.SelectedIndex

        If Not Index = 0 Then SwapDuasLinhas(Index, Index - 1)
    End Sub

    Private Sub cmdDownFormato_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDownFormato.Click
        'Move a seleção uma linha para baixo
        Dim Index As Integer

        Index = lstFormatos.SelectedIndex

        If Not Index = 0 Then SwapDuasLinhas(Index, Index + 1)
    End Sub

    Private Sub SwapDuasLinhas(ByVal Linha1 As Integer, ByVal Linha2 As Integer)
        'Troca duas linhas de lugar
        Dim TempString As String

        TempString = SettingsLabelsPapelTemp(Linha1)
        SettingsLabelsPapelTemp(Linha1) = SettingsLabelsPapelTemp(Linha2)
        SettingsLabelsPapelTemp(Linha2) = TempString

        TempString = SettingsTamanhosPapelTemp(Linha1)
        SettingsTamanhosPapelTemp(Linha1) = SettingsTamanhosPapelTemp(Linha2)
        SettingsTamanhosPapelTemp(Linha2) = TempString

        TempString = lstFormatos.Items(Linha1)
        lstFormatos.Items(Linha1) = lstFormatos.Items(Linha2)
        lstFormatos.Items(Linha2) = TempString

        lstFormatos.SelectedIndex = Linha2
    End Sub

#End Region

#Region "ABA PESQUISA"

    Private Sub EnableComandosPesquisa(ByVal Enable As Boolean)
        'Modifica a propriedade enabled dos botões necessários na aba pesquisa
        cmdUpPesquisa.Enabled = Enable
        cmdDownPesquisa.Enabled = Enable
        cmdExcluirPastaPesquisa.Enabled = Enable
    End Sub

    Private Sub lstLocaisPadraoPesquisa_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstLocaisPadraoPesquisa.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Delete Then
            cmdExcluirPastaPesquisa_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub lstLocaisPadraoPesquisa_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstLocaisPadraoPesquisa.SelectedIndexChanged

        If lstLocaisPadraoPesquisa.SelectedIndex >= lstLocaisPadraoPesquisa.Items.Count _
            Or lstLocaisPadraoPesquisa.SelectedIndex < 0 Then
            EnableComandosPesquisa(False)
        Else
            EnableComandosPesquisa(True)
            If lstLocaisPadraoPesquisa.SelectedIndex = 0 Then
                cmdUpPesquisa.Enabled = False
            End If
            If lstLocaisPadraoPesquisa.SelectedIndex = lstLocaisPadraoPesquisa.Items.Count - 1 Then
                cmdDownPesquisa.Enabled = False
            End If
        End If

    End Sub

    Private Sub cmdAdicionarPastaPesquisa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdicionarPastaPesquisa.Click
        'Adiciona uma nova pasta no caminho de pesquisa
        Dim TempDir As String
        Dim PastaContem As Integer = 0
        Dim PastasContidas() As Integer
        Dim Index As Integer = 0
        Dim I As Integer

        ReDim PastasContidas(0)

        DialogoPasta.ShowDialog()

        If Not DialogoPasta.SelectedPath = vbNullString Then
            'Testa se o diretório selecionado não é subpasta de qualquer dos diretórios já criados
            For Each TempDir In SettingsLocaisPesquisaTemp
                If InStr(DialogoPasta.SelectedPath, TempDir) Then
                    MsgBox("A pasta selecionada já existe ou é subpasta de uma das pastas existentes!", _
                        MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Impossível adicionar")
                    Exit Sub
                End If
            Next
            'Testa se o diretório selecionado não é uma pasta que contém algum dos diretórios já criados
            For Each TempDir In SettingsLocaisPesquisaTemp
                If InStr(TempDir, DialogoPasta.SelectedPath) Then
                    'Adiciona cada pasta contida dentro de uma array
                    ReDim Preserve PastasContidas(PastaContem)
                    PastasContidas(PastaContem) = Index

                    PastaContem = PastaContem + 1
                End If
                Index = Index + 1
            Next

            If PastaContem > 0 Then
                Dim Res As MsgBoxResult
                If PastaContem = 1 Then
                    Res = MsgBox("A pasta selecionada contém uma das pastas existentes como sua subpasta." & _
                                 vbCrLf & "Deseja excluir as pastas existentes e deixar apenas a pasta selecionada?" _
                        , MsgBoxStyle.Information Or MsgBoxStyle.YesNoCancel, "Informação")
                Else
                    Res = MsgBox("A pasta selecionada contém " & Trim(Str(PastaContem)) & " pastas existentes como sua subpasta." & _
                                 vbCrLf & "Deseja excluir as pastas existentes e deixar apenas a pasta selecionada?" _
                        , MsgBoxStyle.Information Or MsgBoxStyle.YesNoCancel, "Informação")
                End If

                If Res = MsgBoxResult.Yes Then
                    'Sim=Excluir as subpastas
                    For I = 0 To PastaContem - 1
                        SettingsLocaisPesquisaTemp.RemoveAt(PastasContidas(I) - I)
                        lstLocaisPadraoPesquisa.Items.RemoveAt(PastasContidas(I) - I)
                    Next
                ElseIf Res = MsgBoxResult.No Then
                    'Não=Incluir a pasta de qualquer maneira
                    '(Não faz nada, afinal o end if segue adiante
                Else
                    'Cancelar!
                    Exit Sub
                End If
            End If

            'Adiciona o diretório
            SettingsLocaisPesquisaTemp.Add(DialogoPasta.SelectedPath)
            lstLocaisPadraoPesquisa.Items.Add(DialogoPasta.SelectedPath)
            lstLocaisPadraoPesquisa.ClearSelected()
        End If
    End Sub

    Private Sub chkPesquisaPrimeiro_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPesquisaPrimeiro.CheckedChanged
        SettingsPesquisarPrimeiroTemp = chkPesquisaPrimeiro.Checked
    End Sub

    Private Sub cmdExcluirPastaPesquisa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcluirPastaPesquisa.Click
        'Exclui um ítem da lista de locais
        Dim Index As Integer
        If Not (lstLocaisPadraoPesquisa.SelectedIndex >= lstLocaisPadraoPesquisa.Items.Count _
            Or lstLocaisPadraoPesquisa.SelectedIndex < 0) Then

            Index = lstLocaisPadraoPesquisa.SelectedIndex
            lstLocaisPadraoPesquisa.Items.RemoveAt(Index)
            SettingsLocaisPesquisaTemp.RemoveAt(Index)

            lstLocaisPadraoPesquisa.ClearSelected()
        End If
    End Sub

    Private Sub cmdUpPesquisa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpPesquisa.Click
        'Move a seleção uma linha para cima
        Dim Index As Integer

        Index = lstLocaisPadraoPesquisa.SelectedIndex

        If Not Index = 0 Then SwapDuasLinhasPesquisa(Index, Index - 1)
    End Sub

    Private Sub cmdDownPesquisa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDownPesquisa.Click
        'Move a seleção uma linha para cima
        Dim Index As Integer

        Index = lstLocaisPadraoPesquisa.SelectedIndex

        If Not Index = lstLocaisPadraoPesquisa.Items.Count - 1 Then SwapDuasLinhasPesquisa(Index, Index + 1)
    End Sub

    Private Sub SwapDuasLinhasPesquisa(ByVal Linha1 As Integer, ByVal Linha2 As Integer)
        'Troca duas linhas de lugar
        Dim TempString As String

        TempString = SettingsLocaisPesquisaTemp(Linha1)
        SettingsLocaisPesquisaTemp(Linha1) = SettingsLocaisPesquisaTemp(Linha2)
        SettingsLocaisPesquisaTemp(Linha2) = TempString

        TempString = lstLocaisPadraoPesquisa.Items(Linha1)
        lstLocaisPadraoPesquisa.Items(Linha1) = lstLocaisPadraoPesquisa.Items(Linha2)
        lstLocaisPadraoPesquisa.Items(Linha2) = TempString

        lstLocaisPadraoPesquisa.SelectedIndex = Linha2
    End Sub

    Private Sub chkPesquisarIDW_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPesquisarIDW.CheckedChanged
        SettingsPesquisarIDW = chkPesquisarIDW.Checked
    End Sub

    Private Sub chkPesquisarDWG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPesquisarDWG.CheckedChanged
        SettingsPesquisarDWG = chkPesquisarDWG.Checked
    End Sub

    Private Sub chkPesquisarSubpastas_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPesquisarSubpastas.CheckedChanged
        SettingsPesquisarSubPastas = chkPesquisarSubpastas.Checked
    End Sub

#End Region

#Region "ABA EXPORTAÇÃO"

    Private Sub cmbFolhasPDF_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFolhasPDF.SelectedIndexChanged
        SettingsPDFFolhasExportar = cmbFolhasPDF.SelectedIndex
    End Sub

    Private Sub cmbResolucaoPDF_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbResolucaoPDF.SelectedIndexChanged
        SettingsPDFResolucao = cmbResolucaoPDF.SelectedIndex
    End Sub

    Private Sub chkMonochromePDF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMonochromePDF.CheckedChanged
        SettingsPDFMonochrome = chkMonochromePDF.Checked
    End Sub

    Private Sub chkRemoveLineweightsPDF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRemoveLineweightsPDF.CheckedChanged
        SettingsPDFLineweights = chkRemoveLineweightsPDF.Checked
    End Sub

    Private Sub cmdIniFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdIniFile.Click
        Dim Caminho As String

        DialogoIni.ShowDialog()

        Caminho = DialogoIni.FileName

        If (Not Caminho = "") Or System.IO.File.Exists(Caminho) Then
            txtIniFile.Text = Caminho
            SettingsIniFile = Caminho
        End If
    End Sub
#End Region

#Region "ABA IMPRESSÃO"

    Private Sub cmbImpressaoFolhas_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbImpressaoFolhas.SelectedIndexChanged
        SettingsImpressaoFolhasImprimir = cmbImpressaoFolhas.SelectedIndex
    End Sub

    Private Sub updNumCopias_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles updNumCopias.ValueChanged
        SettingsImpressaoNumCopias = Int(updNumCopias.Value)
    End Sub

    Private Sub chkImpressaoMonochrome_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkImpressaoMonochrome.CheckedChanged
        SettingsImpressaoMonochrome = chkImpressaoMonochrome.Checked
    End Sub

    Private Sub chkImpressaoRemoveLineweights_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkImpressaoRemoveLineweights.CheckedChanged
        SettingsImpressaoLineweights = chkImpressaoRemoveLineweights.Checked
    End Sub

    Private Sub chkImpressaoRotacionar90_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkImpressaoRotacionar90.CheckedChanged
        SettingsImpressaoRotacionar90 = chkImpressaoRotacionar90.Checked
    End Sub

    Private Sub chkImpressaoAgruparFormatos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkImpressaoAgruparFormatos.CheckedChanged
        SettingsImpressaoAgrupar = chkImpressaoAgruparFormatos.Checked
    End Sub

    Private Sub chkImpressaoInverter_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkImpressaoInverter.CheckedChanged
        SettingsImpressaoInverter = chkImpressaoInverter.Checked
    End Sub
#End Region

End Class
