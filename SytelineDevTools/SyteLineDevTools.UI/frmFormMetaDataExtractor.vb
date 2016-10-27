﻿Imports SyteLineDevTools.UI
Imports Mongoose.IDO
Imports Mongoose.WinStudio
Imports Mongoose.WinStudio.Enums
Imports SyteLineDevTools
Imports System.IO
<FormMenuItem(Description:="Form Scripting")>
Public Class frmSyteLineDevTools

    Private Const STR_SLDevTools As String = "SL Dev Tools"
    Private Function GetClient() As Client
        Dim oclient As New Client(txtServerSpec.Text, IDOProtocol.Http)
        oclient.OpenSession(txtUser.Text, txtPassword.Text, txtConfig.Text)

        Return oclient
    End Function
    Private Sub btnVendor_Click(sender As Object, e As EventArgs) Handles btnVendor.Click
        Try
            ExtractForScope(Enums.ScopeTypes.SYMIX_DEFAULT)
            MessageBox.Show("Vendor Forms Extracted Successfully",STR_SLDevTools,MessageBoxButtons.OK,MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message,STR_SLDevTools,MessageBoxButtons.OK,MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnNonVendor_Click(sender As Object, e As EventArgs) Handles btnNonVendor.Click
        Try
            ExtractForScope(Enums.ScopeTypes.SITE_DEFAULT)
            MessageBox.Show("Site Forms Extracted Successfully",STR_SLDevTools,MessageBoxButtons.OK,MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message,STR_SLDevTools,MessageBoxButtons.OK,MessageBoxIcon.Error)
        End Try
        
    End Sub

    Private Sub ExtractForScope(scope As Enums.ScopeTypes)
        Dim oclient = GetClient()

        Try
            Cursor.Current = Cursors.WaitCursor 
            Dim extractor As New FormExtractor(oclient)
            Dim forms = extractor.ExtractAllFormsForScope(scope, "", "")
            forms.ForEach(Sub(f) ObjectPersister.PersistObject(txtOutputPath.Text, GlobalObjectType.Form, f.Name, f))

        Finally
            Cursor.Current = Cursors.Default  
            oclient.CloseSession()
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim oclient = GetClient()

        Try
            Cursor.Current = Cursors.WaitCursor
            Dim extractor As New FormExtractor(oclient)
            Dim filename = ObjectPersister.GetObjectScript(txtOutputPath.Text, GlobalObjectType.Form,"1099FormPrintingReport")
            extractor.UploadFormToSyteline(filename)
            MessageBox.Show("Site Forms Pushed Successfully",STR_SLDevTools,MessageBoxButtons.OK,MessageBoxIcon.Information)
        Finally
            Cursor.Current = Cursors.Default
            oclient.CloseSession()
        End Try
    End Sub

    Private Sub cboForm_DropDown(sender As Object, e As EventArgs) Handles cboForm.DropDown
        Dim oclient = GetClient()

        cboForm.Items.Clear()
        Try
            Cursor.Current = Cursors.WaitCursor
            Dim formlist As New FormExtractor(oclient)
            If cboScope.SelectedItem = "Vendor" Then
                cboForm.Items.AddRange(formlist.GetFormList(Enums.ScopeTypes.SYMIX_DEFAULT, "", "").ToArray)
            ElseIf cboScope.SelectedItem = "Site" Then
                cboForm.Items.AddRange(formlist.GetFormList(Enums.ScopeTypes.SITE_DEFAULT, "", "").ToArray)
            End If
        Finally
            Cursor.Current = Cursors.Default
            oclient.CloseSession()
        End Try

    End Sub

    Private Sub ExtractFormForScope(ByVal formName As String, ByVal scope As Enums.ScopeTypes)
        Dim oclient = GetClient()

        Try
            Cursor.Current = Cursors.WaitCursor
            Dim extractor As New FormExtractor(oclient)
            Dim form = extractor.GetFormByScope(formName, scope, "", "")
            ObjectPersister.PersistObject(txtOutputPath.Text, GlobalObjectType.Form, form.Name, form)

        Finally
            Cursor.Current = Cursors.Default
            oclient.CloseSession()
        End Try
    End Sub


    Private Sub btnExtractForm_Click(sender As Object, e As EventArgs) Handles btnExtractForm.Click
        Try
            If cboScope.SelectedItem = "Vendor" Then
                ExtractFormForScope(cboForm.SelectedItem, Enums.ScopeTypes.SYMIX_DEFAULT)
            ElseIf cboScope.SelectedItem = "Site" Then
                ExtractFormForScope(cboForm.SelectedItem, Enums.ScopeTypes.SITE_DEFAULT)
            Else
                Throw New System.Exception("Scope must be entered.")
            End If
            MessageBox.Show("Form Extracted Successfully", STR_SLDevTools, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, STR_SLDevTools, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
